using LibrarySolution.Domain.Aggregates.Books.Entities;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySolution.Infrastructure.Persistence.EntityConfigurations;
internal sealed class BookEntityConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable(DbTables.Book.Name, DbTables.Book.Schema);
        builder.HasKey(book => book.Id);

        builder.Property(book => book.Id)
            .IsRequired()
            .HasMaxLength(GuidHelper.LengthWithHypen)
            .IsFixedLength()
            .HasConversion(
                bookId  => bookId.ToString(),
                dbValue => BookId.Parse(dbValue));

        builder.Property(book => book.Title)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(book => book.Description)
            .HasMaxLength(50);

        builder.Property(book => book.Author)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(book => book.Quantity)
            .IsRequired();

        builder.Property(book => book.UpdatedAt);

        builder.Property(book => book.CreatedAt)
            .IsRequired();
    }
}
