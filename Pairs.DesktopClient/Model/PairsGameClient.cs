using Pairs.DesktopClient.Presenter;
using Pairs.InterfaceLibrary;
using Pairs.InterfaceLibrary.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Pairs.DesktopClient.Model
{
    class PairsGameClient : IDisposable
    {
        private readonly ChannelFactory<IPairsGameService> _channelFactory;
        private readonly IPairsGameService _pairsGameService;

        private Player _player;
        private string _opponent;
        private GameLayout _gameLayout;

        private ICard _firstCard;

        public delegate void CardShownEventHandler(ICard card, int cardNumber);
        public event CardShownEventHandler CardShown;
        protected virtual void OnCardShown(ICard card, int cardNumber) => CardShown?.Invoke(card, cardNumber);

        public delegate void CardsHiddenEventHandler(ICard card1, ICard card2);
        public event CardsHiddenEventHandler CardsHidden;
        protected virtual void OnCardsHidden(ICard card1, ICard card2) => CardsHidden?.Invoke(card1, card2);

        public delegate void PairRemovedEventHandler(ICard card1, ICard card2);
        public event PairRemovedEventHandler FoundPairRemoved;
        protected virtual void OnFoundPairRemoved(ICard card1, ICard card2) => FoundPairRemoved?.Invoke(card1, card2);

        public delegate void PlayerOnTurnChangedEventHandler(string playerNick);
        public event PlayerOnTurnChangedEventHandler PlayerOnTurnChanged;
        protected virtual void OnPlayerOnTurnChanged()
        {
            int playerOnTurn = _pairsGameService.GetPlayerOnTurn(_player.Id);
            if (playerOnTurn == _player.Id)
                PlayerOnTurnChanged?.Invoke(_player.Nick);
            else
                PlayerOnTurnChanged?.Invoke(_opponent);
        }

        public delegate void ResultsEvaluatedEventHandler(string winner, int[] scores);
        public event ResultsEvaluatedEventHandler ResultsEvaluated;
        protected virtual void OnResultsEvaluated()
        {
            int winner = _pairsGameService.GetWinner(_player.Id);
            ResultsEvaluated?.Invoke(winner == _player.Id ? _player.Nick : _opponent, _pairsGameService.GetScores(_player.Id));
        }

        public delegate void OpponentsCardShownEventHandler(Card card);
        public event OpponentsCardShownEventHandler OpponentsCardShown;
        protected virtual void OnOpponentsCardShown(Card card) => OpponentsCardShown?.Invoke(card);

        public delegate void OpponentsCardsHiddenEventHandler(Card card1, Card card2);
        public event OpponentsCardsHiddenEventHandler OpponentsCardsHidden;
        protected virtual void OnOpponentsCardsHidden(Card card1, Card card2) => OpponentsCardsHidden?.Invoke(card1, card2);

        public delegate void OpponentsPairRemovedEventHandler(Card card1, Card card2);
        public event OpponentsPairRemovedEventHandler OpponentsPairRemoved;
        protected virtual void OnOpponentsPairRemoved(Card card1, Card card2) => OpponentsPairRemoved?.Invoke(card1, card2);

        public delegate bool InvitationReceivedEventHandler(string fromPlayer);
        public event InvitationReceivedEventHandler InvitationReceived;
        protected virtual bool OnInvitationReceived(string fromPlayer) => InvitationReceived(fromPlayer);

        public delegate void InvitationReplyReceivedEventHandler(bool isAccepted, string opponent, GameLayout gameLayout);
        public event InvitationReplyReceivedEventHandler InvitationReplyReceived;
        protected virtual void OnInvitationReplyReceived(bool isAccepted)
            => InvitationReplyReceived?.Invoke(isAccepted, _opponent, _gameLayout);

        public delegate void InvitationAcceptedEventHandler(string opponent, GameLayout gameLayout);
        public event InvitationAcceptedEventHandler InvitationAccepted;
        protected virtual void OnInvitationAccepted() => InvitationAccepted?.Invoke(_opponent, _gameLayout);

        public PairsGameClient()
        {
            _channelFactory = new ChannelFactory<IPairsGameService>("PairsGameEndpoint");
            _pairsGameService = _channelFactory.CreateChannel();
        }

        internal bool TryToLogIn(string nick, string password)
        {
            Trace.Write(password + " -> ");
            password = Encryption.Encrypt(password);
            Trace.WriteLine(password);

            int? playerId = _pairsGameService.TryToLogIn(nick, password);
            if (playerId.HasValue)
            {
                _player = new Player(playerId.Value, nick);
                ReadInvitationsAsync();
            }
            Trace.WriteLine(playerId.HasValue ? $"Logged in successfully. Id: {_player.Id}" : "Logging in was not successful.");
            return playerId.HasValue;
        }

        internal bool TryToSignIn(string nick, string password)
        {
            password = Encryption.Encrypt(password);
            bool success = _pairsGameService.TryToSignIn(nick, password);
            Trace.WriteLine(success ? "Sign in successfully." : "Signing in was not successful.");
            return success;
        }

        internal List<string> GetAvailablePlayers()
        {
            var ap = _pairsGameService.GetAvailablePlayers(_player.Id);
            Trace.WriteLine("Available players: " + ap);
            return ap;
        }

        internal bool SendInvitation(GameLayout gameLayout, string toPlayer)
        {
            _opponent = toPlayer;
            _gameLayout = gameLayout;
            bool success = _pairsGameService.SendInvitation(_player.Id, toPlayer, gameLayout);
            if (success)
            {
                ReadInvitaionReplyAsync();
            }
            return success;
        }

        private async void ReadInvitationsAsync()
        {
            string fromPlayer = null;
            await Task.Run(() =>
            {
                while (_pairsGameService != null)
                {
                    fromPlayer = _pairsGameService.ReceiveInvitation(_player.Id);
                    if (fromPlayer != null)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
            });
            bool accepted = OnInvitationReceived(fromPlayer);
            var gameLayout = _pairsGameService.AcceptInvitation(_player.Id, fromPlayer, accepted);
            if (accepted)
            {
                _opponent = fromPlayer;
                _gameLayout = gameLayout;
                OnInvitationAccepted();
            }
            else ReadInvitationsAsync();
        }

        private async void ReadInvitaionReplyAsync()
        {
            bool? accepted = null;
            await Task.Run(() =>
            {
                while (_pairsGameService != null)
                {
                    accepted = _pairsGameService.ReadInvitationReply(_player.Id);
                    if (accepted != null)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
            });
            OnInvitationReplyReceived(accepted.Value);
        }

        //internal bool StartNewGame()
        //{
        //    _gameLayout = GameLayout.ThreeTimesTwo;
        //    bool res = _pairsGameService.StartNewGame(_player.Id, 2, _gameLayout);
        //    OnPlayerOnTurnChanged();
        //    return res;
        //}

        //internal int GetRowCount()
        //{
        //    return _pairsGameService.GetRowCount();
        //}

        //internal int GetColumnCount()
        //{
        //    return _pairsGameService.GetColumnCount();
        //}

        internal void NextMove(ICard card)
        {
            // TODO: do not allow make a move when its mot my turn
            int cardNumber = _pairsGameService.NextMove(_player.Id, card.Row, card.Column);
            OnCardShown(card, cardNumber);

            if (_pairsGameService.GetMoveWasCompleted(_player.Id))
            {
                if (_pairsGameService.GetWasSuccessfulMove(_player.Id))
                {
                    OnFoundPairRemoved(_firstCard, card);
                    if (_pairsGameService.IsEndOfGame(_player.Id))
                    {
                        OnResultsEvaluated();
                        OnPlayerOnTurnChanged();
                    }
                }
                else
                {
                    OnPlayerOnTurnChanged();
                    OnCardsHidden(_firstCard, card);
                    ReadFromServiceAsync();
                }
            }
            else
            {
                _firstCard = card;
            }
        }

        // TODO: add cancelation token - when I close the app to and this taks in the Dispose() method
        private async void ReadFromServiceAsync()
        {
            Card firstOpponentCard = null;
            while (true)
            {
                await Wait();
                var opponentMoves = _pairsGameService.ReadOpponentsMoves(_player.Id);
                Trace.WriteLine(_player.Id + " === " + opponentMoves + " COUNT: " + opponentMoves.Count);
                foreach (var move in opponentMoves)
                {
                    OnOpponentsCardShown(move.Card);

                    if (move.IsCompleted)
                    {
                        if (move.IsSuccessful.Value)
                        {
                            OnOpponentsPairRemoved(firstOpponentCard, move.Card);
                            if (_pairsGameService.IsEndOfGame(_player.Id))
                            {
                                OnResultsEvaluated();
                                OnPlayerOnTurnChanged();
                                return;
                            }
                        }
                        else
                        {
                            OnPlayerOnTurnChanged();
                            OnOpponentsCardsHidden(firstOpponentCard, move.Card);
                            return;
                        }
                    }
                    else
                    {
                        firstOpponentCard = move.Card;
                    }
                }
            }
        }

        private async Task Wait()
        {
            await Task.Delay(1000);
        }

        public void Dispose()
        {
            LogOut();
            _channelFactory.Close();
        }

        private void LogOut()
        {
            bool success = _pairsGameService.LogOut(_player.Id);
            Trace.WriteLine($"Player {_player.Nick} was {(success ? "NOT " : " ")}logged out.");
        }

    }
}
