using FluentValidation;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Interfaces;
using LibrarySolution.Application.Abstractions.Commands;
using LibrarySolution.Domain.Aggregates.Books.Exceptions;
using LibrarySolution.Domain.Aggregates.Books.Repositories;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Domain.Aggregates.Rents.Exceptions;
using LibrarySolution.Domain.Aggregates.Rents.Repositories;
using LibrarySolution.Domain.Aggregates.Rents.ValueObjects;
using LibrarySolution.Shared.Helpers;
using Microsoft.Extensions.Logging;
using LibrarySolution.Application.Extensions;

namespace LibrarySolution.Application.UseCases.Rents.Commands;

#region Command
[TargetValidator(typeof(ReturnRentCommandValidator))]
public sealed record ReturnRentCommand : ICommand<ReturnRentCommandResponse>
{
    public required string RentId { get; init; }
}
#endregion

#region Response
public sealed record ReturnRentCommandResponse
{
    public required string RentId { get; init; }
}
#endregion

#region Validator
internal sealed class ReturnRentCommandValidator : AbstractValidator<ReturnRentCommand>
{
    public ReturnRentCommandValidator()
    {
        RuleFor(x => x.RentId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("대여ID는 공백일 수 없습니다.")
            .Length(GuidHelper.LengthWithHypen)
            .WithMessage("올바른 대여ID 길이가 아닙니다.")
            .Must(id => RentId.TryParse(id, out _))
            .WithMessage("올바른 대여ID 형식이 아닙니다.");
    }
}
#endregion

#region Handler
internal sealed class ReturnRentCommandHandler : ICommandHandler<ReturnRentCommand, ReturnRentCommandResponse>
{
    #region Constructor
    private readonly ILogger<ReturnRentCommandHandler> _logger;
    private readonly IRentRepository _rentRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    public ReturnRentCommandHandler(
        ILogger<ReturnRentCommandHandler> logger,
        IRentRepository rentRepository,
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
        _rentRepository = rentRepository;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }
    #endregion

    #region Handle
    public async Task<ReturnRentCommandResponse> Handle(ReturnRentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {commandName}: {command}", request.GetGenericTypeName(), request);
        try
        {
            // 대여 정보 확인
            var rentId = RentId.Parse(request.RentId);
            var rent = await _rentRepository.GetRent(rentId, cancellationToken);
            if (rent is null)
                throw new RentNotFoundException(rentId);

            // 대상 도서 확인
            var book = await _bookRepository.GetBook(rent.BookId);
            if (book is null)
                throw new BookNotFoundException(rent.BookId);

            // 도서 보유 수량 증가처리
            book.IncreaseQunatity();
            await _bookRepository.Update(book, cancellationToken);

            // 반납처리
            rent.Return(_dateTimeProvider.UtcNow);
            await _rentRepository.Update(rent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ReturnRentCommandResponse
            {
                RentId = rent.Id.ToString()
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