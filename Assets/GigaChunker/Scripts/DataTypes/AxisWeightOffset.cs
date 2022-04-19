namespace GigaChunker.DataTypes
{
    public struct AxisWeightOffset
    {
        private byte _positive;
        private byte _negative;

        public byte PositiveWeight
        {
            get => (byte) (_positive & 0b_0000_1111);
            set
            {
                if (value > 15) value = 15;
                _positive = (byte) (_positive & 0b_1111_0000 | value);
            }
        }
        
        public byte NegativeWeight
        {
            get => (byte) (_negative & 0b_0000_1111);
            set
            {
                if (value > 15) value = 15;
                _negative = (byte) (_negative & 0b_1111_0000 | value);
            }
        }
        
        public byte PositiveOffset
        {
            get => (byte) (_positive >> 4);
            set
            {
                if (value > 15) value = 15;
                _positive = (byte) (_positive & 0b_0000_1111 | (value << 4));
            }
        }
        
        public byte NegativeOffset
        {
            get => (byte) (_negative >> 4);
            set
            {
                if (value > 15) value = 15;
                _negative = (byte) (_negative & 0b_0000_1111 | (value << 4));
            }
        }

        public AxisWeightOffset(byte positiveWeight, byte negativeWeight,
            byte positiveOffset = 0, byte negativeOffset = 0)
        {
            if (positiveWeight > 15) positiveWeight = 15;
            if (positiveOffset > 15) positiveOffset = 15;
            if (negativeWeight > 15) negativeWeight = 15;
            if (negativeOffset > 15) negativeOffset = 15;
            _positive = (byte) (positiveWeight | (positiveOffset << 4));
            _negative = (byte) (negativeWeight | (negativeOffset << 4));
        }

        public void Set(byte positiveWeight, byte negativeWeight,
            byte positiveOffset = 0, byte negativeOffset = 0)
        {
            if (positiveWeight > 15) positiveWeight = 15;
            if (positiveOffset > 15) positiveOffset = 15;
            if (negativeWeight > 15) negativeWeight = 15;
            if (negativeOffset > 15) negativeOffset = 15;
            _positive = (byte) (positiveWeight | (positiveOffset << 4));
            _negative = (byte) (negativeWeight | (negativeOffset << 4));
        }
    }
}