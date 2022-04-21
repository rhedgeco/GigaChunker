using System;
using GigaChunker.DataTypes;
using GigaChunker.Extensions;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace GigaChunker.Generators
{
    public class VoxelMarcher
    {
        private readonly FunctionPointer<ProcessVoxel> _processor;

        public VoxelMarcher(ProcessVoxel processor)
        {
            _processor = BurstCompiler.CompileFunctionPointer(processor);
        }

        public void MarchNodes(in GigaChunkNodes nodes, ref MeshData meshData)
        {
            new MeshGeneratorJob(in nodes, ref meshData, in _processor).Run();
        }

        public delegate void ProcessVoxel(ref MeshData meshData, in float3 voxelOffset, in VoxelCorners voxelCorners);

        [BurstCompile]
        private struct MeshGeneratorJob : IJob
        {
            private GigaChunkNodes _nodes;
            private MeshData _meshData;
            private readonly FunctionPointer<ProcessVoxel> _processor;

            public MeshGeneratorJob(in GigaChunkNodes nodes, ref MeshData meshData,
                in FunctionPointer<ProcessVoxel> processor)
            {
                _nodes = nodes;
                _meshData = meshData;
                _processor = processor;
            }

            public unsafe void Execute()
            {
                NativeArray<GigaNode> nodeArray = GigaChunkNodes.ExtractRawArray(ref _nodes);
                if (!nodeArray.IsCreated) throw new ObjectDisposedException("GigaDataCollection is not created.");
                long ptr = (long) nodeArray.GetUnsafePtr();
                long dataSize = sizeof(GigaNode);

                int cSize = _nodes.ChunkSize;
                int vSize = cSize - 1;
                float3 voxelOffset = int3.zero;
                VoxelCorners voxelCorners = new();
                for (int x = 0; x < vSize; x++)
                {
                    for (int y = 0; y < vSize; y++)
                    {
                        for (int z = 0; z < vSize; z++)
                        {
                            voxelOffset.Set(x, y, z);
                            voxelCorners.Set(
                                (GigaNode*) (ptr + (x + 0 + (y + 0) * cSize + (z + 0) * cSize * cSize) * dataSize),
                                (GigaNode*) (ptr + (x + 0 + (y + 0) * cSize + (z + 1) * cSize * cSize) * dataSize),
                                (GigaNode*) (ptr + (x + 1 + (y + 0) * cSize + (z + 1) * cSize * cSize) * dataSize),
                                (GigaNode*) (ptr + (x + 1 + (y + 0) * cSize + (z + 0) * cSize * cSize) * dataSize),
                                (GigaNode*) (ptr + (x + 0 + (y + 1) * cSize + (z + 0) * cSize * cSize) * dataSize),
                                (GigaNode*) (ptr + (x + 0 + (y + 1) * cSize + (z + 1) * cSize * cSize) * dataSize),
                                (GigaNode*) (ptr + (x + 1 + (y + 1) * cSize + (z + 1) * cSize * cSize) * dataSize),
                                (GigaNode*) (ptr + (x + 1 + (y + 1) * cSize + (z + 0) * cSize * cSize) * dataSize)
                            );
                            _processor.Invoke(ref _meshData, in voxelOffset, in voxelCorners);
                        }
                    }
                }
            }
        }
    }
}