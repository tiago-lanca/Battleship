using Battleship.src.Models;
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
        public override int Size => 3;
        public override int Quantity { get; set; } = 2;

        public Frigate() { }
        public Frigate(ShipType type) : base(type) { }
        public Frigate(ShipType type, List<Location> location, Direction direction, Code code, State state = State.Alive) 
            : base(type, location, direction, code, state)
        {

        }
    }
}
