using System.Runtime.Serialization;

namespace Pairs.InterfaceLibrary.Response
{
    [DataContract]
    public class OpponentsMove
    {
        [DataMember]
        public Card Card { get; set; }

        //public Card SecondCard { get; set; }

        [DataMember]
        public bool? IsSuccessful { get; set; }
        //public bool? Success => IsCompletedMove ? (FirstCard.CardFrontFace == SecondCard.CardFrontFace) : (bool?)null;

        public bool IsCompleted => IsSuccessful != null;
        //public bool IsCompletedMove => SecondCard != null;

        public OpponentsMove(Card card, bool? isSuccessful)
        {
            Card = card;
            IsSuccessful = isSuccessful;
        }
    }
}
