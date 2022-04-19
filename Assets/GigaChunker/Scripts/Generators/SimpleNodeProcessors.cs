using GigaChunker.DataTypes;
using Unity.Burst;
using Unity.Mathematics;

namespace GigaChunker.Generators
{
    [BurstCompile]
    public static class SimpleNodeProcessors
    {
        [BurstCompile]
        public static void FlatGround(ref GigaNode node, in int3 position)
        {
            if (position.y <= 0) node.Set(0, AxisWeights.Mid, AxisWeights.Mid, AxisWeights.Mid);
            else node.Set(0, AxisWeights.Min, AxisWeights.Min, AxisWeights.Min);
        }
    }
}