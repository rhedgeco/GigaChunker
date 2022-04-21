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
    public class VoxelGenerator
    {
        private readonly FunctionPointer<ProcessVoxel> _processor;

        public VoxelGenerator(ProcessVoxel processor)
        {
            _processor = BurstCompiler.CompileFunctionPointer(processor);
        }

        public void ProcessNow(in GigaChunkNodes nodes, ref MeshData meshData)
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

                int chunkSize = _nodes.ChunkSize;
                int voxelSize = chunkSize - 1;
                long offsetX = dataSize;
                long offsetY = chunkSize * dataSize;
                long offsetZ = chunkSize * chunkSize * dataSize;

                long dataIndex = 0;
                float3 voxelOffset = int3.zero;
                VoxelCorners voxelCorners = new();
                for (int x = 0; x < voxelSize; x++)
                {
                    for (int y = 0; y < voxelSize; y++)
                    {
                        for (int z = 0; z < voxelSize; z++)
                        {
                            voxelOffset.Set(x, y, z);
                            voxelCorners.Set(
                                (GigaNode*) (ptr + (dataSize * ((x + 0L) + (y + 0L) * chunkSize + (z + 0L) * chunkSize * chunkSize))),
                                (GigaNode*) (ptr + (dataSize * ((x + 0L) + (y + 0L) * chunkSize + (z + 1L) * chunkSize * chunkSize))),
                                (GigaNode*) (ptr + (dataSize * ((x + 1L) + (y + 0L) * chunkSize + (z + 1L) * chunkSize * chunkSize))),
                                (GigaNode*) (ptr + (dataSize * ((x + 1L) + (y + 0L) * chunkSize + (z + 0L) * chunkSize * chunkSize))),
                                (GigaNode*) (ptr + (dataSize * ((x + 0L) + (y + 1L) * chunkSize + (z + 0L) * chunkSize * chunkSize))),
                                (GigaNode*) (ptr + (dataSize * ((x + 0L) + (y + 1L) * chunkSize + (z + 1L) * chunkSize * chunkSize))),
                                (GigaNode*) (ptr + (dataSize * ((x + 1L) + (y + 1L) * chunkSize + (z + 1L) * chunkSize * chunkSize))),
                                (GigaNode*) (ptr + (dataSize * ((x + 1L) + (y + 1L) * chunkSize + (z + 0L) * chunkSize * chunkSize)))
                            );
                            _processor.Invoke(ref _meshData, in voxelOffset, in voxelCorners);
                            dataIndex += dataSize;
                        }

                        dataIndex += offsetX;
                    }

                    dataIndex += offsetY;
                }
            }
        }
    }
}