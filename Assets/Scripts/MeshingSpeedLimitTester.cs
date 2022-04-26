using GigaChunker.DataTypes;
using GigaChunker.Extensions;
using GigaChunker.Generators;
using GigaChunker.Generators.Node;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshingSpeedLimitTester : MonoBehaviour
{
    [SerializeField, Range(2, 64)] private int chunkSize = 32;

    private MeshFilter _filter;

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

        float3 position = transform.position;
        BaseNodeGenerator.Process(ref _chunkNodes, position.RoundToInt());
    }

    private void Update()
    {
        _meshData.Clear();
        VoxelMarcher.MarchNodes(in _chunkNodes, ref _meshData);
        DestroyImmediate(_filter.sharedMesh);
        _filter.sharedMesh = _meshData.CreateMesh(ref _bounds);
    }

    private void OnDestroy()
    {
        _chunkNodes.Dispose();
        _meshData.Dispose();
    }
}