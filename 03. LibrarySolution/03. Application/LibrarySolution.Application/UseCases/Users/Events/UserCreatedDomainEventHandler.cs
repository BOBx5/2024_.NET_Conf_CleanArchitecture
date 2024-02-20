using LibrarySolution.Application.Abstractions.Events;
using LibrarySolution.Application.Interfaces;
using LibrarySolution.Domain.Aggregates.Users.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySolution.Application.UseCases.Users.Events;
internal sealed class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
{
    #region Constructor
    private readonly IEmailService _emailService;
    public UserCreatedDomainEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    } 
    #endregion

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _emailService.SendEmailAsync(
            email: notification.Email,
            subject: "Clean도서관 회원가입을 축하드립니다.",
            htmlMessage: "",
            cancellationToken: cancellationToken
            );
    }
}
