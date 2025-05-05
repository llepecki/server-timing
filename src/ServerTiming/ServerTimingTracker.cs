using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerTiming;

public class ServerTimingTracker
{
    private readonly Dictionary<Guid, (string Name, ServerTimingSpan Span)> _timings = new();
    private readonly TimeProvider _timeProvider;

    public ServerTimingTracker(TimeProvider timeProvider) => _timeProvider = timeProvider;

    public ServerTimingToken Start(string name)
    {
        var token = new ServerTimingToken(this);
        _timings[token.Key] = (name, new ServerTimingSpan(_timeProvider.GetUtcNow().ToUnixTimeMilliseconds()));
        return token;
    }

    public void End(ServerTimingToken token)
    {
        if (token.Key == Guid.Empty)
        {
            return;
        }

        if (!_timings.TryGetValue(token.Key, out var record))
        {
            throw new ArgumentException("Timing token not found", nameof(token));
        }

        _timings[token.Key] = (record.Name, record.Span.EndAt(_timeProvider.GetUtcNow().ToUnixTimeMilliseconds()));
    }

    public override string ToString() => _timings.Count == 0
        ? string.Empty
        : string.Join(", ", _timings.Values.Select(record => $"{record.Name};dur={record.Span.EndedAt - record.Span.StartedAt}"));
}
