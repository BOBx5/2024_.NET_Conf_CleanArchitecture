using FluentValidation;
using FluentValidation.Results;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Abstractions.Queries;
using LibrarySolution.Domain.Aggregates.Users.Exceptions;
using LibrarySolution.Domain.Aggregates.Users.Repositories;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;
using LibrarySolution.Shared.Helpers;
using Microsoft.Extensions.Logging;
using LibrarySolution.Application.Extensions;

namespace LibrarySolution.Application.UseCases.Users.Queries;

#region Query
[TargetValidator(typeof(GetUserInfoQueryValidator))]
public sealed record GetUserInfoQuery : IQuery<GetUserInfoQueryResponse>
{
    public required string UserId { get; init; }
}
#endregion

#region Response
public sealed record GetUserInfoQueryResponse
{
    public required string UserId { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
}
#endregion

#region Validator
internal sealed class GetUserInfoQueryValidator : AbstractValidator<GetUserInfoQuery>
{
    public GetUserInfoQueryValidator()
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("유저ID는 공백일 수 없습니다.")
            .Length(GuidHelper.LengthWithHypen)
            .WithMessage("올바른 유저ID 길이가 아닙니다.")
            .Must(id => UserId.TryParse(id, out _))
            .WithMessage("올바른 유저ID 형식이 아닙니다.");
    }
}
#endregion

#region Handler
internal sealed class GetUserInfoQueryHandler : IQueryHandler<GetUserInfoQuery, GetUserInfoQueryResponse>
{
    #region Constructor
    private readonly ILogger<GetUserInfoQueryHandler> _logger;
    private readonly IUserRepository _UserRepository;
    public GetUserInfoQueryHandler(
        ILogger<GetUserInfoQueryHandler> logger,
        IUserRepository UserRepository)
    {
        _logger = logger;
        _UserRepository = UserRepository;
    }
    #endregion

    #region Handle
    public async Task<GetUserInfoQueryResponse> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var userId = UserId.Parse(request.UserId);
        var user = await _UserRepository.GetUser(userId, cancellationToken);
        if (user is null)
            throw new UserNotFoundException(userId);
        return new GetUserInfoQueryResponse
        {
            UserId = user.Id.ToString(),
            Name = user.Name,
            Email = user.Email
        };
    }
    #endregion
}
#endregion
