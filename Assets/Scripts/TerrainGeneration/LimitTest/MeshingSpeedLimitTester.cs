using GigaChunker.DataTypes;
using GigaChunker.Generators;
using TerrainGeneration.GenerationProcessors;
using TerrainGeneration.GenerationProcessors.MarchingCubes;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace TerrainGeneration.LimitTest
{
    public class MeshingSpeedLimitTester : MonoBehaviour
    {
        [SerializeField, Range(8, 64)] private int chunkSize = 32;

        private GigaChunkNodes _chunkNodes;
        private NodeGenerator _nodeGenerator;
        private VoxelGenerator _voxelGenerator;

        private NativeList<float3> _vertexBuffer;
        private NativeList<float3> _normalBuffer;
        private NativeList<uint> _indexBuffer;

        private void Start()
        {
            _chunkNodes = new(chunkSize);
            _nodeGenerator = new(SimpleNodeProcessors.Simple3dNoise);
            _voxelGenerator = new(MarchingCubesVoxelProcessor.ProcessVoxel);
            _nodeGenerator.ProcessNow(ref _chunkNodes);
        }

        private void Update()
        {
            _voxelGenerator.ProcessNow(in _chunkNodes, ref _vertexBuffer, ref _normalBuffer, ref _indexBuffer);
        }

        private void OnDestroy()
        {
            _chunkNodes.Dispose();
        }
    }
}