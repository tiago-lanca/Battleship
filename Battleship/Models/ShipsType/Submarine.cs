using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Models.Ship;

namespace Battleship.Models.ShipsType
{
    public class Submarine : Ship
    {
        public string Code = "S";
        public int Size = 2;
        public int Quantity = 3;

        public Submarine(ShipType type, Location location, int team, string placeholder)
            : base(type, location, team, placeholder)
        {

        }
    }
}
