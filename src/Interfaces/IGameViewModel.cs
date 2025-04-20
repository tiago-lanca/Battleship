using Battleship.Models;
using Battleship.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Interfaces
{
    public interface IGameViewModel
    {
        #region Variables
        Player Player1 { get; set; }
        List<Ship>? Player1_ShipsToDeploy { get; set; }
        Player? Player2 { get; set; }
        List<Ship>? Player2_ShipsToDeploy { get; set; }

        bool Turn { get; set; }
        bool FirstShot { get; set; }
        string[]? GameInProgress_Players { get; set; }
        bool GameInProgress { get; set; }
        bool CombatInitiated { get; set; }
        #endregion

        #region Functions

        bool PlayerShipsToDeploy_Empty(Player player);
        bool AllPlayersShipsDeployed();
        void AddShip_ToPlayerList(Player player, Ship ship);
        void RemoveShip_InPlayerList(Ship defenderShip, Player player);
        List<Ship> GetPlayerShipToDeployList(Player player);
        string? GetShipTypeName(Ship ship);
        bool IsFinished(Player defender);
        bool FindPlayer_InProgressGame(string name);
        bool IsPlayerInGame(Player player);
        void ResetGameViewModel();
        void ManageTurn(Player player);
        void ChangeTurn();

        #endregion
    }
}
