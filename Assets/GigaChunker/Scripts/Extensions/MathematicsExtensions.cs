using Unity.Mathematics;

namespace GigaChunker.Extensions
{
    public static class MathematicsExtensions
    {
        public static int3 RoundToInt(this float3 float3)
        {
            return new(
                (int) (float3.x + 0.5f),
                (int) (float3.y + 0.5f),
                (int) (float3.z + 0.5f)
            );
        }
    }
}