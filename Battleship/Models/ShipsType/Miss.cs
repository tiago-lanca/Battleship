using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Models.Ship;

namespace Battleship.Models.ShipsType
{
    public class Miss : Ship
    {
        public readonly string Code = "*";
        public override int Size => 1;

        public Miss() { }
        public Miss(ShipType type, List<Location> location, string direction, int team, string placeholder)
            : base(type, location, direction, team, placeholder)
        {

        }
    }
}
