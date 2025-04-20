using Battleship.Interfaces;
using Battleship.Models.ShipsType;
using Battleship.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Models.Ship;

namespace Battleship.Models
{
    public class Ship : IShip<Ship>
    {
        #region Variables
        public ShipType Type { get; set; }
        public virtual int Quantity { get; set; }
        public virtual int Size { get; }
        public int Team { get; set; }
        public List<Location>? Location { get; set; }
        public Direction Direction { get; set; }
        public string? Placeholder { get; set; }
        public State State { get; set; }

        #endregion
        
        public Ship() { }
        public Ship(ShipType type, List<Location> location, Direction direction, int team, string placeholder, State state = State.Alive)
        {
            Type = type;
            Location = location;
            Direction = direction;
            Team = team;             
            Placeholder = placeholder;
            State = state;
        }

        /**
         * Check if the ship type (quantity) is already deployed on the board (quantity = 0)
         * @param playerShipToDeployList , the playerList of ships available to deploy
         * @return the number of ships's type remaining to deploy
         */
        public int GetRemainingQuantity(List<Ship> playerShipToDeployList) =>            
            playerShipToDeployList.FirstOrDefault(ship => ship.Type == Type).Quantity;

        /**
         * Remove the quantity of ship (type) to deploy
         * @param type , the type of ship to remove
         * @param player , the player who owns the ship to remove on OwnBoard
         * @return the number of ships's type remaining to deploy
         */
        public void RemoveQuantityToDeploy(List<Ship> playerShipToDeployList)
        {
            Ship ship = playerShipToDeployList.FirstOrDefault(ship => ship.Type == Type);
            // Remove the quantity of ship to deploy
            ship.Quantity--;

            // Check if the quantity of ship to deploy is 0, if so remove it from the list of ships to deploy
            if (ship.Quantity == 0)
                playerShipToDeployList.Remove(ship);            
        }
   
        public bool Is_ShipSunk(Player defender)
        {
            if (this is null)
                throw new Exception("Ship is null");
            
            else
            {
                // Check if each location of specific ship is hit
                foreach (var location in Location)
                {
                    if (defender.OwnBoard[location.Row, location.Column].State is State.Sunk)
                        continue;
                    else return false;
                }

                return true;
            }
        }
        
        /**
         * Check if the ship is hit
         * @param defender , the player who is defending the attack
         * @param attackLocation, the location of the attack
         * @return true if the ship is hit, false otherwise
         */
        public bool IsHit(Player defender, Location attackLocation)=>
            defender.OwnBoard[attackLocation.Row, attackLocation.Column] is not null;

        // Create a new ship based on the type of the ship
        public Ship? CreateNewShip()
        {
            return this switch
            {
                Speedboat => new Speedboat(),
                Submarine => new Submarine(),
                Frigate => new Frigate(),
                Cruiser => new Cruiser(),
                Aircraft_Carrier => new Aircraft_Carrier(),
                _ => null,
            };
        }

        public void ChangeShipState(Location attackLocation, Player player)
        {
            var ship = player.OwnBoard[attackLocation.Row, attackLocation.Column];
            ship.State = State.Sunk;
            ship.Placeholder = "X";

            var ship1 = player.ShipsInGame.FirstOrDefault(s=>s.Location.Any(l => l.Row == attackLocation.Row && l.Column == attackLocation.Column));
            ship1.State = State.Sunk;
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

        /**
         * Remove the ship from the OwnBoard of the given player
         * @param player , is the player who owns the ship to remove on OwnBoard
         */
        public void RemoveOnBoard(Player player)
        {
            foreach (var location in this.Location)
                player.OwnBoard[location.Row, location.Column] = null;
        }

        /**
         * Clone the ship
         * @return a new ship with the same properties as the original ship
         */
        public Ship CloneShip()
        {
            return this switch
            {
                Speedboat => new Speedboat(Type, Location, Direction, Team, Placeholder, State),
                Submarine => new Submarine(Type, Location, Direction, Team, Placeholder, State),
                Frigate => new Frigate(Type, Location, Direction, Team, Placeholder, State),
                Cruiser => new Cruiser(Type, Location, Direction, Team, Placeholder, State),
                Aircraft_Carrier => new Aircraft_Carrier(Type, Location, Direction, Team, Placeholder, State),
                _ => throw new Exception("Ship type not found"),
            };
        }

        public Direction GetDirection(string orientation)
        {
            return orientation?.ToUpper() switch
            {
                "N" => Direction.N,
                "S" => Direction.S,
                "E" => Direction.E,
                "O" => Direction.O,
                _ => Direction.None,
            };
        }

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
    public enum State
    {
        None,
        Alive,
        Sunk
    }
    public enum Direction
    {
        N,
        S,
        E,
        O,
        None
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
