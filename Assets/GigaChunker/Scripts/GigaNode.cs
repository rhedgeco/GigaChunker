using GigaChunker.DataTypes;

namespace GigaChunker
{
    public struct GigaNode
    {
        public byte Type;
        public bool HasContour;
        public AxisWeights XWeight;
        public AxisWeights YWeight;
        public AxisWeights ZWeight;
    }
}