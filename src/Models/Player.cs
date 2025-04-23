using Battleship.ViewModel;
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
        public string Name { get; set; }
        public int NumGames { get; set; }
        public int NumVictory { get; set; }
        public List<Ship> ShipsInGame { get; set; }
        public List<Ship>? ShipsToDeploy { get; set; }
        public Ship[,] OwnBoard { get; set; }
        public Ship[,] AttackBoard { get; set; }
        public int Shots { get; set; }
        public int ShotsOnTargets { get; set; }
        public int EnemySunkShips { get; set; }
        #endregion

        public Player(string name)
        {
            Name = name;
            NumGames = 0;
            NumVictory = 0;
        }

        public void AddShotStats(int shots = 0, int shotsOnTargets = 0, int  enemySunkShips = 0)
        {
            Shots += shots;
            ShotsOnTargets += shotsOnTargets;
            EnemySunkShips += enemySunkShips;
        }

        public override string ToString()
        {
            return $"{Name} {NumGames} {NumVictory}";
        }
    }
}
