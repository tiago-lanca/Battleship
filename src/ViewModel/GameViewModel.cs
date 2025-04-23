using Battleship.Controllers;
using Battleship.Interfaces;
using Battleship.Models;
using Battleship.Models.ShipsType;
using Battleship.src.Models;
using Battleship.Views;
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
        
        public Player? Player2 { get; set; }
        
        public bool Turn { get; set; } = true; // true = Player1, false = Player2
        public bool FirstShot { get; set; } = true;
        public string[]? GameInProgress_Players { get; set; } = new string[2];
        public bool GameInProgress { get; set; } = false;
        public bool CombatInitiated { get; set; } = false;

        private readonly ViewConsole _view;
        #endregion

        public GameViewModel(ViewConsole view)
        {
            _view = view;   
        }
        
        /**
         * Verifies if the player is in the active game
         * @param playerName
         * @return true if player is in game, false otherwise
         */
        public bool IsPlayerInGame(Player? player)
        {
            if(player is null) return false;

            foreach (string name in GameInProgress_Players)
            {
                if (name == player.Name) return true;
            }

            return false;
        }

        /**
         * Checks if the given name is the same as any of the players in the active game
         * @param name
         * @return Player object in the active game or null if not found
         */
        public Player? FindPlayerInGameByName(string name)
        {
            if (Player1 is null || Player2 is null)
                return null;

            return Player1.Name == name ? Player1 :
                   Player2.Name == name ? Player2 : null;
        }

        public Player? GetDefenderPlayer(Player attacker) => Player1?.Name == attacker.Name ? Player2 : Player1;
        
        // Returns true if the given player has ships to deploy, otherwise returns false
        public bool PlayerShipsToDeploy_Empty(Player player)
        {
            return player.Name == Player1.Name ? Player1.ShipsToDeploy.Count == 0 : Player2.ShipsToDeploy.Count == 0;
        }

        // Returns true if all ships of the players are deployed, otherwise returns false
        public bool AllPlayersShipsDeployed()
        {
            return Player1.ShipsToDeploy.Count == 0 && Player2.ShipsToDeploy.Count == 0;
        }

        // Verify if the ShipList of the given player is null, if so then create a new list and add the ship to it
        public void AddShip_ToPlayerList(Player player, Ship ship)
        {
            if (player == Player1)
            {
                // Verify if the ShipsToDeploy is null, instatiate a new list
                Player1.ShipsInGame ??= new List<Ship>();
                Player1.ShipsInGame.Add(ship);
            }
            else
            {
                Player2.ShipsInGame ??= new List<Ship>();
                Player2.ShipsInGame.Add(ship);
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
                player.ShipsInGame.Remove(defenderShip);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
       
        // Returns the list of ships, of the given player, to get deployed into the board in order to start the game
        public List<Ship> GetPlayerShipToDeployList(Player player)
        {
            return player.Name == Player1.Name ? Player1.ShipsToDeploy : Player2.ShipsToDeploy;
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
            foreach (var ship in defender.ShipsInGame)
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
            Player1.ShipsToDeploy.Clear();
            Player1.ShipsInGame.Clear();
            ClearPlayerGameStats(Player1);
            Player1 = null;

            Player2.ShipsToDeploy.Clear();
            Player2.ShipsInGame.Clear();
            ClearPlayerGameStats(Player2);
            Player2 = null;            

            GameInProgress_Players = new string[2];
            GameInProgress = false;
            CombatInitiated = false;
            FirstShot = true;
            
        }

        public bool VerifySurroundings(Player player, Location initLocation, Ship ship, Direction? direction = Direction.None)
        {
            Ship nextSpace;
            Location newLocation;

            if (direction == Direction.None)
            {

                nextSpace = player.OwnBoard[initLocation.Row, initLocation.Column];
                // Verify if the next space is empty
                if (nextSpace != null)
                {
                    _view.InvalidPosition();
                    return false;
                }

                return IsEmptyAround(player, initLocation);
            }

            switch (direction)
            {
                case Direction.E:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        newLocation = new Location(initLocation.Row, initLocation.Column + i); // Right

                        if (!IsInLimits(newLocation))
                        {
                            _view.InvalidPosition();
                            return false;
                        }

                        nextSpace = player.OwnBoard[initLocation.Row, initLocation.Column];
                        if (nextSpace != null)
                        {
                            _view.InvalidPosition();
                            return false;
                        }

                        if (!IsEmptyAround(player, initLocation))
                            return false;
                    }
                    return true;

                case Direction.N:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        newLocation = new Location(initLocation.Row - 1, initLocation.Column); // Up

                        if (!IsInLimits(newLocation))
                        {
                            _view.InvalidPosition();
                            return false;
                        }

                        nextSpace = player.OwnBoard[initLocation.Row, initLocation.Column];
                        // Verify if the next space is empty
                        if (nextSpace != null)
                        {
                            _view.InvalidPosition();
                            return false;
                        }

                        if (!IsEmptyAround(player, initLocation))
                            return false;
                    }
                    return true;

                case Direction.S:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        newLocation = new Location(initLocation.Row + 1, initLocation.Column); // Down

                        if (!IsInLimits(newLocation))
                        {
                            _view.InvalidPosition();
                            return false;
                        }

                        nextSpace = player.OwnBoard[initLocation.Row, initLocation.Column];
                        // Verify if the next space is empty
                        if (nextSpace != null)
                        {
                            _view.InvalidPosition();
                            return false;
                        }

                        if (!IsEmptyAround(player, initLocation))
                            return false;
                    }
                    return true;

                case Direction.O:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        newLocation = new Location(initLocation.Row, initLocation.Column - i); // Left

                        if (!IsInLimits(newLocation))
                        {
                            _view.InvalidPosition();
                            return false;
                        }

                        nextSpace = player.OwnBoard[initLocation.Row, initLocation.Column];
                        // Verify if the next space is empty
                        if (nextSpace != null)
                        {
                            _view.InvalidPosition();
                            return false;
                        }

                        if (!IsEmptyAround(player, initLocation))
                            return false;
                    }
                    return true;
                    

                default:
                    _view.InvalidPosition();
                    return false;
            }

        }
        public bool IsEmptyAround(Player player, Location location)
        {
            // Check positions around
            var positionsToCheck = new (int, int)[]
            {
                        (location.Row - 1, location.Column),
                        (location.Row + 1, location.Column),
                        (location.Row, location.Column + 1),
                        (location.Row, location.Column - 1),
                        (location.Row - 1, location.Column - 1),
                        (location.Row - 1, location.Column + 1),
                        (location.Row + 1, location.Column - 1),
                        (location.Row + 1, location.Column +1)
            };

            foreach (var (r, col) in positionsToCheck)
            {
                if (IsInLimits(new Location(r, col)) && player.OwnBoard[r, col] != null)
                {
                    _view.InvalidPosition();
                    return false;
                }
            }

            return true;
        }

        public void RegistMissedShot(Player attacker, Player defender, Location attackLocation)
        {
            attacker.AddShotStats(shots: 1);
            attacker.AttackBoard[attackLocation.Row, attackLocation.Column] = new Ship(ShipType.Miss, null, Direction.None, Code.Miss);
            defender.OwnBoard[attackLocation.Row, attackLocation.Column] = new Ship(ShipType.Miss, null, Direction.None, Code.Miss);
        }

        public void RegistHitShot(Player attacker, Player defender, Location attackLocation)
        {
            attacker.AddShotStats(shots: 1, shotsOnTargets: 1);
            attacker.AttackBoard[attackLocation.Row, attackLocation.Column] = new Ship(ShipType.Hit, null, Direction.None, Code.Hit);
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

        // Change the [Turn] variable true/false
        public void ChangeTurn() => Turn = !Turn;

        public void SetupPlayersIntoGame(Player player1, Player player2)
        {
            Player1 = player1; 
            Player2 = player2;
            GameInProgress_Players[0] = player1.Name;
            GameInProgress_Players[1] = player2.Name;

            // Setup Player 1
            Player1.OwnBoard = new Ship[10, 10];
            Player1.AttackBoard = new Ship[10, 10];
            Player1.ShipsToDeploy ??= new List<Ship>();
            Player1.ShipsToDeploy.AddRange(new List<Ship>
            {
                new Speedboat(ShipType.Speedboat, null, Direction.None, Code.Speedboat),
                new Submarine(ShipType.Submarine, null, Direction.None, Code.Submarine),
                new Frigate(ShipType.Frigate, null, Direction.None, Code.Frigate),
                new Cruiser(ShipType.Cruiser, null, Direction.None, Code.Cruiser),
                new Aircraft_Carrier(ShipType.Aircraft_Carrier, null, Direction.None, Code.Aircraft_Carrier)
            });

            // Setup Player 2
            Player2.OwnBoard = new Ship[10, 10];
            Player2.AttackBoard = new Ship[10, 10];
            Player2.ShipsToDeploy ??= new List<Ship>();
            Player2.ShipsToDeploy.AddRange(new List<Ship>
            {
                new Speedboat(ShipType.Speedboat, null, Direction.None, Code.Speedboat),
                new Submarine(ShipType.Submarine, null, Direction.None, Code.Submarine),
                new Frigate(ShipType.Frigate, null, Direction.None, Code.Frigate),
                new Cruiser(ShipType.Cruiser, null, Direction.None, Code.Cruiser),
                new Aircraft_Carrier(ShipType.Aircraft_Carrier, null, Direction.None, Code.Aircraft_Carrier)
            });
        }

        public void ClearPlayerGameStats(Player player)
        {
            player.Shots = 0;
            player.ShotsOnTargets = 0;
            player.EnemySunkShips = 0;
        }

        public bool IsInLimits(Location location) =>
           location.Row >= 0 && location.Row < 10 && location.Column >= 0 && location.Column < 10;

        public bool IsInRowLimits(int row) =>
            row >= 0 && row < 10;

        public bool IsInColumnLimits(int column) =>
            column >= 0 && column < 10;

    }
}
