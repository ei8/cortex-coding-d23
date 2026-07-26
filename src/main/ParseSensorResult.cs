namespace ei8.Cortex.Coding.d23
{
    public class ParseSensorResult(object @object, Neuron value)
    {
        public object Object { get; } = @object;

        public Neuron Value { get; } = value;
    }
}
