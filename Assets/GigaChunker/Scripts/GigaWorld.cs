using GigaChunker.Jobs;
using Unity.Jobs;
using UnityEngine;

namespace GigaChunker
{
    public class GigaWorld : MonoBehaviour
    {
        [SerializeField, Range(8, 64)] private int chunkSize = 32;
        [SerializeField, Range(1, 16)] private int renderDistance = 4;
        [SerializeField] private bool debug;

        private GigaChunkData.ChunkDataManager _dataManager;

        private void Awake()
        {
            _dataManager = new(chunkSize, renderDistance);
        }

        private void Update()
        {
            _dataManager.SetPlayerPosition(transform.position);
            _dataManager.Run();
        }

        private void OnDrawGizmos()
        {
            if (!debug || !Application.isPlaying) return;
            Gizmos.color = Color.red;
            _dataManager.ForEach((ref GigaChunkData data, int _) =>
            {
                if (!GigaChunkData.RefInRange(ref data)) return;
                Gizmos.DrawWireCube(GigaChunkData.RefWorldCenter(ref data), Vector3.one * data.ChunkSize);
            });
        }

        private void OnDestroy()
        {
            _dataManager.Dispose();
        }
    }
}
