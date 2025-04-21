using Battleship.Interfaces;
using Battleship.Models;
using Battleship.Models.ShipsType;
using Battleship.ViewModel;
using Battleship.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Models.Ship;

namespace Battleship.Controllers
{  
    public class GameController
    {
        #region Variables

        private readonly IGameViewModel _gameVM = new GameViewModel();
        private readonly ViewConsole view = new ViewConsole(); 
        private readonly IPlayerManager _playerManager;

        public GameController(IGameViewModel gameVM, IPlayerManager playerManager)
        {
            _gameVM = gameVM;
            _playerManager = playerManager;
        }

        #endregion

        #region Functions
        
        public void StartGame(string player1, string player2, List<Player> playerList)
        {
            if (!_gameVM.GameInProgress)
            {
                Player? Player1 = playerList.Find(player => player.Name == player1);
                Player? Player2 = playerList.Find(player => player.Name == player2);

                // Check if both players are registered in the active PlayerList
                if (Player1 is not null && Player2 is not null)
                {
                    // Setup in a function the gameVM data //

                    _gameVM.SetupPlayersIntoGame(Player1, Player2);

                    // Sort the players by name and print the message
                    view.MessageGameStart(SortByName_GameStart(player1, player2));
                    _gameVM.GameInProgress = true;
                }
                else
                    view.DisplayPlayerNotRegistered();
            }

            else
                view.DisplayGameInProgress();
        }

        public void Setup_Ship(Player player, string type, string row, string column, Direction direction = Direction.None)
        {
            if (_gameVM.GameInProgress)
            {
                if (_gameVM.IsPlayerInGame(player))
                {
                    if (!_gameVM.PlayerShipsToDeploy_Empty(player))
                    {
                        var initLocation = new Location(GetRowCoord(row), GetColumnCoord(char.Parse(column)));
                        List<Ship> playerShipsToDeployList = _gameVM.GetPlayerShipToDeployList(player);

                        var ship = Ship.CreateNewShip(type);
                        if(ship is null) { view.InvalidInstruction(); return; }

                        // Verify if there is available ships to deploy of that type
                        if (ship.GetRemainingQuantity(playerShipsToDeployList) > 0)
                        {
                            // Verify if surroundings are empty spaces
                            bool emptyAround = VerifySurroundings(player, initLocation, ship, direction);

                            if (emptyAround)
                            {
                                // Creates submarine for the player
                                ship = ship.CreateNewShip(
                                    type: ship.Type,
                                    shipLocations: ship.AddLocations(initLocation, direction),
                                    direction: direction,
                                    code: ship.Code
                                 );

                                InsertShip_InPlayer_OwnBoard(ship, initLocation, player, direction);
                                _gameVM.AddShip_ToPlayerList(player, ship);

                                // Remove Quantity of ship type available to deploy. If it's 0 remaining to deploy,
                                // then remove from the ShipsToDeploy list
                                ship.RemoveQuantityToDeploy(playerShipsToDeployList);

                                view.ShipDeployed_Success();
                            }
                        }
                        else
                        {
                            view.Unavailable_ShipsTypeToDeploy();
                        }
                    }
                    else
                        view.DisplayPlayerShipListEmpty();
                }
                else
                    view.DisplayPlayerNotInProgressGame();
            }
            else
                view.DisplayGameNotInProgress();
        }
        private void InsertShip_InPlayer_OwnBoard(Ship ship, Location initLocation, Player player, Direction? direction = null)
        {
            switch (direction) {

                case Direction.E:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        player.OwnBoard[initLocation.Row, initLocation.Column + i] = ship.CloneShip();
                    }
                    break;

                case Direction.N:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        player.OwnBoard[initLocation.Row - i, initLocation.Column] = ship.CloneShip();
                    }
                    break;

                case Direction.S:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        player.OwnBoard[initLocation.Row + i, initLocation.Column] = ship.CloneShip();
                    }
                    break;

                case Direction.O:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        player.OwnBoard[initLocation.Row, initLocation.Column - i] = ship.CloneShip();
                    }
                    break;

                case null:
                    player.OwnBoard[initLocation.Row, initLocation.Column] = ship.CloneShip();
                    break;
            }
        }
        public void RemoveShip(Player player, string row, string column)
        {
            var initLocation = new Location(GetRowCoord(row), GetColumnCoord(char.Parse(column)));

            if (_gameVM.GameInProgress)
            {
                if (!_gameVM.CombatInitiated)
                {
                    if(_gameVM.IsPlayerInGame(player))
                    {
                        Ship ship = player.OwnBoard[initLocation.Row, initLocation.Column];
                        if (ship is not null)
                        {
                            List<Ship> playerShipsToDeployList = _gameVM.GetPlayerShipToDeployList(player);
                            ship.RemoveOnBoard(player);

                            if (playerShipsToDeployList.Find(s => s.Type == ship.Type) is null)
                            {
                                playerShipsToDeployList.Add(ship);                                
                            }

                            view.DisplayShipRemovedSuccess();

                        }
                        else
                            view.DisplayShipNotFound();
                    }
                    else
                        view.DisplayPlayerNotInProgressGame();
                }
                else
                    view.DisplayCombatInitiate();
            }
            else
                view.DisplayGameNotInProgress();
        }
        public void InitiateCombat()
        {
            if (_gameVM.GameInProgress)
            {
                if (_gameVM.AllPlayersShipsDeployed())
                {
                    view.DisplayCombatInitiate();
                    _gameVM.CombatInitiated = true;
                }
                else
                    view.NeededShipsToDeploy();
            }
            else 
                view.DisplayGameNotInProgress(); 
        }
        public void MakeAttack(Player player, string row, string column) 
        {
            if (_gameVM.GameInProgress)
            {
                if (_gameVM.CombatInitiated)
                {
                    if (_gameVM.IsPlayerInGame(player))
                    {

                        if (_gameVM.FirstShot || _playerManager.IsPlayerTurn(player))
                        {
                            Location attackLocation = new Location(
                                GetRowCoord(row),
                                GetColumnCoord(char.Parse(column))
                            );

                            // Verify if the attack location is valid
                            if (IsInLimits(attackLocation.Row, attackLocation.Column))
                            {
                                Player attacker = player;
                                Player defender = (_gameVM.Player1!.Name == player.Name ? _gameVM.Player2 : _gameVM.Player1)!;
                                Ship defenderShip = defender.OwnBoard[attackLocation.Row, attackLocation.Column];

                                // Verify if the attack location has Ship
                                if (defenderShip is not null && defenderShip.IsHit(defender, attackLocation))
                                {
                                    // Checks is the attackLocation is already hit and the ship is partially sunk
                                    if (defenderShip.State is State.Sunk)
                                    { view.DisplayAlreadySunkShip(defenderShip, _gameVM); return; }
                                    
                                    attacker.Shots++;
                                    attacker.ShotsOnTargets++;
                                    attacker.AttackBoard[attackLocation.Row, attackLocation.Column] = new Ship(ShipType.Hit, null, Direction.None, Code.Hit);

                                    defenderShip.ChangeShipState(attackLocation, defender);

                                    // Verify if the ship is sunk
                                    if (defenderShip.Is_ShipSunk(defender))
                                    {
                                        attacker.EnemySunkShips++;

                                        // Checks if the game is finished, if all ships of the defender are sunk
                                        if (_gameVM.IsFinished(defender))
                                        {
                                            _playerManager.AddStatsToPlayers(attacker, defender);

                                            _gameVM.ResetGameViewModel();

                                            // Resets the InGameStats (ShipList, Nr Shots, AttackBoard, OwnBoard) of the given player
                                            _playerManager.ResetInGameStats(attacker);
                                            _playerManager.ResetInGameStats(defender);

                                            view.ShipSunk_GameFinished(defenderShip, _gameVM);
                                        }
                                        else
                                            view.DisplaySunkShip(defenderShip, _gameVM);
                                    }
                                    else
                                        view.DisplayShipHit(defenderShip, _gameVM);
                                }

                                // When the shot is a miss, in the water.
                                else
                                {
                                    attacker.Shots++;
                                    attacker.AttackBoard[attackLocation.Row, attackLocation.Column] = new Ship(ShipType.Miss, null, Direction.None, Code.Miss);
                                    defender.OwnBoard[attackLocation.Row, attackLocation.Column] = new Ship(ShipType.Miss, null, Direction.None, Code.Miss);
                                    Console.WriteLine("Tiro na água.\n");
                                }

                                _gameVM.ManageTurn(player);
                            }

                            else
                                view.InvalidPosition();
                        }
                        else
                            view.DisplayPlayerNotTurn();
                    }
                    else
                        view.DisplayPlayerNotInProgressGame();
                }
                else
                    view.DisplayCombatNotInitiate();
            }
            else
                view.DisplayGameNotInProgress();
        }
        private bool VerifySurroundings(Player player, Location initLocation, Ship ship, Direction? direction = Direction.None)
        {

            if (direction == Direction.None)
            {
                int row = initLocation.Row;
                int column = initLocation.Column;

                var space = player.OwnBoard[row, column];
                // Verify if the next space is empty
                if (space != null)
                {
                    view.InvalidPosition();
                    return false;
                }

                return IsEmptyAround(player, row, column);
                
            }

            switch (direction)
            {
                case Direction.E:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        int row = initLocation.Row;
                        int column = initLocation.Column + i; // Right

                        if (!IsInLimits(row, column))
                        {
                            view.InvalidPosition();
                            return false;
                        }

                        var nextSpace = player.OwnBoard[row, column];
                        if (nextSpace != null)
                        {
                            view.InvalidPosition();
                            return false;
                        }

                        if(!IsEmptyAround(player, row, column)) 
                            return false;
                    }
                    return true;
                    
                case Direction.N:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        int row = initLocation.Row - i; // Up
                        int column = initLocation.Column;

                        if (!IsInLimits(row, column))
                        {
                            view.InvalidPosition();
                            return false;
                        }

                        var nextSpace = player.OwnBoard[row, column];
                        // Verify if the next space is empty
                        if (nextSpace != null)
                        {
                            view.InvalidPosition();
                            return false;
                        }

                        if (!IsEmptyAround(player, row, column))
                            return false;
                    }
                    return true;

                case Direction.S:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        int row = initLocation.Row + i; // Down
                        int column = initLocation.Column;

                        if (!IsInLimits(row, column)) {
                            view.InvalidPosition();
                            return false; 
                        }

                        var nextSpace = player.OwnBoard[row, column];
                        // Verify if the next space is empty
                        if (nextSpace != null)
                        {
                            view.InvalidPosition();
                            return false;
                        }

                        if (!IsEmptyAround(player, row, column))
                            return false;
                    }
                    return true;

                case Direction.O:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        int row = initLocation.Row;
                        int column = initLocation.Column - i; // Left

                        if (!IsInLimits(row, column))
                        {
                            view.InvalidPosition();
                            return false;
                        }

                        var nextSpace = player.OwnBoard[row, column];
                        // Verify if the next space is empty
                        if (nextSpace != null)
                        {
                            view.InvalidPosition();
                            return false;
                        }

                        if (!IsEmptyAround(player, row, column))
                            return false;
                    }
                    return true;

                default:
                    view.InvalidPosition();
                    return false;
            }
            
        }

        public void ForfeitGame(Player player1, Player player2 = null)
        {
            if (_gameVM.GameInProgress)
            {
                if (_gameVM.IsPlayerInGame(player1))
                {
                    // Both players want to forfeit the game
                    if (player2 is not null && _gameVM.IsPlayerInGame(player2))
                    {
                        player1.NumGames++;
                        player2.NumGames++;
                    }

                    // Only one player wants to forfeit the game
                    else
                    {
                        Player playerLoss = player1;
                        Player playerWin = _gameVM.Player1.Name == player1.Name ? _gameVM.Player2 : _gameVM.Player1;

                        playerWin.NumGames++;
                        playerWin.NumVictory++;
                        playerLoss.NumGames++;
                    }

                    _gameVM.ResetGameViewModel();
                    view.MessageForfeitGame();
                }
                else view.DisplayPlayerNotInProgressGame();
            }
            else view.DisplayGameNotInProgress();
            
        }

        /**
          * Verifies if the positions around the given position are empty
          * @param player
          * @param row
          * @param column
          * @return true if the positions are empty, false otherwise
          */
        public bool IsEmptyAround(Player player, int row, int column)
        {
            // Check positions around
            var positionsToCheck = new (int, int)[]
            {
                        (row - 1, column),
                        (row + 1, column),
                        (row, column + 1),
                        (row, column - 1),
                        (row - 1, column - 1),
                        (row - 1, column + 1),
                        (row + 1, column - 1),
                        (row + 1, column +1)
            };

            foreach (var (r, col) in positionsToCheck)
            {
                if (IsInLimits(r, col) && player.OwnBoard[r, col] != null)
                {
                    view.InvalidPosition();
                    return false;
                }
            }

            return true;
        }

        public bool IsInLimits(int row, int column) => 
            row >= 0 && row < 10 && column >= 0 && column < 10;
                
        public bool IsInRowLimits(int row) =>
            row >= 0 && row < 10;
        
        public bool IsInColumnLimits(int column) =>
            column >= 0 && column < 10;


        /*public bool IsEmptySpace(Player player, int row, int column)
        {
            return player.OwnBoard[row, column] is null;
        }*/

        /**
         * Sorts the players by name
         * @param player1
         * @param player2
         * @return sortedNames
         */
        public string[] SortByName_GameStart(string player1, string player2)
        {
            string[] sortedNames = new string[2];

            if (player1.ToUpper()[0] < player2.ToUpper()[0])
            {
                sortedNames[0] = _gameVM.Player1.Name;
                sortedNames[1] = _gameVM.Player2.Name;
            }
            else
            {
                sortedNames[0] = _gameVM.Player2.Name;
                sortedNames[1] = _gameVM.Player1.Name;
            }

            return sortedNames;
        }

        /**
         * Gets the row coordinate from the string
         * @param y
         * @return int
         */
        public int GetRowCoord(string y)
        {
            return int.Parse(y) - 1;
        }

        /**
         * Gets the column coordinate from the int
         * @param x
         * @return int
         */
        public int GetColumnCoord(int x)
        {
            return x - 'A';
        }        

        #endregion
    }
}
