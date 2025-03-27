using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Models
{
    public class Player
    {
        #region Variables
        public string? Name { get; set; }
        public int NumGames { get; set; }
        public int NumVitory { get; set; }
        #endregion

        public Player(string name)
        {
            name = Name;
            NumGames = 0;
            NumVitory = 0;
        }

        public override string ToString()
        {
            return $"{Name} {NumGames} {NumVitory}\n";
        }
    }
}
