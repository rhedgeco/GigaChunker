using GigaChunker.DataTypes;
using Unity.Burst;
using Unity.Mathematics;

namespace TerrainGeneration.GenerationProcessors
{
    [BurstCompile]
    public static class SimpleNodeProcessors
    {
        private const float CNoiseOffset = 0.72354f;
        private const float CNoiseScale = 0.05f;

        [BurstCompile]
        public static void SimpleFlatGround(ref GigaNode node, in int3 position)
        {
            if (position.y <= 2) node.Set(1, new(64, 64), new(64, 64), new(64, 64));
            else node.Set(0, AxisWeights.Zero, AxisWeights.Zero, AxisWeights.Zero);
        }

        [BurstCompile]
        public static void Simple3dNoise(ref GigaNode node, in int3 position)
        {
            float3 floatPos = position;
            float weight = noise.cnoise(floatPos * CNoiseOffset * CNoiseScale);
            sbyte bWeight = (sbyte) (weight <= 0 ? 0 : 64);
            byte type = (byte) (bWeight == 0 ? 0 : 1);
            node.Set(type, new(bWeight,bWeight), new(bWeight,bWeight), new(bWeight,bWeight));
        }
    }
}