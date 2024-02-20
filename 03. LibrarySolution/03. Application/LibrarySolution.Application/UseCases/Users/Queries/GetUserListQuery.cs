using FluentValidation;
using FluentValidation.Results;
using LibrarySolution.Shared.Helpers;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Abstractions.Queries;
using LibrarySolution.Domain.Aggregates.Users.Repositories;
using Microsoft.Extensions.Logging;

namespace LibrarySolution.Application.UseCases.Users.Queries;

#region Query
[TargetValidator(typeof(GetUserListQueryValidator))]
public sealed record GetUserListQuery : IQuery<List<GetUserListQueryResponse>>
{
    public string? Name { get; set; } = null;
    public string? Email { get; set; } = null;
}
#endregion

#region Response
public sealed record GetUserListQueryResponse
{
    public required string UserId { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
}
#endregion

#region Validator
internal sealed class GetUserListQueryValidator : AbstractValidator<GetUserListQuery>
{
    public GetUserListQueryValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .WithMessage("유저명은 50자 이내로 입력해주세요.")
            .When(x => x.Name is not null);

        RuleFor(x => x.Email)
            .MaximumLength(50)
            .WithMessage("이메일은 50자 이내로 입력해주세요.")
            .When(x => x.Email is not null);
    }
}
#endregion

#region Handler
internal sealed class GetUserListQueryHandler : IQueryHandler<GetUserListQuery, List<GetUserListQueryResponse>>
{
    #region Constructor
    private readonly ILogger<GetUserListQueryHandler> _logger;
    private readonly IUserRepository _userRepository;
    public GetUserListQueryHandler(
        ILogger<GetUserListQueryHandler> logger,
        IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }
    #endregion

    #region Handle
    public async Task<List<GetUserListQueryResponse>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetUsers(request.Name, request.Email, cancellationToken);
        return users.Select(user => new GetUserListQueryResponse
        {
            UserId = user.Id.ToString(),
            Name = user.Name,
            Email = user.Email
        }).ToList();
    }
    #endregion
}
#endregion
