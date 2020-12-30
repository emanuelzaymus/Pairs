using System.Runtime.Serialization;

namespace Pairs.InterfaceLibrary.Response
{
    [DataContract]
    public class OpponentsMove
    {
        [DataMember]
        public Card Card { get; set; }

        [DataMember]
        public bool? IsSuccessful { get; set; }

        public bool IsCompleted => IsSuccessful != null;

        public OpponentsMove(Card card, bool? isSuccessful)
        {
            Card = card;
            IsSuccessful = isSuccessful;
        }
    }
}
