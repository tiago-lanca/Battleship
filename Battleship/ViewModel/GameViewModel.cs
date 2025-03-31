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

        public bool PlayerShipsToDeploy_Empty(Player player)
        {
            return player.Name == Player1.Name ? Player1_ShipsToDeploy.Count == 0 : Player2_ShipsToDeploy.Count == 0;
        }

        public bool AllPlayersShipsDeployed()
        {
            return Player1_ShipsToDeploy.Count == 0 && Player2_ShipsToDeploy.Count == 0;
        }

    }
}
