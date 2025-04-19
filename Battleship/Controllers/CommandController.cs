using Battleship.Interfaces;
using Battleship.ViewModel;
using Battleship.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Controllers
{
    public class CommandController
    {
        private readonly ViewConsole view = new ViewConsole();
        private GameViewModel _gameVM;
        private readonly IPlayerList _playerList;
        private readonly GameController _gameController;
        private readonly PlayerController _playerController;
        public CommandController(GameViewModel gameVM, GameController gameController, PlayerController playerController, IPlayerList playerList) 
        { 
            _gameVM = gameVM;
            _gameController = gameController;
            _playerController = playerController;
            _playerList = playerList;
        }

        public void CheckCommand(string command)
        {
            string[] words = command.Split(' ');

            switch (words[0])
            {
                case "RJ": // Register Player
                    if (_HasRequiredInputs(words.Length, 2))
                        _playerList.RegisterPlayer(words[1]);
                    else view.InvalidInstruction();
                    break;

                case "EJ": // Remove Player
                    if (_HasRequiredInputs(words.Length, 2))
                        _playerList.RemovePlayer(words[1], _gameVM);
                    else view.InvalidInstruction();
                    break;

                case "LJ": // List Players
                    if (_HasRequiredInputs(words.Length, 1))
                        _playerList.ShowAllPlayers();
                    else view.InvalidInstruction();
                    break;

                case "IJ": // Initiate Game
                    if (_HasRequiredInputs(words.Length, 3))
                        _gameController.StartGame(words[1], words[2], _playerList.GetPlayerList());
                    else view.InvalidInstruction();
                    break;

                case "IC": // Initiate Combat
                    if (_HasRequiredInputs(words.Length, 1))
                        _gameController.InitiateCombat();
                    else view.InvalidInstruction();
                    break;

                case "D": // Forfeit
                    if (_HasRequiredInputs(words.Length, 2))
                        _gameController.ForfeitGame(_playerController.FindPlayerByName(words[1]));
                    else if (_HasRequiredInputs(words.Length, 3))
                        _gameController.ForfeitGame(_playerController.FindPlayerByName(words[1]), _playerController.FindPlayerByName(words[2]));
                    else
                        view.InvalidInstruction();
                    break;

                case "CN": // Setup Ships
                    if (words.Length == 5)
                        _gameController.Setup_Ship(_playerController.FindPlayerByName(words[1]), words[2], words[3], words[4]);
                    else if (words.Length == 6)
                        _gameController.Setup_Ship(_playerController.FindPlayerByName(words[1]), words[2], words[3], words[4], words[5]);
                    else
                        view.InvalidInstruction();
                    break;

                case "RN": // Remove Ship
                    if (_HasRequiredInputs(words.Length, 4))
                        _gameController.RemoveShip(_playerController.FindPlayerByName(words[1]), words[2], words[3]);
                    else
                        view.InvalidInstruction();
                    break;

                case "T": // Execute Attack
                    if (_HasRequiredInputs(words.Length, 4))
                        if (_gameVM.CombatInitiated) _gameController.MakeAttack(_playerController.FindPlayerByName(words[1]), words[2], words[3]);
                        else view.InvalidInstruction();
                    else
                        view.InvalidInstruction();
                    break;

                case "V": // Visualize Boards
                    if (_gameVM.GameInProgress)
                    {
                        if (_gameVM.CombatInitiated)
                        {
                            view.PrintAttackBoard(_gameVM.Player1);
                            view.PrintAttackBoard(_gameVM.Player2);
                        }
                        else
                            view.DisplayCombatNotInitiate();
                    }
                    else
                        view.DisplayGameNotInProgress();

                    break;

                case "XO": //Print Own Board of each player
                    if (_gameVM.GameInProgress)
                    {
                        if (_gameVM.CombatInitiated)
                        {
                            view.PrintOwnBoard(_gameVM.Player1);
                            view.PrintOwnBoard(_gameVM.Player2);
                        }
                        else view.DisplayCombatNotInitiate();
                    }
                    else view.DisplayGameNotInProgress();

                    break;

                case "Xclear": // Clear Console
                    Console.Clear();
                    break;

                default:
                    view.InvalidInstruction();
                    break;
            }
        }
        public bool _HasRequiredInputs(int nr_inputs, int nr_reqInputs) =>
            nr_inputs == nr_reqInputs;

        public void UpdateGameViewModel(GameViewModel newGameVM)
        {
            _gameVM = newGameVM;
        }

    }
}
