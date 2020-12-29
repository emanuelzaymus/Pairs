using System.Runtime.Serialization;

namespace Pairs.InterfaceLibrary.Response
{
    [DataContract]
    public class Card
    {
        [DataMember]
        public int Row { get; set; }

        [DataMember]
        public int Column { get; set; }

        [DataMember]
        public int CardNumber { get; set; }

        public Card(int row, int column, int cardNumber)
        {
            Row = row;
            Column = column;
            CardNumber = cardNumber;
        }
    }
}