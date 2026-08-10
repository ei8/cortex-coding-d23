using ei8.Cortex.Coding.Spiker;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ei8.Cortex.Coding.d23.Loops
{
    public class DoUntil
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ISpikableReporting spikableReporting;
        private readonly IEnumerable<Neuron> actions;

        // TODO: might be possible to make independent, ie. keep on firing output neurons until condition is met, "working memory"
        // clear working memory once condition is met
        private Neuron variable;
        private readonly Neuron condition;
        private readonly Timer timer;
        
        public DoUntil(ISpikableReporting spikableReporting, IEnumerable<Neuron> actions, Neuron variable, Neuron condition)
        {
            this.timer = new Timer(this.DoCallback);

            this.spikableReporting = spikableReporting;

            this.actions = actions;
            this.variable = variable;
            this.condition = condition;
        }

        private void spikableReporting_Fired(object? sender, FiredEventArgs e)
        {
            DoUntil.logger.Info(
                new LogMessageGenerator(
                    () => $"Fired: {e.FireInfo.Target.ToReadableString()}"
                )
            );

            var presynaptics = this.spikableReporting.Network.GetPresynapticNeurons(e.FireInfo.Target.Id).ToArray();
            if (presynaptics.Any() && presynaptics.Any(pr => pr.Id == this.variable.Id))
            {
                this.variable = e.FireInfo.Target;
                DoUntil.logger.Info(
                new LogMessageGenerator(
                    () => $"Updated variable to: {this.variable.ToReadableString()}"
                )
            );
            }

            if (e.FireInfo.Target.Id == this.condition.Id)
                this.Stop();
        }

        public void Stop()
        {
            this.spikableReporting.Fired -= this.spikableReporting_Fired;
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Start()
        {
            this.spikableReporting.Fired += this.spikableReporting_Fired;
            this.timer.Change(0, 2000);
        }

        private void DoCallback(object? state)
        {
            this.spikableReporting.Spike(
                [
                    ..this.actions,
                    this.variable
                ]
            );
        }
    }
}
