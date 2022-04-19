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
    public class NodeGenerator
    {
        private readonly FunctionPointer<ProcessNode> _processor;

        public NodeGenerator(ProcessNode processor)
        {
            _processor = BurstCompiler.CompileFunctionPointer(processor);
        }
        
        public void ProcessNow(ref GigaChunkNodes nodes)
        {
            new NodeGeneratorJob(in nodes, in _processor).Run();
        }

        public delegate void ProcessNode(ref GigaNode node, in int3 nodePosition);

        [BurstCompile]
        private struct NodeGeneratorJob : IJob
        {
            private GigaChunkNodes _nodes;
            private readonly FunctionPointer<ProcessNode> _processor;

            public NodeGeneratorJob(in GigaChunkNodes nodes, in FunctionPointer<ProcessNode> processor)
            {
                _nodes = nodes;
                _processor = processor;
            }

            public unsafe void Execute()
            {
                NativeArray<GigaNode> array = GigaChunkNodes.ExtractRawArray(ref _nodes);
                if (!array.IsCreated) throw new ObjectDisposedException("GigaDataCollection is not created.");
                long ptr = (long) array.GetUnsafePtr();
                long dataSize = sizeof(GigaNode);

                long dataIndex = 0;
                int3 index3d = int3.zero;
                for (int x = 0; x < _nodes.ChunkSize; x++)
                {
                    for (int y = 0; y < _nodes.ChunkSize; y++)
                    {
                        for (int z = 0; z < _nodes.ChunkSize; z++)
                        {
                            index3d.Set(x, y, z);
                            ref GigaNode node = ref *(GigaNode*) (ptr + dataIndex);
                            _processor.Invoke(ref node, in index3d);
                            dataIndex += dataSize;
                        }
                    }
                }
            }
        }
    }
}