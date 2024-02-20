using LibrarySolution.Application.Interfaces;

namespace LibrarySolution.Infrastructure.DateTimeProvider;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;
}
