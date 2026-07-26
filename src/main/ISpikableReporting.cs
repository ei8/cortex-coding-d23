using ei8.Cortex.Coding.Spiker;
using System;

namespace ei8.Cortex.Coding.d23
{
    public interface ISpikableReporting : ISpikable
    {
        float ProcessingRatio { get; }

        event EventHandler<TriggeredEventArgs>? Triggered;

        event EventHandler<FiredEventArgs>? Fired;
    }
}
