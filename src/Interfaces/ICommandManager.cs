using Battleship.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Interfaces
{
    public interface ICommandManager
    {
        void CheckCommand(string command);
        void UpdateGameViewModel(GameViewModel newGameVM);
    }
}
