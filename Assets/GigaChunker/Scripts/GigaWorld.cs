using GigaChunker.DataTypes;
using UnityEngine;

namespace GigaChunker
{
    public class GigaWorld : MonoBehaviour
    {
        [Header("Generation"),SerializeField] private Transform center;
        [SerializeField, Range(8, 64)] private int chunkSize = 32;
        [SerializeField, Range(1, 16)] private int renderDistance = 4;
        [SerializeField, Min(0)] private float distanceToRegen = 1;
        
        [Header("Debug"), SerializeField] private bool debugChunks;
        [SerializeField] private Material testMaterial;

        private Vector3 _lastPosition;
        private GigaChunkData.Array _chunkDataArray;

        private void Awake()
        {
            _chunkDataArray = new(chunkSize, renderDistance);

            Vector3 centerPosition = center.position;
            _chunkDataArray.Relocate(centerPosition);
            _lastPosition = centerPosition;
        }

        private void Update()
        {
            Vector3 newPosition = center.position;
            if (Vector3.Distance(_lastPosition, newPosition) < distanceToRegen) return;
            _lastPosition = newPosition;
            _chunkDataArray.Relocate(newPosition);
        }

        private void OnDrawGizmos()
        {
            if (!debugChunks || !Application.isPlaying) return;
            Gizmos.color = new(0, 1, 0, 0.1f);
            Gizmos.DrawSphere(center.position, _chunkDataArray.WorldRenderDistance);
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
