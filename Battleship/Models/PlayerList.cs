using Battleship.Interfaces;
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

        public void RemovePlayer(string name)
        {
            Player player = playersList.Find(player => player.Name == name);

            if (player != null)
            {
                playersList.Remove(player);
                view.PlayerRemoved();
            }

            else
                view.PlayerNotFound();
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
