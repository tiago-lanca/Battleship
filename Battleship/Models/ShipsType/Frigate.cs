using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Models.Ship;

namespace Battleship.Models.ShipsType
{
    public class Frigate : Ship
    {
        public readonly string Code = "F";
        public int Size = 3;
        public int Quantity = 2;

        public Frigate(ShipType type, Location location, int team, string placeholder)
            : base(type, location, team, placeholder)
        {

        }
    }
}
