using System;
using GigaChunker.DataTypes;
using GigaChunker.Generators;
using TerrainGeneration.GenerationProcessors;
using TerrainGeneration.GenerationProcessors.MarchingCubes;
using Unity.Collections;
using UnityEngine;

namespace TerrainGeneration.LimitTest
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshingSpeedLimitTester : MonoBehaviour
    {
        [SerializeField, Range(2, 64)] private int chunkSize = 32;

        [Header("Debug Nodes"), SerializeField] private bool debugNodes;

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
            _bounds.SetMinMax(Vector3.zero, Vector3.one * (chunkSize - 1));

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

        private void OnDrawGizmos()
        {
            if (!debugNodes) return;
            NativeArray<GigaNode> _nodeArray = GigaChunkNodes.ExtractRawArray(ref _chunkNodes);
            for (int x = 0; x < _chunkNodes.ChunkSize; x++)
            {
                for (int y = 0; y < _chunkNodes.ChunkSize; y++)
                {
                    for (int z = 0; z < _chunkNodes.ChunkSize; z++)
                    {
                        GigaNode node = _nodeArray[x + y * _chunkNodes.ChunkSize
                                                     + z * _chunkNodes.ChunkSize * _chunkNodes.ChunkSize];
                        Gizmos.color = node.Type == 0 ? Color.black : Color.white;
                        Gizmos.DrawSphere(new(x, y, z), 0.125f);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            _chunkNodes.Dispose();
            _meshData.Dispose();
        }
    }
}