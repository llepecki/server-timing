namespace ServerTiming;

public readonly record struct ServerTimingSpan
{
    public ServerTimingSpan(long startedAt)
    {
        StartedAt = startedAt;
        EndedAt = long.MaxValue;
    }

    private ServerTimingSpan(long startedAt, long endedAt)
    {
        StartedAt = startedAt;
        EndedAt = endedAt;
    }

    public long StartedAt { get; }

    public long EndedAt { get; }

    public ServerTimingSpan EndAt(long endedAt) => new ServerTimingSpan(StartedAt, endedAt);
}
