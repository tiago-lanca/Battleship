using Battleship.Controllers;
using Battleship.Interfaces;
using Battleship.Models;
using Battleship.Models.ShipsType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        public bool Turn { get; set; } = true; // true = Player1, false = Player2
        public bool FirstShot { get; set; } = true;
        public string[]? GameInProgress_Players { get; set; } = new string[2];
        public bool GameInProgress { get; set; } = false;
        public bool CombatInitiated { get; set; } = false;
        #endregion

        /**
         * Verifies if the player is in the active game
         * @param playerName
         * @return true if player is in game, false otherwise
         */
        public bool IsPlayerInGame(Player player)
        {
            foreach (string name in GameInProgress_Players)
            {
                if (name == player.Name) return true;
            }

            return false;
        }

        // Returns true if the given player has ships to deploy, otherwise returns false
        public bool PlayerShipsToDeploy_Empty(Player player)
        {
            return player.Name == Player1.Name ? Player1_ShipsToDeploy.Count == 0 : Player2_ShipsToDeploy.Count == 0;
        }

        // Returns true if all ships of the players are deployed, otherwise returns false
        public bool AllPlayersShipsDeployed()
        {
            return Player1_ShipsToDeploy.Count == 0 && Player2_ShipsToDeploy.Count == 0;
        }

        // Verify if the ShipList of the given player is null, if so then create a new list and add the ship to it
        public void AddShip_ToPlayerList(Player player, Ship ship)
        {
            if (player == Player1)
            {
                // Verify if the ShipsToDeploy is null, instatiate a new list
                Player1.Ships ??= new List<Ship>();
                Player1.Ships.Add(ship);
            }
            else
            {
                Player2.Ships ??= new List<Ship>();
                Player2.Ships.Add(ship);
            }
        }

        // Remove the ship from the player ShipList and remove the ship from the board
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
       
        // Returns the list of ships, of the given player, to get deployed into the board in order to start the game
        public List<Ship> GetPlayerShipToDeployList(Player player)
        {
            return player.Name == Player1.Name ? Player1_ShipsToDeploy : Player2_ShipsToDeploy;
        }
        
        // Returns the name of the given ship
        public string? GetShipTypeName(Ship ship)
        {
            return ship switch
            {
                Speedboat => "Lancha",
                Submarine => "Submarino",
                Frigate => "Fragata",
                Cruiser => "Cruzador",
                Aircraft_Carrier => "Porta Aviões",
                _ => default,
            };
        }
        
        // Verify if every ship of the defender player is sunk, if so then return true to finish the game,
        // otherwise return false
        public bool IsFinished(Player defender)
        {
            foreach (var ship in defender.Ships)
            {
                if(ship.State is State.Sunk)
                    continue;
                else
                    return false;
            }
            return true;
        }

        /// Verify if the player is in the current active game
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

        // Manage the turn of the players to verify who is going to play and manage the [FirstShot] variable
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

        // Change the [Turn] variable true-false
        public void ChangeTurn() => Turn = !Turn;

        
    }
}
