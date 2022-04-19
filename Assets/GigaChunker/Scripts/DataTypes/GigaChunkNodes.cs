using System;
using Unity.Collections;

namespace GigaChunker.DataTypes
{
    public struct GigaChunkNodes : IDisposable
    {
        private NativeArray<GigaNode> _nodes;

        public readonly int ChunkSize;

        public static ref readonly NativeArray<GigaNode> ExtractRawArray(ref GigaChunkNodes chunkNodes) =>
            ref chunkNodes._nodes;

        public GigaChunkNodes(int chunkSize)
        {
            ChunkSize = chunkSize;
            _nodes = new(chunkSize * chunkSize * chunkSize, Allocator.Persistent);
        }

        public void Dispose()
        {
            if (_nodes.IsCreated) _nodes.Dispose();
        }
    }
}