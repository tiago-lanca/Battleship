using Battleship.Interfaces;
using Battleship.Models.ShipsType;
using Battleship.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Models.Ship;

namespace Battleship.Models
{
    public class Ship
    {
        public ShipType Type { get; set; }
        public virtual int Quantity { get; }
        public virtual int Size { get; }
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

        public int GetRemainingQuantity(ShipType type, Player player, GameViewModel gameVM)
        {
            List<Ship> shipsToDeploy = player.Name == gameVM.Player1.Name ? gameVM.Player1_ShipsToDeploy : gameVM.Player2_ShipsToDeploy;
                
            Ship ship = shipsToDeploy.FirstOrDefault(ship => ship.Type == type);

            return ship.Quantity;
        }

        public int RemoveQuantity(int quantity, ShipType type, Player player, GameViewModel gameVM)
        {
            List<Ship> shipsToDeploy = player.Name == gameVM.Player1.Name ? gameVM.Player1_ShipsToDeploy : gameVM.Player2_ShipsToDeploy;

            Ship ship = shipsToDeploy.FirstOrDefault(ship => ship.Type == type);

            return ship.Quantity - quantity;
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
