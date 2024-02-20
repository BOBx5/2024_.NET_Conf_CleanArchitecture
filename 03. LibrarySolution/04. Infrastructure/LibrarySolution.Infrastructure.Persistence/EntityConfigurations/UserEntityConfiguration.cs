using LibrarySolution.Domain.Aggregates.Users.Entities;
using LibrarySolution.Domain.Aggregates.Users.Enums;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;
using LibrarySolution.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySolution.Infrastructure.Persistence.EntityConfigurations;
internal sealed class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(DbTables.User.Name, DbTables.User.Schema);
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .IsRequired()
            .HasMaxLength(GuidHelper.LengthWithHypen)
            .IsFixedLength()
            .HasConversion(
                userId  => userId.ToString(),
                dbValue => UserId.Parse(dbValue));

        builder.Property(user => user.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(user => user.Status)
            .IsRequired()
            .HasConversion(
                userStatus => (int)userStatus,
                dbValue    => (UserStatus)dbValue);

        builder.Property(user => user.Email)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(book => book.UpdatedAt);

        builder.Property(book => book.CreatedAt)
            .IsRequired();
    }
}
