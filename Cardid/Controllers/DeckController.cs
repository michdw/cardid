using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Cardid.DAL;
using Cardid.Models;

namespace Cardid.Controllers
{
    public class DeckController : Controller
    {
        CardSqlDAL cardSql = new CardSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);
        DeckSqlDAL deckSql = new DeckSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);
        StudySqlDAL studySql = new StudySqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);
        TagSqlDAL tagSql = new TagSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);

        private string GetUser()
        {
            return Session["userid"].ToString();
        }
        
        private List<Card> CardsToRedo(string redo)
        {
            List<string> redoList = redo.Split(',').ToList();
            List<Card> redoCards = new List<Card>();
            foreach (string cardID in redoList)
            {
                Card card = cardSql.GetCardByID(cardID);
                redoCards.Add(card);
            }
            return redoCards;
        }



        public ActionResult Index()
        {
            if (Session["userid"] == null)
            {
                Session["anon"] = "Deck";
                return RedirectToAction("Login", "Home");
            }
            string userID = GetUser();
            ViewBag.UserID = userID;

            ViewBag.TagsByName = tagSql.GetAllTagsByName();
            ViewBag.TagsByPopularity = tagSql.GetAllTagsByPopularity();

            List<Deck> allDecks = deckSql.GetAllDecks(userID);
            return View("MainDeckView", allDecks);
        }


        public ActionResult AddDeckTag(string deckID, string tagID)
        {
            GetUser();

            Deck deck = deckSql.GetDeckByID(deckID);
            tagSql.AddTagToDeck(deckID, tagID);

            Tag tag = tagSql.GetTagByID(tagID);
            TempData["addedtag"] = tag;

            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult CardsInDeck(string deckID)
        {
            GetUser();

            Deck deck = deckSql.GetDeckByID(deckID);
            return View(deck);
        }


        public ActionResult ChooseDecksInit(string cardID)
        {
            GetUser();

            Card card = cardSql.GetCardByID(cardID);
            List<Deck> availableDecks = deckSql.DecksNotWithCard(card);

            ViewBag.Card = card;
            return View("ChooseDecks", availableDecks);
        }


        public ActionResult ChooseDecksSubmit(string cardID, string deckID)
        {
            GetUser();

            Card card = cardSql.GetCardByID(cardID);
            cardSql.AddCardToDeck(card, deckID);

            TempData["addedCard"] = card;
            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult ChooseOtherDeck(string cardID)
        {
            string userID = GetUser();
            Card publicCard = cardSql.GetCardByID(cardID);

            Card userCard = new Card();
            userCard.Front = publicCard.Front;
            userCard.Back = publicCard.Back;
            userCard = cardSql.CreateCard(userCard, userID);
            return RedirectToAction("ChooseDecksInit", new { cardID = userCard.CardID });
        }


        public ActionResult CreateDeckInit()
        {
            GetUser();
            return View("CreateDeck");
        }


        [HttpPost]
        public ActionResult CreateDeckSubmit(Deck newDeck)
        {
            string userID = GetUser();
            newDeck = deckSql.CreateDeck(newDeck.DeckName, userID);

            return RedirectToAction("EditDeck", new { deckID = newDeck.DeckID });
        }


        public ActionResult CreateTag(string tagName)
        {
            string userID = GetUser();
            tagName = tagName.ToLower();
            tagSql.CreateTag(tagName, userID);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult CreateTagForDeck(Deck deck)
        {
            GetUser();

            Tag newTag = deck.NewTag;
            string tagName = newTag.TagName;
            tagName = tagName.ToLower();
            
            deck = deckSql.GetDeckByID(deck.DeckID);
            newTag = tagSql.CreateTag(tagName, deck.UserID);
            tagSql.AddTagToDeck(deck.DeckID, newTag.TagID);

            return RedirectToAction("EditDeck", new { deckID = deck.DeckID });
        }


        [HttpPost]
        public ActionResult DeleteDeck(string deckID)
        {
            GetUser();

            Deck deck = deckSql.GetDeckByID(deckID);
            List<Card> cardsInDeck = cardSql.GetCardsByDeckID(deck.DeckID);

            foreach (Card card in cardsInDeck)
            {
                if (card.Decks().Count > 1)
                {
                    cardsInDeck.Remove(card);
                }
            }
            deckSql.DeleteDeck(deckID, cardsInDeck);

            return RedirectToAction("Index");
        }


        public ActionResult ChangeDeck
            
            (Deck deck)
        {
            Deck currentDeck = deckSql.GetDeckByID(deck.DeckID);
            deckSql.ChangeDeckName(deck.DeckName, deck.DeckID);
            deck = deckSql.GetDeckByID(deck.DeckID);

            TempData["decknamechanged"] = true;
            return RedirectToAction("EditDeck", new { deckID = deck.DeckID });
        }


        public ActionResult DeleteTag(string tagID)
        {
            GetUser();

            tagSql.DeleteTag(tagID);
            TempData["deletedtag"] = true;
            return RedirectToAction("TagView");
        }


        public ActionResult EditDeck(string deckID)
        {
            GetUser();

            Deck deck = deckSql.GetDeckByID(deckID);

            ViewBag.OtherTagsByName = tagSql.GetOtherTagsByName(deckID);
            ViewBag.OtherTagsByPopularity = tagSql.GetOtherTagsByPopularity(deckID);
            return View(deck);
        }


        public ActionResult MakeDeckPrivate(string deckID)
        {
            GetUser();

            deckSql.MakeDeckPrivate(deckID);

            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult MakeDeckPublic(string deckID)
        {
            GetUser();

            deckSql.MakeDeckPublic(deckID);

            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult RemoveCardFromDeck(string cardID, string deckID)
        {
            GetUser();

            deckSql.RemoveCardFromDeck(cardID, deckID);

            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult RemoveDeckTag(string deckID, string tagID)
        {
            GetUser();

            Deck deck = deckSql.GetDeckByID(deckID);
            Tag tag = tagSql.GetTagByID(tagID);
            tagSql.RemoveTagFromDeck(deckID, tagID);

            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult SearchDeckNames(string searchString)
        {
            string userID = GetUser();
            ViewBag.UserID = userID;
            ViewBag.TagsByName = tagSql.GetAllTagsByName();
            ViewBag.TagsByPopularity = tagSql.GetAllTagsByPopularity();

            List<Deck> matchingDecks = deckSql.SearchDecksByName(searchString);
            ViewBag.SearchName = searchString;
            return View("MainDeckView", matchingDecks);
        }


        public ActionResult SearchDeckTags(string searchString)
        {
            string userID = GetUser();
            ViewBag.UserID = userID;
            ViewBag.TagsByName = tagSql.GetAllTagsByName();
            ViewBag.TagsByPopularity = tagSql.GetAllTagsByPopularity();

            List<Deck> matchingDecks = deckSql.SearchDecksByTag(searchString);
            ViewBag.SearchTag = searchString;
            return View("MainDeckView", matchingDecks);
        }


        public ActionResult StudyBegin(string deckID)
        {
            GetUser();

            Deck deck = deckSql.GetDeckByID(deckID);
            ViewBag.Deck = deck;

            List<Card> cards = deck.Cards();
            cards.Shuffle();
            ViewBag.Cards = cards;

            Study study = new Study
            {
                DeckID = deckID,
                UserID = Session["userid"].ToString()
            };
            return View("Study", study);
        }


        [HttpPost]
        public ActionResult StudyLog(Study study, bool wholeDeck)
        {
            GetUser();

            if (wholeDeck)
            {
                study.TimeOf = DateTime.Now;
                studySql.LogStudySession(study);
            }
            return View("StudyComplete", study);
        }


        public ActionResult StudyRedo(string redo, string deckID)
        {
            string userID = GetUser();

            Deck deck = deckSql.GetDeckByID(deckID);
            ViewBag.Deck = deck;

            List<Card> redoCards = CardsToRedo(redo);
            redoCards.Shuffle();
            ViewBag.Cards = redoCards;

            Study study = new Study
            {
                DeckID = deckID,
                UserID = userID
            };
            return View("Study", study);
        }



        public ActionResult TagView(List<Tag> userTags)
        {
            GetUser();

            return View("ManageTags", userTags);
        }

    }



    public static class Randomize
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}