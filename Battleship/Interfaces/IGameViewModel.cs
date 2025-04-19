using Battleship.Models;
using Battleship.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Interfaces
{
    public interface IGameViewModel
    {
        public bool PlayerShipsToDeploy_Empty(Player player);
        public bool AllPlayersShipsDeployed();
        public void AddShip_ToPlayerList(Player player, Ship ship);
        public void RemoveShip_InPlayerList(Ship defenderShip, Player player);
        public List<Ship> GetPlayerShipToDeployList(Player player, GameViewModel gameVM);
        public void RemoveShipToDeploy(Ship.ShipType type, Player player, GameViewModel gameVM);
        public string GetShipType_PT(Ship ship);
        public Ship GetShipByType(Ship.ShipType type, Player player, GameViewModel gameVM);
        public bool IsFinished(Player defender);
        public bool FindPlayer_InProgressGame(string name);
    }
}
