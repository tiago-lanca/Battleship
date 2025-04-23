using Battleship.Controllers;
using Battleship.Interfaces;
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
    public class Submarine : Ship
    {
        public override int Size => 2;
        public override int Quantity { get; set; } = 3;

        public Submarine() { }
        public Submarine(ShipType type) : base(type) { }
        public Submarine(ShipType type, List<Location> location, Direction direction, Code code, State state = State.Alive)
            : base(type, location, direction, code, state)
        {

        }

    }
}
