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
        public override Code Code => Code.Cruiser;
        public override int Size => 4;
        public override int Quantity { get; set; } = 1;

        public Cruiser() { }
        public Cruiser(ShipType type) : base(type) { }
        public Cruiser(ShipType type, List<Location> location, Direction direction, Code code, State state = State.Alive)
            : base(type, location, direction, code, state)
        {

        }
    }
}
