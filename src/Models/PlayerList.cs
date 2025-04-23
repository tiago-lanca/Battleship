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
    public class PlayerList() : IPlayerList
    {
        private readonly ViewConsole view = new ViewConsole();
        public List<Player> playerList { get; set; } = new List<Player>();

        public void ShowAllPlayers()
        {
            if (IsPlayerListEmpty())
                view.PlayerListEmpty();

            else
            {
                playerList = playerList.OrderByDescending(player => player.Name).ToList();

                foreach (Player player in playerList)
                {
                    view.ShowPlayerInfo(player);
                }
                Console.WriteLine();
            }
        }

        public void RegisterPlayer(string name)
        {
            if (!playerList.Exists(player => player.Name == name))
            {
                Player newPlayer = new Player(name);
                playerList.Add(newPlayer);
                view.PlayerRegistered();
            }

            else
                view.PlayerAlreadyExists();
        }

        public void RemovePlayer(string name, IGameViewModel gameVM)
        {
            Player player = playerList.Find(player => player.Name == name);

            if (player is null)
            {
                view.PlayerNotFound();
                return;
            }

            if (gameVM.FindPlayer_InProgressGame(name))
            {
                view.DisplayPlayerInProgressGame();
                return;
            }

            playerList.Remove(player);
            view.PlayerRemoved();
        }

        
        public List<Player> GetPlayerList() => playerList;

        public bool IsPlayerListEmpty() { return playerList.Count == 0; }

    }

}
