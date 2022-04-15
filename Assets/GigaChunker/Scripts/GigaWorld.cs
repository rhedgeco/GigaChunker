using GigaChunker.Jobs;
using UnityEngine;

namespace GigaChunker
{
    public class GigaWorld : MonoBehaviour
    {
        [Header("Generation"),SerializeField] private Transform generationCenter;
        [SerializeField, Range(8, 64)] private int chunkSize = 32;
        [SerializeField, Range(1, 16)] private int renderDistance = 4;
        
        [Header("Debug"), SerializeField] private bool debugChunks;

        private GigaChunkDataArray _chunkDataArray;
        private GigaChunkData.RelocateChunksJob _relocateChunks;

        private void Awake()
        {
            _chunkDataArray = new(chunkSize, renderDistance);
            _relocateChunks = new(_chunkDataArray);
        }

        private void Update()
        {
            _relocateChunks.Relocate(generationCenter.position);
        }

        private void OnDrawGizmos()
        {
            if (!debugChunks || !Application.isPlaying) return;
            Gizmos.color = Color.red;
            _chunkDataArray.ForEach(DrawDebugChunk);
        }

        private static void DrawDebugChunk(ref GigaChunkData data)
        {
            if (!GigaChunkData.RefInRange(ref data)) return;
            Gizmos.DrawWireCube(GigaChunkData.RefWorldCenter(ref data), Vector3.one * data.ChunkSize);
        }

        private void OnDestroy()
        {
            _chunkDataArray.Dispose();
        }
    }
}
