﻿using Battleship.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Models.ShipsType
{
    public class Aircraft_Carrier : Ship
    {
        public readonly string Code = "P";
        public int Size = 5;
        public override int Quantity => 1;

        public Aircraft_Carrier () { }
        public Aircraft_Carrier(ShipType type, List<Location> location, string direction, int team, string placeholder)
            : base(type, location, direction, team, placeholder)
        {
            
        }
    }
}
