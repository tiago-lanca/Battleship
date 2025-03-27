using Battleship.Models;
using Battleship.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Controllers
{
    public class GameController
    {
        #region Variables

        public Player? Player1 { get; set; }
        public Player? Player2 { get; set; }
        public string[]? GameInProgress_Players = new string[2];
        public bool GameInProgress = false;
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
                    if(words.Length > 4 && words.Length < 7)

                    break;

                case "RN":
                    break;

                case "T":
                    break;

                case "V":
                    view.PrintBoard(Player1, Player2);
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
            if (!GameInProgress)
            { 
                bool player1Exists = list.playersList.Exists(player => player.Name == player1);
                bool player2Exists = list.playersList.Exists(player => player.Name == player2);

                // Verifica se player 1 e player 2 estão registados
                if (player1Exists && player2Exists)
                {
                    Player1 = list.playersList.Find(player => player.Name == player1);
                    Player2 = list.playersList.Find(player => player.Name == player2);

                    // Filtra alfabeticamente os jogadores para jogo
                    SortByName_GameStart(player1, player2);

                    view.GameStartedSortedNames(SortByName_GameStart(player1, player2));
                    GameInProgress = true;
                }
                else
                    view.DisplayPlayerNotRegistered();
            }

            else
                view.DisplayGameInProgress();
        }


        public void Setup_Ship(Player player, string type, int row, int column, string orientation = null)
        {

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
                sortedNames[0] = Player1.Name;
                sortedNames[1] = Player2.Name;
            }
            else
            {
                sortedNames[0] = Player2.Name;
                sortedNames[1] = Player1.Name;
            }

            return sortedNames;
        }

        #endregion
    }
}
