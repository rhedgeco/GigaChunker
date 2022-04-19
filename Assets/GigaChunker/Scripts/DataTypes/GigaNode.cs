namespace GigaChunker.DataTypes
{
    public struct GigaNode
    {
        public byte Type;
        public AxisWeights XWeight;
        public AxisWeights YWeight;
        public AxisWeights ZWeight;

        public void Set(byte type, AxisWeights xWeight, AxisWeights yWeight, AxisWeights zWeight)
        {
            Type = type;
            XWeight = xWeight;
            YWeight = yWeight;
            ZWeight = zWeight;
        }
    }
}