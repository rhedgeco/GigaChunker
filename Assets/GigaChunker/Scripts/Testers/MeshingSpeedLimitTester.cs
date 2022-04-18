using GigaChunker.DataTypes;
using GigaChunker.Jobs;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace GigaChunker.Testers
{
    public class MeshingSpeedLimitTester : MonoBehaviour
    {
        [SerializeField, Range(8, 64)] private int chunkSize = 32;

        private NativeArray3d<GigaNode> _nodes;

        private void Start()
        {
            _nodes = new(chunkSize, Allocator.Persistent);
            GenerateNodesJob job = new()
            {
                Position = int3.zero,
                Nodes = _nodes
            };
            job.Run();
        }

        private void OnDestroy()
        {
            if (_nodes.IsCreated) _nodes.Dispose();
        }
    }
}