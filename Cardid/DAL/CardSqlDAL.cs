using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Cardid.Models;
using Dapper;

namespace Cardid.DAL
{
    public class CardSqlDAL
    {
        readonly string connectionString;
        public CardSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        readonly string addCardToDeck = "INSERT INTO card_deck (CardID, DeckID) VALUES (@cardID, @deckID);";
        readonly string createCard = "INSERT INTO [cards] (Front, Back, UserID) VALUES (@front, @back, @userID);";
        readonly string editCard = "UPDATE [cards] SET Front = @front, Back = @back WHERE CardID = @cardID";
        readonly string getCardByID = "SELECT * FROM [cards] WHERE CardID = @cardID";
        readonly string getCardsByDeckID = "SELECT * FROM [cards] JOIN [card_deck] ON card_deck.CardID = cards.CardID "
            + "WHERE card_deck.DeckID = @deckID ORDER BY Front ASC";
        readonly string getCardsByUserID = "SELECT * FROM [cards] WHERE UserID = @userID";
        readonly string getNewCard = "SELECT TOP 1 * FROM [cards] WHERE UserID = @userID ORDER BY CardID DESC";
        readonly string removeCard = "DELETE FROM [cards] WHERE CardID = @cardID";
        readonly string removeCardFromDeck = "DELETE FROM [card_deck] WHERE CardID = @cardID AND DeckID = @deckID";
        readonly string searchCardsForText = "SELECT * FROM[cards] JOIN[card_deck] ON card_deck.CardID = cards.CardID "
            + "JOIN[decks] ON decks.DeckID = card_deck.DeckID WHERE (IsPublic = 1 OR decks.UserID = @userID) "
            + "AND (Front LIKE @text OR Back LIKE @text) ORDER BY Front ASC";



        public void AddCardToDeck(Card card, string deckID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(addCardToDeck, new { cardID = card.CardID, deckID });
            }
        }


        public Card CreateCard(Card card, string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(createCard, new { front = card.Front, back = card.Back, userID });
                Card newCard = db.Query<Card>(getNewCard, new { userID }).ToList().FirstOrDefault();
                return newCard.TrimValues();
            }
        }


        public void EditCard(Card card)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(editCard, new { front = card.Front, back = card.Back, cardID = card.CardID });
            }
        }


        public Card GetCardByID(string cardID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Card>(getCardByID, new { cardID }).ToList().FirstOrDefault().TrimValues();
            }
        }


        public List<Card> GetCardsByDeckID(string deckID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Card> list = db.Query<Card>(getCardsByDeckID, new { deckID }).ToList();
                foreach (Card card in list)
                {
                    card.TrimValues();
                }
                return list;
            }
        }


        public List<Card> GetCardsByUserID(string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Card> list = db.Query<Card>(getCardsByUserID, new { userID }).ToList();
                foreach (Card card in list)
                {
                    card.TrimValues();
                }
                return list;
            }
        }


        public void RemoveCard(string cardID, string deckID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(removeCardFromDeck, new { cardID, deckID });
                db.Execute(removeCard, new { cardID });
            }
        }


        public List<Card> SearchCardsForText(string text, string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Card> list = db.Query<Card>(searchCardsForText, new { text = "%" + text + "%", userID }).ToList();
                foreach (Card card in list)
                {
                    card.TrimValues();
                }
                return list;
            }
        }

    }
}