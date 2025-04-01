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
            var inputs = new StringReader("RJ tiago\nRJ diogo\nIJ tiago diogo\n" +
                "CN tiago P 1 A E\nCN tiago C 2 I S\nCN tiago F 3 B S\nCN tiago F 10 H E\nCN tiago S 3 F S\n" +
                "CN tiago S 8 F S\nCN tiago S 10 C O\nCN tiago L 8 B\nCN tiago L 6 E\nCN tiago L 6 G\nCN tiago L 7 I\n" +
                "CN diogo P 1 A E\nCN diogo C 2 I S\nCN diogo F 3 B S\nCN diogo F 10 H E\nCN diogo S 3 F S\n" +
                "CN diogo S 8 F S\nCN diogo S 10 C O\nCN diogo L 8 B\nCN diogo L 6 E\nCN diogo L 6 G\nCN diogo L 7 I\nIC\n" +
                "T tiago 10 B\nT tiago 10 C\nT tiago 10 H\nT tiago 10 I\nT tiago 10 J\n" +
                "T tiago 9 F\nT tiago 8 B\nT tiago 8 F\nT tiago 7 I\nT tiago 6 E\nT tiago 6 G\n" +
                "T tiago 5 B\nT tiago 5 I\nT tiago 4 B\nT tiago 4 F\nT tiago 4 I\nT tiago 3 B\nT tiago 3 F\nT tiago 3 I\n" +
                "T tiago 2 I\nT tiago 1 A\nT tiago 1 B\nT tiago 1 C\nT tiago 1 D\n");

            Console.SetIn(inputs);

            int automatedInputs = 0;

            while (automatedInputs < 51 && (command = view.GetCommand()) != null)  
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
