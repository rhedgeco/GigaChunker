using System;
using GigaChunker.DataTypes;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace GigaChunker.Generators
{
    public static class NodeGenerator
    {
        public static void Process(ref GigaChunkNodes nodes, int3 position)
        {
            new NodeGeneratorJob(in nodes, in position).Run();
        }

        [BurstCompile(OptimizeFor = OptimizeFor.Performance, FloatMode = FloatMode.Fast,
            FloatPrecision = FloatPrecision.Low)]
        private struct NodeGeneratorJob : IJob
        {
            private const float CNoiseOffset = 0.72354f;
            private const float CNoiseScale = 0.05f;

            private GigaChunkNodes _nodes;
            private readonly int3 _position;

            public NodeGeneratorJob(in GigaChunkNodes nodes, in int3 position)
            {
                _nodes = nodes;
                _position = position;
            }

            public unsafe void Execute()
            {
                NativeArray<GigaNode> array = GigaChunkNodes.ExtractRawArray(ref _nodes);
                if (!array.IsCreated) return;
                long ptr = (long) array.GetUnsafePtr();
                long dataSize = sizeof(GigaNode);

                long dataIndex = 0;
                int cSize = _nodes.ChunkSize;
                for (int z = 0; z < cSize; z++)
                {
                    for (int y = 0; y < cSize; y++)
                    {
                        for (int x = 0; x < cSize; x++)
                        {
                            ref GigaNode node = ref *(GigaNode*) (ptr + dataIndex);

                            float3 floatPos = _position + new int3(x, y, z);
                            float weight = noise.cnoise(floatPos * CNoiseOffset * CNoiseScale);
                            sbyte bWeight = (sbyte) (weight <= 0 ? 0 : 64);
                            byte type = (byte) (bWeight == 0 ? 0 : 1);
                            node.Set(type, new(bWeight, bWeight), new(bWeight, bWeight), new(bWeight, bWeight));

                            dataIndex += dataSize;
                        }
                    }
                }
            }
        }
    }
}