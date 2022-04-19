using GigaChunker.DataTypes;
using GigaChunker.Generators;
using TerrainGeneration.GenerationProcessors;
using TerrainGeneration.GenerationProcessors.MarchingCubes;
using UnityEngine;

namespace TerrainGeneration.LimitTest
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshingSpeedLimitTester : MonoBehaviour
    {
        [SerializeField, Range(8, 64)] private int chunkSize = 32;

        private MeshFilter _filter;
        
        private NodeGenerator _nodeGenerator;
        private VoxelGenerator _voxelGenerator;

        private GigaChunkNodes _chunkNodes;
        private MeshData _meshData;
        private Bounds _bounds;
        private Mesh.MeshData _data;

        private void Start()
        {
            _filter = GetComponent<MeshFilter>();
            
            _chunkNodes = new(chunkSize);
            _meshData = new(chunkSize * chunkSize * chunkSize * 16);
            _bounds.SetMinMax(Vector3.zero, Vector3.one * chunkSize);
            
            _nodeGenerator = new(SimpleNodeProcessors.Simple3dNoise);
            _voxelGenerator = new(MarchingCubesVoxelProcessor.ProcessVoxel);
            _nodeGenerator.ProcessNow(ref _chunkNodes);
        }

        private void Update()
        {
            _meshData.Clear();
            _voxelGenerator.ProcessNow(in _chunkNodes, ref _meshData);
            DestroyImmediate(_filter.sharedMesh);
            _filter.sharedMesh = _meshData.CreateMesh(ref _bounds);
        }

        private void OnDestroy()
        {
            _chunkNodes.Dispose();
            _meshData.Dispose();
        }
    }
}