using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerTiming.Fakes;

public class FakeDatabase
{
    private readonly ServerTimingTracker _tracker;

    public FakeDatabase(ServerTimingTracker tracker) => _tracker = tracker;

    public async Task<string> Query(CancellationToken cancellationToken)
    {
        using ServerTimingToken token = _tracker.Start("db");
        await Task.Delay(new Random().Next(500, 1000), cancellationToken);
        return "database result";
    }
}
