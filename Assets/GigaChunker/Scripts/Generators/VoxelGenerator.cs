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

        public delegate void ProcessVoxel(ref MeshData meshData, in int3 voxelOffset, in VoxelCorners voxelCorners);

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

                long offsetX = 1;
                long offsetY = _nodes.ChunkSize;
                long offsetZ = _nodes.ChunkSize * _nodes.ChunkSize;

                long dataIndex = 0;
                int3 voxelOffset = int3.zero;
                VoxelCorners voxelCorners = new();
                for (int x = 0; x < _nodes.ChunkSize - 1; x++)
                {
                    for (int y = 0; y < _nodes.ChunkSize - 1; y++)
                    {
                        for (int z = 0; z < _nodes.ChunkSize - 1; z++)
                        {
                            voxelOffset.Set(x, y, z);
                            voxelCorners.Set(
                                (GigaNode*) (ptr + dataIndex),
                                (GigaNode*) (ptr + dataIndex + offsetX),
                                (GigaNode*) (ptr + dataIndex + offsetZ),
                                (GigaNode*) (ptr + dataIndex + offsetX + offsetZ),
                                (GigaNode*) (ptr + dataIndex + offsetY),
                                (GigaNode*) (ptr + dataIndex + offsetY + offsetX),
                                (GigaNode*) (ptr + dataIndex + offsetY + offsetZ),
                                (GigaNode*) (ptr + dataIndex + offsetY + offsetX + offsetZ)
                            );
                            _processor.Invoke(ref _meshData, in voxelOffset, in voxelCorners);
                            dataIndex += dataSize;
                        }
                    }
                }
            }
        }
    }
}