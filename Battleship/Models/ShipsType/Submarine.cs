using Battleship.Controllers;
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
    public class Submarine : Ship
    {
        public string Code = "S";
        public override int Size => 2;
        public override int Quantity => 3;

        public Submarine() { }
        public Submarine(ShipType type, List<Location> location, string direction, int team, string placeholder)
            : base(type, location, direction, team, placeholder)
        {

        }

    }
}
