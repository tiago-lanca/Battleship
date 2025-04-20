using Battleship.Interfaces;
using Battleship.Models;
using Battleship.ViewModel;
using Battleship.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Battleship.Controllers
{
    public class PlayerController : IPlayerManager
    {
        
        private readonly IGameViewModel _gameVM;

        public PlayerController(IGameViewModel gameVM)
        {
            _gameVM = gameVM;
        }

        
        /**
         * Finds the player by name
         * @param name
         * @return Player object
         */
        public Player? FindPlayerByName(string name)
        {
            if (_gameVM is null) return null;

            return _gameVM.Player1!.Name == name ? _gameVM.Player1 : _gameVM.Player2;
        }

        /**
         * Gets the team of the given player
         * @param player
         * @return int
         */
        public int GetPlayerTeam(Player player)
        {
            return player.Name == _gameVM.GameInProgress_Players[0] ? 1 : 2;
        }

        public bool CheckTurn(Player player)
        {
            return player.Name == _gameVM.GameInProgress_Players[GetTurn(_gameVM.Turn)];
        }

        /**
         * Adds stats to players, number of victories and number of games
         * @param attacker
         * @param defender
         */
        public void AddStatsToPlayers(Player attacker, Player defender)
        {
            attacker.NumVictory++;
            attacker.NumGames++;
            defender.NumGames++;
        }


        public void ResetInGameStats(Player player)
        {
            player.Ships.Clear();
            player.OwnBoard = null;
            player.AttackBoard = null;
            player.Shots = 0;
            player.ShotsOnTargets = 0;
            player.EnemySunkShips = 0;
        }

        public int GetTurn(bool turn) => Convert.ToInt16(turn);
    }
}
