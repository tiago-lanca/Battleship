using Battleship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Interfaces
{
    public interface IPlayerManager
    {
        int GetPlayerTeam(Player player);
        bool IsPlayerTurn(Player player);
        void AddNrGames(Player attacker, Player defender);
        void ResetInGameStats(Player player);
        int GetTurn(bool turn);
    }
}
