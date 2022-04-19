using System;

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

        public static readonly AxisWeights Zero = new(0, 0);
        public static readonly AxisWeights Max = new(byte.MaxValue, byte.MaxValue);
    }
}