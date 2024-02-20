using FluentValidation;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Abstractions.Queries;
using LibrarySolution.Domain.Aggregates.Rents.Repositories;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;
using LibrarySolution.Shared.Helpers;
using Microsoft.Extensions.Logging;
using LibrarySolution.Application.Extensions;

namespace LibrarySolution.Application.UseCases.Rents.Queries;

#region Query
[TargetValidator(typeof(GetRentListQueryValidator))]
public sealed record GetRentListQuery : IQuery<List<GetRentListQueryResponse>>
{
    public required string? BookId { get; init; }
    public required string? UserId { get; init; }
}
#endregion

#region Response
public sealed record GetRentListQueryResponse
{
    public required string RentId { get; init; }
    public required string BookId { get; init; }
    public required string UserId { get; init; }
    public required DateTime BorrowedAt { get; init; }
    public required DateTime DueDate { get; init; }
    public DateTime? ReturnedAt { get; init; }
}
#endregion

#region Validator
internal sealed class GetRentListQueryValidator : AbstractValidator<GetRentListQuery>
{
    public GetRentListQueryValidator()
    {
        RuleFor(x => x.BookId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("도서ID는 공백일 수 없습니다.")
            .Length(GuidHelper.LengthWithHypen)
            .WithMessage("올바른 도서ID 길이가 아닙니다.")
            .Must(id => BookId.TryParse(id, out _))
            .WithMessage("올바른 도서ID 형식이 아닙니다.")
            .When(x => !string.IsNullOrWhiteSpace(x.BookId));

        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("유저ID는 공백일 수 없습니다.")
            .Length(GuidHelper.LengthWithHypen)
            .WithMessage("올바른 유저ID 길이가 아닙니다.")
            .Must(id => UserId.TryParse(id, out _))
            .WithMessage("올바른 유저ID 형식이 아닙니다.")
            .When(x => !string.IsNullOrWhiteSpace(x.UserId));
    }
}
#endregion

#region Handler
internal sealed class GetRentListQueryHandler : IQueryHandler<GetRentListQuery, List<GetRentListQueryResponse>>
{
    #region Constructor
    private readonly ILogger<GetRentListQueryHandler> _logger;
    private readonly IRentRepository _rentRepository;
    public GetRentListQueryHandler(
        ILogger<GetRentListQueryHandler> logger,
        IRentRepository rentRepository)
    {
        _logger = logger;
        _rentRepository = rentRepository;
    }
    #endregion

    #region Handle
    public async Task<List<GetRentListQueryResponse>> Handle(GetRentListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {queryName}: {query}", request.GetGenericTypeName(), request);
        try
        {
            BookId? bookId = !string.IsNullOrWhiteSpace(request.BookId)
                ? BookId.Parse(request.BookId)
                : null;

            UserId? userId = !string.IsNullOrWhiteSpace(request.UserId)
                ? UserId.Parse(request.UserId)
                : null;

            var rents = await _rentRepository.GetRents(userId, bookId, cancellationToken);
            var response = rents.Select(i => new GetRentListQueryResponse
            {
                RentId = i.Id.ToString(),
                BookId = i.BookId.ToString(),
                UserId = i.UserId.ToString(),
                BorrowedAt = i.BorrowedAt,
                DueDate = i.DueDate,
                ReturnedAt = i.ReturnedAt
            }).ToList();
            return response;
        }
        finally
        {
            _logger.LogInformation("Finishing {queryName}: {query}", request.GetGenericTypeName(), request);
        }
    }
    #endregion
}
#endregion
