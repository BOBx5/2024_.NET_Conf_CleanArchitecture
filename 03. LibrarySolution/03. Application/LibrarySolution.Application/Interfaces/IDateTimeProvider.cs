namespace LibrarySolution.Application.Interfaces;
/// <summary>
/// 시간제공 서비스 (NTP, Local, ...)
/// </summary>
public interface IDateTimeProvider
{
    public DateTime Now { get; }
    public DateTime UtcNow { get; }
}
