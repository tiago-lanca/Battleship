using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Models
{
    public class Ship
    {
        public ShipType Type { get; set; }
        public int Team { get; set; }
        public Location? Location { get; set; }
        public string Placeholder { get; set; }

        public Ship(ShipType type, int team, Location location, string placeholder)
        {
            Type = type; 
            Team = team; 
            Location = location; 
            Placeholder = placeholder;
        }

        public enum ShipType
        {
            Speedboat,
            Submarine,
            Frigate,
            Cruiser,
            Aircraft_Carrier
        }        
    }

    public class Location
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Location(int row, int col)
        {
            Row = row;
            Column = col;
        }

        public override string ToString() => $"Row: {Row} / Column: {Column}";
    }
}
