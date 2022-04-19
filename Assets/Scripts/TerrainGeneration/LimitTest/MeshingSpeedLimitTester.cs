using GigaChunker.DataTypes;
using GigaChunker.Generators;
using TerrainGeneration.GenerationProcessors;
using UnityEngine;

namespace TerrainGeneration.LimitTest
{
    public class MeshingSpeedLimitTester : MonoBehaviour
    {
        [SerializeField, Range(8, 64)] private int chunkSize = 32;

        private GigaChunkNodes _chunkNodes;
        private NodeGenerator _nodeGenerator;
        private MeshGenerator _meshGenerator;

        private void Start()
        {
            _chunkNodes = new(chunkSize);
            
            _nodeGenerator = new(SimpleNodeProcessors.Simple3dNoise);
            _nodeGenerator.ProcessNow(ref _chunkNodes);
        }

        private void OnDestroy()
        {
            _chunkNodes.Dispose();
        }
    }
}