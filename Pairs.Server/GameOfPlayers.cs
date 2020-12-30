using Pairs.Core;
using Pairs.InterfaceLibrary;
using Pairs.InterfaceLibrary.Response;
using System.Collections.Generic;
using System.Linq;

namespace Pairs.Server
{
    internal class GameOfPlayers
    {
        public Player FirstPlayer { get; }

        public Player SecondPlayer { get; }

        public PairsGame Game { get; }

        private List<OpponentsMove> OpponentsMovesBuffer { get; } = new List<OpponentsMove>();

        public GameOfPlayers(Player firstPlayer, Player secondPlayer, GameLayout gameLayout)
        {
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
            Game = new PairsGame(gameLayout, firstPlayer.Id, secondPlayer.Id);
        }

        public void AddOpponentsMove(int row, int column, int cardNumber)
        {
            bool? isSuccessful = Game.MoveWasCompleted ? Game.WasSuccessfulMove : (bool?)null;
            OpponentsMovesBuffer.Add(new OpponentsMove(new Card(row, column, cardNumber), isSuccessful));
        }

        public List<OpponentsMove> PopAllOpponentsMove()
        {
            var ret = OpponentsMovesBuffer.Select(x => x).ToList();
            OpponentsMovesBuffer.Clear();
            return ret;
        }

    }
}