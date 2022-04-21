using System;
using GigaChunker.DataTypes;
using GigaChunker.Extensions;
using GigaChunker.Generators.MarchData;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace GigaChunker.Generators
{
    public static class VoxelMarcher
    {
        public static void MarchNodes(in GigaChunkNodes nodes, ref MeshData meshData)
        {
            new MeshGeneratorJob(in nodes, ref meshData).Run();
        }

        [BurstCompile(OptimizeFor = OptimizeFor.Performance, FloatMode = FloatMode.Fast,
            FloatPrecision = FloatPrecision.Low)]
        private struct MeshGeneratorJob : IJob
        {
            private GigaChunkNodes _nodes;
            private MeshData _meshData;

            public MeshGeneratorJob(in GigaChunkNodes nodes, ref MeshData meshData)
            {
                _nodes = nodes;
                _meshData = meshData;
            }

            [SkipLocalsInit]
            public unsafe void Execute()
            {
                NativeArray<GigaNode> nodeArray = GigaChunkNodes.ExtractRawArray(ref _nodes);
                if (!nodeArray.IsCreated) throw new ObjectDisposedException("GigaDataCollection is not created.");
                long ptr = (long) nodeArray.GetUnsafePtr();
                long dataSize = sizeof(GigaNode);

                GigaNode** cNode = stackalloc GigaNode*[8];

                int cSize = _nodes.ChunkSize;
                int cSizeSquare = cSize * cSize;
                int vSize = cSize - 1;
                float3 voxelOffset = int3.zero;
                for (int z = 0; z < vSize; z++)
                {
                    for (int y = 0; y < vSize; y++)
                    {
                        for (int x = 0; x < vSize; x++)
                        {
                            voxelOffset.Set(x, y, z);

                            cNode[0] = (GigaNode*) (ptr + (x + 0 + (y + 0) * cSize + (z + 0) * cSizeSquare) * dataSize);
                            cNode[1] = (GigaNode*) (ptr + (x + 0 + (y + 0) * cSize + (z + 1) * cSizeSquare) * dataSize);
                            cNode[2] = (GigaNode*) (ptr + (x + 1 + (y + 0) * cSize + (z + 1) * cSizeSquare) * dataSize);
                            cNode[3] = (GigaNode*) (ptr + (x + 1 + (y + 0) * cSize + (z + 0) * cSizeSquare) * dataSize);
                            cNode[4] = (GigaNode*) (ptr + (x + 0 + (y + 1) * cSize + (z + 0) * cSizeSquare) * dataSize);
                            cNode[5] = (GigaNode*) (ptr + (x + 0 + (y + 1) * cSize + (z + 1) * cSizeSquare) * dataSize);
                            cNode[6] = (GigaNode*) (ptr + (x + 1 + (y + 1) * cSize + (z + 1) * cSizeSquare) * dataSize);
                            cNode[7] = (GigaNode*) (ptr + (x + 1 + (y + 1) * cSize + (z + 0) * cSizeSquare) * dataSize);

                            byte cubeIndex = 0;
                            if (cNode[0]->Type > 0) cubeIndex |= 0b_00000001;
                            if (cNode[1]->Type > 0) cubeIndex |= 0b_00000010;
                            if (cNode[2]->Type > 0) cubeIndex |= 0b_00000100;
                            if (cNode[3]->Type > 0) cubeIndex |= 0b_00001000;
                            if (cNode[4]->Type > 0) cubeIndex |= 0b_00010000;
                            if (cNode[5]->Type > 0) cubeIndex |= 0b_00100000;
                            if (cNode[6]->Type > 0) cubeIndex |= 0b_01000000;
                            if (cNode[7]->Type > 0) cubeIndex |= 0b_10000000;
                            if (cubeIndex is 0 or 255) continue;

                            for (int i = 0; i < 16; i += 3)
                            {
                                int index = cubeIndex * 16 + i;
                                if (MarchTables.Triangulation[index] == -1) break;

                                ref CornerRay r1 =
                                    ref MarchTables.CornerRayFromEdge[MarchTables.Triangulation[index + 0]];
                                ref CornerRay r2 =
                                    ref MarchTables.CornerRayFromEdge[MarchTables.Triangulation[index + 1]];
                                ref CornerRay r3 =
                                    ref MarchTables.CornerRayFromEdge[MarchTables.Triangulation[index + 2]];

                                float w1 = cNode[r1.Corner]->GetWeight(r1.Direction);
                                float w2 = cNode[r2.Corner]->GetWeight(r2.Direction);
                                float w3 = cNode[r3.Corner]->GetWeight(r3.Direction);

                                float3 v1 = r1.GetPoint(w1);
                                float3 v2 = r2.GetPoint(w2);
                                float3 v3 = r3.GetPoint(w3);
                                float3 normal = math.cross(v2 - v1, v3 - v1);

                                _meshData.AddIndex(_meshData.VertexCount);
                                _meshData.AddVertex(v1 + voxelOffset, normal);

                                _meshData.AddIndex(_meshData.VertexCount);
                                _meshData.AddVertex(v2 + voxelOffset, normal);

                                _meshData.AddIndex(_meshData.VertexCount);
                                _meshData.AddVertex(v3 + voxelOffset, normal);
                            }
                        }
                    }
                }
            }
        }
    }
}