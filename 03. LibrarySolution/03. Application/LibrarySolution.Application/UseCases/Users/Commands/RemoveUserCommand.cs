using FluentValidation;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Interfaces;
using LibrarySolution.Application.Abstractions.Commands;
using LibrarySolution.Domain.Aggregates.Users.Exceptions;
using LibrarySolution.Domain.Aggregates.Users.Repositories;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;
using LibrarySolution.Shared.Helpers;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LibrarySolution.Application.Extensions;

namespace LibrarySolution.Application.UseCases.Users.Commands;

#region Command
[TargetValidator(typeof(RemoveUserCommandValidator))]
public sealed record RemoveUserCommand : ICommand<RemoveUserCommandResponse>
{
    [DefaultValue(GuidHelper.EmptyGuidWithHypen)]
    public required string UserId { get; init; }
}
#endregion

#region Response
public sealed record RemoveUserCommandResponse
{
    public required string UserId { get; init; }
}
#endregion

#region Validator
internal sealed class RemoveUserCommandValidator : AbstractValidator<RemoveUserCommand>
{
    public RemoveUserCommandValidator()
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
internal sealed class RemoveUserCommandHandler : ICommandHandler<RemoveUserCommand, RemoveUserCommandResponse>
{
    #region Constructor
    private readonly ILogger<RemoveUserCommandHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RemoveUserCommandHandler(
        ILogger<RemoveUserCommandHandler> logger,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region Handle
    public async Task<RemoveUserCommandResponse> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        var userId = UserId.Parse(request.UserId);
        var user = await _userRepository.GetUser(userId, cancellationToken);
        if (user is null)
            throw new UserNotFoundException(userId);

        await _userRepository.Remove(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new RemoveUserCommandResponse
        {
            UserId = user.Id.ToString()
        };
    }
    #endregion
}
#endregion
