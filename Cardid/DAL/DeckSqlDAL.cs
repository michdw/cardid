using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Cardid.Models;
using Dapper;

namespace Cardid.DAL
{
    public class DeckSqlDAL
    {
        private string connectionString;
        public DeckSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private string changeDeckName = "UPDATE [decks] SET DeckName = @deckName WHERE DeckID = @deckID";
        private string createDeck = "INSERT INTO [decks] (DeckName, IsPublic, UserID) VALUES (@deckName, @isPublic, @userID)";
        private string getAllDecks = "SELECT * FROM [decks] WHERE (IsPublic = 1 OR UserID = @userID) ORDER BY DeckName ASC";
        private string getDeckByID = "SELECT * FROM [decks] WHERE DeckID = @deckID";
        private string getDeckByCardID = "SELECT * FROM [decks] JOIN [card_deck] ON card_deck.DeckID = decks.DeckID "
            + "WHERE CardID = @cardID";
        private string getDecksByUserID = "SELECT * FROM [decks] WHERE (UserID = @userID)";
        private string getDecksByTagID = "SELECT * FROM [decks] JOIN [deck_tag] on deck_tag.DeckID = decks.DeckID "
            + "WHERE deck_tag.TagID = @tagID";
        private string getNewUserDeck = "SELECT TOP 1 * FROM [decks] WHERE UserID = @userID ORDER BY DeckID DESC";
        private string makeDeckPrivate = "UPDATE [decks] set IsPublic = 0 WHERE DeckID = @deckID";
        private string makeDeckPublic = "UPDATE [decks] set IsPublic = 1 WHERE DeckID = @deckID";
        private string removeAllCardsFromDeck = "DELETE FROM [card_deck] WHERE DeckID = @deckID";
        private string removeAllSessionsWithDeck = "DELETE FROM [sessions] WHERE DeckID = @deckID";
        private string removeAllTagsFromDeck = "DELETE FROM [deck_tag] WHERE DeckID = @deckID";
        private string removeCard = "DELETE FROM [cards] WHERE CardID = @cardID";
        private string removeDeck = "DELETE FROM [decks] WHERE DeckID = @deckID";
        private string searchDecksByName = "SELECT * FROM decks WHERE DeckName LIKE @text";
        private string searchDecksByTag = "SELECT * FROM decks JOIN [deck_tag] ON deck_tag.DeckID = decks.DeckID "
            + "JOIN [tags] ON tags.TagID = deck_tag.TagID WHERE tags.TagName = @text";


        public void ChangeDeckName(string deckName, string deckID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(changeDeckName, new { deckName, deckID });
            }
        }


        public Deck CreateDeck(string deckName, string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(createDeck, new { deckName, isPublic = 0, userID });
                Deck newDeck = db.Query<Deck>(getNewUserDeck, new { userID }).ToList().FirstOrDefault<Deck>();
                return newDeck.TrimValues();
            }
        }


        public void DeleteDeck(string deckID, List<Card> cardsInDeck)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(removeAllCardsFromDeck, new { deckID });
                db.Execute(removeAllSessionsWithDeck, new { deckID });
                db.Execute(removeAllTagsFromDeck, new { deckID });
                foreach (Card card in cardsInDeck)
                {
                    db.Execute(removeCard, new { cardID = card.CardID });
                }
                db.Execute(removeDeck, new { deckID });
            }
        }


        public List<Deck> GetAllDecks(string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Deck> list = db.Query<Deck>(getAllDecks, new { userID }).ToList<Deck>();
                foreach (Deck deck in list)
                {
                    deck.TrimValues();
                }
                return list;
            }
        }


        public Deck GetDeckByID(string deckID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Deck>(getDeckByID, new { deckID }).ToList().FirstOrDefault<Deck>().TrimValues();
            }
        }


        public Deck GetDeckByCardID(string cardID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Deck>(getDeckByCardID, new { cardID }).ToList().FirstOrDefault<Deck>().TrimValues();
            }
        }


        public List<Deck> GetDecksByTagID(string tagID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Deck> list = db.Query<Deck>(getDecksByTagID, new { tagID }).ToList<Deck>();
                foreach (Deck deck in list)
                {
                    deck.TrimValues();
                }
                return list;
            }
        }


        public List<Deck> GetDecksByUserID(string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Deck> list = db.Query<Deck>(getDecksByUserID, new { userID }).ToList<Deck>();
                foreach (Deck deck in list)
                {
                    deck.TrimValues();
                }
                return list;
            }
        }


        public void MakeDeckPrivate(string deckID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(makeDeckPrivate, new { deckID });
            }
        }


        public void MakeDeckPublic(string deckID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(makeDeckPublic, new { deckID });
            }
        }


        public List<Deck> SearchDecksByName(string text)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Deck> list = db.Query<Deck>(searchDecksByName, new { text = "%" + text + "%" }).ToList<Deck>();
                foreach (Deck deck in list)
                {
                    deck.TrimValues();
                }
                return list;
            }
        }


        public List<Deck> SearchDecksByTag(string text)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Deck> list = db.Query<Deck>(searchDecksByTag, new { text }).ToList<Deck>();
                foreach (Deck deck in list)
                {
                    deck.TrimValues();
                }
                return list;
            }
        }

    }
}