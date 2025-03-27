using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Models.Ship;

namespace Battleship.Models.ShipsType
{
    public class Cruiser : Ship
    {
        public readonly string Code = "C";
        public int Size = 4;
        public int Quantity = 1;

        public Cruiser(ShipType type, Location location, int team, string placeholder)
            : base(type, location, team, placeholder)
        {

        }
    }
}
