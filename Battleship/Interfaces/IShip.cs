using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Models;
using Battleship.Models.ShipsType;
using Battleship.ViewModel;

namespace Battleship.Interfaces
{
    public interface IShip<T>
    {
        public int GetRemainingQuantity(T type, Player player, GameViewModel gameVM);
        public int RemoveQuantityToDeploy(T type, Player player, GameViewModel gameVM);
        public List<Location> AddLocations(Location initLocation, string orientation = null);
        public bool Is_ShipSunk(Player attacker);
        
    }
}
