using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace GigaChunker.Jobs
{
    public partial struct GigaChunkData
    {
        [BurstCompile]
        public struct ChunkDataManager : IJob, IDisposable
        {
            public readonly int RenderDistance;
            public readonly int RenderDiameter;
            public readonly int RenderVolume;

            private int3 _offset;
            private float3 _playerPos;
            private int _chunkSize;
            private NativeArray<GigaChunkData> _array;

            public ChunkDataManager(int chunkSize, int renderDistance)
            {
                RenderDistance = renderDistance;
                RenderDiameter = renderDistance * 2;
                RenderVolume = RenderDiameter * RenderDiameter * RenderDiameter;

                _offset = int3.zero;
                _playerPos = float3.zero;
                _chunkSize = chunkSize;
                _array = new(RenderVolume, Allocator.Persistent);

                int index = 0;
                for (int x = 0; x < RenderDiameter; x++)
                {
                    for (int y = 0; y < RenderDiameter; y++)
                    {
                        for (int z = 0; z < RenderDiameter; z++)
                        {
                            _array[index] = new(chunkSize, index, new(x, y, z));
                            index++;
                        }
                    }
                }
            }

            public void SetPlayerPosition(Vector3 playerPosition)
            {
                _playerPos = playerPosition;
                Vector3Int offset = Vector3Int.RoundToInt(playerPosition / _chunkSize);
                _offset = new(offset.x, offset.y, offset.z);
            }

            public unsafe void Execute()
            {
                if (!_array.IsCreated) throw new ObjectDisposedException("GigaDataCollection is not created.");
                long ptr = (long) (IntPtr) NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(_array);
                long dataSize = sizeof(GigaChunkData);
                long length = _array.Length * dataSize;
                for (long i = 0; i < length; i += dataSize)
                {
                    ref GigaChunkData data = ref *(GigaChunkData*) (ptr + i);

                    int3 newPos = data.Chunk3dIndex;
                    newPos -= _offset;
                    newPos.x %= RenderDiameter;
                    newPos.y %= RenderDiameter;
                    newPos.z %= RenderDiameter;
                    if (newPos.x < 0) newPos.x += RenderDiameter;
                    if (newPos.y < 0) newPos.y += RenderDiameter;
                    if (newPos.z < 0) newPos.z += RenderDiameter;

                    newPos -= new int3(RenderDistance, RenderDistance, RenderDistance);
                    newPos += _offset;
                    data.MoveChunk(newPos);
                    data._inRange = math.distance(data._worldCenter, _playerPos) <= RenderDistance * _chunkSize;
                }
            }

            public unsafe void ForEach(ForEachDelegate callback)
            {
                if (!_array.IsCreated) throw new ObjectDisposedException("GigaDataCollection is not created.");
                long ptr = (long) (IntPtr) NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(_array);
                long dataSize = sizeof(GigaChunkData);
                long length = _array.Length * dataSize;
                for (long i = 0; i < length; i += dataSize)
                {
                    ref GigaChunkData data = ref *(GigaChunkData*) (ptr + i);
                    callback(ref data, (int) i);
                }
            }

            public void Dispose()
            {
                if (!_array.IsCreated) return;
                _array.Dispose();
            }

            public delegate void ForEachDelegate(ref GigaChunkData data, int index);
        }
    }
}