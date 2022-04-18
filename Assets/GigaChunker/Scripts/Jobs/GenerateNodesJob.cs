using GigaChunker.DataTypes;
using Unity.Jobs;
using Unity.Mathematics;

namespace GigaChunker.Jobs
{
    public struct GenerateNodesJob : IJob
    {
        private const float OffsetConst = 0.72354f;
        
        public int3 Position;
        public NativeArray3d<GigaNode> Nodes;

        public void Execute()
        {
            Nodes.ForEach(PopulateNode);
        }

        private void PopulateNode(ref GigaNode node, in int3 nodePos)
        {
            int yPos = nodePos.y + Position.y;
            switch (yPos)
            {
                case < 5:
                    CreateFullNode(ref node);
                    break;
                case 5:
                    CreateFloorNode(ref node);
                    break;
                default:
                    CreateEmptyNode(ref node);
                    break;
            }
        }

        private static void CreateFullNode(ref GigaNode node)
        {
            node.Type = 0;
            node.HasContour = false;
            node.XWeight = AxisWeights.Max;
            node.YWeight = AxisWeights.Max;
            node.ZWeight = AxisWeights.Max;
        }

        private static void CreateFloorNode(ref GigaNode node)
        {
            node.Type = 0;
            node.HasContour = true;
            node.XWeight = AxisWeights.Max;
            node.YWeight = new(128, byte.MaxValue);
            node.ZWeight = AxisWeights.Max;
        }

        private static void CreateEmptyNode(ref GigaNode node)
        {
            node.Type = 0;
            node.HasContour = false;
            node.XWeight = AxisWeights.Min;
            node.YWeight = AxisWeights.Min;
            node.ZWeight = AxisWeights.Min;
        }
    }
}