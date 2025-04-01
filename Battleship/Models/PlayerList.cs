using Battleship.Interfaces;
using Battleship.ViewModel;
using Battleship.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Models
{
    public class PlayerList : IPlayerList
    {
        public List<Player> playersList = new List<Player>();
        public ViewConsole view = new ViewConsole();
        //private readonly IGameViewModel _gameVM;

        public void RegisterPlayer(string name)
        {
            if (!playersList.Exists(player => player.Name == name))
            {
                Player newPlayer = new Player(name);
                playersList.Add(newPlayer);
                view.PlayerRegistered();
            }

            else
                view.PlayerAlreadyExists();
        }

        public void RemovePlayer(string name, GameViewModel gameVM)
        {
            Player player = playersList.Find(player => player.Name == name);

            if (player is null)
            {
                view.PlayerNotFound();
                return;
            }

            if(gameVM.FindPlayer_InProgressGame(name))
            {
                view.DisplayPlayerInProgressGame();
                return;
            }

            playersList.Remove(player);
            view.PlayerRemoved();
        }

        public void ShowAllPlayers()
        {
            if (IsPlayerListEmpty())
                view.PlayerListEmpty();

            else
            {
                playersList = playersList.OrderByDescending(player => player.Name).ToList();

                foreach (Player player in playersList)
                {
                    view.ShowAllPlayers(player);
                }
            }
        }

        public bool IsPlayerListEmpty() { return playersList.Count == 0; }
    }
}
