using Battleship.Interfaces;
using Battleship.ViewModel;
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
        public string Code = "L";
        public override int Size => 1;
        public override int Quantity { get; set; } = 4;

        public Speedboat() { }
        public Speedboat(ShipType type, List<Location> location, Direction direction, int team, string placeholder, State state = State.Alive)
            : base(type, location, direction, team, placeholder, state)
        {

        }
    }
}
