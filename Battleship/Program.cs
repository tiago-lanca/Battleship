using Battleship.Controllers;
using Battleship.Models;
using Battleship.Views;

namespace Battleship
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PlayerList playersList = new PlayerList();
            ViewConsole view = new ViewConsole();
            GameController controller = new GameController();
            string command;

            do
            {
                command = view.GetCommand();
                controller.CheckCommand(command, view, playersList);
            }
            while (!string.IsNullOrWhiteSpace(command));
        }
    }
}
