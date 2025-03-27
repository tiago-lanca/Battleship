using Battleship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Views
{
    public class ViewConsole
    {

        public string GetCommand()
        {
            return Console.ReadLine()!;
        }

        public void InvalidInstruction()
        {
            Console.WriteLine("Instrução inválida.\n");
        }
        public void ShowAllPlayers(Player player)
        {
            Console.WriteLine(player);
        }

        public void PlayerListEmpty()
        {
            Console.WriteLine("Não existem jogadores registados.\n");
        }

        public void PlayerRemoved()
        {
            Console.WriteLine("Jogador removido com sucesso.\n");
        }

        public void PlayerNotFound() 
        { 
            Console.WriteLine("Jogador inexistente.\n"); 
        }

        public void PlayerRegistered()
        {
            Console.WriteLine("Jogador registado com sucesso.\n");
        }

        public void PlayerAlreadyExists()
        {
            Console.WriteLine("Jogador existente.\n");
        }

        public void PrintBoard(Board board)
        {
            Console.Write("   ");
            foreach (char letter in board.)
            {
                Console.Write($" {letter}   ");
            }
            Console.WriteLine("");

            for (int line = 0; line < board.GetLength(0); line++)
            {
                Console.Write($"{numbers[line]}  ");
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[line, col] != null)
                        Console.Write(board[line, col].PlaceHolder);
                    else Console.Write("   ");

                    Console.Write("  ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
    }
}
