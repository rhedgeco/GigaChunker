using System;
using GigaChunker.Extensions;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace GigaChunker.DataTypes
{
    public partial struct GigaChunkData
    {
        [BurstCompile]
        private struct RelocateJob : IJob
        {
            private Array _array;
            private readonly float3 _centerPos;

            public RelocateJob(Array array, float3 centerPos)
            {
                _array = array;
                _centerPos = centerPos;
            }

            public unsafe void Execute()
            {
                NativeArray<GigaChunkData> array = Array.ExtractRawArray(ref _array);
                if (!array.IsCreated) throw new ObjectDisposedException("GigaDataCollection is not created.");
                long ptr = (long) array.GetUnsafePtr();
                long dataSize = sizeof(GigaChunkData);
                long length = array.Length * dataSize;
                int3 chunksOffset = (_centerPos / _array.ChunkSize).RoundToInt();
                for (long i = 0; i < length; i += dataSize)
                {
                    ref GigaChunkData data = ref *(GigaChunkData*) (ptr + i);

                    // create new pos and loop within array bounds
                    int3 newPos = data.Chunk3dIndex;
                    newPos -= chunksOffset;
                    newPos.x %= _array.RenderDiameter;
                    newPos.y %= _array.RenderDiameter;
                    newPos.z %= _array.RenderDiameter;
                    if (newPos.x < 0) newPos.x += _array.RenderDiameter;
                    if (newPos.y < 0) newPos.y += _array.RenderDiameter;
                    if (newPos.z < 0) newPos.z += _array.RenderDiameter;

                    // offset newPos to reach real position
                    newPos += chunksOffset;
                    newPos -= new int3(_array.RenderDistance, _array.RenderDistance, _array.RenderDistance);
                
                    // move chunk, and set in range value
                    data.MoveChunk(newPos);
                    data._inRange = math.distance(data._worldCenter, _centerPos) <= _array.WorldRenderDistance;
                }
            }
        }
    }
}