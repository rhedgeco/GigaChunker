using GigaChunker.DataTypes;
using Unity.Mathematics;

namespace GigaChunker.Generators.MarchData
{
    public struct CornerRay
    {
        public byte Corner;
        public float3 Origin;
        public Axis.Direction Direction;

        public float3 GetPoint(float scale)
        {
            return Origin + Direction.ToVector() * scale;
        }
    }
}