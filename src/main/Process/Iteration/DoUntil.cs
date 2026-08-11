using ei8.Cortex.Coding.Spiker;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ei8.Cortex.Coding.d23.Process.Iteration
{
    public class DoUntil : IProcess<DoUntil.WorkingMemoryKeys>
    {
        public enum WorkingMemoryKeys
        {
            Actions,
            Variable,
            Condition
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private ISpikableReporting? spikableReporting;
        // TODO: might be possible to make independent, ie. keep on firing output neurons until condition is met, "working memory"
        // clear working memory once condition is met
        private IWorkingMemory<WorkingMemoryKeys>? workingMemory;
        private readonly Timer timer;
        
        public DoUntil()
        {
            this.timer = new Timer(this.DoCallback);
        }

        private void spikableReporting_Fired(object? sender, FiredEventArgs e)
        {
            DoUntil.logger.Info(
                new LogMessageGenerator(
                    () => $"Fired: {e.FireInfo.Target.ToReadableString()}"
                )
            );

            if (this.spikableReporting != null && this.workingMemory != null)
            {
                var presynaptics = this.spikableReporting.Network.GetPresynapticNeurons(e.FireInfo.Target.Id).ToArray();
                if (
                    presynaptics.Any() &&
                    this.workingMemory.TryGetContents<
                        WorkingMemoryKeys, 
                        WriteableKeyedChunk<WorkingMemoryKeys>, 
                        IList<Neuron>
                    >(
                        WorkingMemoryKeys.Variable, 
                        out var variables
                    ) &&
                    presynaptics.Intersect(variables).Any()
                )
                {
                    variables.Clear();
                    variables.Add(e.FireInfo.Target);
                    DoUntil.logger.Info(
                    new LogMessageGenerator(
                        () => $"Updated variable to: {e.FireInfo.Target.ToReadableString()}"
                    )
                );
                }

                if (
                    this.workingMemory.TryGetContents<
                        WorkingMemoryKeys,
                        ReadableKeyedChunk<WorkingMemoryKeys>,
                        IEnumerable<Neuron>
                    >(
                        WorkingMemoryKeys.Condition,
                        out var condition
                    ) &&
                    e.FireInfo.Target.Id == condition.Single().Id
                )
                    this.Stop();
            }
        }

        public void Stop()
        {
            ArgumentNullException.ThrowIfNull(this.spikableReporting);

            this.spikableReporting.Fired -= this.spikableReporting_Fired;
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
        }


        public void Start(ISpikable spikable, IWorkingMemory workingMemory) =>
            this.Start(spikable, (IWorkingMemory<WorkingMemoryKeys>) workingMemory);

        public void Start(ISpikable spikable, IWorkingMemory<WorkingMemoryKeys> workingMemory)
        {
            this.spikableReporting = (ISpikableReporting) spikable;
            this.spikableReporting.Fired += this.spikableReporting_Fired;

            this.workingMemory = workingMemory;
            
            this.timer.Change(0, 2000);
        }

        private void DoCallback(object? state)
        {
            if (
                this.workingMemory != null &&
                this.workingMemory.TryGetContents<
                    WorkingMemoryKeys,
                    ReadableKeyedChunk<WorkingMemoryKeys>,
                    IEnumerable<Neuron>
                >(
                    WorkingMemoryKeys.Actions,
                    out var actions
                ) &&
                this.workingMemory.TryGetContents<
                    WorkingMemoryKeys,
                    WriteableKeyedChunk<WorkingMemoryKeys>,
                    IList<Neuron>
                >(
                    WorkingMemoryKeys.Variable,
                    out var variable
                )
            )
            this.spikableReporting?.Spike(
                [
                    ..actions,
                    ..variable
                ]
            );
        }

    }
}
