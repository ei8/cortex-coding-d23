namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class DualInputInterneuronTagInfo : InterneuronTagInfo
    {
        public DualInputInterneuronTagInfo(string commonTagPrefix) :
        this(
            commonTagPrefix,
            commonTagPrefix,
            commonTagPrefix
        )
        {
        }

        public DualInputInterneuronTagInfo(
            string input1TagPrefix,
            string input2TagPrefix,
            string typeTagPrefix
        ) : base(
            [
                input1TagPrefix,
                input2TagPrefix
            ],
            typeTagPrefix
        )
        {
        }
    }
}
