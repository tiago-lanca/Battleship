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

        public void GameStartedSortedNames(string[] players)
        {
            Console.WriteLine($"Jogo iniciado entre {players[0]} e {players[1]}.\n");
        }

        public void PrintAttackBoard(Player player1, Player player2)
        {
            Board boardOutline = new Board();

            // PLAYER 1
            Console.WriteLine($"Nome: {player1.Name} Tiros: {player1.Shots} TirosEmNavios: {player1.ShotsOnTargets}" +
                                    $" NaviosAfundados: {player1.EnemySunkShips}\n");
            Console.Write("   ");
            foreach (char letter in boardOutline.letters)
            {
                Console.Write($" {letter}  ");
            }
            Console.WriteLine("");

            for (int line = 0; line < boardOutline.numbers.Length; line++)
            {
                if (line != 9)
                    Console.Write($" {boardOutline.numbers[line]}  ");
                else
                    Console.Write($"{boardOutline.numbers[line]}  ");

                for (int col = 0; col < boardOutline.letters.Length; col++)
                {
                    if (player1.AttackBoard[line, col] != null)
                        Console.Write(player1.AttackBoard[line, col].Placeholder);
                    else
                        Console.Write(" ");

                    Console.Write("   ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");


            // PLAYER 2
            Console.WriteLine($"Nome: {player2.Name} Tiros: {player2.Shots} TirosEmNavios: {player2.ShotsOnTargets}" +
                                    $" NaviosAfundados: {player2.EnemySunkShips}\n");
            Console.Write("   ");
            foreach (char letter in boardOutline.letters)
            {
                Console.Write($" {letter}  ");
            }
            Console.WriteLine("");

            for (int line = 0; line < boardOutline.numbers.Length; line++)
            {
                if (line != 9)
                    Console.Write($" {boardOutline.numbers[line]}  ");
                else
                    Console.Write($"{boardOutline.numbers[line]}  ");

                for (int col = 0; col < boardOutline.letters.Length; col++)
                {
                    if (player2.AttackBoard[line, col] != null)
                        Console.Write(player2.AttackBoard[line, col].Placeholder);
                    else
                        Console.Write(" ");

                    Console.Write("   ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        public void PrintOwnBoard(Player player1, Player player2)
        {
            Board boardOutline = new Board();

            // PLAYER 1
            Console.WriteLine($"Nome: {player1.Name} Tiros: {player1.Shots} TirosEmNavios: {player1.ShotsOnTargets}" +
                                    $" NaviosAfundados: {player1.EnemySunkShips}\n");
            Console.Write("   ");
            foreach (char letter in boardOutline.letters)
            {
                Console.Write($" {letter}  ");
            }
            Console.WriteLine("");

            for (int line = 0; line < boardOutline.numbers.Length; line++)
            {
                if(line != 9)
                    Console.Write($" {boardOutline.numbers[line]}  ");
                else
                    Console.Write($"{boardOutline.numbers[line]}  ");

                for (int col = 0; col < boardOutline.letters.Length; col++)
                {
                    if (player1.OwnBoard[line, col] != null)
                        Console.Write(player1.OwnBoard[line, col].Placeholder);
                    else
                        Console.Write(" ");

                    Console.Write("   ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");


            // PLAYER 2
            Console.WriteLine($"Nome: {player2.Name} Tiros: {player2.Shots} TirosEmNavios: {player2.ShotsOnTargets}" +
                                    $" NaviosAfundados: {player2.EnemySunkShips}\n");
            Console.Write("   ");
            foreach (char letter in boardOutline.letters)
            {
                Console.Write($" {letter}  ");
            }
            Console.WriteLine("");

            for (int line = 0; line < boardOutline.numbers.Length; line++)
            {
                if (line != 9)
                    Console.Write($" {boardOutline.numbers[line]}  ");
                else
                    Console.Write($"{boardOutline.numbers[line]}  ");

                for (int col = 0; col < boardOutline.letters.Length; col++)
                {
                    if (player2.OwnBoard[line, col] != null)
                        Console.Write(player2.OwnBoard[line, col].Placeholder);
                    else
                        Console.Write(" ");

                    Console.Write("   ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        public void DisplayGameInProgress()
        {
            Console.WriteLine("Existe um jogo em curso.\n");
        }

        public void DisplayPlayerNotRegistered()
        {
            Console.WriteLine("Jogador não registado.\n");
        }

        public void DisplayPlayerNotInGameProgress()
        {
            Console.WriteLine("Jogador não participa no jogo em curso.\n");
        }
    }
}
