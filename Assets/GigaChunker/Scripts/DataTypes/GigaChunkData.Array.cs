using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace GigaChunker.DataTypes
{
    public partial struct GigaChunkData
    {
        public struct Array
        {
            private NativeArray<GigaChunkData> _array;

            public readonly int ChunkSize;
            public readonly int WorldRenderDistance;
            public readonly int RenderDistance;
            public readonly int RenderDiameter;
            public readonly int RenderVolume;
        
            public bool IsCreated => _array.IsCreated;
            public NativeArray<GigaChunkData> RawArray => _array;

            public Array(int chunkSize, int renderDistance)
            {
                ChunkSize = chunkSize;
                RenderDistance = renderDistance;
                RenderDiameter = renderDistance * 2;
                WorldRenderDistance = renderDistance * chunkSize;
                RenderVolume = RenderDiameter * RenderDiameter * RenderDiameter;
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

            public void Relocate(Vector3 centerPosition) => new RelocateJob(this, centerPosition).Run();
            
            public unsafe void ForEach(ForEachDelegate callback)
            {
                if (!_array.IsCreated) throw new ObjectDisposedException("GigaDataCollection is not created.");
                long ptr = (long) _array.GetUnsafePtr();
                long dataSize = sizeof(GigaChunkData);
                long length = _array.Length * dataSize;
                for (long i = 0; i < length; i += dataSize)
                {
                    ref GigaChunkData data = ref *(GigaChunkData*) (ptr + i);
                    callback(ref data);
                }
            }
        
            public void Dispose()
            {
                if (!_array.IsCreated) return;
                _array.Dispose();
            }

            public delegate void ForEachDelegate(ref GigaChunkData data);
        }
    }
}