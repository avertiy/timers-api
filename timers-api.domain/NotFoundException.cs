namespace timers_api.domain;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}