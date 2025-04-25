using System;

namespace ServerTiming;

public sealed class ServerTimingToken : IDisposable
{
    private ServerTimingTracker? _tracker;

    public ServerTimingToken(ServerTimingTracker tracker)
    {
        Key = Guid.NewGuid();
        _tracker = tracker;
    }

    public Guid Key { get; }

    public void Dispose()
    {
        if (_tracker is not null)
        {
            var tracker = _tracker;
            _tracker = null;
            tracker.End(this);
        }
    }
}
