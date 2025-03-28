using Battleship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.ViewModel
{
    public class GameViewModel
    {
        public Player? Player1 { get; set; }
        public List<Ship>? Player1_ShipsToDeploy { get; set; }
        public Player? Player2 { get; set; }
        public List<Ship>? Player2_ShipsToDeploy { get; set; }        
        public string[]? GameInProgress_Players = new string[2];
        public bool IsInProgress = false;

    }
}
