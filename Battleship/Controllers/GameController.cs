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

        public GameViewModel Game;
        public ViewConsole view = new ViewConsole();
        bool combatInitiated = false;

        #endregion

        #region Functions
        public void CheckCommand(string command, ViewConsole view, PlayerList playersList)
        {
            string[] words = command.Split(' ');

            switch (words[0])
            {
                case "RJ": // Register Player
                    if (_HasRequiredInputs(words.Length, 2))
                        playersList.RegisterPlayer(words[1]);
                    else view.InvalidInstruction();
                    break;

                case "EJ": // Remove Player
                    if (_HasRequiredInputs(words.Length, 2))
                        playersList.RemovePlayer(words[1]);
                    else view.InvalidInstruction();
                    break;

                case "LJ": // List Players
                    if (_HasRequiredInputs(words.Length, 1))
                        playersList.ShowAllPlayers();
                    else view.InvalidInstruction();
                    break;

                case "IJ": // Initiate Game
                    if (_HasRequiredInputs(words.Length, 3))
                        StartGame(words[1], words[2], playersList);
                    else view.InvalidInstruction();
                    break;

                case "IC": // Initiate Combat
                    if (_HasRequiredInputs(words.Length, 1))
                        InitiateCombat();
                    else view.InvalidInstruction();
                    break;

                case "D": // Forfeit
                    if (_HasRequiredInputs(words.Length, 2))
                        ForfeitGame(FindPlayer(words[1]));
                    else if (_HasRequiredInputs(words.Length, 3))
                        ForfeitGame(FindPlayer(words[1]), FindPlayer(words[2]));
                    else
                        view.InvalidInstruction();
                    break;

                case "CN": // Colocar Navios
                    if (words.Length == 5)
                        Setup_Ship(FindPlayer(words[1]), words[2], words[3], words[4]);
                    else if (words.Length == 6)
                        Setup_Ship(FindPlayer(words[1]), words[2], words[3], words[4], words[5]);
                    else
                        view.InvalidInstruction();
                    break;

                case "RN": // Remove Ship
                    if(_HasRequiredInputs(words.Length, 4))
                        RemoveShip(FindPlayer(words[1]), words[2], words[3]);
                    else
                        view.InvalidInstruction();
                    break;

                case "T":
                    break;

                case "V":
                    view.PrintAttackBoard(Game.Player1, Game.Player2);
                    break;

                case "XO": //Print Own Board of each player
                    view.PrintOwnBoard(Game.Player1, Game.Player2);
                    break;

                case "Xclear":
                    Console.Clear();
                    break;

                default:
                    view.InvalidInstruction();
                    break;
            }
        }

        private void StartGame(string player1, string player2, PlayerList list)
        {
            if (Game is null || !Game.IsInProgress)
            {
                bool player1Exists = list.playersList.Exists(player => player.Name == player1);
                bool player2Exists = list.playersList.Exists(player => player.Name == player2);

                // Verifica se player 1 e player 2 estão registados
                if (player1Exists && player2Exists)
                {
                    Game = new GameViewModel();

                    Game.Player1 = list.playersList.Find(player => player.Name == player1);
                    Game.Player2 = list.playersList.Find(player => player.Name == player2);

                    Game.GameInProgress_Players[0] = player1;
                    Game.GameInProgress_Players[1] = player2;

                    // Filtra alfabeticamente os jogadores para jogo
                    SortByName_GameStart(player1, player2);

                    view.GameStartedSortedNames(SortByName_GameStart(player1, player2));
                    Game.IsInProgress = true;

                    Game.Player1.OwnBoard = new Ship[10, 10];
                    Game.Player1.AttackBoard = new Ship[10, 10];

                    Game.Player2.OwnBoard = new Ship[10, 10];
                    Game.Player2.AttackBoard = new Ship[10, 10];

                    Game.Player1_ShipsToDeploy ??= new List<Ship>();
                    Game.Player1_ShipsToDeploy.AddRange(new List<Ship>
                    {
                        new Speedboat(ShipType.Speedboat, null, null, 1, "L"),
                        new Submarine(ShipType.Submarine, null, null, 1, "S"),
                        new Frigate(ShipType.Frigate, null, null, 1, "F"),
                        new Cruiser(ShipType.Cruiser, null, null, 1, "C"),
                        new Aircraft_Carrier(ShipType.Aircraft_Carrier, null, null, 1, "P")
                    });

                    Game.Player2_ShipsToDeploy ??= new List<Ship>();
                    Game.Player2_ShipsToDeploy.AddRange(new List<Ship>
                    {
                        new Speedboat(ShipType.Speedboat, null, null, 2, "L"),
                        new Submarine(ShipType.Submarine, null, null, 2, "S"),
                        new Frigate(ShipType.Frigate, null, null, 2, "F"),
                        new Cruiser(ShipType.Cruiser, null, null, 2, "C"),
                        new Aircraft_Carrier(ShipType.Aircraft_Carrier, null, null, 2, "P")
                    });
                }
                else
                    view.DisplayPlayerNotRegistered();
            }

            else
                view.DisplayGameInProgress();
        }

        private void Setup_Ship(Player player, string type, string row, string column, string orientation = null)
        {
            if (Game.IsInProgress)
            {
                if (IsPlayerInGame(player.Name))
                {
                    if (!Game.PlayerShipsToDeploy_Empty(player))
                    {
                        var initLocation = new Location(GetRowCoord(row), GetColumnCoord(char.Parse(column)));
                        int remainingShips;
                        bool emptyAround;

                        switch (type)
                        {
                            case "L": // Speeboat
                                var speedboat = new Speedboat();
                                remainingShips = speedboat.GetRemainingQuantity(ShipType.Speedboat, player, Game);

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
                                            null,
                                            GetPlayerTeam(player),
                                            speedboat.Code
                                        );

                                        InsertShip_InPlayer_OwnBoard(playerShip, initLocation, player);
                                        playerShip.RemoveQuantity(ShipType.Speedboat, player, Game);

                                        if(playerShip.GetRemainingQuantity(ShipType.Speedboat, player, Game) == 0)
                                            playerShip.GetPlayerShipToDeployList(player, Game).Remove(playerShip);

                                        view.ShipDeployed_Success();
                                    }
                                }
                                break;

                            case "S": // Submarine
                                var submarine = new Submarine();
                                remainingShips = submarine.GetRemainingQuantity(ShipType.Submarine, player, Game);

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
                                            orientation,
                                            GetPlayerTeam(player),
                                            submarine.Code
                                            );

                                        InsertShip_InPlayer_OwnBoard(playerShip, initLocation, player, orientation);
                                        // Remove Quantity of ship available to deploy
                                        playerShip.RemoveQuantity(ShipType.Submarine, player, Game);

                                        if (playerShip.GetRemainingQuantity(ShipType.Submarine, player, Game) == 0)
                                            playerShip.GetPlayerShipToDeployList(player, Game).Remove(playerShip);


                                        view.ShipDeployed_Success();
                                    }
                                }
                                else
                                {
                                    // Remove the submarine category from player's ShipList to deploy
                                    submarine.RemoveShipToDeploy(ShipType.Submarine, player, Game);

                                    view.Unavailable_ShipsTypeToDeploy();
                                }

                                break;

                            case "F": // Frigate
                                var frigate = new Frigate();
                                remainingShips = frigate.GetRemainingQuantity(ShipType.Frigate, player, Game);

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
                                            orientation,
                                            GetPlayerTeam(player),
                                            frigate.Code
                                            );

                                        InsertShip_InPlayer_OwnBoard(playerShip, initLocation, player, orientation);
                                        // Remove Quantity of ship available to deploy
                                        playerShip.RemoveQuantity(ShipType.Frigate, player, Game);

                                        if (playerShip.GetRemainingQuantity(ShipType.Frigate, player, Game) == 0)
                                            playerShip.GetPlayerShipToDeployList(player, Game).Remove(playerShip);


                                        view.ShipDeployed_Success();
                                    }
                                }
                                else
                                {
                                    // Remove the submarine category from player's ShipList to deploy
                                    frigate.RemoveShipToDeploy(ShipType.Frigate, player, Game);

                                    view.Unavailable_ShipsTypeToDeploy();
                                }

                                break;

                            case "C": // Cruiser
                                var cruiser = new Cruiser();
                                remainingShips = cruiser.GetRemainingQuantity(ShipType.Cruiser, player, Game);

                                // Verify if there is available ships to deploy of that type
                                if (remainingShips > 0)
                                {
                                    // Verify if surroundings are empty spaces
                                    emptyAround = VerifySurroundings(player, initLocation, cruiser, orientation);

                                    if (emptyAround)
                                    {
                                        // Creates submarine for the player
                                        Cruiser playerShip = new Cruiser(
                                            ShipType.Cruiser,
                                            new List<Location>(cruiser.AddLocations(initLocation, orientation)),
                                            orientation,
                                            GetPlayerTeam(player),
                                            cruiser.Code
                                            );

                                        InsertShip_InPlayer_OwnBoard(playerShip, initLocation, player, orientation);
                                        // Remove Quantity of ship available to deploy
                                        playerShip.RemoveQuantity(ShipType.Cruiser, player, Game);

                                        if (playerShip.GetRemainingQuantity(ShipType.Cruiser, player, Game) == 0)
                                            playerShip.GetPlayerShipToDeployList(player, Game).Remove(playerShip);


                                        view.ShipDeployed_Success();
                                    }
                                }
                                else
                                {
                                    // Remove the submarine category from player's ShipList to deploy
                                    cruiser.RemoveShipToDeploy(ShipType.Cruiser, player, Game);

                                    view.Unavailable_ShipsTypeToDeploy();
                                }

                                break;

                            case "P": // Aircraft_Carrier
                                var aircraft_carrier = new Aircraft_Carrier();
                                remainingShips = aircraft_carrier.GetRemainingQuantity(ShipType.Aircraft_Carrier, player, Game);

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
                                            orientation,
                                            GetPlayerTeam(player),
                                            aircraft_carrier.Code
                                            );

                                        InsertShip_InPlayer_OwnBoard(playerShip, initLocation, player, orientation);
                                        // Remove Quantity of ship available to deploy
                                        playerShip.RemoveQuantity(ShipType.Aircraft_Carrier, player, Game);

                                        if (playerShip.GetRemainingQuantity(ShipType.Aircraft_Carrier, player, Game) == 0)
                                        {
                                            Ship shipToRemove = playerShip.GetPlayerShipToDeployList(player, Game).OfType<Aircraft_Carrier>().FirstOrDefault();
                                            playerShip.GetPlayerShipToDeployList(player, Game).Remove(shipToRemove);
                                        }


                                        view.ShipDeployed_Success();
                                    }
                                }
                                else
                                {
                                    // Remove the submarine category from player's ShipList to deploy
                                    aircraft_carrier.RemoveShipToDeploy(ShipType.Aircraft_Carrier, player, Game);

                                    view.Unavailable_ShipsTypeToDeploy();
                                }

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
                        player.OwnBoard[initLocation.Row, initLocation.Column + i] = ship;
                    }
                    break;

                case "N":
                    for (int i = 0; i < ship.Size; i++)
                    {
                        player.OwnBoard[initLocation.Row - i, initLocation.Column] = ship;
                    }
                    break;

                case "S":
                    for (int i = 0; i < ship.Size; i++)
                    {
                        player.OwnBoard[initLocation.Row + i, initLocation.Column] = ship;
                    }
                    break;

                case "O":
                    for (int i = 0; i < ship.Size; i++)
                    {
                        player.OwnBoard[initLocation.Row, initLocation.Column - i] = ship;
                    }
                    break;

                case null:
                    player.OwnBoard[initLocation.Row, initLocation.Column] = ship;
                    break;
            }
        }

        private void RemoveShip(Player player, string row, string column)
        {
            var initLocation = new Location(GetRowCoord(row), GetColumnCoord(char.Parse(column)));

            if (Game.IsInProgress)
            {
                if (!combatInitiated)
                {
                    if(IsPlayerInGame(player.Name))
                    {
                        Ship ship = player.OwnBoard[initLocation.Row, initLocation.Column];
                        if (ship is not null)
                        {

                            foreach(var location in ship.Location)
                            {
                                player.OwnBoard[location.Row, location.Column] = null;
                            }


                            if (ship.GetPlayerShipToDeployList(player, Game).Find(s => s.Type == ship.Type) is null)
                            {
                                Ship newship = ship.CreateNewShip();
                                ship.GetPlayerShipToDeployList(player, Game).Add(ship);                                
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
        private void InitiateCombat()
        {
            if (Game.IsInProgress)
            {
                if (Game.AllPlayersShipsDeployed())
                {
                    view.DisplayCombatInitiate();
                    combatInitiated = true;
                }
                else
                {
                    view.NeededShipsToDeploy();
                }
            }
            else 
            { 
                view.DisplayGameNotInProgress(); 
            }
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
        
        private void ForfeitGame(Player player1, Player player2 = null)
        {
            if (player2 is null)
            {
                Player playerLoss = player1;
                Player playerWin = Game.Player1.Name == player1.Name ? Game.Player2 : Game.Player1;
                

                playerWin.NumGames++;
                playerWin.NumVictory++;
                playerLoss.NumGames++;

                
            }
            else
            {
                player1.NumGames++;
                player2.NumGames++;
            }
            
            Game = null;
            Console.WriteLine("Desistência com sucesso. Jogo terminado.\n");


        }
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
                //Console.WriteLine((r,col));
                if (IsInLimits(r, col) && player.OwnBoard[r, col] != null)
                {
                    view.InvalidPosition();
                    return false;
                }
            }

            return true;
        }

        public bool IsInLimits(int row, int column)
        {
            return row >= 0 && row < 10 && column >= 0 && column < 10;
        }
        
        public bool IsInRowLimits(int row)
        {
            return row >= 0 && row < 10;
        }

        public bool IsInColumnLimits(int column)
        {
            return column >= 0 && column < 10;
        }

        /*public bool IsEmptySpace(Player player, int row, int column)
        {
            return player.OwnBoard[row, column] is null;
        }*/

        public bool _HasRequiredInputs(int nr_inputs, int nr_reqInputs)
        {
            return nr_inputs == nr_reqInputs;
        }

        public string[] SortByName_GameStart(string player1, string player2)
        {
            string[] sortedNames = new string[2];

            if (player1.ToUpper()[0] < player2.ToUpper()[0])
            {
                sortedNames[0] = Game.Player1.Name;
                sortedNames[1] = Game.Player2.Name;
            }
            else
            {
                sortedNames[0] = Game.Player2.Name;
                sortedNames[1] = Game.Player1.Name;
            }

            return sortedNames;
        }
        public bool IsPlayerInGame(string playerName)
        {
            foreach(string name in Game.GameInProgress_Players)
            {
                if(name == playerName) return true;
            }

            return false;
        }

        public Player FindPlayer(string name)
        {
            return Game.Player1.Name == name ? Game.Player1 : Game.Player2;
        }

        public int GetRowCoord(string y)
        {
            return int.Parse(y) - 1;
        }

        public int GetColumnCoord(int x)
        {
            return x - 'A';
        }

        public int GetPlayerTeam(Player player)
        {
            return player.Name == Game.GameInProgress_Players[0] ? 1 : 2;
        }

        #endregion
    }
}
