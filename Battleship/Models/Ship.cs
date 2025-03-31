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

        public Ship GetShipByType(ShipType type, Player player, GameViewModel gameVM)
        {
            List<Ship> shipList = GetPlayerShipToDeployList(player, gameVM);

            return shipList.FirstOrDefault(ship => ship.Type == type);
        }

        public void RemoveShipToDeploy(ShipType type, Player player, GameViewModel gameVM)
        {
            GetPlayerShipToDeployList(player,gameVM).Remove(GetShipByType(type, player, gameVM));
        }        

        public int GetRemainingQuantity(ShipType type, Player player, GameViewModel gameVM)
        {
            List<Ship> shipsToDeploy = GetPlayerShipToDeployList(player, gameVM);

            Ship ship = shipsToDeploy.FirstOrDefault(ship => ship.Type == type);

            return ship.Quantity;
        }

        public int RemoveQuantity(ShipType type, Player player, GameViewModel gameVM)
        {
            List<Ship> shipsToDeploy = GetPlayerShipToDeployList(player, gameVM);

            Ship ship = shipsToDeploy.FirstOrDefault(ship => ship.Type == type);
            ship.Quantity -= 1;

            return ship.Quantity;
        }

        public List<Ship> GetPlayerShipToDeployList(Player player, GameViewModel gameVM)
        {
            return player.Name == gameVM.Player1.Name ? gameVM.Player1_ShipsToDeploy : gameVM.Player2_ShipsToDeploy;
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

        public override string ToString() => $"Row: {Row} / Column: {Column}";
    }
}
