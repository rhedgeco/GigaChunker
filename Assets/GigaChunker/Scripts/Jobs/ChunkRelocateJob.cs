using System;
using GigaChunker.DataTypes;
using GigaChunker.Extensions;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace GigaChunker.Jobs
{
    public partial struct GigaData
    {
        [BurstCompile]
        public struct ChunkRelocateJob : IJob
        {
            private int3 _offset;
            private float3 _playerPos;
            private GigaDataArray _chunks;

            public ChunkRelocateJob(GigaDataArray dataArray)
            {
                _offset = int3.zero;
                _playerPos = float3.zero;
                _chunks = dataArray;
            }

            public void Relocate(Vector3 generationCenter)
            {
                _playerPos = generationCenter;
                _offset = (_playerPos / _chunks.ChunkSize).RoundToInt();
                this.Run();
            }

            public unsafe void Execute()
            {
                NativeArray<GigaData> array = _chunks.RawArray;
                if (!array.IsCreated) throw new ObjectDisposedException("GigaDataCollection is not created.");
                long ptr = (long) array.GetUnsafePtr();
                long dataSize = sizeof(GigaData);
                long length = array.Length * dataSize;
                for (long i = 0; i < length; i += dataSize)
                {
                    ref GigaData data = ref *(GigaData*) (ptr + i);
                    RelocateChunk(ref data);
                }
            }
            
            private void RelocateChunk(ref GigaData data)
            {
                // create new pos and loop within array bounds
                int3 newPos = data.Chunk3dIndex;
                newPos -= _offset;
                newPos.x %= _chunks.RenderDiameter;
                newPos.y %= _chunks.RenderDiameter;
                newPos.z %= _chunks.RenderDiameter;
                if (newPos.x < 0) newPos.x += _chunks.RenderDiameter;
                if (newPos.y < 0) newPos.y += _chunks.RenderDiameter;
                if (newPos.z < 0) newPos.z += _chunks.RenderDiameter;

                // offset newPos to reach real position
                newPos += _offset;
                newPos -= new int3(_chunks.RenderDistance, _chunks.RenderDistance, _chunks.RenderDistance);
                
                // move chunk, and set in range value
                data.MoveChunk(newPos);
                data._inRange = math.distance(data._worldCenter, _playerPos) <= _chunks.WorldRenderDistance;
            }
        }
    }
}