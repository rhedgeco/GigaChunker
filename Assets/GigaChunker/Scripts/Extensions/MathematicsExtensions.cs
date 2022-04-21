using Unity.Mathematics;

namespace GigaChunker.Extensions
{
    public static class MathematicsExtensions
    {
        public static int3 RoundToInt(this float3 float3)
        {
            return new(
                (int) math.round(float3.x),
                (int) math.round(float3.y),
                (int) math.round(float3.z)
            );
        }

        public static void Set(this ref int3 value, int x, int y, int z)
        {
            value.x = x;
            value.y = y;
            value.z = z;
        }
        
        public static void Set(this ref float3 value, int x, int y, int z)
        {
            value.x = x;
            value.y = y;
            value.z = z;
        }
    }
}