using Battleship.Models;
using Battleship.Models.ShipsType;
using Battleship.ViewModel;
using Battleship.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Models.Ship;

namespace Battleship.Controllers
{
    public class GameController
    {
        #region Variables

        public GameViewModel Game = new GameViewModel();
        public ViewConsole view = new ViewConsole();

        #endregion

        #region Functions
        public void CheckCommand(string command, ViewConsole view, PlayerList playersList)
        {
            string[] words = command.Split(' ');

            switch (words[0])
            {
                case "RJ": // Registar Jogador
                    if (_HasRequiredInputs(words.Length, 2))
                        playersList.RegisterPlayer(words[1]);
                    else view.InvalidInstruction();
                    break;

                case "EJ": // Eliminar Jogador
                    if (_HasRequiredInputs(words.Length, 2))
                        playersList.RemovePlayer(words[1]);
                    else view.InvalidInstruction();
                    break;

                case "LJ": // Listar Jogadores
                    if (_HasRequiredInputs(words.Length, 1))
                        playersList.ShowAllPlayers();
                    else view.InvalidInstruction();
                    break;

                case "IJ": // Iniciar Jogo
                    if (_HasRequiredInputs(words.Length, 3))
                        StartGame(words[1], words[2], playersList);
                    else view.InvalidInstruction();
                    break;

                case "IC":
                    break;

                case "D":
                    break;

                case "CN": // Colocar Navios
                    if (words.Length == 5)
                        Setup_Ship(FindPlayer(words[1]), words[2], words[3], words[4]);
                    else if (words.Length == 6)
                        Setup_Ship(FindPlayer(words[1]), words[2], words[3], words[4], words[5]);
                    else
                        view.InvalidInstruction();
                    break;

                case "RN":
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


        public void StartGame(string player1, string player2, PlayerList list)
        {
            if (!Game.IsInProgress)
            { 
                bool player1Exists = list.playersList.Exists(player => player.Name == player1);
                bool player2Exists = list.playersList.Exists(player => player.Name == player2);

                // Verifica se player 1 e player 2 estão registados
                if (player1Exists && player2Exists)
                {
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
                        new Speedboat(ShipType.Speedboat, null, null, 2, "L"),
                        new Submarine(ShipType.Submarine, null, null, 2, "S"),
                        new Frigate(ShipType.Frigate, null, null, 2, "F"),
                        new Cruiser(ShipType.Cruiser, null, null, 2, "C"),
                        new Aircraft_Carrier(ShipType.Aircraft_Carrier, null, null, 2, "P")
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


        public void Setup_Ship(Player player, string type, string row, string column, string orientation = null)
        {
            if (Game.IsInProgress)
            {
                if (IsPlayerInGame(player.Name))
                {
                    var initLocation = new Location(GetRowCoord(row), GetColumnCoord(char.Parse(column)));

                    switch (type)
                    {
                        case "L": // Speeboat
                            player.OwnBoard[initLocation.Row, initLocation.Column] =
                                new Speedboat(
                                    ShipType.Speedboat,
                                    new List<Location>
                                    {
                                        new Location(0,0)
                                    },
                                    orientation,
                                    GetPlayerTeam(player),
                                    "L"
                                );
                            break;

                        case "S": // Submarine
                            var submarine = new Submarine();
                            bool emptySurround = true;
                            if (orientation == "E") {

                                // Verify if surroundings are empty spaces
                                for (int i = 0; i < submarine.Size; i++)
                                {
                                    var nextSpace = player.OwnBoard[initLocation.Row, initLocation.Column + i];

                                    if(nextSpace != null)
                                    {
                                        if (player.OwnBoard[initLocation.Row - 1, initLocation.Column + i] is not null)
                                        {
                                            Console.WriteLine("Posição irregular.\n");
                                            emptySurround = false;
                                            break;
                                        }
                                    }
                                }

                                if (emptySurround)
                                {
                                    List<Location> locations = new List<Location>();

                                    // Add's locations for the ship Location
                                    for (int i = 0; i < submarine.Size; i++)
                                    {
                                        locations.Add(new Location(initLocation.Row, initLocation.Column + i));
                                    }

                                    Submarine playerSubmarine = new Submarine(
                                        ShipType.Submarine,
                                        locations,
                                        orientation,
                                        GetPlayerTeam(player),
                                        "S"
                                    );

                                    for (int i = 0; i < submarine.Size; i++)
                                    {
                                        player.OwnBoard[initLocation.Row, initLocation.Column + i] = playerSubmarine;
                                    }

                                    Console.WriteLine("Navio colocado com sucesso.\n");
                                    
                                }
                            }

                            
                            break;

                        case "F":
                            break;

                        case "C":
                            break;

                        case "P":
                            break;
                    }
                }

                else
                    view.DisplayPlayerNotInGameProgress();
            }
            else
                view.DisplayGameNotInProgress();
        }

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
