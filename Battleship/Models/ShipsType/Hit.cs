using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Models.Ship;

namespace Battleship.Models.ShipsType
{
    public class Hit : Ship
    {
        public readonly string Code = "X";
        public override int Size => 1;

        public Hit() { }
        public Hit(ShipType type, List<Location> location, string direction, int team, string placeholder)
            : base(type, location, direction, team, placeholder)
        {

        }
    }
}
