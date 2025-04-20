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
        Player? FindPlayerByName(string name);
        int GetPlayerTeam(Player player);
        bool CheckTurn(Player player);
        void AddStatsToPlayers(Player attacker, Player defender);
        void ResetInGameStats(Player player);
        int GetTurn(bool turn);
    }
}
