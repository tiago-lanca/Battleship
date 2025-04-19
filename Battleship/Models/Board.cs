using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Models
{
    public class Board
    {
        #region Variables
        public readonly char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'};
        public readonly int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        #endregion
    }
}
