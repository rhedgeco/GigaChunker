using Unity.Collections.LowLevel.Unsafe;

namespace GigaChunker.DataTypes
{
    public unsafe struct VoxelCorners
    {
        [NativeDisableUnsafePtrRestriction]
        private GigaNode* _0;
        
        [NativeDisableUnsafePtrRestriction]
        private GigaNode* _1;
        
        [NativeDisableUnsafePtrRestriction]
        private GigaNode* _2;
        
        [NativeDisableUnsafePtrRestriction]
        private GigaNode* _3;
        
        [NativeDisableUnsafePtrRestriction]
        private GigaNode* _4;
        
        [NativeDisableUnsafePtrRestriction]
        private GigaNode* _5;
        
        [NativeDisableUnsafePtrRestriction]
        private GigaNode* _6;
        
        [NativeDisableUnsafePtrRestriction]
        private GigaNode* _7;

        public VoxelCorners(
            GigaNode* node0,
            GigaNode* node1,
            GigaNode* node2,
            GigaNode* node3,
            GigaNode* node4,
            GigaNode* node5,
            GigaNode* node6,
            GigaNode* node7
        )
        {
            _0 = node0;
            _1 = node1;
            _2 = node2;
            _3 = node3;
            _4 = node4;
            _5 = node5;
            _6 = node6;
            _7 = node7;
        }
        
        public void Set(
            GigaNode* node0,
            GigaNode* node1,
            GigaNode* node2,
            GigaNode* node3,
            GigaNode* node4,
            GigaNode* node5,
            GigaNode* node6,
            GigaNode* node7
        )
        {
            _0 = node0;
            _1 = node1;
            _2 = node2;
            _3 = node3;
            _4 = node4;
            _5 = node5;
            _6 = node6;
            _7 = node7;
        }

        public ref GigaNode this[int index]
        {
            get
            {
                switch (index)
                {
                    case 1:
                        return ref *_1;
                    case 2:
                        return ref *_2;
                    case 3:
                        return ref *_3;
                    case 4:
                        return ref *_4;
                    case 5:
                        return ref *_5;
                    case 6:
                        return ref *_6;
                    case 7:
                        return ref *_7;
                    default:
                        return ref *_0;
                }
            }
        }
    }
}