using FluentValidation;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Interfaces;
using LibrarySolution.Application.Abstractions.Commands;
using LibrarySolution.Domain.Aggregates.Books.Exceptions;
using LibrarySolution.Domain.Aggregates.Books.Repositories;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Shared.Helpers;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LibrarySolution.Application.Extensions;

namespace LibrarySolution.Application.UseCases.Books.Commands;

#region Command
[TargetValidator(typeof(RemoveBookCommandValidator))]
public sealed record RemoveBookCommand : ICommand<RemoveBookCommandResponse>
{
    [DefaultValue(GuidHelper.EmptyGuidWithHypen)]
    public required string BookId { get; init; }
}
#endregion

#region Response
public sealed record RemoveBookCommandResponse
{
    public required string BookId { get; init; }
}
#endregion

#region Validator
internal sealed class RemoveBookCommandValidator : AbstractValidator<RemoveBookCommand>
{
    public RemoveBookCommandValidator()
    {
        RuleFor(x => x.BookId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("도서ID는 공백일 수 없습니다.")
            .Length(GuidHelper.LengthWithHypen)
            .WithMessage("올바른 도서ID 길이가 아닙니다.")
            .Must(id => BookId.TryParse(id, out _))
            .WithMessage("올바른 도서ID 형식이 아닙니다.");
    }
}
#endregion

#region Handler
internal sealed class RemoveBookCommandHandler : ICommandHandler<RemoveBookCommand, RemoveBookCommandResponse>
{
    #region Constructor
    private readonly ILogger<RemoveBookCommandHandler> _logger;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RemoveBookCommandHandler(
        ILogger<RemoveBookCommandHandler> logger,
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region Handle
    public async Task<RemoveBookCommandResponse> Handle(RemoveBookCommand request, CancellationToken cancellationToken)
    {
        var bookId = BookId.Parse(request.BookId);
        var book = await _bookRepository.GetBook(bookId, cancellationToken);
        if (book is null)
            throw new BookNotFoundException(bookId);

        await _bookRepository.Remove(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RemoveBookCommandResponse
        {
            BookId = book.Id.ToString()
        };
    }
    #endregion
}
#endregion

