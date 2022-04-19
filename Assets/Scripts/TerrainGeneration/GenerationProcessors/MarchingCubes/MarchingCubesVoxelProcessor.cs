using GigaChunker.DataTypes;
using TerrainGeneration.GenerationProcessors.MarchingCubes.DataTypes;
using Unity.Burst;
using Unity.Mathematics;

namespace TerrainGeneration.GenerationProcessors.MarchingCubes
{
    [BurstCompile]
    public static class MarchingCubesVoxelProcessor
    {
        [BurstCompile]
        public static void ProcessVoxel(ref MeshData meshData, in int3 voxelOffset, in VoxelCorners voxelCorners)
        {
            byte cubeIndex = 0;
            if (voxelCorners[0].Type > 0) cubeIndex |= 0b_00000001;
            if (voxelCorners[1].Type > 0) cubeIndex |= 0b_00000010;
            if (voxelCorners[2].Type > 0) cubeIndex |= 0b_00000100;
            if (voxelCorners[3].Type > 0) cubeIndex |= 0b_00001000;
            if (voxelCorners[4].Type > 0) cubeIndex |= 0b_00010000;
            if (voxelCorners[5].Type > 0) cubeIndex |= 0b_00100000;
            if (voxelCorners[6].Type > 0) cubeIndex |= 0b_01000000;
            if (voxelCorners[7].Type > 0) cubeIndex |= 0b_10000000;
            if (cubeIndex is 0 or 255) return;

            for (int i = 0; i < 16; i += 3)
            {
                int index = cubeIndex * 16 + i;
                if (MarchTables.Triangulation[index] == -1) return;

                ref CornerRay r1 = ref MarchTables.CornerRayFromEdge[MarchTables.Triangulation[index + 0]];
                ref CornerRay r2 = ref MarchTables.CornerRayFromEdge[MarchTables.Triangulation[index + 1]];
                ref CornerRay r3 = ref MarchTables.CornerRayFromEdge[MarchTables.Triangulation[index + 2]];

                float w1 = voxelCorners[r1.Corner].GetWeight(r1.Direction);
                float w2 = voxelCorners[r2.Corner].GetWeight(r2.Direction);
                float w3 = voxelCorners[r3.Corner].GetWeight(r3.Direction);

                float3 v1 = r1.GetPoint(w1);
                float3 v2 = r2.GetPoint(w2);
                float3 v3 = r3.GetPoint(w3);
                float3 normal = math.cross(v2 - v1, v3 - v1);
                
                meshData.AddIndex(meshData.VertexCount);
                meshData.AddVertex(v1 + voxelOffset, normal);
                
                meshData.AddIndex(meshData.VertexCount);
                meshData.AddVertex(v2 + voxelOffset, normal);
                
                meshData.AddIndex(meshData.VertexCount);
                meshData.AddVertex(v3 + voxelOffset, normal);
            }
        }
    }
}