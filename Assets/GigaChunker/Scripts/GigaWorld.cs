using GigaChunker.Jobs;
using UnityEngine;

namespace GigaChunker
{
    public class GigaWorld : MonoBehaviour
    {
        [Header("Generation"),SerializeField] private Transform generationCenter;
        [SerializeField, Range(8, 64)] private int chunkSize = 32;
        [SerializeField, Range(1, 16)] private int renderDistance = 4;
        [SerializeField, Min(0)] private float distanceToRegen = 1;
        
        [Header("Debug"), SerializeField] private bool debugChunks;
        [SerializeField] private Material testMaterial;

        private Vector3 _lastPosition;
        private GigaDataArray _dataArray;
        private GigaData.ChunkRelocateJob _chunkRelocateJob;

        private void Awake()
        {
            _dataArray = new(chunkSize, renderDistance);
            _chunkRelocateJob = new(_dataArray);

            Vector3 centerPosition = generationCenter.position;
            _chunkRelocateJob.Relocate(centerPosition);
            _lastPosition = centerPosition;
        }

        private void Update()
        {
            Vector3 newPosition = generationCenter.position;
            if (Vector3.Distance(_lastPosition, newPosition) < distanceToRegen) return;
            _lastPosition = newPosition;
            _chunkRelocateJob.Relocate(newPosition);
        }

        private void OnDrawGizmos()
        {
            if (!debugChunks || !Application.isPlaying) return;
            Gizmos.color = Color.red;
            _dataArray.ForEach(DrawDebugChunk);
        }

        private static void DrawDebugChunk(ref GigaData data)
        {
            if (!GigaData.RefInRange(ref data)) return;
            Gizmos.DrawWireCube(GigaData.RefWorldCenter(ref data), Vector3.one * data.ChunkSize);
        }

        private void OnDestroy()
        {
            _dataArray.Dispose();
        }
    }
}
