using LibrarySolution.Domain.Aggregates.Rents.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Rents.Exceptions;
public sealed class RentAlreadyReturnedException : DomainException
{
    public string RentId { get; }
    public RentAlreadyReturnedException(RentId rentId) : base(
        message: "이미 반납된 대여 정보입니다.",
        errors: new() { { nameof(RentId), rentId.ToString() } })
    {
        RentId = rentId.ToString();
    }
}
