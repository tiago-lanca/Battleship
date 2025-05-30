using Battleship.Interfaces;
using Battleship.Models.ShipsType;
using Battleship.src.Models;
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
        public List<Location>? Location { get; set; }
        public Direction Direction { get; set; }
        public Code Code { get; set; }
        public State State { get; set; }

        #endregion
        
        public Ship() { }
        public Ship(ShipType type) { Type = type; }
        public Ship(ShipType type, List<Location>? location, Direction direction, Code code, State state = State.Alive)
        {
            Type = type;
            Location = location;
            Direction = direction;          
            Code = code;
            State = state;
        }

        /**
         * Check if the ship type (quantity) is already deployed on the board (quantity = 0)
         * @param playerShipToDeployList , the playerList of ships available to deploy
         * @return the number of ships's type remaining to deploy
         */
        public int? GetRemainingQuantity(List<Ship> playerShipToDeployList) =>            
            playerShipToDeployList.FirstOrDefault(ship => ship.Type == Type) is null ? 
                0 : playerShipToDeployList.FirstOrDefault(ship => ship.Type == Type).Quantity;

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

        public static Ship? CreateNewShip(Code type)
        {
            return type switch
            {
                Code.Speedboat => new Speedboat(type: ShipType.Speedboat),
                Code.Submarine => new Submarine(type: ShipType.Submarine),
                Code.Frigate => new Frigate(type: ShipType.Frigate),
                Code.Cruiser => new Cruiser(type: ShipType.Cruiser),
                Code.Aircraft_Carrier => new Aircraft_Carrier(type: ShipType.Aircraft_Carrier),
                _ => null,
            };
        }

        public static Ship? CreateNewShip(ShipType type, List<Location> shipLocations, Direction direction, Code code)
        {
            return type switch
            {
                ShipType.Speedboat => new Speedboat(type, shipLocations, direction, Code.Speedboat),
                ShipType.Submarine => new Submarine(type, shipLocations, direction, Code.Submarine),
                ShipType.Frigate => new Frigate(type, shipLocations, direction, Code.Frigate),
                ShipType.Cruiser => new Cruiser(type, shipLocations, direction, Code.Cruiser),
                ShipType.Aircraft_Carrier => new Aircraft_Carrier(type, shipLocations, direction, Code.Aircraft_Carrier),
                _ => null,
            };
        }

        public void ChangeShipState(Location attackLocation, Player player)
        {
            Ship shipOnBoardView = player.OwnBoard[attackLocation.Row, attackLocation.Column];
            shipOnBoardView.State = State.Sunk;
            shipOnBoardView.Code = Code.Hit;

            Ship? shipInGame = player.ShipsInGame
                .FirstOrDefault(ship => ship.Location is not null && ship.Location.
                    Any(location => location.Row == attackLocation.Row && location.Column == attackLocation.Column));

            if (shipInGame is not null)
            {
                bool AnyAlivePart = shipInGame.Location is not null && shipInGame.Location.
                    Any(location => player.OwnBoard[location.Row, location.Column].State is State.Alive);

                shipInGame.State = AnyAlivePart ? State.Alive : State.Sunk;
            }
        }

        public Location GetNewLocation(Location initLocation, Direction direction, int offset)
        {
            return direction switch
            {
                Direction.N => new Location(initLocation.Row - offset, initLocation.Column),
                Direction.S => new Location(initLocation.Row + offset, initLocation.Column),
                Direction.E => new Location(initLocation.Row, initLocation.Column + offset),
                Direction.O => new Location(initLocation.Row, initLocation.Column - offset),
                _ => initLocation,
            };
        }
        public List<Location> AddLocations(Location initLocation, Direction? direction = null)
        {
            List<Location> locations = new List<Location>();

            switch (direction)
            {
                case Direction.N:
                    for (int i = 0; i < Size; i++)
                    {
                        locations.Add(GetNewLocation(initLocation, Direction.N, i));
                    }
                    break;

                case Direction.S:
                    for (int i = 0; i < Size; i++)
                    {
                        locations.Add(GetNewLocation(initLocation, Direction.S, i));
                    }
                    break;

                case Direction.E:
                    for (int i = 0; i < Size; i++)
                    {
                        locations.Add(GetNewLocation(initLocation, Direction.E, i));
                    }
                    break;

                case Direction.O:
                    for (int i = 0; i < Size; i++)
                    {
                        locations.Add(GetNewLocation(initLocation, Direction.O, i));
                    }
                    break;

                case null:
                    locations.Add(initLocation);
                    break;
            }

            return locations;
        }

        public static Ship? GetShipFromOwnBoard(Player player, Location location) => player.OwnBoard[location.Row, location.Column];
        
        public void RemoveShip(Player player, IGameViewModel gameVM)
        {
            List<Ship> playerShipsToDeployList = gameVM.GetPlayerShipToDeployList(player);
            RemoveOnBoard(player);

            if (playerShipsToDeployList.Find(s => s.Type == this.Type) is null)
                playerShipsToDeployList.Add(this);
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
                Speedboat => new Speedboat(Type, Location, Direction, Code, State),
                Submarine => new Submarine(Type, Location, Direction, Code, State),
                Frigate => new Frigate(Type, Location, Direction, Code, State),
                Cruiser => new Cruiser(Type, Location, Direction, Code, State),
                Aircraft_Carrier => new Aircraft_Carrier(Type, Location, Direction, Code, State),
                _ => throw new Exception("Ship type not found"),
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

    public enum Code
    {
        Speedboat = 'L',
        Submarine = 'S',
        Frigate = 'F',
        Cruiser = 'C',
        Aircraft_Carrier = 'P',
        Hit = 'X',
        Miss = '*',
        Null = ' '
    }
    
}
