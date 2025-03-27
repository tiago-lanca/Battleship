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
        public ViewConsole view;

        public bool IsPlayerListEmpty() { return playersList.Count == 0; }

        public void RegisterPlayer()
        {
            throw new NotImplementedException();
        }

        public void RemovePlayer()
        {
            throw new NotImplementedException();
        }

        public void ShowAllPlayers()
        {
            playersList = playersList.OrderByDescending(player => player.Name).ToList();

            foreach (Player player in playersList)
            {
                view.DisplayAllPlayers(player);
            }
        }
    }
}
