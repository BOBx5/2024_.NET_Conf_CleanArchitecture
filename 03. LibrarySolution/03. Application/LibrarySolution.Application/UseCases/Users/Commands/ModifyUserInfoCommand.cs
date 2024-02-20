using FluentValidation;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Interfaces;
using LibrarySolution.Application.Abstractions.Commands;
using LibrarySolution.Domain.Aggregates.Users.Exceptions;
using LibrarySolution.Domain.Aggregates.Users.Repositories;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LibrarySolution.Shared.Helpers;

namespace LibrarySolution.Application.UseCases.Users.Commands;

#region Command
[TargetValidator(typeof(ModifyUserInfoCommandValidator))]
public sealed record ModifyUserInfoCommand : ICommand<ModifyUserInfoCommandResponse>
{
    [DefaultValue(GuidHelper.EmptyGuidWithHypen)]
    public required string UserId { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
}
#endregion

#region Response
public sealed record ModifyUserInfoCommandResponse
{
    public required string UserId { get; init; }
}
#endregion

#region Validator
internal sealed class ModifyUserInfoCommandValidator : AbstractValidator<ModifyUserInfoCommand>
{
    public ModifyUserInfoCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("유저명은 공백일 수 없습니다.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("올바른 이메일 양식이 아닙니다.");
    }
}
#endregion

#region Handler
internal sealed class ModifyUserInfoCommandHandler : ICommandHandler<ModifyUserInfoCommand, ModifyUserInfoCommandResponse>
{
    #region Constructor
    private readonly ILogger<ModifyUserInfoCommandHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ModifyUserInfoCommandHandler(
        ILogger<ModifyUserInfoCommandHandler> logger,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region Handle
    public async Task<ModifyUserInfoCommandResponse> Handle(ModifyUserInfoCommand request, CancellationToken cancellationToken)
    {
        var userId = UserId.Parse(request.UserId);
        var user = await _userRepository.GetUser(userId, cancellationToken);
        if (user is null)
            throw new UserNotFoundException(userId);

        user.ModifyInfo(
            name: request.Name ?? user.Name,
            email: request.Email);

        await _userRepository.Update(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new ModifyUserInfoCommandResponse
        {
            UserId = user.Id.ToString()
        };
    }
    #endregion
}
#endregion