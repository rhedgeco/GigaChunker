namespace GigaChunker.DataTypes
{
    public struct AxisWeights
    {
        public sbyte Positive;
        public sbyte Negative;

        public bool Solid => Positive != 0 && Negative != 0;

        public AxisWeights(sbyte positive, sbyte negative)
        {
            Positive = positive;
            Negative = negative;
        }

        public static readonly AxisWeights Zero = new(0, 0);
        public static readonly AxisWeights Min = new(sbyte.MinValue, sbyte.MinValue);
        public static readonly AxisWeights Max = new(sbyte.MaxValue, sbyte.MaxValue);
    }
}