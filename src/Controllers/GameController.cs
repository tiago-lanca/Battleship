﻿using Battleship.Interfaces;
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
            if (_gameVM is null || !_gameVM.GameInProgress)
            {
                bool player1Exists = playerList.Exists(player => player.Name == player1);
                bool player2Exists = playerList.Exists(player => player.Name == player2);

                // Verifica se player 1 e player 2 estão registados
                if (player1Exists && player2Exists)
                {                    
                   
                    _gameVM.Player1 = playerList.Find(player => player.Name == player1);
                    _gameVM.Player2 = playerList.Find(player => player.Name == player2);

                    _gameVM.GameInProgress_Players[0] = player1;
                    _gameVM.GameInProgress_Players[1] = player2;

                    // Filtra alfabeticamente os jogadores para jogo
                    SortByName_GameStart(player1, player2);

                    view.GameStartedSortedNames(SortByName_GameStart(player1, player2));
                    _gameVM.GameInProgress = true;

                    _gameVM.Player1.OwnBoard = new Ship[10, 10];
                    _gameVM.Player1.AttackBoard = new Ship[10, 10];

                    _gameVM.Player2.OwnBoard = new Ship[10, 10];
                    _gameVM.Player2.AttackBoard = new Ship[10, 10];

                    _gameVM.Player1_ShipsToDeploy ??= new List<Ship>();
                    _gameVM.Player1_ShipsToDeploy.AddRange(new List<Ship>
                    {
                        new Speedboat(ShipType.Speedboat, null, Direction.None, 1, "L"),
                        new Submarine(ShipType.Submarine, null, Direction.None, 1, "S"),
                        new Frigate(ShipType.Frigate, null, Direction.None, 1, "F"),
                        new Cruiser(ShipType.Cruiser, null, Direction.None, 1, "C"),
                        new Aircraft_Carrier(ShipType.Aircraft_Carrier, null, Direction.None, 1, "P")
                    });

                    _gameVM.Player2_ShipsToDeploy ??= new List<Ship>();
                    _gameVM.Player2_ShipsToDeploy.AddRange(new List<Ship>
                    {
                        new Speedboat(ShipType.Speedboat, null, Direction.None, 2, "L"),
                        new Submarine(ShipType.Submarine, null, Direction.None, 2, "S"),
                        new Frigate(ShipType.Frigate, null, Direction.None, 2, "F"),
                        new Cruiser(ShipType.Cruiser, null, Direction.None, 2, "C"),
                        new Aircraft_Carrier(ShipType.Aircraft_Carrier, null, Direction.None, 2, "P")
                    });
                }
                else
                    view.DisplayPlayerNotRegistered();
            }

            else
                view.DisplayGameInProgress();
        }

        public void Setup_Ship(Player player, string type, string row, string column, string orientation = null)
        {
            if (_gameVM.GameInProgress)
            {
                if (_gameVM.IsPlayerInGame(player))
                {
                    if (!_gameVM.PlayerShipsToDeploy_Empty(player))
                    {
                        var initLocation = new Location(GetRowCoord(row), GetColumnCoord(char.Parse(column)));
                        List<Ship> playerShipsToDeployList = _gameVM.GetPlayerShipToDeployList(player);
                        int remainingShips;
                        bool emptyAround;

                        switch (type)
                        {
                            case "L": // Speeboat
                                var speedboat = new Speedboat();
                                remainingShips = speedboat.GetRemainingQuantity(playerShipsToDeployList);

                                if (remainingShips > 0)
                                {
                                    // Verify if surroundings are empty spaces
                                    emptyAround = VerifySurroundings(player, initLocation, speedboat);

                                    if (emptyAround)
                                    {
                                        // Creates Speedboat for the player                                        
                                        Speedboat playerShip = new Speedboat(
                                            ShipType.Speedboat,
                                            new List<Location>(speedboat.AddLocations(initLocation)),
                                            speedboat.GetDirection(orientation),
                                            _playerManager.GetPlayerTeam(player),
                                            speedboat.Code
                                        );

                                        InsertShip_InPlayer_OwnBoard(playerShip, initLocation, player);
                                        _gameVM.AddShip_ToPlayerList(player, playerShip);

                                        // Remove Quantity of ship type available to deploy and checks if it is 0.
                                        // If is 0, then removes from the ShipsToDeploy list
                                        playerShip.RemoveQuantityToDeploy(playerShipsToDeployList);

                                        view.ShipDeployed_Success();
                                    }
                                }
                                else
                                {
                                    view.Unavailable_ShipsTypeToDeploy();
                                }
                                break;

                            case "S": // Submarine
                                var submarine = new Submarine();
                                remainingShips = submarine.GetRemainingQuantity(playerShipsToDeployList);

                                // Verify if there is available ships to deploy of that type
                                if (remainingShips > 0)
                                {
                                    // Verify if surroundings are empty spaces
                                    emptyAround = VerifySurroundings(player, initLocation, submarine, orientation);

                                    if (emptyAround)
                                    {
                                        // Creates submarine for the player
                                        Submarine playerShip = new Submarine(
                                            ShipType.Submarine,
                                            new List<Location>(submarine.AddLocations(initLocation, orientation)),
                                            submarine.GetDirection(orientation),
                                            _playerManager.GetPlayerTeam(player),
                                            submarine.Code
                                            );

                                        InsertShip_InPlayer_OwnBoard(playerShip, initLocation, player, orientation);
                                        _gameVM.AddShip_ToPlayerList(player, playerShip);

                                        // Remove Quantity of ship type available to deploy. If it's 0 remaining to deploy,
                                        // then remove from the ShipsToDeploy list
                                        playerShip.RemoveQuantityToDeploy(playerShipsToDeployList);

                                        view.ShipDeployed_Success();
                                    }
                                }
                                else
                                {
                                    view.Unavailable_ShipsTypeToDeploy();
                                }

                                break;

                            case "F": // Frigate
                                var frigate = new Frigate();
                                remainingShips = frigate.GetRemainingQuantity(playerShipsToDeployList);

                                // Verify if there is available ships to deploy of that type
                                if (remainingShips > 0)
                                {
                                    // Verify if surroundings are empty spaces
                                    emptyAround = VerifySurroundings(player, initLocation, frigate, orientation);

                                    if (emptyAround)
                                    {
                                        // Creates submarine for the player
                                        Frigate playerShip = new Frigate(
                                            ShipType.Frigate,
                                            new List<Location>(frigate.AddLocations(initLocation, orientation)),
                                            frigate.GetDirection(orientation),
                                            _playerManager.GetPlayerTeam(player),
                                            frigate.Code
                                            );

                                        InsertShip_InPlayer_OwnBoard(playerShip, initLocation, player, orientation);
                                        _gameVM.AddShip_ToPlayerList(player, playerShip);

                                        // Remove Quantity of ship available to deploy
                                        playerShip.RemoveQuantityToDeploy(playerShipsToDeployList);

                                        view.ShipDeployed_Success();
                                    }
                                }
                                else
                                {
                                    view.Unavailable_ShipsTypeToDeploy();
                                }

                                break;

                            case "C": // Cruiser
                                var cruiser = new Cruiser();
                                remainingShips = cruiser.GetRemainingQuantity(playerShipsToDeployList);

                                // Verify if there is available ships to deploy of that type
                                if (remainingShips > 0)
                                {
                                    // Verify if surroundings are empty spaces
                                    emptyAround = VerifySurroundings(player, initLocation, cruiser, orientation);

                                    if (emptyAround)
                                    {
                                        // Creates cruiser for the player
                                        Cruiser playerShip = new Cruiser(
                                            ShipType.Cruiser,
                                            new List<Location>(cruiser.AddLocations(initLocation, orientation)),
                                            cruiser.GetDirection(orientation),
                                            _playerManager.GetPlayerTeam(player),
                                            cruiser.Code
                                            );

                                        InsertShip_InPlayer_OwnBoard(playerShip, initLocation, player, orientation);
                                        _gameVM.AddShip_ToPlayerList(player, playerShip);

                                        // Remove Quantity of ship available to deploy
                                        playerShip.RemoveQuantityToDeploy(playerShipsToDeployList);

                                        view.ShipDeployed_Success();
                                    }
                                }
                                else
                                {
                                    view.Unavailable_ShipsTypeToDeploy();
                                }

                                break;

                            case "P": // Aircraft_Carrier
                                var aircraft_carrier = new Aircraft_Carrier();
                                remainingShips = aircraft_carrier.GetRemainingQuantity(playerShipsToDeployList);

                                // Verify if there is available ships to deploy of that type
                                if (remainingShips > 0)
                                {
                                    // Verify if surroundings are empty spaces
                                    emptyAround = VerifySurroundings(player, initLocation, aircraft_carrier, orientation);

                                    if (emptyAround)
                                    {
                                        // Creates submarine for the player
                                        Aircraft_Carrier playerShip = new Aircraft_Carrier(
                                            ShipType.Aircraft_Carrier,
                                            new List<Location>(aircraft_carrier.AddLocations(initLocation, orientation)),
                                            aircraft_carrier.GetDirection(orientation),
                                            _playerManager.GetPlayerTeam(player),
                                            aircraft_carrier.Code
                                            );

                                        InsertShip_InPlayer_OwnBoard(playerShip, initLocation, player, orientation);
                                        _gameVM.AddShip_ToPlayerList(player, playerShip);

                                        // Remove Quantity of ship available to deploy
                                        playerShip.RemoveQuantityToDeploy(playerShipsToDeployList);

                                        view.ShipDeployed_Success();
                                    }
                                }
                                else
                                    view.Unavailable_ShipsTypeToDeploy();

                                break;
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
        private void InsertShip_InPlayer_OwnBoard(Ship ship, Location initLocation, Player player, string orientation = null)
        {
            switch (orientation) {

                case "E":
                    for (int i = 0; i < ship.Size; i++)
                    {
                        player.OwnBoard[initLocation.Row, initLocation.Column + i] = ship.CloneShip();
                    }
                    break;

                case "N":
                    for (int i = 0; i < ship.Size; i++)
                    {
                        player.OwnBoard[initLocation.Row - i, initLocation.Column] = ship.CloneShip();
                    }
                    break;

                case "S":
                    for (int i = 0; i < ship.Size; i++)
                    {
                        player.OwnBoard[initLocation.Row + i, initLocation.Column] = ship.CloneShip();
                    }
                    break;

                case "O":
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
                            ship.RemoveOnBoard(player);

                            if (_gameVM.GetPlayerShipToDeployList(player).Find(ship => ship.Type == ship.Type) is null)
                            {
                                _gameVM.GetPlayerShipToDeployList(player).Add(ship);                                
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

                        if (_gameVM.FirstShot || _playerManager.CheckTurn(player))
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

                                    attacker.Shots++;
                                    attacker.ShotsOnTargets++;
                                    attacker.AttackBoard[attackLocation.Row, attackLocation.Column] = new Ship(ShipType.Hit, null, Direction.None, _playerManager.GetPlayerTeam(attacker), "X");

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
                                    attacker.AttackBoard[attackLocation.Row, attackLocation.Column] = new Ship(ShipType.Miss, null, Direction.None, _playerManager.GetPlayerTeam(attacker), "*");
                                    defender.OwnBoard[attackLocation.Row, attackLocation.Column] = new Ship(ShipType.Miss, null, Direction.None, _playerManager.GetPlayerTeam(defender), "*");
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
        private bool VerifySurroundings(Player player, Location initLocation, Ship ship, string orientation = null)
        {

            if (orientation == null)
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

            switch (orientation)
            {
                case "E":
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
                    
                case "N":
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

                case "S":
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

                case "O":
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
            if (player2 is null)
            {
                Player playerLoss = player1;
                Player playerWin = _gameVM.Player1.Name == player1.Name ? _gameVM.Player2 : _gameVM.Player1;
                
                playerWin.NumGames++;
                playerWin.NumVictory++;
                playerLoss.NumGames++;
                
            }
            else
            {
                player1.NumGames++;
                player2.NumGames++;
            }


            _gameVM.ResetGameViewModel();
            //_playerController.UpdateGameViewModel(_gameVM);
            //_playerController.UpdateGameViewModel(_gameVM);
            Console.WriteLine("Desistência com sucesso. Jogo terminado.\n");
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
         * Verifies if the number of inputs is correct
         * @param nr_inputs
         * @param nr_reqInputs
         * @return true if the number of inputs is correct, false otherwise
         */
        

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
