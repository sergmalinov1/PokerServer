using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{

    public enum CardType
    {
        Ace = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    };

    public enum CardSuit
    {
        Club = 1, // ♣
        Diamond, // ♦
        Heart, // ♥
        Spade  // ♠
    };

    /*
    public enum CardSuit
    {
        Club = 0, // ♣
        Diamond = 1, // ♦
        Heart = 2, // ♥
        Spade = 3, // ♠
    }

    public enum CardType
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14,
    }
    */
    public class Card 
    {
        public Card(CardSuit suit, CardType type)
        {
            this.Suit = suit;
            this.Type = type;
        }

        public CardSuit Suit { get; }

        public CardType Type { get; }

        public static Card FromHashCode(int hashCode)
        {
            var suitId = hashCode / 13;
            return new Card((CardSuit)suitId, (CardType)(hashCode - (suitId * 13) + 2));
        }

        public override bool Equals(object obj)
        {
            var anotherCard = obj as Card;
            return anotherCard != null && this.Equals(anotherCard);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)this.Suit * 13) + (int)this.Type - 2;
            }
        }

        public Card DeepClone()
        {
            return new Card(this.Suit, this.Type);
        }

        public override string ToString()
        {
            return $"{this.Type.ToFriendlyString()}{this.Suit.ToFriendlyString()}";
        }

        private bool Equals(Card other)
        {
            return this.Suit == other.Suit && this.Type == other.Type;
        }

    }

}
