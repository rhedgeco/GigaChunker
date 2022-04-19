namespace GigaChunker.DataTypes
{
    public struct AxisWeights
    {
        public byte Positive;
        public byte Negative;

        public AxisWeights(byte positive, byte negative)
        {
            Positive = positive;
            Negative = negative;
        }

        public static AxisWeights Max => new() {Positive = byte.MaxValue, Negative = byte.MaxValue};
        public static AxisWeights Mid => new() {Positive = 128, Negative = 128};
        public static AxisWeights Min => new() {Positive = byte.MinValue, Negative = byte.MinValue};
    }
}