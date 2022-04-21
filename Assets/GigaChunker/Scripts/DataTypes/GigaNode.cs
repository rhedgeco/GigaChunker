using System;

namespace GigaChunker.DataTypes
{
    public struct GigaNode
    {
        public byte Type;
        public AxisWeights XWeight;
        public AxisWeights YWeight;
        public AxisWeights ZWeight;

        public GigaNode(byte type, AxisWeights xWeight, AxisWeights yWeight, AxisWeights zWeight)
        {
            Type = type;
            XWeight = xWeight;
            YWeight = yWeight;
            ZWeight = zWeight;
        }

        public void Set(byte type, AxisWeights xWeight, AxisWeights yWeight, AxisWeights zWeight)
        {
            Type = type;
            XWeight = xWeight;
            YWeight = yWeight;
            ZWeight = zWeight;
        }

        public float GetWeight(Axis.Direction direction)
        {
            return direction switch
            {
                Axis.Direction.Right => XWeight.Positive / 128f,
                Axis.Direction.Left => XWeight.Negative / 128f,
                Axis.Direction.Up => YWeight.Positive / 128f,
                Axis.Direction.Down => YWeight.Negative / 128f,
                Axis.Direction.Forward => ZWeight.Positive / 128f,
                Axis.Direction.Back => ZWeight.Negative / 128f,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}