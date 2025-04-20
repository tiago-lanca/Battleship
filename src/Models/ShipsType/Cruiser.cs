using Battleship.ViewModel;
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
        public override int Size => 4;
        public override int Quantity { get; set; } = 1;

        public Cruiser() { }
        public Cruiser(ShipType type, List<Location> location, Direction direction, int team, string placeholder, State state = State.Alive)
            : base(type, location, direction, team, placeholder, state)
        {

        }
    }
}
