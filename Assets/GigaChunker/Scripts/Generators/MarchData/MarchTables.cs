using GigaChunker.DataTypes;
using Unity.Mathematics;

namespace GigaChunker.Generators.MarchData
{
    public static class MarchTables
    {
        // modified from http://paulbourke.net/geometry/polygonise/
        public static readonly int[] Triangulation =
        {
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            3, 8, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            9, 1, 12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            3, 8, 1, 1, 8, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            10, 14, 13, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            3, 8, 0, 10, 14, 13, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            10, 14, 9, 9, 14, 12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            3, 8, 14, 8, 10, 14, 8, 9, 10, -1, -1, -1, -1, -1, -1, -1,
            2, 11, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            2, 11, 0, 0, 11, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            12, 9, 1, 11, 15, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            2, 11, 1, 11, 9, 1, 11, 8, 9, -1, -1, -1, -1, -1, -1, -1,
            13, 10, 15, 15, 10, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            13, 10, 0, 10, 8, 0, 10, 11, 8, -1, -1, -1, -1, -1, -1, -1,
            12, 9, 15, 9, 11, 15, 9, 10, 11, -1, -1, -1, -1, -1, -1, -1,
            10, 8, 9, 11, 8, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            20, 7, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            0, 3, 4, 4, 3, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            9, 1, 12, 7, 4, 20, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            9, 1, 4, 1, 7, 4, 1, 3, 7, -1, -1, -1, -1, -1, -1, -1,
            10, 14, 13, 7, 4, 20, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            7, 4, 3, 4, 0, 3, 10, 14, 13, -1, -1, -1, -1, -1, -1, -1,
            10, 14, 9, 14, 12, 9, 7, 4, 20, -1, -1, -1, -1, -1, -1, -1,
            9, 10, 14, 7, 9, 14, 3, 7, 14, 4, 9, 7, -1, -1, -1, -1,
            7, 4, 20, 2, 11, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            7, 4, 11, 4, 2, 11, 4, 0, 2, -1, -1, -1, -1, -1, -1, -1,
            1, 12, 9, 7, 4, 20, 11, 15, 2, -1, -1, -1, -1, -1, -1, -1,
            11, 7, 4, 11, 4, 9, 2, 11, 9, 1, 2, 9, -1, -1, -1, -1,
            13, 10, 15, 10, 11, 15, 4, 20, 7, -1, -1, -1, -1, -1, -1, -1,
            10, 11, 13, 11, 4, 13, 4, 0, 13, 4, 11, 7, -1, -1, -1, -1,
            20, 7, 4, 11, 12, 9, 10, 11, 9, 15, 12, 11, -1, -1, -1, -1,
            11, 7, 4, 9, 11, 4, 10, 11, 9, -1, -1, -1, -1, -1, -1, -1,
            16, 5, 21, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            16, 5, 21, 3, 8, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            16, 5, 12, 12, 5, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            16, 5, 8, 5, 3, 8, 5, 1, 3, -1, -1, -1, -1, -1, -1, -1,
            10, 14, 13, 16, 5, 21, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            8, 0, 3, 10, 14, 13, 5, 21, 16, -1, -1, -1, -1, -1, -1, -1,
            10, 14, 5, 14, 16, 5, 14, 12, 16, -1, -1, -1, -1, -1, -1, -1,
            5, 10, 14, 5, 14, 3, 16, 5, 3, 8, 16, 3, -1, -1, -1, -1,
            16, 5, 21, 11, 15, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            2, 11, 0, 11, 8, 0, 5, 21, 16, -1, -1, -1, -1, -1, -1, -1,
            16, 5, 12, 5, 1, 12, 11, 15, 2, -1, -1, -1, -1, -1, -1, -1,
            5, 1, 2, 8, 5, 2, 11, 8, 2, 5, 8, 16, -1, -1, -1, -1,
            11, 15, 10, 15, 13, 10, 16, 5, 21, -1, -1, -1, -1, -1, -1, -1,
            5, 21, 16, 13, 8, 0, 13, 10, 8, 10, 11, 8, -1, -1, -1, -1,
            12, 16, 5, 11, 12, 5, 10, 11, 5, 15, 12, 11, -1, -1, -1, -1,
            8, 16, 5, 10, 8, 5, 11, 8, 10, -1, -1, -1, -1, -1, -1, -1,
            20, 7, 21, 21, 7, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            0, 3, 21, 3, 5, 21, 3, 7, 5, -1, -1, -1, -1, -1, -1, -1,
            20, 7, 12, 7, 1, 12, 7, 5, 1, -1, -1, -1, -1, -1, -1, -1,
            3, 5, 1, 7, 5, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            20, 7, 21, 7, 5, 21, 14, 13, 10, -1, -1, -1, -1, -1, -1, -1,
            14, 13, 10, 0, 5, 21, 0, 3, 5, 3, 7, 5, -1, -1, -1, -1,
            14, 12, 20, 5, 14, 20, 7, 5, 20, 14, 5, 10, -1, -1, -1, -1,
            5, 10, 14, 3, 5, 14, 7, 5, 3, -1, -1, -1, -1, -1, -1, -1,
            5, 21, 7, 21, 20, 7, 2, 11, 15, -1, -1, -1, -1, -1, -1, -1,
            7, 5, 21, 2, 7, 21, 0, 2, 21, 11, 7, 2, -1, -1, -1, -1,
            11, 15, 2, 20, 1, 12, 20, 7, 1, 7, 5, 1, -1, -1, -1, -1,
            1, 2, 11, 7, 1, 11, 5, 1, 7, -1, -1, -1, -1, -1, -1, -1,
            20, 5, 21, 7, 5, 20, 15, 13, 10, 11, 15, 10, -1, -1, -1, -1,
            0, 7, 5, 21, 0, 5, 0, 11, 7, 10, 0, 13, 0, 10, 11, -1,
            12, 10, 11, 15, 12, 11, 12, 5, 10, 7, 12, 20, 12, 7, 5, -1,
            5, 10, 11, 5, 11, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            17, 18, 22, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            3, 8, 0, 18, 22, 17, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            1, 12, 9, 18, 22, 17, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            3, 8, 1, 8, 9, 1, 18, 22, 17, -1, -1, -1, -1, -1, -1, -1,
            17, 18, 13, 13, 18, 14, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            17, 18, 13, 18, 14, 13, 8, 0, 3, -1, -1, -1, -1, -1, -1, -1,
            17, 18, 9, 18, 12, 9, 18, 14, 12, -1, -1, -1, -1, -1, -1, -1,
            8, 9, 17, 14, 8, 17, 18, 14, 17, 8, 14, 3, -1, -1, -1, -1,
            11, 15, 2, 17, 18, 22, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            8, 0, 11, 0, 2, 11, 17, 18, 22, -1, -1, -1, -1, -1, -1, -1,
            9, 1, 12, 11, 15, 2, 18, 22, 17, -1, -1, -1, -1, -1, -1, -1,
            18, 22, 17, 2, 9, 1, 2, 11, 9, 11, 8, 9, -1, -1, -1, -1,
            11, 15, 18, 15, 17, 18, 15, 13, 17, -1, -1, -1, -1, -1, -1, -1,
            11, 8, 0, 17, 11, 0, 13, 17, 0, 18, 11, 17, -1, -1, -1, -1,
            18, 11, 15, 18, 15, 12, 17, 18, 12, 9, 17, 12, -1, -1, -1, -1,
            9, 17, 18, 11, 9, 18, 8, 9, 11, -1, -1, -1, -1, -1, -1, -1,
            18, 22, 17, 20, 7, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            0, 3, 4, 3, 7, 4, 22, 17, 18, -1, -1, -1, -1, -1, -1, -1,
            12, 9, 1, 18, 22, 17, 7, 4, 20, -1, -1, -1, -1, -1, -1, -1,
            17, 18, 22, 7, 9, 1, 3, 7, 1, 4, 9, 7, -1, -1, -1, -1,
            14, 13, 18, 13, 17, 18, 20, 7, 4, -1, -1, -1, -1, -1, -1, -1,
            17, 14, 13, 18, 14, 17, 4, 0, 3, 7, 4, 3, -1, -1, -1, -1,
            7, 4, 20, 17, 12, 9, 17, 18, 12, 18, 14, 12, -1, -1, -1, -1,
            9, 3, 7, 4, 9, 7, 9, 14, 3, 18, 9, 17, 9, 18, 14, -1,
            2, 11, 15, 4, 20, 7, 17, 18, 22, -1, -1, -1, -1, -1, -1, -1,
            18, 22, 17, 2, 7, 4, 0, 2, 4, 11, 7, 2, -1, -1, -1, -1,
            9, 1, 12, 20, 7, 4, 11, 15, 2, 18, 22, 17, -1, -1, -1, -1,
            1, 2, 9, 2, 11, 9, 11, 4, 9, 4, 11, 7, 18, 22, 17, -1,
            7, 4, 20, 17, 11, 15, 13, 17, 15, 18, 11, 17, -1, -1, -1, -1,
            11, 13, 17, 18, 11, 17, 11, 0, 13, 4, 11, 7, 11, 4, 0, -1,
            9, 17, 12, 17, 18, 12, 18, 15, 12, 15, 18, 11, 7, 4, 20, -1,
            9, 17, 18, 11, 9, 18, 9, 7, 4, 9, 11, 7, -1, -1, -1, -1,
            21, 16, 22, 22, 16, 18, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            18, 22, 16, 22, 21, 16, 3, 8, 0, -1, -1, -1, -1, -1, -1, -1,
            1, 12, 22, 12, 18, 22, 12, 16, 18, -1, -1, -1, -1, -1, -1, -1,
            1, 3, 8, 18, 1, 8, 16, 18, 8, 22, 1, 18, -1, -1, -1, -1,
            21, 16, 13, 16, 14, 13, 16, 18, 14, -1, -1, -1, -1, -1, -1, -1,
            8, 0, 3, 21, 14, 13, 21, 16, 14, 16, 18, 14, -1, -1, -1, -1,
            16, 14, 12, 18, 14, 16, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            14, 3, 8, 16, 14, 8, 18, 14, 16, -1, -1, -1, -1, -1, -1, -1,
            21, 16, 22, 16, 18, 22, 15, 2, 11, -1, -1, -1, -1, -1, -1, -1,
            2, 8, 0, 11, 8, 2, 22, 21, 16, 18, 22, 16, -1, -1, -1, -1,
            2, 11, 15, 18, 1, 12, 16, 18, 12, 22, 1, 18, -1, -1, -1, -1,
            1, 16, 18, 22, 1, 18, 1, 8, 16, 11, 1, 2, 1, 11, 8, -1,
            16, 18, 21, 18, 15, 21, 15, 13, 21, 15, 18, 11, -1, -1, -1, -1,
            13, 11, 8, 0, 13, 8, 13, 18, 11, 16, 13, 21, 13, 16, 18, -1,
            18, 11, 15, 12, 18, 15, 16, 18, 12, -1, -1, -1, -1, -1, -1, -1,
            8, 16, 18, 8, 18, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            18, 22, 7, 22, 20, 7, 22, 21, 20, -1, -1, -1, -1, -1, -1, -1,
            3, 7, 0, 7, 22, 0, 22, 21, 0, 22, 7, 18, -1, -1, -1, -1,
            7, 18, 22, 7, 22, 1, 20, 7, 1, 12, 20, 1, -1, -1, -1, -1,
            7, 18, 22, 1, 7, 22, 3, 7, 1, -1, -1, -1, -1, -1, -1, -1,
            18, 14, 13, 20, 18, 13, 21, 20, 13, 7, 18, 20, -1, -1, -1, -1,
            21, 18, 14, 13, 21, 14, 21, 7, 18, 3, 21, 0, 21, 3, 7, -1,
            12, 20, 7, 18, 12, 7, 14, 12, 18, -1, -1, -1, -1, -1, -1, -1,
            14, 3, 7, 14, 7, 18, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            11, 15, 2, 20, 18, 22, 21, 20, 22, 7, 18, 20, -1, -1, -1, -1,
            7, 0, 2, 11, 7, 2, 7, 21, 0, 22, 7, 18, 7, 22, 21, -1,
            12, 20, 1, 20, 7, 1, 7, 22, 1, 22, 7, 18, 11, 15, 2, -1,
            1, 2, 11, 7, 1, 11, 1, 18, 22, 1, 7, 18, -1, -1, -1, -1,
            18, 21, 20, 7, 18, 20, 18, 13, 21, 15, 18, 11, 18, 15, 13, -1,
            13, 21, 0, 7, 18, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            12, 20, 7, 18, 12, 7, 12, 11, 15, 12, 18, 11, -1, -1, -1, -1,
            18, 11, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            23, 6, 19, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            8, 0, 3, 6, 19, 23, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            9, 1, 12, 6, 19, 23, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            9, 1, 8, 1, 3, 8, 6, 19, 23, -1, -1, -1, -1, -1, -1, -1,
            14, 13, 10, 19, 23, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            10, 14, 13, 8, 0, 3, 19, 23, 6, -1, -1, -1, -1, -1, -1, -1,
            12, 9, 14, 9, 10, 14, 19, 23, 6, -1, -1, -1, -1, -1, -1, -1,
            19, 23, 6, 3, 10, 14, 3, 8, 10, 8, 9, 10, -1, -1, -1, -1,
            15, 2, 19, 19, 2, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            8, 0, 19, 0, 6, 19, 0, 2, 6, -1, -1, -1, -1, -1, -1, -1,
            6, 19, 2, 19, 15, 2, 9, 1, 12, -1, -1, -1, -1, -1, -1, -1,
            2, 6, 1, 6, 8, 1, 8, 9, 1, 6, 19, 8, -1, -1, -1, -1,
            6, 19, 10, 19, 13, 10, 19, 15, 13, -1, -1, -1, -1, -1, -1, -1,
            6, 19, 10, 10, 19, 13, 19, 8, 13, 8, 0, 13, -1, -1, -1, -1,
            19, 15, 12, 10, 19, 12, 9, 10, 12, 19, 10, 6, -1, -1, -1, -1,
            10, 6, 19, 8, 10, 19, 9, 10, 8, -1, -1, -1, -1, -1, -1, -1,
            4, 20, 6, 6, 20, 23, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            23, 6, 3, 6, 0, 3, 6, 4, 0, -1, -1, -1, -1, -1, -1, -1,
            23, 6, 20, 6, 4, 20, 1, 12, 9, -1, -1, -1, -1, -1, -1, -1,
            6, 4, 9, 3, 6, 9, 1, 3, 9, 6, 3, 23, -1, -1, -1, -1,
            4, 20, 6, 20, 23, 6, 13, 10, 14, -1, -1, -1, -1, -1, -1, -1,
            10, 14, 13, 23, 0, 3, 23, 6, 0, 6, 4, 0, -1, -1, -1, -1,
            20, 23, 4, 23, 6, 4, 9, 14, 12, 9, 10, 14, -1, -1, -1, -1,
            3, 9, 10, 14, 3, 10, 3, 4, 9, 6, 3, 23, 3, 6, 4, -1,
            15, 2, 20, 2, 4, 20, 2, 6, 4, -1, -1, -1, -1, -1, -1, -1,
            2, 4, 0, 2, 6, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            12, 9, 1, 4, 15, 2, 6, 4, 2, 20, 15, 4, -1, -1, -1, -1,
            4, 9, 1, 2, 4, 1, 6, 4, 2, -1, -1, -1, -1, -1, -1, -1,
            15, 13, 20, 13, 6, 20, 6, 4, 20, 13, 10, 6, -1, -1, -1, -1,
            0, 13, 10, 6, 0, 10, 4, 0, 6, -1, -1, -1, -1, -1, -1, -1,
            15, 6, 4, 20, 15, 4, 15, 10, 6, 9, 15, 12, 15, 9, 10, -1,
            4, 9, 10, 4, 10, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            5, 21, 16, 23, 6, 19, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            3, 8, 0, 5, 21, 16, 6, 19, 23, -1, -1, -1, -1, -1, -1, -1,
            1, 12, 5, 12, 16, 5, 23, 6, 19, -1, -1, -1, -1, -1, -1, -1,
            6, 19, 23, 16, 3, 8, 16, 5, 3, 5, 1, 3, -1, -1, -1, -1,
            16, 5, 21, 14, 13, 10, 23, 6, 19, -1, -1, -1, -1, -1, -1, -1,
            19, 23, 6, 10, 14, 13, 3, 8, 0, 5, 21, 16, -1, -1, -1, -1,
            23, 6, 19, 10, 16, 5, 10, 14, 16, 14, 12, 16, -1, -1, -1, -1,
            8, 16, 3, 16, 5, 3, 5, 14, 3, 14, 5, 10, 6, 19, 23, -1,
            15, 2, 19, 2, 6, 19, 21, 16, 5, -1, -1, -1, -1, -1, -1, -1,
            16, 5, 21, 6, 8, 0, 2, 6, 0, 19, 8, 6, -1, -1, -1, -1,
            2, 6, 15, 6, 19, 15, 12, 5, 1, 12, 16, 5, -1, -1, -1, -1,
            8, 2, 6, 19, 8, 6, 8, 1, 2, 5, 8, 16, 8, 5, 1, -1,
            16, 5, 21, 6, 13, 10, 6, 19, 13, 19, 15, 13, -1, -1, -1, -1,
            10, 6, 13, 6, 19, 13, 19, 0, 13, 0, 19, 8, 16, 5, 21, -1,
            10, 12, 16, 5, 10, 16, 10, 15, 12, 19, 10, 6, 10, 19, 15, -1,
            10, 6, 19, 8, 10, 19, 10, 16, 5, 10, 8, 16, -1, -1, -1, -1,
            5, 21, 6, 21, 23, 6, 21, 20, 23, -1, -1, -1, -1, -1, -1, -1,
            23, 6, 3, 3, 6, 0, 6, 5, 0, 5, 21, 0, -1, -1, -1, -1,
            20, 23, 12, 23, 5, 12, 5, 1, 12, 23, 6, 5, -1, -1, -1, -1,
            3, 23, 6, 5, 3, 6, 1, 3, 5, -1, -1, -1, -1, -1, -1, -1,
            10, 14, 13, 23, 5, 21, 20, 23, 21, 6, 5, 23, -1, -1, -1, -1,
            3, 23, 0, 23, 6, 0, 6, 21, 0, 21, 6, 5, 10, 14, 13, -1,
            5, 20, 23, 6, 5, 23, 5, 12, 20, 14, 5, 10, 5, 14, 12, -1,
            3, 23, 6, 5, 3, 6, 3, 10, 14, 3, 5, 10, -1, -1, -1, -1,
            21, 20, 5, 20, 2, 5, 2, 6, 5, 2, 20, 15, -1, -1, -1, -1,
            6, 5, 21, 0, 6, 21, 2, 6, 0, -1, -1, -1, -1, -1, -1, -1,
            20, 5, 1, 12, 20, 1, 20, 6, 5, 2, 20, 15, 20, 2, 6, -1,
            6, 5, 1, 6, 1, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            6, 15, 13, 10, 6, 13, 6, 20, 15, 21, 6, 5, 6, 21, 20, -1,
            0, 13, 10, 6, 0, 10, 0, 5, 21, 0, 6, 5, -1, -1, -1, -1,
            20, 15, 12, 10, 6, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            6, 5, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            22, 17, 23, 23, 17, 19, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            22, 17, 23, 17, 19, 23, 0, 3, 8, -1, -1, -1, -1, -1, -1, -1,
            19, 23, 17, 23, 22, 17, 12, 9, 1, -1, -1, -1, -1, -1, -1, -1,
            17, 19, 22, 19, 23, 22, 1, 8, 9, 1, 3, 8, -1, -1, -1, -1,
            14, 13, 23, 13, 19, 23, 13, 17, 19, -1, -1, -1, -1, -1, -1, -1,
            3, 8, 0, 19, 14, 13, 17, 19, 13, 23, 14, 19, -1, -1, -1, -1,
            17, 19, 9, 19, 14, 9, 14, 12, 9, 19, 23, 14, -1, -1, -1, -1,
            14, 17, 19, 23, 14, 19, 14, 9, 17, 8, 14, 3, 14, 8, 9, -1,
            22, 17, 2, 17, 15, 2, 17, 19, 15, -1, -1, -1, -1, -1, -1, -1,
            0, 2, 8, 2, 17, 8, 17, 19, 8, 17, 2, 22, -1, -1, -1, -1,
            1, 12, 9, 15, 22, 17, 19, 15, 17, 2, 22, 15, -1, -1, -1, -1,
            2, 8, 9, 1, 2, 9, 2, 19, 8, 17, 2, 22, 2, 17, 19, -1,
            17, 15, 13, 17, 19, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            19, 8, 0, 13, 19, 0, 17, 19, 13, -1, -1, -1, -1, -1, -1, -1,
            15, 12, 9, 17, 15, 9, 19, 15, 17, -1, -1, -1, -1, -1, -1, -1,
            19, 8, 9, 19, 9, 17, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            4, 20, 17, 20, 22, 17, 20, 23, 22, -1, -1, -1, -1, -1, -1, -1,
            4, 0, 17, 0, 23, 17, 23, 22, 17, 0, 3, 23, -1, -1, -1, -1,
            9, 1, 12, 22, 4, 20, 23, 22, 20, 17, 4, 22, -1, -1, -1, -1,
            4, 23, 22, 17, 4, 22, 4, 3, 23, 1, 4, 9, 4, 1, 3, -1,
            13, 17, 14, 17, 20, 14, 20, 23, 14, 20, 17, 4, -1, -1, -1, -1,
            23, 4, 0, 3, 23, 0, 23, 17, 4, 13, 23, 14, 23, 13, 17, -1,
            17, 14, 12, 9, 17, 12, 17, 23, 14, 20, 17, 4, 17, 20, 23, -1,
            17, 4, 9, 3, 23, 14, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            22, 17, 2, 2, 17, 15, 17, 4, 15, 4, 20, 15, -1, -1, -1, -1,
            2, 22, 17, 4, 2, 17, 0, 2, 4, -1, -1, -1, -1, -1, -1, -1,
            2, 22, 15, 22, 17, 15, 17, 20, 15, 20, 17, 4, 9, 1, 12, -1,
            2, 22, 17, 4, 2, 17, 2, 9, 1, 2, 4, 9, -1, -1, -1, -1,
            17, 4, 20, 15, 17, 20, 13, 17, 15, -1, -1, -1, -1, -1, -1, -1,
            17, 4, 0, 17, 0, 13, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            17, 4, 20, 15, 17, 20, 17, 12, 9, 17, 15, 12, -1, -1, -1, -1,
            17, 4, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            19, 23, 16, 23, 21, 16, 23, 22, 21, -1, -1, -1, -1, -1, -1, -1,
            3, 8, 0, 19, 21, 16, 19, 23, 21, 23, 22, 21, -1, -1, -1, -1,
            23, 22, 1, 16, 23, 1, 12, 16, 1, 23, 16, 19, -1, -1, -1, -1,
            16, 1, 3, 8, 16, 3, 16, 22, 1, 23, 16, 19, 16, 23, 22, -1,
            19, 23, 16, 16, 23, 21, 23, 14, 21, 14, 13, 21, -1, -1, -1, -1,
            16, 19, 21, 19, 23, 21, 23, 13, 21, 13, 23, 14, 3, 8, 0, -1,
            16, 19, 23, 14, 16, 23, 12, 16, 14, -1, -1, -1, -1, -1, -1, -1,
            16, 19, 23, 14, 16, 23, 16, 3, 8, 16, 14, 3, -1, -1, -1, -1,
            22, 21, 2, 21, 19, 2, 19, 15, 2, 21, 16, 19, -1, -1, -1, -1,
            19, 22, 21, 16, 19, 21, 19, 2, 22, 0, 19, 8, 19, 0, 2, -1,
            22, 19, 15, 2, 22, 15, 22, 16, 19, 12, 22, 1, 22, 12, 16, -1,
            2, 22, 1, 16, 19, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            13, 21, 16, 19, 13, 16, 15, 13, 19, -1, -1, -1, -1, -1, -1, -1,
            13, 21, 16, 19, 13, 16, 13, 8, 0, 13, 19, 8, -1, -1, -1, -1,
            15, 12, 16, 15, 16, 19, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            19, 8, 16, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            20, 22, 21, 20, 23, 22, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            21, 0, 3, 23, 21, 3, 22, 21, 23, -1, -1, -1, -1, -1, -1, -1,
            22, 1, 12, 20, 22, 12, 23, 22, 20, -1, -1, -1, -1, -1, -1, -1,
            22, 1, 3, 22, 3, 23, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            23, 14, 13, 21, 23, 13, 20, 23, 21, -1, -1, -1, -1, -1, -1, -1,
            21, 0, 3, 23, 21, 3, 21, 14, 13, 21, 23, 14, -1, -1, -1, -1,
            23, 14, 12, 23, 12, 20, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            23, 14, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            20, 15, 2, 22, 20, 2, 21, 20, 22, -1, -1, -1, -1, -1, -1, -1,
            2, 22, 21, 2, 21, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            20, 15, 2, 22, 20, 2, 20, 1, 12, 20, 22, 1, -1, -1, -1, -1,
            2, 22, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            20, 15, 13, 20, 13, 21, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            13, 21, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            20, 15, 12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        };

        // represents an edges corner ray
        // 12 rays. 2 for each edge from opposing corners
        public static readonly CornerRay[] CornerRayFromEdge =
        {
            new() {Corner = 0, Origin = new float3(0, 0, 0), Direction = Axis.Direction.Forward}, // 0
            new() {Corner = 1, Origin = new float3(0, 0, 1), Direction = Axis.Direction.Right}, // 1
            new() {Corner = 3, Origin = new float3(1, 0, 0), Direction = Axis.Direction.Forward}, // 2
            new() {Corner = 0, Origin = new float3(0, 0, 0), Direction = Axis.Direction.Right}, // 3

            new() {Corner = 4, Origin = new float3(0, 1, 0), Direction = Axis.Direction.Forward}, // 4
            new() {Corner = 5, Origin = new float3(0, 1, 1), Direction = Axis.Direction.Right}, // 5
            new() {Corner = 7, Origin = new float3(1, 1, 0), Direction = Axis.Direction.Forward}, // 6
            new() {Corner = 4, Origin = new float3(0, 1, 0), Direction = Axis.Direction.Right}, // 7

            new() {Corner = 0, Origin = new float3(0, 0, 0), Direction = Axis.Direction.Up}, // 8
            new() {Corner = 1, Origin = new float3(0, 0, 1), Direction = Axis.Direction.Up}, // 9
            new() {Corner = 2, Origin = new float3(1, 0, 1), Direction = Axis.Direction.Up}, // 10
            new() {Corner = 3, Origin = new float3(1, 0, 0), Direction = Axis.Direction.Up}, // 11

            new() {Corner = 1, Origin = new float3(0, 0, 1), Direction = Axis.Direction.Back}, // -0
            new() {Corner = 2, Origin = new float3(1, 0, 1), Direction = Axis.Direction.Left}, // -1
            new() {Corner = 2, Origin = new float3(1, 0, 1), Direction = Axis.Direction.Back}, // -2
            new() {Corner = 3, Origin = new float3(1, 0, 0), Direction = Axis.Direction.Left}, // -3

            new() {Corner = 5, Origin = new float3(0, 1, 1), Direction = Axis.Direction.Back}, // -4
            new() {Corner = 6, Origin = new float3(1, 1, 1), Direction = Axis.Direction.Left}, // -5
            new() {Corner = 6, Origin = new float3(1, 1, 1), Direction = Axis.Direction.Back}, // -6
            new() {Corner = 7, Origin = new float3(1, 1, 0), Direction = Axis.Direction.Left}, // -7

            new() {Corner = 4, Origin = new float3(0, 1, 0), Direction = Axis.Direction.Down}, // -8
            new() {Corner = 5, Origin = new float3(0, 1, 1), Direction = Axis.Direction.Down}, // -9
            new() {Corner = 6, Origin = new float3(1, 1, 1), Direction = Axis.Direction.Down}, // -10
            new() {Corner = 7, Origin = new float3(1, 1, 0), Direction = Axis.Direction.Down}, // -11
        };
    }
}