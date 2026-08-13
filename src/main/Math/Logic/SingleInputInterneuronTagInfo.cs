namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class SingleInputInterneuronTagInfo : InterneuronTagInfo
    {
        public SingleInputInterneuronTagInfo(string commonTagPrefix) :
        this(
            commonTagPrefix,
            commonTagPrefix
        )
        {
        }

        public SingleInputInterneuronTagInfo(
            string input,
            string typeTagPrefix
        ) : base(
            [
                input,
            ],
            typeTagPrefix
        )
        {
        }
    }
}
