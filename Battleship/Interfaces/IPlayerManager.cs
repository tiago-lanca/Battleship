using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Interfaces
{
    public interface IPlayerManager
    {
        void ShowAllPlayers();
        void RegisterPlayer(string name);
        //void RemovePlayer(string name);
        bool IsPlayerListEmpty();

    }
}
