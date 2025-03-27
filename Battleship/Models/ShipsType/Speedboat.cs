using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Models.Ship;

namespace Battleship.Models.ShipsType
{
    public class Speedboat : Ship
    {
        public readonly string Code = "L";
        public int Size = 1;
        public int Quantity = 4;

        public Speedboat(ShipType type, Location location, int team, string placeholder)
            : base(type, location, team, placeholder)
        {

        }
    }
}
