using FluentValidation;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Interfaces;
using LibrarySolution.Application.Abstractions.Commands;
using LibrarySolution.Domain.Aggregates.Books.Exceptions;
using LibrarySolution.Domain.Aggregates.Books.Repositories;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Domain.Aggregates.Rents.Entities;
using LibrarySolution.Domain.Aggregates.Rents.Repositories;
using LibrarySolution.Domain.Aggregates.Users.Exceptions;
using LibrarySolution.Domain.Aggregates.Users.Repositories;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;
using LibrarySolution.Shared.Helpers;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LibrarySolution.Application.Extensions;

namespace LibrarySolution.Application.UseCases.Rents.Commands;

#region Command
[TargetValidator(typeof(CreateRentCommandValidator))]
public sealed record CreateRentCommand : ICommand<CreateRentCommandResponse>
{
    [DefaultValue(GuidHelper.EmptyGuidWithHypen)]
    public required string BookId { get; init; }

    [DefaultValue(GuidHelper.EmptyGuidWithHypen)]
    public required string UserId { get; init; }
}
#endregion

#region Response
public sealed record CreateRentCommandResponse
{
    public required string RentId { get; init; }
    public required string BookId { get; init; }
    public required string UserId { get; init; }
    public required DateTime BorrowedAt { get; init; }
    public required DateTime DueDate { get; init; }
}
#endregion

#region Validator
internal sealed class CreateRentCommandValidator : AbstractValidator<CreateRentCommand>
{
    public CreateRentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("유저ID는 공백일 수 없습니다.")
            .Length(GuidHelper.LengthWithHypen)
            .WithMessage("올바른 유저ID 길이가 아닙니다.")
            .Must(id => UserId.TryParse(id, out _))
            .WithMessage("올바른 유저ID 형식이 아닙니다.");

        RuleFor(x => x.BookId)
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
internal sealed class CreateRentCommandHandler : ICommandHandler<CreateRentCommand, CreateRentCommandResponse>
{
    #region Constructor
    private readonly ILogger<CreateRentCommandHandler> _logger;
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRentRepository _rentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    public CreateRentCommandHandler(
        ILogger<CreateRentCommandHandler> logger,
        IBookRepository bookRepository,
        IUserRepository userRepository,
        IRentRepository rentRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
        _bookRepository = bookRepository;
        _userRepository = userRepository;
        _rentRepository = rentRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }
    #endregion

    #region Handle
    public async Task<CreateRentCommandResponse> Handle(CreateRentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {commandName}: {command}", request.GetGenericTypeName(), request);
        try
        {
            // 대상 유저 존재 유무 확인
            var userId = UserId.Parse(request.UserId);
            var user = _userRepository.GetUser(userId, cancellationToken);
            if (user is null)
                throw new UserNotFoundException(userId);

            // 도서가 대여 가능한 상태인지 확인
            var bookId = BookId.Parse(request.BookId);
            var book = await _bookRepository.GetBook(bookId, cancellationToken);
            if (book is null)
                throw new BookNotFoundException(bookId);

            // 도서 보유 수량 감소처리
            book.DecreaseQuantity();
            await _bookRepository.Update(book, cancellationToken);

            // 대여 정보 생성
            var rent = Rent.Create(userId, bookId, _dateTimeProvider.UtcNow);
            await _rentRepository.AddRent(rent, cancellationToken);




            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateRentCommandResponse
            {
                RentId = rent.Id.ToString(),
                BookId = rent.BookId.ToString(),
                UserId = rent.UserId.ToString(),
                BorrowedAt = rent.BorrowedAt,
                DueDate = rent.DueDate
            };
        }
        finally
        {
            _logger.LogInformation("Handled {commandName}: {command}", request.GetGenericTypeName(), request);
        }
    }
    #endregion
}
#endregion
