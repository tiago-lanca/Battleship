using Battleship.Interfaces;
using Battleship.Models;
using Battleship.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Views
{
    public class ViewConsole
    {
        Board boardOutline = new Board();
        public string GetCommand()
        {
            return Console.ReadLine()!;
        }
        public void InvalidInstruction()
        {
            Console.WriteLine("Instrução inválida.\n");
        }
        public void ShowPlayerInfo(Player player)
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

        public void DisplayPlayerNotTurn()
        {
            Console.WriteLine("Não é a vez do jogador.\n");
        }

        public void PlayerRegistered()
        {
            Console.WriteLine("Jogador registado com sucesso.\n");
        }

        public void PlayerAlreadyExists()
        {
            Console.WriteLine("Jogador existente.\n");
        }

        public void MessageGameStart(string[] players)
        {
            Console.WriteLine($"Jogo iniciado entre {players[0]} e {players[1]}.\n");
        }

        public void PrintAttackBoard(Player? player)
        {
            // Print stats of the player
            Console.WriteLine($"Nome: {player.Name} Tiros: {player.Shots} TirosEmNavios: {player.ShotsOnTargets}" +
                                    $" NaviosAfundados: {player.EnemySunkShips}\n");
            Console.Write("   ");

            // Print letters
            foreach (char letter in boardOutline.letters)
            {
                Console.Write($" {letter}");
            }
            Console.WriteLine("");

            // Print numbers and attack board with each ship deployed
            for (int line = 0; line < boardOutline.numbers.Length; line++)
            {
                if (line != 9)
                    Console.Write($" {boardOutline.numbers[line]}  ");
                else
                    Console.Write($"{boardOutline.numbers[line]}  ");

                for (int col = 0; col < boardOutline.letters.Length; col++)
                {
                    if (player.AttackBoard[line, col] != null)
                        Console.Write(player.AttackBoard[line, col].Placeholder);
                    else
                        Console.Write(" ");

                    Console.Write(" ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        public void PrintOwnBoard(Player player)
        {
            // Print stats of the player
            Console.WriteLine($"Nome: {player.Name} Tiros: {player.Shots} TirosEmNavios: {player.ShotsOnTargets}" +
                                    $" NaviosAfundados: {player.EnemySunkShips}\n");
            Console.Write("   ");

            // Print letters
            foreach (char letter in boardOutline.letters)
            {
                Console.Write($" {letter}");
            }
            Console.WriteLine("");

            // Print numbers and attack board with each ship deployed
            for (int line = 0; line < boardOutline.numbers.Length; line++)
            {
                if(line != 9)
                    Console.Write($" {boardOutline.numbers[line]}  ");
                else
                    Console.Write($"{boardOutline.numbers[line]}  ");

                for (int col = 0; col < boardOutline.letters.Length; col++)
                {
                    if (player.OwnBoard[line, col] != null)
                        Console.Write(player.OwnBoard[line, col].Placeholder);
                    else
                        Console.Write(" ");

                    Console.Write(" ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        public void DisplayGameInProgress()
        {
            Console.WriteLine("Existe um jogo em curso.\n");
        }

        public void DisplayGameNotInProgress()
        {
            Console.WriteLine("Não existe jogo em curso.\n");
        }

        public void DisplayPlayerNotRegistered()
        {
            Console.WriteLine("Jogador não registado.\n");
        }

        public void DisplayPlayerNotInProgressGame()
        {
            Console.WriteLine("Jogador não participa no jogo em curso.\n");
        }

        public void DisplayCombatInProgress()
        {
            Console.WriteLine("Combate iniciado.\n");
        }

        public void ShipDeployed_Success()
        {
            Console.WriteLine("Navio colocado com sucesso.\n");
        }

        public void Unavailable_ShipsTypeToDeploy()
        {
            Console.WriteLine("Não tem mais navios dessa tipologia disponíveis.\n");
        }

        public void InvalidPosition()
        {
            Console.WriteLine("Posição irregular.\n");
        }
        public void DisplayPlayerShipListEmpty()
        {
            Console.WriteLine("Não é possivel colocar navio.\n");
        }
        public void DisplayCombatInitiate()
        {
            Console.WriteLine("Combate iniciado.\n");
        }
        public void NeededShipsToDeploy()
        {
            Console.WriteLine("Navios não colocados.\n");
        }
        public void DisplayShipNotFound()
        {
            Console.WriteLine("Não existe navio na posição.\n");
        }
        public void DisplayShipRemovedSuccess()
        {
            Console.WriteLine("Navio removido com sucesso.\n");
        }
        public void DisplayCombatNotInitiate()
        {
            Console.WriteLine("Jogo em curso sem combate iniciado.\n");
        }
        public void DisplaySunkShip(Ship ship, IGameViewModel gameVM)
        {
            Console.WriteLine($"Navio {gameVM.GetShipTypeName(ship)} afundado.\n");
        }

        public void DisplayShipHit(Ship ship, IGameViewModel gameVM)
        {
            Console.WriteLine($"Tiro em navio {gameVM.GetShipTypeName(ship)}.\n");
        }
        public void ShipSunk_GameFinished(Ship ship, IGameViewModel gameVM)
        {
            Console.WriteLine($"Navio {gameVM.GetShipTypeName(ship)} afundado. Jogo terminado.\n");
        }
        public void DisplayPlayerInProgressGame()
        {
            Console.WriteLine("Jogador participa no jogo em curso.\n");
        }
        public void MessageForfeitGame()
        {
            Console.WriteLine("Desistência com sucesso. Jogo terminado.\n");
        }
    }
}
