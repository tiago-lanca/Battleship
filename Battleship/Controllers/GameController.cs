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

        public Board board = new Board();

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

                case "IJ":
                    break;

                case "IC":
                    break;

                case "D":
                    break;

                case "CN":
                    break;

                case "RN":
                    break;

                case "T":
                    break;

                case "V":
                    view.PrintBoard(board);
                    break;

                case "Xclear":
                    Console.Clear();
                    break;

                default:
                    view.InvalidInstruction(); 
                    break;
            }
        }

        bool _HasRequiredInputs(int nr_inputs, int nr_reqInputs)
        {
            return nr_inputs == nr_reqInputs;
        }

        #endregion
    }
}
