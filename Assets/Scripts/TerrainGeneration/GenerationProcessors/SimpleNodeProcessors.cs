using GigaChunker.DataTypes;
using Unity.Burst;
using Unity.Mathematics;

namespace TerrainGeneration.GenerationProcessors
{
    [BurstCompile]
    public static class SimpleNodeProcessors
    {
        private const float OffsetConst = 0.72354f;

        [BurstCompile]
        public static void FlatGround(ref GigaNode node, in int3 position)
        {
            if (position.y <= 0) node.Set(0, AxisWeights.Mid, AxisWeights.Mid, AxisWeights.Mid);
            else node.Set(0, AxisWeights.Min, AxisWeights.Min, AxisWeights.Min);
        }

        [BurstCompile]
        public static void Simple3dNoise(ref GigaNode node, in int3 position)
        {
            float3 floatPos = position;
            float weight = noise.cnoise(floatPos * OffsetConst);
            float up = noise.cnoise((floatPos + new int3(0, 1, 0)) * OffsetConst);
            float down = noise.cnoise((floatPos + new int3(0, -1, 0)) * OffsetConst);
            float right = noise.cnoise((floatPos + new int3(1, 0, 0)) * OffsetConst);
            float left = noise.cnoise((floatPos + new int3(-1, 0, 0)) * OffsetConst);
            float forward = noise.cnoise((floatPos + new int3(0, 0, 1)) * OffsetConst);
            float back = noise.cnoise((floatPos + new int3(0, 0, -1)) * OffsetConst);
            node.Set(0,
                new(CreateNodeWeight(weight, right), CreateNodeWeight(weight, left)),
                new(CreateNodeWeight(weight, up), CreateNodeWeight(weight, down)),
                new(CreateNodeWeight(weight, forward), CreateNodeWeight(weight, back))
            );
        }

        private static byte CreateNodeWeight(float baseWeight, float altWeight)
        {
            if (baseWeight <= 0) return 0;
            float t = (0 - baseWeight) / (altWeight - baseWeight);
            return (byte) math.clamp(t * 255, 1, 255);
        }
    }
}