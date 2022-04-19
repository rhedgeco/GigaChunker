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
        public static void SimpleFlatGround(ref GigaNode node, in int3 position)
        {
            if (position.y <= 5) node.Set(1, AxisWeights.Max, AxisWeights.Max, AxisWeights.Max);
            else node.Set(0, AxisWeights.Zero, AxisWeights.Zero, AxisWeights.Zero);
        }

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

            AxisWeights xWeight = new(CreateNodeWeight(weight, right), CreateNodeWeight(weight, left));
            AxisWeights yWeight = new(CreateNodeWeight(weight, right), CreateNodeWeight(weight, left));
            AxisWeights zWeight = new(CreateNodeWeight(weight, right), CreateNodeWeight(weight, left));
            byte type = (byte) (!xWeight.Solid && !yWeight.Solid && !zWeight.Solid ? 0 : 1);
            node.Set(type, xWeight, yWeight, zWeight);
        }

        private static sbyte CreateNodeWeight(float baseWeight, float altWeight)
        {
            if (baseWeight <= 0) return 0;
            float t = (0 - baseWeight) / (altWeight - baseWeight);
            return (sbyte) math.clamp(t * 127, 1, 127);
        }
    }
}