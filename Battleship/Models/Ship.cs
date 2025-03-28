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
        public List<Location>? Location { get; set; }
        public string? Direction { get; set; }
        public string? Placeholder { get; set; }
        public bool Deployed {  get; set; }
        public ShipState State { get; set; }

        public Ship() { }
        public Ship(ShipType type, List<Location> location, string direction, int team, string placeholder)
        {
            Type = type;
            Location = location;
            Direction = direction;
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

        public Location(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override string ToString() => $"Start: {Row} / End: {Column}";
    }
}
