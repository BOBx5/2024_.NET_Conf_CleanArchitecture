using FluentValidation;
using LibrarySolution.Application.Abstractions.Commands;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Interfaces;
using LibrarySolution.Domain.Aggregates.Users.Entities;
using LibrarySolution.Domain.Aggregates.Users.Repositories;
using LibrarySolution.Shared.Helpers;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibrarySolution.Application.UseCases.Users.Commands;

#region Command
[TargetValidator(typeof(CreateUserCommandValidator))]
public sealed record CreateUserCommand : ICommand<CreateUserCommandResponse>
{
    [DefaultValue("유저1")]
    public required string Name { get; init; }

    [EmailAddress]
    [DefaultValue("user1@gmail.com")]
    public required string Email { get; init; }
}
#endregion

#region Response
public sealed record CreateUserCommandResponse
{
    public required string UserId { get; init; }
}
#endregion

#region Validator
internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage($"유저 이름은 공백일 수 없습니다.")
            .MaximumLength(50)
            .WithMessage($"유저 이름은 50자를 넘을 수 없습니다.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage($"유저 이메일은 공백일 수 없습니다.")
            //.EmailAddress() // FluentValidation에서 자체 제공하는 Email 형식 검증으로 '@' 포함 여부만 판단하는 약한 검증입니다.
            .Must(RegisteredRegex.Email.IsMatch)
            .WithMessage($"올바른 이메일 형식이 아닙니다.");
    }
}
#endregion

#region Handler
internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserCommandResponse>
{
    #region Constructor
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateUserCommandHandler(
        ILogger<CreateUserCommandHandler> logger,
        IUserRepository UserRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _userRepository = UserRepository;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region Handle
    public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(request.Name, request.Email);
        await _userRepository.AddUser(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new CreateUserCommandResponse
        {
            UserId = user.Id.ToString()
        };
    }
    #endregion
}
#endregion