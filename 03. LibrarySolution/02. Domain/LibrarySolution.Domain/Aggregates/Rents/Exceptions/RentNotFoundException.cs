using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Domain.Aggregates.Rents.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Rents.Exceptions;
public sealed class RentNotFoundException : DomainException
{
    public string RentId { get; }
    public RentNotFoundException(RentId rentId) : base(
        message: "해당하는 ID의 대여 정보를 찾을 수 없습니다.",
        errors: new() { { nameof(RentId), rentId.ToString() } })
    {
        RentId = rentId.ToString();
    }
}
