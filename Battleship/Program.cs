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

            // Executing automated inputs for testing 
            var inputs = new StringReader("RJ tiago\nRJ diogo\nIJ tiago diogo\nCN tiago L 1 A\nXO");
            Console.SetIn(inputs);

            int automatedInputs = 0;

            while (automatedInputs < 5 && (command = view.GetCommand()) != null)  
            {                
                Console.WriteLine($"{command}");
                controller.CheckCommand(command, view, playersList);
                automatedInputs++;
            }
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            // ------------------------------------------------------  //

            do
            {
                command = view.GetCommand();
                controller.CheckCommand(command, view, playersList);
            }
            while (!string.IsNullOrWhiteSpace(command));

        }
    }
}
