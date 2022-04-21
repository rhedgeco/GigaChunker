using System;
using GigaChunker.DataTypes;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace GigaChunker.Generators
{
    public static class BaseNodeGenerator
    {
        public static void Process(ref GigaChunkNodes nodes, int3 position)
        {
            NodeGeneratorJob job = new(in nodes, in position);
            job.Run();
            job.Dispose();
        }

        [BurstCompile(OptimizeFor = OptimizeFor.Performance)]
        private struct NodeGeneratorJob : IJob, IDisposable
        {
            private const float CNoiseScale = 0.72354f * 0.05f;

            private GigaChunkNodes _nodes;
            private readonly int3 _position;
            private NativeArray<float> _tempWeights;

            public NodeGeneratorJob(in GigaChunkNodes nodes, in int3 position)
            {
                _nodes = nodes;
                _position = position;
                _tempWeights = new(nodes.ChunkSize * nodes.ChunkSize * nodes.ChunkSize, Allocator.Persistent);
            }

            public unsafe void Execute()
            {
                NativeArray<GigaNode> array = GigaChunkNodes.ExtractRawArray(ref _nodes);
                if (!array.IsCreated) return;
                long nodesPointer = (long) array.GetUnsafePtr();
                long weightsPointer = (long) _tempWeights.GetUnsafePtr();
                long nodeSizeOf = sizeof(GigaNode);
                const long weightSizeOf = sizeof(float);

                int chunkSize = _nodes.ChunkSize;
                int chunkSizeSquared = chunkSize * chunkSize;
                int voxelSize = chunkSize - 1;
                for (int z = 0; z <= voxelSize; z++)
                {
                    for (int y = 0; y <= voxelSize; y++)
                    {
                        for (int x = 0; x <= voxelSize; x++)
                        {
                            // create indices
                            long i = x + y * chunkSize + z * chunkSizeSquared;
                            long xi = x + 1 + y * chunkSize + z * chunkSizeSquared;
                            long yi = x + (y + 1) * chunkSize + z * chunkSizeSquared;
                            long zi = x + y * chunkSize + (z + 1) * chunkSizeSquared;

                            // get weight pointers
                            float* weight = (float*) (weightsPointer + i * weightSizeOf);
                            float* xWeight = (float*) (weightsPointer + xi * weightSizeOf);
                            float* yWeight = (float*) (weightsPointer + yi * weightSizeOf);
                            float* zWeight = (float*) (weightsPointer + zi * weightSizeOf);

                            // populate necessary weights
                            if (Hint.Unlikely(x == 0 && y == 0 && z == 0))
                                *weight = noise.cnoise((_position + new float3(x, y, z)) * CNoiseScale);
                            if (Hint.Unlikely( /*y == 0 && */x != voxelSize))
                                *xWeight = noise.cnoise((_position + new float3(x + 1, y, z)) * CNoiseScale);
                            if (Hint.Unlikely( /*z == 0 && */y != voxelSize))
                                *yWeight = noise.cnoise((_position + new float3(x, y + 1, z)) * CNoiseScale);
                            if (Hint.Unlikely(z != voxelSize))
                                *zWeight = noise.cnoise((_position + new float3(x, y, z + 1)) * CNoiseScale);

                            // get node pointers
                            GigaNode* node = (GigaNode*) (nodesPointer + i * nodeSizeOf);
                            GigaNode* xNode = (GigaNode*) (nodesPointer + xi * nodeSizeOf);
                            GigaNode* yNode = (GigaNode*) (nodesPointer + yi * nodeSizeOf);
                            GigaNode* zNode = (GigaNode*) (nodesPointer + zi * nodeSizeOf);

                            // create weight values
                            sbyte xByteWeight = *weight > 0 ? CreateNodeWeight(weight, xWeight) : (sbyte) 0;
                            sbyte yByteWeight = *weight > 0 ? CreateNodeWeight(weight, yWeight) : (sbyte) 0;
                            sbyte zByteWeight = *weight > 0 ? CreateNodeWeight(weight, zWeight) : (sbyte) 0;
                            sbyte flipXByteWeight = *xWeight > 0 ? CreateNodeWeight(xWeight, weight) : (sbyte) 0;
                            sbyte flipYByteWeight = *yWeight > 0 ? CreateNodeWeight(yWeight, weight) : (sbyte) 0;
                            sbyte flipZByteWeight = *zWeight > 0 ? CreateNodeWeight(zWeight, weight) : (sbyte) 0;

                            // set node values
                            node->Type = (byte) (*weight > 0 ? 1 : 0);
                            node->XWeight.Positive = xByteWeight;
                            node->YWeight.Positive = yByteWeight;
                            node->ZWeight.Positive = zByteWeight;
                            
                            if (Hint.Unlikely(x != voxelSize)) xNode->XWeight.Negative = flipXByteWeight;
                            if (Hint.Unlikely(y != voxelSize)) yNode->YWeight.Negative = flipYByteWeight;
                            if (Hint.Unlikely(z != voxelSize)) zNode->ZWeight.Negative = flipZByteWeight;
                        }
                    }
                }
            }

            private static unsafe sbyte CreateNodeWeight(float* baseWeight, float* altWeight)
            {
                if (*baseWeight <= 0) return 0;
                float t = (0 - *baseWeight) / (*altWeight - *baseWeight);
                return (sbyte) math.clamp(t * 127, 0, 127);
            }

            public void Dispose()
            {
                _tempWeights.Dispose();
            }
        }
    }
}