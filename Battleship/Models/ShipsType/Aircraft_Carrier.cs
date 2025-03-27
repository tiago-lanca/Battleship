using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Models.ShipsType
{
    public class Aircraft_Carrier : Ship
    {
        public readonly string Code = "P";
        public int Size = 5;
        public int Quantity = 1;

        public Aircraft_Carrier(ShipType type, Location location, int team, string placeholder)
            : base(type, location, team, placeholder)
        {
            
        }
    }
}
