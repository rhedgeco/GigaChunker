using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace GigaChunker.DataTypes
{
    public readonly unsafe struct MeshData : IDisposable
    {
        private static readonly VertexAttributeDescriptor[] VertexAttributes =
        {
            new(VertexAttribute.Position, stream: 0),
            new(VertexAttribute.Normal, stream: 1)
        };

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private readonly AtomicSafetyHandle _safetyHandle;
#endif

        private readonly uint _bufferSize;

        [NativeDisableUnsafePtrRestriction]
        private readonly uint* _indexCount;

        [NativeDisableUnsafePtrRestriction]
        private readonly uint* _vertexCount;

        [NativeDisableUnsafePtrRestriction]
        private readonly float3* _vertexBuffer;

        [NativeDisableUnsafePtrRestriction]
        private readonly float3* _normalBuffer;

        [NativeDisableUnsafePtrRestriction]
        private readonly uint* _indexBuffer;

        public uint VertexCount => *_vertexCount;

        public MeshData(int bufferSize)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            _safetyHandle = AtomicSafetyHandle.Create();
#endif
            _bufferSize = (uint) bufferSize;
            _vertexCount = (uint*) UnsafeUtility.Malloc(UnsafeUtility.SizeOf<uint>(),
                UnsafeUtility.AlignOf<uint>(), Allocator.Persistent);
            _indexCount = (uint*) UnsafeUtility.Malloc(UnsafeUtility.SizeOf<uint>(),
                UnsafeUtility.AlignOf<uint>(), Allocator.Persistent);
            _vertexBuffer = (float3*) UnsafeUtility.Malloc(UnsafeUtility.SizeOf<float3>() * bufferSize,
                UnsafeUtility.AlignOf<float3>(), Allocator.Persistent);
            _normalBuffer = (float3*) UnsafeUtility.Malloc(UnsafeUtility.SizeOf<float3>() * bufferSize,
                UnsafeUtility.AlignOf<float3>(), Allocator.Persistent);
            _indexBuffer = (uint*) UnsafeUtility.Malloc(UnsafeUtility.SizeOf<uint>() * bufferSize,
                UnsafeUtility.AlignOf<uint>(), Allocator.Persistent);
        }

        public void Clear()
        {
            *_indexCount = 0;
            *_vertexCount = 0;
        }

        public void AddVertex(float3 vertex, float3 normal)
        {
            if (*_vertexCount >= _bufferSize) return;
            long vertOffset = *_vertexCount * sizeof(float3);
            *(float3*) ((long) _vertexBuffer + vertOffset) = vertex;
            *(float3*) ((long) _normalBuffer + vertOffset) = normal;
            *_vertexCount += 1;
        }

        public void AddIndex(uint index)
        {
            if (*_indexCount >= _bufferSize) return;
            *(uint*) ((long) _indexBuffer + *_indexCount * sizeof(uint)) = index;
            *_indexCount += 1;
        }

        public Mesh CreateMesh(ref Bounds bounds)
        {
            Mesh mesh = new();
            int vertexCount = (int) *_vertexCount;
            int indexCount = (int) *_indexCount;
            mesh.SetVertexBufferParams(vertexCount, VertexAttributes);
            mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt32);

            NativeArray<float3> vertexBuffer = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<float3>(
                _vertexBuffer, vertexCount, Allocator.Invalid);
            NativeArray<float3> normalBuffer = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<float3>(
                _normalBuffer, vertexCount, Allocator.Invalid);
            NativeArray<uint> indexBuffer = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<uint>(
                _indexBuffer, indexCount, Allocator.Invalid);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref vertexBuffer, _safetyHandle);
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref normalBuffer, _safetyHandle);
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref indexBuffer, _safetyHandle);
#endif

            mesh.subMeshCount = 1;
            SubMeshDescriptor descriptor = new(0, vertexCount);
            mesh.SetVertexBufferData(vertexBuffer, 0, 0, vertexCount);
            mesh.SetVertexBufferData(normalBuffer, 0, 0, vertexCount, 1);
            mesh.SetIndexBufferData(indexBuffer, 0, 0, indexCount, MeshUpdateFlags.DontValidateIndices);
            mesh.SetSubMesh(0, descriptor, MeshUpdateFlags.DontRecalculateBounds);
            mesh.bounds = bounds;
            mesh.UploadMeshData(true);

            return mesh;
        }

        public void Dispose()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.Release(_safetyHandle);
#endif
            UnsafeUtility.Free(_vertexBuffer, Allocator.Persistent);
            UnsafeUtility.Free(_normalBuffer, Allocator.Persistent);
            UnsafeUtility.Free(_indexBuffer, Allocator.Persistent);
        }
    }
}