using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerTiming.Fakes;

public class FakeFile
{
    private readonly ServerTimingTracker _tracker;

    public FakeFile(ServerTimingTracker tracker) => _tracker = tracker;

    public async Task<string> Read(CancellationToken cancellationToken)
    {
        using ServerTimingToken token = _tracker.Start("io");
        await Task.Delay(new Random().Next(400, 500), cancellationToken);
        return "file result";
    }
}
