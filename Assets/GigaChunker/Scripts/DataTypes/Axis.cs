using System;
using Unity.Mathematics;

namespace GigaChunker.DataTypes
{
    public static class Axis
    {
        public static float3 ToVector(this Direction axisDirection)
        {
            return axisDirection switch
            {
                Direction.Right => new(1, 0, 0),
                Direction.Left => new(-1, 0, 0),
                Direction.Up => new(0, 1, 0),
                Direction.Down => new(0, -1, 0),
                Direction.Forward => new(0, 0, 1),
                Direction.Back => new(0, 0, -1),
                _ => throw new ArgumentOutOfRangeException(nameof(axisDirection), axisDirection, null)
            };
        }

        public enum Direction
        {
            Right,
            Left,
            Up,
            Down,
            Forward,
            Back
        }
    }
}