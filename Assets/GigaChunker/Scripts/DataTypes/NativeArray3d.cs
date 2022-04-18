using System;
using GigaChunker.Extensions;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace GigaChunker.DataTypes
{
    public struct NativeArray3d<T> : IDisposable where T : unmanaged
    {
        private int _sideWidth;
        private int _doubleSideWidth;
        private NativeArray<T> _rawArray;
        private long _pointer;
        private long _dataSize;

        public bool IsCreated => _rawArray.IsCreated;

        public unsafe NativeArray3d(int sideWidth, Allocator allocator)
        {
            _sideWidth = sideWidth;
            _doubleSideWidth = sideWidth * sideWidth;
            _rawArray = new(sideWidth * sideWidth * sideWidth, allocator);
            _pointer = (long) _rawArray.GetUnsafePtr();
            _dataSize = sizeof(T);
        }

        public unsafe void ForEach(ForEachDelegate<T> callback)
        {
            //if (!callback.IsCreated) throw new ObjectDisposedException("Function callback is disposed.");
            if (!_rawArray.IsCreated) throw new ObjectDisposedException("Native array is disposed.");

            long dataIndex = 0;
            int3 index3d = int3.zero;
            for (int x = 0; x < _sideWidth; x++)
            {
                for (int y = 0; y < _sideWidth; y++)
                {
                    for (int z = 0; z < _sideWidth; z++)
                    {
                        index3d.Set(x, y, z);
                        ref T item = ref *(T*) (_pointer + dataIndex);
                        callback.Invoke(ref item, in index3d);
                        dataIndex += _dataSize;
                    }
                }
            }
        }

        public void Dispose() => _rawArray.Dispose();

        public delegate void ForEachDelegate<TD>(ref TD item, in int3 index3d);
    }
}