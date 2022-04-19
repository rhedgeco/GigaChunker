namespace GigaChunker.DataTypes
{
    public struct GigaNode
    {
        public byte Type;
        public bool Solid;
        public AxisWeightOffset XWeightOffset;
        public AxisWeightOffset YWeightOffset;
        public AxisWeightOffset ZWeightOffset;

        public void Set(byte type, AxisWeightOffset xWeightOffset, AxisWeightOffset yWeightOffset, AxisWeightOffset zWeightOffset)
        {
            Type = type;
            XWeightOffset = xWeightOffset;
            YWeightOffset = yWeightOffset;
            ZWeightOffset = zWeightOffset;
        }
    }
}