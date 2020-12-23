using Pairs.Core;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Pairs.ConsoleApp
{
    class PairsGameCli
    {
        private const char _emptyCardPosition = ' ';
        private const char _cardBackFace = '#';
        private static readonly List<char> _cardFrontFaces = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };

        private readonly PairsGame _pairsGame;

        private readonly char[,] _cards;

        public PairsGameCli(PairsGame pairsGame)
        {
            _pairsGame = pairsGame;
            _cards = new char[pairsGame.RowCount, pairsGame.ColumnCount];

            HideCards();
        }

        internal void Run()
        {
            while (!_pairsGame.IsEndOfGame())
            {
                int playerNumber = _pairsGame.PlayerOnTheTurn;

                Console.WriteLine($"PLAYER {playerNumber}, GET READY !");
                Thread.Sleep(1000);

                PrintCards(); // HideAll

                while (_pairsGame.IsPlayerOnTheTurn(playerNumber))
                {
                    ReadMove(playerNumber, out int row, out int column);
                    int cardNumber = _pairsGame.NextMove(row, column);
                    if (cardNumber >= 0) // If the move is not valid, onthing happens
                    {
                        ShowCard(row, column, cardNumber);
                        PrintCards(); // ShowCard(row, column, cardNumber)

                        if (_pairsGame.MoveWasCompleted)
                        {
                            if (_pairsGame.WasSuccessfulMove)
                            {
                                Console.WriteLine("SUCCESS !");
                                Thread.Sleep(1000);
                                RemoveFoundPair();
                                if (_pairsGame.IsEndOfGame())
                                {
                                    break;
                                }
                                PrintCards(); // Remove eliminated cards // RemoveFoundPair
                            }
                            else
                            {
                                Console.WriteLine("FAILURE");
                                Thread.Sleep(1000);
                                HideCards();
                            }
                        }
                    }
                }
            }
            PrintResults();
        }

        private void HideCards()
        {
            SetValues(_cardBackFace);
        }

        private void RemoveFoundPair()
        {
            SetValues(_emptyCardPosition);
        }

        private void SetValues(char newCard)
        {
            for (int i = 0; i < _cards.Length; i++)
            {
                char card = _cards[i / _cards.GetLength(1), i % _cards.GetLength(1)];
                if (card != _cardBackFace && card != _emptyCardPosition)
                {
                    _cards[i / _cards.GetLength(1), i % _cards.GetLength(1)] = newCard;
                }
            }
        }

        private void ShowCard(int row, int column, int cardNumber)
        {
            _cards[row, column] = _cardFrontFaces[cardNumber];
        }

        private void ReadMove(int playerNumber, out int row, out int column)
        {
            Console.WriteLine($"Player {playerNumber} on the turn.");
            Console.Write("Row: ");
            if (!int.TryParse(Console.ReadLine(), out row))
            {
                row = -1;
            }
            Console.Write("Column: ");
            if (!int.TryParse(Console.ReadLine(), out column))
            {
                column = -1;
            }
        }

        private void PrintCards()
        {
            Console.Clear();
            for (int r = 0; r < _cards.GetLength(0); r++)
            {
                Console.Write("\n ");
                for (int c = 0; c < _cards.GetLength(1); c++)
                {
                    Console.Write(" " + _cards[r, c]);
                }
            }
            Console.WriteLine("\n");
        }

        private void PrintResults()
        {
            Console.Clear();
            Console.WriteLine("End of the game.");
            int winner = _pairsGame.Winner;
            if (winner >= 0)
            {
                Console.WriteLine($"The winner is player {winner}");
            }
            else
            {
                Console.WriteLine("It's draw");
            }
        }

    }
}
