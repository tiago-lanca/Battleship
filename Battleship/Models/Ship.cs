using Battleship.Interfaces;
using Battleship.Models.ShipsType;
using Battleship.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Models.Ship;

namespace Battleship.Models
{
    public class Ship : IShip<ShipType>
    {
        public ShipType Type { get; set; }
        public virtual int Quantity { get; set; }
        public virtual int Size { get; }
        public int Team { get; set; }
        public List<Location>? Location { get; set; }
        public string? Direction { get; set; }
        public string? Placeholder { get; set; }
        public ShipState State { get; set; }

        public Ship() { }
        public Ship(ShipType type, List<Location> location, string direction, int team, string placeholder)
        {
            Type = type;
            Location = location;
            Direction = direction;
            Team = team;             
            Placeholder = placeholder;
            State = ShipState.Alive;
        }

        
        public int GetRemainingQuantity(ShipType type, Player player, GameViewModel gameVM)
        {
            List<Ship> shipsToDeploy = gameVM.GetPlayerShipToDeployList(player, gameVM);

            Ship ship = shipsToDeploy.FirstOrDefault(ship => ship.Type == type);

            return ship.Quantity;
        }

        public int RemoveQuantityToDeploy(ShipType type, Player player, GameViewModel gameVM)
        {
            List<Ship> shipsToDeploy = gameVM.GetPlayerShipToDeployList(player, gameVM);

            Ship ship = shipsToDeploy.FirstOrDefault(ship => ship.Type == type);
            ship.Quantity -= 1;

            return ship.Quantity;
        }
   
        public bool Is_ShipSunk(Player attacker)
        {
            if (this is null)
            {
                throw new Exception("Ship is null");
            }

            else
            {
                foreach (var location in Location)
                {
                    if (attacker.AttackBoard[location.Row, location.Column] is null)
                        return false;
                }

                return true;
            }
        }
                public bool IsHit(Player defender, Location attackLocation)
        {
            return defender.OwnBoard[attackLocation.Row, attackLocation.Column] is not null;
        }
        public Ship CreateNewShip()
        {
            switch (this)
            {
                case Speedboat:
                    return new Speedboat();

                case Submarine:
                    return new Submarine();

                case Frigate:
                    return new Frigate();

                case Cruiser:
                    return new Cruiser();

                case Aircraft_Carrier:
                    return new Aircraft_Carrier();
            }

            return null;
        }

        public List<Location> AddLocations(Location initLocation, string orientation = null)
        {
            List<Location> locations = new List<Location>();

            switch (orientation)
            {

                case "E":
                    for (int i = 0; i < Size; i++)
                    {
                        locations.Add(new Location(initLocation.Row, initLocation.Column + i));
                    }
                    break;

                case "N":
                    for (int i = 0; i < Size; i++)
                    {
                        locations.Add(new Location(initLocation.Row - i, initLocation.Column));
                    }
                    break;

                case "S":
                    for (int i = 0; i < Size; i++)
                    {
                        locations.Add(new Location(initLocation.Row + i, initLocation.Column));
                    }
                    break;

                case "O":
                    for (int i = 0; i < Size; i++)
                    {
                        locations.Add(new Location(initLocation.Row, initLocation.Column - i));
                    }
                    break;

                case null:
                    locations.Add(new Location(initLocation.Row, initLocation.Column));
                    break;
            }

            return locations;
        }

        public enum ShipType
        {
            Speedboat,
            Submarine,
            Frigate,
            Cruiser,
            Aircraft_Carrier,
            Hit,
            Miss
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

        public override string ToString() => $"Row: {Row} / Column: {Column}";
    }
}
