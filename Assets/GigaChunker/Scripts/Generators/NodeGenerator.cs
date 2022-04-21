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

        public void Process(ref GigaChunkNodes nodes, int3 position)
        {
            new NodeGeneratorJob(in nodes, in position, in _processor).Run();
        }

        public delegate void ProcessNode(ref GigaNode node, in int3 nodePosition);

        [BurstCompile]
        private struct NodeGeneratorJob : IJob
        {
            private GigaChunkNodes _nodes;
            private readonly int3 _position;
            private readonly FunctionPointer<ProcessNode> _processor;

            public NodeGeneratorJob(in GigaChunkNodes nodes, in int3 position,
                in FunctionPointer<ProcessNode> processor)
            {
                _nodes = nodes;
                _position = position;
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
                int cSize = _nodes.ChunkSize;
                for (int z = 0; z < cSize; z++)
                {
                    for (int y = 0; y < cSize; y++)
                    {
                        for (int x = 0; x < cSize; x++)
                        {
                            index3d.Set(x, y, z, in _position);
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