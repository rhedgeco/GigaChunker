using GigaChunker.DataTypes;
using GigaChunker.Generators;
using UnityEngine;

namespace GigaChunker.Testers
{
    public class MeshingSpeedLimitTester : MonoBehaviour
    {
        [SerializeField, Range(8, 64)] private int chunkSize = 32;

        private GigaChunkNodes _chunkNodes;
        private NodeGenerator _generator;

        private void Start()
        {
            _chunkNodes = new(chunkSize);
            _generator = new(SimpleNodeProcessors.FlatGround);
            _generator.ProcessNow(ref _chunkNodes);
        }

        private void OnDestroy()
        {
            _chunkNodes.Dispose();
        }
    }
}