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
        public bool Deployed {  get; set; }
        public ShipState State { get; set; }

        public Ship(ShipType type, Location location, int team, string placeholder)
        {
            Type = type;
            Location = location;
            Team = team;             
            Placeholder = placeholder;
            Deployed = false;
            State = ShipState.Alive;
        }

        public enum ShipType
        {
            Speedboat,
            Submarine,
            Frigate,
            Cruiser,
            Aircraft_Carrier
        } 
        
        public enum ShipState
        {
            None,
            Alive,
            Sunk
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
