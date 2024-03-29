﻿using System.Runtime.Serialization;

namespace Pairs.InterfaceLibrary
{
    [DataContract]
    public sealed class GameLayout
    {
        public static GameLayout ThreeTimesTwo = new GameLayout(3, 2);
        public static GameLayout FourTimesThree = new GameLayout(4, 3);
        public static GameLayout FourTimesFour = new GameLayout(4, 4);
        public static GameLayout FiveTimesFour = new GameLayout(5, 4);
        public static GameLayout SixTimesFive = new GameLayout(6, 5);
        public static GameLayout SixTimesSix = new GameLayout(6, 6);
        public static GameLayout SevenTimesSix = new GameLayout(7, 6);
        public static GameLayout EightTimesSeven = new GameLayout(8, 7);
        public static GameLayout EightTimesEight = new GameLayout(8, 8);

        public static GameLayout[] GetValues()
        {
            return new[] { ThreeTimesTwo, FourTimesThree, FourTimesFour, FiveTimesFour, SixTimesFive, SixTimesSix, SevenTimesSix, EightTimesSeven, EightTimesEight };
        }

        [DataMember]
        public int ColumnCount { get; set; }

        [DataMember]
        public int RowCount { get; set; }

        private GameLayout(int columnCount, int rowCount)
        {
            ColumnCount = columnCount;
            RowCount = rowCount;
        }

        public override string ToString()
        {
            return $"{ColumnCount} x {RowCount}";
        }

    }
}
