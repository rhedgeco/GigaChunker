namespace GigaChunker.DataTypes
{
    public struct AxisOffset
    {
        public sbyte XOffset;
        public sbyte YOffset;
        public sbyte ZOffset;

        public AxisOffset(sbyte xOffset, sbyte yOffset, sbyte zOffset)
        {
            XOffset = xOffset;
            YOffset = yOffset;
            ZOffset = zOffset;
        }

        public static readonly AxisOffset Zero = new(0, 0, 0);
        public static readonly AxisOffset Min = new(sbyte.MinValue, sbyte.MinValue, sbyte.MinValue);
        public static readonly AxisOffset Max = new(sbyte.MaxValue, sbyte.MaxValue, sbyte.MaxValue);
    }
}