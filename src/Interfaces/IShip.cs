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
        public int GetRemainingQuantity(List<T> playerShipToDeployList);
        public void RemoveQuantityToDeploy(List<T> playerShipToDeployList);
        public List<Location> AddLocations(Location initLocation, Direction? direction);
        public bool Is_ShipSunk(Player attacker);
        
    }
}
