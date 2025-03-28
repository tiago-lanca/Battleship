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
        public Location Location { get; set; }
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
        public int Start { get; set; }
        public int End { get; set; }

        public Location(int start, int end)
        {
            Start = start;
            End = end;
        }

        public override string ToString() => $"Start: {Start} / End: {End}";
    }
}
