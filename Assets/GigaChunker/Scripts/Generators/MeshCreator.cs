using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace GigaChunker.Generators
{
    public static class MeshCreator
    {
        private static readonly VertexAttributeDescriptor[] VertexAttributes =
        {
            new(VertexAttribute.Position, stream: 0),
            new(VertexAttribute.Normal, stream: 1)
        };
        
        public static Mesh CreateMesh(ref NativeList<float3> vertexBuffer, ref NativeList<float3> normalBuffer,
            ref NativeList<uint> indexBuffer, in Bounds bounds)
        {
            Mesh mesh = new();
            int vertexCount = vertexBuffer.Length;
            int indexCount = indexBuffer.Length;
            mesh.SetVertexBufferParams(vertexCount, VertexAttributes);
            mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt32);

            mesh.subMeshCount = 1;
            SubMeshDescriptor descriptor = new(0, vertexCount);
            mesh.SetVertexBufferData(vertexBuffer.AsArray(), 0, 0, vertexCount);
            mesh.SetVertexBufferData(normalBuffer.AsArray(), 0, 0, vertexCount, 1);
            mesh.SetIndexBufferData(indexBuffer.AsArray(), 0, 0, indexCount, MeshUpdateFlags.DontValidateIndices);
            mesh.SetSubMesh(0, descriptor, MeshUpdateFlags.DontRecalculateBounds);
            mesh.bounds = bounds;
            mesh.UploadMeshData(true);

            return mesh;
        }
    }
}