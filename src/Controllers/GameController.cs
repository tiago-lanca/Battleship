using Battleship.Interfaces;
using Battleship.Models;
using Battleship.src.Models;
using Battleship.ViewModel;
using Battleship.Views;

namespace Battleship.Controllers
{  
    public class GameController
    {
        #region Variables
                
        private readonly ViewConsole view = new ViewConsole();
        private readonly IGameViewModel _gameVM;
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

        public void Setup_Ship(Player player, Code type, Location initLocation, Direction direction = Direction.None)
        {
            if (_gameVM.GameInProgress)
            {
                if (_gameVM.IsPlayerInGame(player))
                {
                    if (!_gameVM.PlayerShipsToDeploy_Empty(player))
                    {
                        List<Ship> playerShipsToDeployList = _gameVM.GetPlayerShipToDeployList(player);

                        var ship = Ship.CreateNewShip(type);
                        if(ship is null) { view.InvalidInstruction(); return; }

                        // Verify if there is available ships to deploy of that type
                        if (ship.GetRemainingQuantity(playerShipsToDeployList) > 0)
                        {                            
                            // Verify if surroundings are empty spaces
                            bool emptyAround = _gameVM.VerifySurroundings(player, initLocation, ship, direction);

                            if (emptyAround)
                            {
                                // Creates submarine for the player
                                ship = Ship.CreateNewShip(
                                    type: ship.Type,
                                    shipLocations: ship.AddLocations(initLocation, direction),
                                    direction: direction,
                                    code: ship.Code
                                );

                                InsertShip_InPlayer_OwnBoard(ship!, initLocation, player, direction);
                                _gameVM.AddShip_ToPlayerList(player, ship!);

                                // Remove Quantity of ship type available to deploy. If it's 0 remaining to deploy,
                                // then remove from the ShipsToDeploy list
                                ship!.RemoveQuantityToDeploy(playerShipsToDeployList);

                                view.ShipDeployed_Success();
                            }
                        }
                        else
                            view.Unavailable_ShipsTypeToDeploy();
                        
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
                        player.OwnBoard[initLocation.Row, initLocation.Column + i] = ship.CloneShip();
                    
                    break;

                case Direction.N:
                    for (int i = 0; i < ship.Size; i++)
                        player.OwnBoard[initLocation.Row - i, initLocation.Column] = ship.CloneShip();
                    
                    break;

                case Direction.S:
                    for (int i = 0; i < ship.Size; i++)
                        player.OwnBoard[initLocation.Row + i, initLocation.Column] = ship.CloneShip();
                    
                    break;

                case Direction.O:
                    for (int i = 0; i < ship.Size; i++)
                        player.OwnBoard[initLocation.Row, initLocation.Column - i] = ship.CloneShip();
                    
                    break;

                case null:
                    player.OwnBoard[initLocation.Row, initLocation.Column] = ship.CloneShip();
                    break;
            }
        }
        public void RemoveShip(Player player, Location initLocation)
        {
            if (_gameVM.GameInProgress)
            {
                if (!_gameVM.CombatInitiated)
                {
                    if(_gameVM.IsPlayerInGame(player))
                    {
                        Ship ship = player.OwnBoard[initLocation.Row, initLocation.Column];

                        if (ship is not null)
                        {
                            ship.RemoveShip(player, _gameVM);
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
        public void MakeAttack(Player attacker, Location attackLocation) 
        {
            if (_gameVM.GameInProgress)
            {
                if (_gameVM.CombatInitiated)
                {
                    if (_gameVM.IsPlayerInGame(attacker))
                    {
                        if (_gameVM.FirstShot || _playerManager.IsPlayerTurn(attacker))
                        {                           
                            // Verify if the attack location is valid
                            if (_gameVM.IsInLimits(attackLocation))
                            {
                                // Getting the defender player and respective ship
                                Player? defender = _gameVM.GetDefenderPlayer(attacker);
                                Ship? defenderShip = defender != null ? Ship.GetShipFromOwnBoard(defender, attackLocation) : null;

                                // Verify if the attack location has Ship
                                if (defenderShip is not null && defenderShip.IsHit(defender, attackLocation))
                                {
                                    // Checks is the attackLocation is already hit and the ship is partially sunk
                                    if (defenderShip.State is State.Sunk)
                                    { view.DisplayAlreadySunkShip(defenderShip, _gameVM); return; }

                                    _gameVM.RegistHitShot(attacker, defender, attackLocation);
                                    defenderShip.ChangeShipState(attackLocation, defender);

                                    // Verify if the ship is sunk
                                    if (defenderShip.Is_ShipSunk(defender))
                                    {
                                        attacker.AddShotStats(enemySunkShips: 1);

                                        // Checks if the game is finished, if all ships of the defender are sunk
                                        if (_gameVM.IsFinished(defender))
                                        {
                                            _playerManager.AddNrGames(attacker, defender);

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
                                    _gameVM.RegistMissedShot(attacker, defender, attackLocation);
                                    view.DisplayMissedShot();
                                }

                                _gameVM.ManageTurn(attacker);
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

        

        #endregion
    }
}
