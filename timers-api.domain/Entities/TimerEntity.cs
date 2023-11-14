namespace timers_api.domain.Entities;

public record class TimerEntity
{
    public Guid Id { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int Seconds { get; set; }
    public string WebhookUrl { get; set; } = null!;
    public TimerStatus Status { get; set; }
    public DateTime Timestamp { get; set; }

    public int CalculateTimeLeft(DateTime serverTime)
    {
        var elapsedTime = serverTime - Timestamp;
        var totalSeconds = Hours * 3600 + Minutes * 60 + Seconds;
        var timeLeft = totalSeconds - (int)elapsedTime.TotalSeconds;
        return timeLeft;
    }

    public int UpdateStatus(DateTime serverTime)
    {
        var timeLeft = CalculateTimeLeft(serverTime);
        if (timeLeft <= 0)
            Status = TimerStatus.Finished;
        return timeLeft;
    }
}

public enum TimerStatus
{
    Started = 0,    
    Finished =1,
    Expired, // when to use an expired status??
}