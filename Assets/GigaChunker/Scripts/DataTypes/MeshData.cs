using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace GigaChunker.DataTypes
{
    public unsafe struct MeshData : IDisposable
    {
        private static readonly VertexAttributeDescriptor[] VertexAttributes =
        {
            new(VertexAttribute.Position, stream: 0),
            new(VertexAttribute.Normal, stream: 1)
        };
        
        private readonly uint _bufferSize;
        
        [NativeDisableUnsafePtrRestriction]
        private long* _indexCount;
        
        [NativeDisableUnsafePtrRestriction]
        private long* _vertexCount;

        [NativeDisableUnsafePtrRestriction]
        private readonly long* _vertexBuffer;

        [NativeDisableUnsafePtrRestriction]
        private readonly long* _normalBuffer;

        [NativeDisableUnsafePtrRestriction]
        private readonly long* _indexBuffer;

        public uint VertexCount => *(uint*) _vertexCount;
        public uint IndexCount => *(uint*) _indexCount;

        public MeshData(int bufferSize)
        {
            _bufferSize = (uint) bufferSize;
            _vertexCount = (long*) UnsafeUtility.Malloc(UnsafeUtility.SizeOf<uint>(),
                UnsafeUtility.AlignOf<uint>(), Allocator.Persistent);
            _indexCount = (long*) UnsafeUtility.Malloc(UnsafeUtility.SizeOf<uint>(),
                UnsafeUtility.AlignOf<uint>(), Allocator.Persistent);
            _vertexBuffer = (long*) UnsafeUtility.Malloc(UnsafeUtility.SizeOf<float3>() * bufferSize,
                UnsafeUtility.AlignOf<float3>(), Allocator.Persistent);
            _normalBuffer = (long*) UnsafeUtility.Malloc(UnsafeUtility.SizeOf<float3>() * bufferSize,
                UnsafeUtility.AlignOf<float3>(), Allocator.Persistent);
            _indexBuffer = (long*) UnsafeUtility.Malloc(UnsafeUtility.SizeOf<uint>() * bufferSize,
                UnsafeUtility.AlignOf<uint>(), Allocator.Persistent);
        }

        public void Clear()
        {
            *(uint*)_indexCount = 0;
            *(uint*)_vertexCount = 0;
        }

        public void AddVertex(float3 vertex, float3 normal)
        {
            ref uint vertCount = ref *(uint*)_vertexCount;
            if (vertCount >= _bufferSize) return;
            long vertOffset = vertCount * sizeof(float3);
            *(float3*) ((long)_vertexBuffer + vertOffset) = vertex;
            *(float3*) ((long)_normalBuffer + vertOffset) = normal;
            vertCount++;
        }

        public void AddIndex(uint index)
        {
            ref uint indexCount = ref *(uint*)_indexCount;
            if (indexCount >= _bufferSize) return;
            *(uint*) ((long)_indexBuffer + indexCount * sizeof(uint)) = index;
            indexCount++;
        }

        public Mesh CreateMesh(ref Bounds bounds)
        {
            Mesh mesh = new();
            int vertexCount = (int) VertexCount;
            int indexCount = (int) IndexCount;
            mesh.SetVertexBufferParams(vertexCount, VertexAttributes);
            mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt32);

            NativeArray<float3> vertexBuffer = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<float3>(
                _vertexBuffer, vertexCount, Allocator.Invalid);
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref vertexBuffer, AtomicSafetyHandle.Create());
            NativeArray<float3> normalBuffer = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<float3>(
                _normalBuffer, vertexCount, Allocator.Invalid);
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref normalBuffer, AtomicSafetyHandle.Create());
            NativeArray<uint> indexBuffer = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<uint>(
                _indexBuffer, indexCount, Allocator.Invalid);
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref indexBuffer, AtomicSafetyHandle.Create());

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
            UnsafeUtility.Free(_vertexBuffer, Allocator.Persistent);
            UnsafeUtility.Free(_normalBuffer, Allocator.Persistent);
            UnsafeUtility.Free(_indexBuffer, Allocator.Persistent);
        }
    }
}