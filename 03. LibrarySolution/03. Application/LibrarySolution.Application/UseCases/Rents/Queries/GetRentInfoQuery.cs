using FluentValidation;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Abstractions.Queries;
using LibrarySolution.Domain.Aggregates.Rents.Exceptions;
using LibrarySolution.Domain.Aggregates.Rents.Repositories;
using LibrarySolution.Domain.Aggregates.Rents.ValueObjects;
using LibrarySolution.Shared.Helpers;
using Microsoft.Extensions.Logging;
using LibrarySolution.Application.Extensions;

namespace LibrarySolution.Application.UseCases.Rents.Queries;

#region Query
[TargetValidator(typeof(GetRentInfoQueryValidator))]
public sealed record GetRentInfoQuery : IQuery<GetRentInfoQueryResponse>
{
    public required string RentId { get; init; }
}
#endregion

#region Response
public sealed record GetRentInfoQueryResponse
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
internal sealed class GetRentInfoQueryValidator : AbstractValidator<GetRentInfoQuery>
{
    public GetRentInfoQueryValidator()
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
internal sealed class GetRentInfoQueryHandler : IQueryHandler<GetRentInfoQuery, GetRentInfoQueryResponse>
{
    #region Constructor
    private readonly ILogger<GetRentInfoQueryHandler> _logger;
    private readonly IRentRepository _rentRepository;
    public GetRentInfoQueryHandler(
        ILogger<GetRentInfoQueryHandler> logger,
        IRentRepository rentRepository)
    {
        _logger = logger;
        _rentRepository = rentRepository;
    }
    #endregion

    #region Handle
    public async Task<GetRentInfoQueryResponse> Handle(GetRentInfoQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {queryName}: {query}", request.GetGenericTypeName(), request);
        try
        {
            var rentId = RentId.Parse(request.RentId);
            var rent = await _rentRepository.GetRent(rentId);
            if (rent is null)
                throw new RentNotFoundException(rentId);

            return new GetRentInfoQueryResponse
            {
                RentId = rent.Id.ToString(),
                BookId = rent.BookId.ToString(),
                UserId = rent.UserId.ToString(),
                BorrowedAt = rent.BorrowedAt,
                DueDate = rent.DueDate,
                ReturnedAt = rent.ReturnedAt
            };
        }
        finally
        {
            _logger.LogInformation("Finishing {queryName}: {query}", request.GetGenericTypeName(), request);
        }
    }
    #endregion
}
#endregion
