using Battleship.ViewModel;
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
        public override int Size => 3;
        public override int Quantity { get; set; } = 2;

        public Frigate() { }
        public Frigate(ShipType type, List<Location> location, string direction, int team, string placeholder) 
            : base(type, location, direction, team, placeholder)
        {

        }
    }
}
