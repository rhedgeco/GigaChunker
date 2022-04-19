using System;
using GigaChunker.DataTypes;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace GigaChunker.Generators
{
    public class MeshGenerator
    {
        private static readonly VertexAttributeDescriptor[] VertexAttributes =
        {
            new(VertexAttribute.Position, stream: 0),
            new(VertexAttribute.Normal, stream: 1)
        };
        
        private FunctionPointer<ProcessVoxel> _processor;

        public MeshGenerator(int initialBufferSize, ProcessVoxel processor)
        {
            _processor = BurstCompiler.CompileFunctionPointer(processor);
        }

        public void ProcessNow(in GigaChunkNodes nodes,
            ref NativeList<float3> vertexBuffer, ref NativeList<float3> normalBuffer, ref NativeList<uint> indexBuffer)
        {
            new MeshGeneratorJob(in nodes, ref vertexBuffer, ref normalBuffer, ref indexBuffer, in _processor).Run();
        }

        public static Mesh CreateMesh(ref NativeList<float3> vertexBuffer, ref NativeList<float3> normalBuffer,
            ref NativeList<uint> indexBuffer, in Bounds bounds)
        {
            Mesh mesh = new();
            int vertexCount = vertexBuffer.Length;
            int indexCount = indexBuffer.Length;
            mesh.SetVertexBufferParams(vertexCount, VertexAttributes);
            mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt32);

            mesh.subMeshCount = 1;
            SubMeshDescriptor descriptor = new(0, vertexCount);
            mesh.SetVertexBufferData(vertexBuffer.AsArray(), 0, 0, vertexCount);
            mesh.SetVertexBufferData(normalBuffer.AsArray(), 0, 0, vertexCount, 1);
            mesh.SetIndexBufferData(indexBuffer.AsArray(), 0, 0, indexCount, MeshUpdateFlags.DontValidateIndices);
            mesh.SetSubMesh(0, descriptor, MeshUpdateFlags.DontRecalculateBounds);
            mesh.bounds = bounds;
            mesh.UploadMeshData(true);

            return mesh;
        }

        public delegate void ProcessVoxel(
            ref NativeList<float3> vertexBuffer, ref NativeList<float3> normalBuffer, ref NativeList<uint> indexBuffer,
            in GigaNode node0, in GigaNode node1, in GigaNode node2, in GigaNode node3,
            in GigaNode node4, in GigaNode node5, in GigaNode node6, in GigaNode node7
        );

        private struct MeshGeneratorJob : IJob
        {
            private GigaChunkNodes _nodes;
            private NativeList<float3> _vertexBuffer;
            private NativeList<float3> _normalBuffer;
            private NativeList<uint> _indexBuffer;
            private FunctionPointer<ProcessVoxel> _processor;

            public MeshGeneratorJob(in GigaChunkNodes nodes,
                ref NativeList<float3> vertexBuffer,
                ref NativeList<float3> normalBuffer,
                ref NativeList<uint> indexBuffer,
                in FunctionPointer<ProcessVoxel> processor)
            {
                _nodes = nodes;
                _vertexBuffer = vertexBuffer;
                _normalBuffer = normalBuffer;
                _indexBuffer = indexBuffer;
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
                for (int x = 0; x < _nodes.ChunkSize - 1; x++)
                {
                    for (int y = 0; y < _nodes.ChunkSize - 1; y++)
                    {
                        for (int z = 0; z < _nodes.ChunkSize - 1; z++)
                        {
                            ref GigaNode node0 = ref *(GigaNode*) (ptr + dataIndex);
                            ref GigaNode node1 = ref *(GigaNode*) (ptr + dataIndex + offsetX);
                            ref GigaNode node2 = ref *(GigaNode*) (ptr + dataIndex + offsetZ);
                            ref GigaNode node3 = ref *(GigaNode*) (ptr + dataIndex + offsetX + offsetZ);
                            ref GigaNode node4 = ref *(GigaNode*) (ptr + dataIndex + offsetY);
                            ref GigaNode node5 = ref *(GigaNode*) (ptr + dataIndex + offsetY + offsetX);
                            ref GigaNode node6 = ref *(GigaNode*) (ptr + dataIndex + offsetY + offsetZ);
                            ref GigaNode node7 = ref *(GigaNode*) (ptr + dataIndex + offsetY + offsetX + offsetZ);
                            _processor.Invoke(ref _vertexBuffer, ref _normalBuffer, ref _indexBuffer,
                                in node0, in node1, in node2, in node3, in node4, in node5, in node6, in node7);
                            dataIndex += dataSize;
                        }
                    }
                }
            }
        }
    }
}