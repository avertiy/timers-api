namespace timers_api.domain.Data
{
    public interface IDateTimeProvider
    {
        DateTime GetSystemTime();
    }

    /// <summary>
    /// single point to manage server time DateTime.Now or UtcNow etc.
    /// </summary>
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetSystemTime()
        {
            return DateTime.Now;
        }
    }
}
