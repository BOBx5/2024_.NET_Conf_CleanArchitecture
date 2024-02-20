using FluentValidation;
using LibrarySolution.Application.Abstractions.Queries;
using LibrarySolution.Domain.Aggregates.Books.Exceptions;
using LibrarySolution.Domain.Aggregates.Books.Repositories;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LibrarySolution.Shared.Helpers;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Extensions;

namespace LibrarySolution.Application.UseCases.Books.Queries;

#region Query
/// <summary>도서 정보 조회 Query</summary>
[TargetValidator(typeof(GetBookInfoQueryValidator))]
public sealed record GetBookInfoQuery : IQuery<GetBookInfoQueryResponse>
{
    public required string BookId { get; init; }
}
#endregion

#region Response
/// <summary>도서 정보 조회 Query의 응답</summary>
public sealed record GetBookInfoQueryResponse
{
    public required string BookId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Author { get; init; }
    public required int Quantity { get; init; }
}

#endregion

#region Validator
/// <summary>도서 정보 조회 Query의 Validator</summary>
internal sealed class GetBookInfoQueryValidator : AbstractValidator<GetBookInfoQuery>
{
    public GetBookInfoQueryValidator()
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
/// <summary>도서 정보 조회 Query의 Handler</summary>
internal sealed class GetBookInfoQueryHandler : IQueryHandler<GetBookInfoQuery, GetBookInfoQueryResponse>
{
    #region Constructor
    private readonly ILogger<GetBookInfoQueryHandler> _logger;
    private readonly IBookRepository _bookRepository;
    public GetBookInfoQueryHandler(
        ILogger<GetBookInfoQueryHandler> logger,
        IBookRepository bookRepository)
    {
        _logger = logger;
        _bookRepository = bookRepository;
    }
    #endregion

    #region Handle
    public async Task<GetBookInfoQueryResponse> Handle(GetBookInfoQuery request, CancellationToken cancellationToken)
    {
        BookId bookId = BookId.Parse(request.BookId);
        var book = await _bookRepository.GetBook(bookId, cancellationToken);
        if (book is null)
            throw new BookNotFoundException(bookId);

        return new GetBookInfoQueryResponse
        {
            BookId = book.Id.ToString(),
            Title = book.Title,
            Description = book.Description,
            Author = book.Author,
            Quantity = book.Quantity,
        };
    }
    #endregion
}
#endregion
