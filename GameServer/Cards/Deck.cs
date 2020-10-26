using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GameServer
{
    class Deck : IDeck
    {
        public List<Card> AllCards = new List<Card>();

        private static readonly IEnumerable<CardType> AllCardTypes = new List<CardType>
                                                                         {
                                                                             CardType.Two,
                                                                             CardType.Three,
                                                                             CardType.Four,
                                                                             CardType.Five,
                                                                             CardType.Six,
                                                                             CardType.Seven,
                                                                             CardType.Eight,
                                                                             CardType.Nine,
                                                                             CardType.Ten,
                                                                             CardType.Jack,
                                                                             CardType.Queen,
                                                                             CardType.King,
                                                                             CardType.Ace,
                                                                         };

        private static readonly IEnumerable<CardSuit> AllCardSuits = new List<CardSuit>
                                                                         {
                                                                             CardSuit.Club,
                                                                             CardSuit.Diamond,
                                                                             CardSuit.Heart,
                                                                             CardSuit.Spade,
                                                                         };

        private IList<Card> listOfCards;

        private int cardIndex;

        public Deck()
        {
           // var cards = new List<Card>();
            foreach (var cardSuit in AllCardSuits)
            {
                foreach (var cardType in AllCardTypes)
                {
                    AllCards.Add(new Card(cardSuit, cardType));
                }
            }

         //   AllCards = cards.AsReadOnly();    
        }

        public void Shuffle()
        {
            listOfCards = AllCards.Shuffle().ToList();
            cardIndex = AllCards.Count;
        }

        public void ShowAllCard()
        {
            int i = 1;
            foreach(Card card in listOfCards)
            {
                Console.WriteLine("Card " + i + " : " + card.ToString());
                i++;
            }
        }

        /*public Deck()
        {
            //this.listOfCards = AllCards.Shuffle().ToList();
           // this.cardIndex = AllCards.Count;
        }*/

        public Card GetNextCard()
        {
            if (this.cardIndex == 0)
            {
             //   throw new InternalGameException("Deck is empty!");
            }

            this.cardIndex--;
            var card = this.listOfCards[this.cardIndex];
            return card;
        }
    }
}
