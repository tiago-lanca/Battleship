using Battleship.Interfaces;
using Battleship.Models;
using Battleship.ViewModel;
using Battleship.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Controllers
{
    public class PlayerController : IPlayerManager
    {
        public List<Player> playerList { get; set; } = new List<Player>();
        private readonly GameViewModel _gameVM;
        private readonly ViewConsole view = new ViewConsole();

        public PlayerController(GameViewModel gameVM)
        {
            _gameVM = gameVM;
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

        public void RemovePlayer(string name)
        {
            Player player = playerList.Find(player => player.Name == name);

            if (player is null)
            {
                view.PlayerNotFound();
                return;
            }

            if (_gameVM.FindPlayer_InProgressGame(name))
            {
                view.DisplayPlayerInProgressGame();
                return;
            }

            playerList.Remove(player);
            view.PlayerRemoved();
        }

        public void ShowAllPlayers()
        {
            if (IsPlayerListEmpty())
                view.PlayerListEmpty();

            else
            {
                playerList = playerList.OrderByDescending(player => player.Name).ToList();

                foreach (Player player in playerList)
                {
                    view.ShowAllPlayers(player);
                }
            }
        }

        public bool IsPlayerListEmpty() { return playerList.Count == 0; }


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

    }
}
