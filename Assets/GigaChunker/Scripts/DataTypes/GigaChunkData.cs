using Unity.Mathematics;
using UnityEngine;

namespace GigaChunker.DataTypes
{
    public partial struct GigaChunkData
    {
        public readonly int ChunkSize;
        public readonly int ChunkIndex;
        public readonly int3 Chunk3dIndex;

        private int3 _chunkPosition;
        private float3 _worldPosition;
        private float3 _worldCenter;
        private Matrix4x4 _worldMatrix;
        private bool _inRange;

        // expose private editable values by ref so that copying does not occur on read
        public static ref readonly int3 RefChunkPosition(ref GigaChunkData gigaData) => ref gigaData._chunkPosition;
        public static ref readonly float3 RefWorldPosition(ref GigaChunkData gigaData) => ref gigaData._worldPosition;
        public static ref readonly float3 RefWorldCenter(ref GigaChunkData gigaData) => ref gigaData._worldCenter;
        public static ref readonly Matrix4x4 RefWorldMatrix(ref GigaChunkData gigaData) => ref gigaData._worldMatrix;
        public static ref readonly bool RefInRange(ref GigaChunkData gigaData) => ref gigaData._inRange;
        
        public GigaChunkData(int chunkSize, int index, int3 index3d)
        {
            ChunkSize = chunkSize;
            ChunkIndex = index;
            Chunk3dIndex = index3d;

            _chunkPosition = int3.zero;
            _worldPosition = float3.zero;
            _worldCenter = _worldPosition + new float3(chunkSize / 2f);
            _worldMatrix = Matrix4x4.Translate(_worldPosition);
            _inRange = false;
        }
        
        private void MoveChunk(int3 chunkPosition)
        {
            if (_chunkPosition.Equals(chunkPosition)) return;
            _chunkPosition = chunkPosition;
            _worldPosition = chunkPosition * ChunkSize;
            _worldCenter = _worldPosition + new float3(ChunkSize / 2f);
            _worldMatrix = Matrix4x4.Translate(_worldPosition);
        }
    }
}