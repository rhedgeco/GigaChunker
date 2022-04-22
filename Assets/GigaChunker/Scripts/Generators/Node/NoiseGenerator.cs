using Unity.Mathematics;

namespace GigaChunker.Generators.Node
{
    public class NoiseGenerator
    {
        private const float CNoiseScale = 0.72354f;
        
        public static float Perlin(float3 point, int octaves, float frequency, float lacunarity, float persistence)
        {
            float noise3d = 0;
            float factor = 1;
            float factorTotal = 0;
            for (int i = 0; i < octaves; i++)
            {
                factorTotal += factor;
                noise3d += noise.cnoise(point * frequency + i * CNoiseScale) * factor;
                factor *= persistence;
                frequency *= lacunarity;
            }

            return noise3d / factorTotal;
        }
        
        public static float Perlin(float2 point, int octaves, float frequency, float lacunarity, float persistence)
        {
            float noise2d = 0;
            float factor = 1;
            float factorTotal = 0;
            for (int i = 0; i < octaves; i++)
            {
                factorTotal += factor;
                noise2d += noise.cnoise(point * frequency + i * CNoiseScale) * factor;
                factor *= persistence;
                frequency *= lacunarity;
            }

            return noise2d / factorTotal;
        }
    }
}