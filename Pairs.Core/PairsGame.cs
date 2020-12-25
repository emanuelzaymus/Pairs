using Pairs.Core.Extensions;
using System;
using System.Linq;

namespace Pairs.Core
{
    public class PairsGame
    {
        private readonly int[,] _cardNumbers;

        public int[] Scores { get; }
        private int NumberOfPlayers => Scores.Length;
        private int _firstFoundCardOfPlayer = -1;

        public int RowCount => _cardNumbers.GetLength(0);
        public int ColumnCount => _cardNumbers.GetLength(1);
        public int PlayerOnTheTurn { get; private set; } = 0;
        public int Winner
        {
            get
            {
                if (IsEndOfGame())
                {
                    int maxScore = Scores.Max();
                    int count = Scores.Count(x => x == maxScore);
                    if (count == 1)
                    {
                        return Array.IndexOf(Scores, maxScore);
                    }
                    return -1;
                }
                throw new Exception("It is not end of the game yet.");
            }
        }
        public bool MoveWasCompleted => _firstFoundCardOfPlayer < 0;
        public bool WasSuccessfulMove { get; private set; }

        public PairsGame(GameLayout gameLayout, int numberOfPlayers = 2)
        {
            if (numberOfPlayers <= 0)
            {
                throw new ArgumentException("Cannot by less than 1.", nameof(numberOfPlayers));
            }
            _cardNumbers = GetNewCardArray(gameLayout.RowCount, gameLayout.ColumnCount);
            Scores = new int[numberOfPlayers];
        }

        private int[,] GetNewCardArray(int rowCount, int columnCount)
        {
            var ret = new int[rowCount, columnCount];
            int cardCount = rowCount * columnCount;

            var shuffledCards = Enumerable.Range(0, cardCount / 2).ToList().DoubleValues().Shuffle();

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i / columnCount, i % columnCount] = shuffledCards[i];
            }
            return ret;
        }

        public bool IsEndOfGame()
        {
            foreach (var card in _cardNumbers)
            {
                if (card >= 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Next move of current player.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>Whether the move was valid.</returns>
        public int NextMove(int row, int column)
        {
            if (row >= 0 && row < RowCount && column >= 0 && column < ColumnCount)
            {
                int foundCardNum = _cardNumbers[row, column];
                if (foundCardNum < 0)
                {
                    return -1;
                }
                // Turn first card
                if (_firstFoundCardOfPlayer < 0)
                {
                    _firstFoundCardOfPlayer = foundCardNum;
                }
                // Successuful move
                else if (_firstFoundCardOfPlayer == foundCardNum)
                {
                    WasSuccessfulMove = true;
                    AddPointToPlayerOnTurn();
                    RemoveFoundPair(foundCardNum);
                    _firstFoundCardOfPlayer = -1;
                }
                else // Unsuccessful move
                {
                    WasSuccessfulMove = false;
                    SetNextPlayersTurn();
                    _firstFoundCardOfPlayer = -1;
                }
                return foundCardNum;
            }
            return -1;
        }

        private void SetNextPlayersTurn()
        {
            PlayerOnTheTurn = (PlayerOnTheTurn + 1) % NumberOfPlayers;
        }

        private void RemoveFoundPair(int cardNumber)
        {
            for (int i = 0; i < _cardNumbers.Length; i++)
            {
                if (_cardNumbers[i / ColumnCount, i % ColumnCount] == cardNumber)
                {
                    _cardNumbers[i / ColumnCount, i % ColumnCount] = -1;
                }
            }
        }

        private void AddPointToPlayerOnTurn()
        {
            Scores[PlayerOnTheTurn]++;
        }

        public bool IsPlayerOnTheTurn(int playerNumber)
        {
            return playerNumber == PlayerOnTheTurn;
        }

    }
}
