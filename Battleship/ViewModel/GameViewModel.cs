using Battleship.Interfaces;
using Battleship.Models;
using Battleship.Models.ShipsType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Models.Ship;

namespace Battleship.ViewModel
{
    public class GameViewModel : IGameViewModel
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
        public void AddShip_ToPlayerList(Player player, Ship ship)
        {
            if (player == Player1)
            {
                // Is the ShipsToDeploy is null, instatiate a new list
                Player1.Ships ??= new List<Ship>();
                Player1.Ships.Add(ship);
            }
            else
            {
                Player2.Ships ??= new List<Ship>();
                Player2.Ships.Add(ship);
            }
        }

        public void RemoveShip_InPlayerList(Ship defenderShip, Player player)
        {
            foreach (var location in defenderShip.Location)
            {
                player.OwnBoard[location.Row, location.Column] = null;
            }

            try
            {
                player.Ships.Remove(defenderShip);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public List<Ship> GetPlayerShipToDeployList(Player player, GameViewModel gameVM)
        {
            return player.Name == gameVM.Player1.Name ? gameVM.Player1_ShipsToDeploy : gameVM.Player2_ShipsToDeploy;
        }
        
        public void RemoveShipToDeploy(ShipType type, Player player, GameViewModel gameVM)
        {
            GetPlayerShipToDeployList(player, gameVM).Remove(GetShipByType(type, player, gameVM));
        }
        public string GetShipType_PT(Player defender, Location attackLocation)
        {
            Ship ship = defender.OwnBoard[attackLocation.Row, attackLocation.Column];

            switch (ship)
            {
                case Speedboat:
                    return "Lancha";
                case Submarine:
                    return "Submarino";
                case Frigate:
                    return "Fragata";
                case Cruiser:
                    return "Cruzador";
                case Aircraft_Carrier:
                    return "Porta Aviões";
            }

            return default;
        }
        public Ship GetShipByType(ShipType type, Player player, GameViewModel gameVM)
        {
            List<Ship> shipList = GetPlayerShipToDeployList(player, gameVM);

            return shipList.FirstOrDefault(ship => ship.Type == type);
        }
        public bool IsFinished(Player defender)
        {
            return defender.Ships.Count == 0;
        }

        public bool FindPlayer_InProgressGame(string name)
        {
            if (GameInProgress_Players is not null)
            {
                foreach (string player in GameInProgress_Players)
                {
                    if (player == name)
                        return true;
                }
            }

            return false;
        }
    }
}
