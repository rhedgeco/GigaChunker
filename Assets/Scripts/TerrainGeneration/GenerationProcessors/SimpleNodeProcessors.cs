using GigaChunker.DataTypes;
using Unity.Burst;
using Unity.Mathematics;

namespace TerrainGeneration.GenerationProcessors
{
    [BurstCompile]
    public static class SimpleNodeProcessors
    {
        private const float CNoiseScale = 0.72354f;

        [BurstCompile]
        public static void Simple3dNoise(ref GigaNode node, in int3 position)
        {
            float3 floatPos = position;
            float weight = noise.cnoise(floatPos * CNoiseScale);
            float up = noise.cnoise((floatPos + new int3(0, 1, 0)) * CNoiseScale);
            float down = noise.cnoise((floatPos + new int3(0, -1, 0)) * CNoiseScale);
            float right = noise.cnoise((floatPos + new int3(1, 0, 0)) * CNoiseScale);
            float left = noise.cnoise((floatPos + new int3(-1, 0, 0)) * CNoiseScale);
            float forward = noise.cnoise((floatPos + new int3(0, 0, 1)) * CNoiseScale);
            float back = noise.cnoise((floatPos + new int3(0, 0, -1)) * CNoiseScale);
            node.Set(0,
                new(CreateNodeWeight(weight, right), CreateNodeWeight(weight, left)),
                new(CreateNodeWeight(weight, up), CreateNodeWeight(weight, down)),
                new(CreateNodeWeight(weight, forward), CreateNodeWeight(weight, back))
            );
        }

        private static sbyte CreateNodeWeight(float baseWeight, float altWeight)
        {
            if (baseWeight <= 0) return 0;
            float t = (0 - baseWeight) / (altWeight - baseWeight);
            return (sbyte) math.clamp(t * 127, 1, 127);
        }
    }
}