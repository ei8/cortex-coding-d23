namespace ei8.Cortex.Coding.d23
{
    public enum StimulusType
    {
        Internal,
        External
    }

    public class StimulusInfo
    {
        public StimulusInfo(StimulusType type, object value)
        {
            this.Type = type;
            this.Value = value;
        }

        public StimulusType Type { get; }

        public object Value { get; }
    }
}