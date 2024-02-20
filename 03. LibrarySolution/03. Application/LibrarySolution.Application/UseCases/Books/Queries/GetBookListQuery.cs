using LibrarySolution.Application.Abstractions.Queries;
using LibrarySolution.Domain.Aggregates.Books.Repositories;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FluentValidation;
using LibrarySolution.Application.Attributes;

namespace LibrarySolution.Application.UseCases.Books.Queries;

#region Query
/// <summary>
/// 도서 목록 조회 Query
/// </summary>
[TargetValidator<GetBookListQueryValidator>]
public sealed record GetBookListQuery : IQuery<List<GetBookListQueryResponse>>
{
    /// <summary>검색할 도서 제목 (null일 경우 무시)</summary>
    public string? Title { get; set; }

    /// <summary>검색할 도서 저자 (null일 경우 무시)</summary>
    public string? Author { get; set; }
}
#endregion

#region Validator
internal sealed class GetBookListQueryValidator : AbstractValidator<GetBookListQuery>
{
    public GetBookListQueryValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(50)
            .WithMessage("도서 제목은 50자 이내로 입력해주세요.")
            .When(x => x.Title != null);

        RuleFor(x => x.Author)
            .MaximumLength(50)
            .WithMessage("저자명은 50자 이내로 입력해주세요.")
            .When(x => x.Author != null);
    }
}
#endregion

#region Response
/// <summary>
/// 도서 목록 조회 Query의 응답
/// </summary>
public sealed record GetBookListQueryResponse
{
    /// <summary>도서ID</summary>
    public required string BookId { get; init; }

    /// <summary>도서 제목</summary>
    public required string Title { get; init; }

    /// <summary>도서 설명</summary>
    public required string Description { get; init; }

    /// <summary>도서 저자</summary>
    public required string Author { get; init; }

    /// <summary>보유 수량</summary>
    public required int Quantity { get; init; }
}
#endregion

#region Handler
/// <summary>
/// 도서 목록 조회 Query Handler
/// </summary>
internal sealed class GetBookListQueryHandler : IQueryHandler<GetBookListQuery, List<GetBookListQueryResponse>>
{
    #region Constructor
    private readonly ILogger<GetBookListQueryHandler> _logger;
    private readonly IBookRepository _bookRepository;
    public GetBookListQueryHandler(
        ILogger<GetBookListQueryHandler> logger,
        IBookRepository bookRepository)
    {
        _logger = logger;
        _bookRepository = bookRepository;
    }
    #endregion

    #region Handle
    public async Task<List<GetBookListQueryResponse>> Handle(GetBookListQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetBooks(request.Title, request.Author, cancellationToken);
        var response = books.Select(book => new GetBookListQueryResponse
        {
            BookId = book.Id.ToString(),
            Title = book.Title,
            Description = book.Description,
            Author = book.Author,
            Quantity = book.Quantity
        }).ToList();
        return response;
    }
    #endregion
}
#endregion
