using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.src.Models
{    public class Location
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Location(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public static Location GetLocation(string row, string column) => 
            new Location(GetRowCoord(row), GetColumnCoord(char.Parse(column)));
        

        /**
         * Gets the row coordinate from the string
         * @param y
         * @return int
         */
        public static int GetRowCoord(string y)
        {
            return int.Parse(y) - 1;
        }

        /**
         * Gets the column coordinate from the int
         * @param x
         * @return int
         */
        public static int GetColumnCoord(int x)
        {
            return x - 'A';
        }

        public override string ToString() => $"Row: {Row} / Column: {Column}";
    }
}
