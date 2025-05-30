﻿using Battleship.Models;
using Battleship.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Interfaces
{
    public interface IPlayerList
    {
        void ShowAllPlayers();
        void RegisterPlayer(string name);
        void RemovePlayer(string name, IGameViewModel gameVM);
        List<Player> GetPlayerList();
        //void RemovePlayer(string name);
        bool IsPlayerListEmpty();

    }
}
