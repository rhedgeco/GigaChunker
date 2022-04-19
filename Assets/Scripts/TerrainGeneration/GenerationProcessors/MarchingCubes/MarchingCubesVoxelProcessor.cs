using GigaChunker.DataTypes;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace TerrainGeneration.GenerationProcessors.MarchingCubes
{
    [BurstCompile]
    public static class MarchingCubesVoxelProcessor
    {
        [BurstCompile]
        public static void ProcessVoxel(
            ref NativeList<float3> vertexBuffer, ref NativeList<float3> normalBuffer, ref NativeList<uint> indexBuffer,
            in GigaNode node0, in GigaNode node1, in GigaNode node2, in GigaNode node3,
            in GigaNode node4, in GigaNode node5, in GigaNode node6, in GigaNode node7
        )
        {

        }
    }
}