using System;
using Unity.Collections;
using Unity.Mathematics;

namespace GigaChunker.DataTypes
{
    public struct MeshData : IDisposable
    {
        public NativeList<float3> Vertices;
        public NativeList<float3> Normals;
        public NativeList<uint> Indices;

        public MeshData(int initialSize)
        {
            Vertices = new(initialSize, Allocator.Persistent);
            Normals = new(initialSize, Allocator.Persistent);
            Indices = new(initialSize, Allocator.Persistent);
        }

        public void Dispose()
        {
            if (Vertices.IsCreated) Vertices.Dispose();
            if (Normals.IsCreated) Normals.Dispose();
            if (Indices.IsCreated) Indices.Dispose();
        }
    }
}