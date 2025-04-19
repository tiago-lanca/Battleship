using Battleship.Controllers;
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
        #region Variables
        public Player? Player1 { get; set; }
        public List<Ship>? Player1_ShipsToDeploy { get; set; }
        public Player? Player2 { get; set; }
        public List<Ship>? Player2_ShipsToDeploy { get; set; }

        public bool Turn = true; // true = Player1, false = Player2
        public bool FirstShot = true;
        public string[]? GameInProgress_Players = new string[2];
        public bool GameInProgress = false;
        public bool CombatInitiated = false;
        #endregion

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
        public List<Ship> GetPlayerShipToDeployList(Player player)
        {
            return player.Name == Player1.Name ? Player1_ShipsToDeploy : Player2_ShipsToDeploy;
        }
        
        public void RemoveShipToDeploy(ShipType type, Player player, GameViewModel gameVM)
        {
            GetPlayerShipToDeployList(player).Remove(GetShipByType(type, player, gameVM));
        }
        public string GetShipType_PT(Ship ship)
        {     
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
            List<Ship> shipList = GetPlayerShipToDeployList(player);

            return shipList.FirstOrDefault(ship => ship.Type == type);
        }
        public bool IsFinished(Player defender)
        {
            foreach (var ship in defender.Ships)
            {
                if(ship.State is ShipState.Sunk)
                    continue;
                else
                    return false;
            }
            return true;
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

        /**
         * Resets the GameViewModel data to its initial state to avoid creating a new instance.
         */
        public void ResetGameViewModel()
        {
            Player1 = null;
            Player1_ShipsToDeploy = null;
            Player2 = null;
            Player2_ShipsToDeploy = null;
            GameInProgress_Players = new string[2];
            GameInProgress = false;
            CombatInitiated = false;
            FirstShot = true;
            
        }

        public void ManageTurn(Player player)
        {
            if (FirstShot)
            {
                // Defining turn based on the player who started the game
                FirstShot = false;
                Turn = GameInProgress_Players[0] == player.Name ? true : false;
            }
            else ChangeTurn();
        }

        public void ChangeTurn() => Turn = !Turn;
    }
}
