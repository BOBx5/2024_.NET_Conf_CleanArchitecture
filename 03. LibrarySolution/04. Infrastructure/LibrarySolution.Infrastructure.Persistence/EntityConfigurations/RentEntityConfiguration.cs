using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Domain.Aggregates.Rents.Entities;
using LibrarySolution.Domain.Aggregates.Rents.ValueObjects;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;
using LibrarySolution.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySolution.Infrastructure.Persistence.EntityConfigurations;
internal sealed class RentEntityConfiguration : IEntityTypeConfiguration<Rent>
{
    public void Configure(EntityTypeBuilder<Rent> builder)
    {
        builder.ToTable(DbTables.Rent.Name, DbTables.Rent.Schema);
        builder.HasKey(rent => rent.Id);

        builder.Property(rent => rent.Id)
            .IsRequired()
            .HasMaxLength(GuidHelper.LengthWithHypen)
            .IsFixedLength()
            .HasConversion(
                rentId  => rentId.ToString(),
                dbValue => RentId.Parse(dbValue));

        builder.Property(rent => rent.UserId)
            .IsRequired()
            .HasMaxLength(GuidHelper.LengthWithHypen)
            .IsFixedLength()
            .HasConversion(
                userId  => userId.ToString(),
                dbValue => UserId.Parse(dbValue));

        builder.Property(book => book.BookId)
            .IsRequired()
            .HasMaxLength(GuidHelper.LengthWithHypen)
            .IsFixedLength()
            .HasConversion(
                bookId  => bookId.ToString(),
                dbValue => BookId.Parse(dbValue));

        builder.Property(book => book.BorrowedAt)
            .IsRequired();

        builder.Property(book => book.DueDate)
            .IsRequired();

        builder.Property(book => book.ReturnedAt);

        builder.Property(rent => rent.UpdatedAt);

        builder.Property(rent => rent.CreatedAt)
            .IsRequired();
    }
}
