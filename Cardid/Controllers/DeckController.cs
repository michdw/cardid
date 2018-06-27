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
        UserSqlDAL userSql = new UserSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);

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
            TempData["tag-added"] = tag;

            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult CardsInDeck(string deckID)
        {
            GetUser();

            Deck deck = deckSql.GetDeckByID(deckID);
            return View(deck);
        }


        public ActionResult ChangeDeckName(Deck deck)
        {
            Deck currentDeck = deckSql.GetDeckByID(deck.DeckID);
            deckSql.ChangeDeckName(deck.DeckName, deck.DeckID);
            deck = deckSql.GetDeckByID(deck.DeckID);

            TempData["deckname-changed"] = true;
            return RedirectToAction("EditDeck", new { deckID = deck.DeckID });
        }


        public ActionResult ChooseDecksInit(string cardID)
        {
            GetUser();

            Card card = cardSql.GetCardByID(cardID);
            return View("ChooseDecks", card);
        }


        public ActionResult ChooseDecksSubmit(string cardID, string deckID)
        {
            GetUser();

            Card card = cardSql.GetCardByID(cardID);
            cardSql.AddCardToDeck(card, deckID);

            TempData["card-added"] = card;
            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult ChooseOtherDeck(string cardID)
        {
            string userID = GetUser();
            Card publicCard = cardSql.GetCardByID(cardID);
            Card userCard = new Card
            {
                Front = publicCard.Front,
                Back = publicCard.Back
            };
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

            if (newDeck.DeckName == null)
            {
                TempData["deckname-missing"] = true;
                return View("CreateDeck");
            }

            newDeck = deckSql.CreateDeck(newDeck.DeckName, userID);

            TempData["deck-created"] = true;
            return RedirectToAction("EditDeck", new { deckID = newDeck.DeckID });
        }


        public ActionResult CreateTag(string tagName)
        {
            string userID = GetUser();
            tagName = tagName.ToLower();

            //check if tag name exists already
            List<Tag> allTags = tagSql.GetAllTagsByName();
            foreach (Tag tag in allTags)
            {
                if (tagName == tag.TagName)
                {
                    TempData["tag-exists"] = tag.TagName;
                    return RedirectToAction("Index");
                }
            }

            tagSql.CreateTag(tagName, userID);
            TempData["tag-added"] = tagName;
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult CreateTagForDeck(Deck deck)
        {
            GetUser();

            Tag newTag = deck.NewTag;
            string tagName = newTag.TagName;
            tagName = tagName.ToLower();

            //if tag name exists already, add it to deck
            List<Tag> allTags = tagSql.GetAllTagsByName();
            foreach (Tag tag in allTags)
            {
                if (tagName == tag.TagName)
                {
                    tagSql.AddTagToDeck(deck.DeckID, tag.TagID);
                }
            }

            deck = deckSql.GetDeckByID(deck.DeckID);
            newTag = tagSql.CreateTag(tagName, deck.UserID);
            tagSql.AddTagToDeck(deck.DeckID, newTag.TagID);

            TempData["tag-added"] = newTag.TagName;
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
            TempData["deck-deleted"] = true;
            return RedirectToAction("Index");
        }


        public ActionResult DeleteTag(string tagID)
        {
            string userID = GetUser();

            tagSql.DeleteTag(tagID);
            TempData["tag-deleted"] = true;
            return RedirectToAction("TagView", new { userID });
        }


        public ActionResult EditDeck(string deckID)
        {
            GetUser();

            Deck deck = deckSql.GetDeckByID(deckID);
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

            TempData["card-removed"] = true;
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


        public ActionResult StudyBegin(string deckID, bool frontFirst)
        {
            string userID = GetUser();

            Deck deck = deckSql.GetDeckByID(deckID);
            List<Card> cards = deck.Cards();
            cards.Shuffle();

            Study study = new Study
            {
                DeckID = deckID,
                UserID = userID,
                Cards = cards,
                FrontFirst = frontFirst,
                WholeDeck = true
            };
            return View("Study", study);
        }


        [HttpPost]
        public ActionResult StudyLog(Study study)
        {
            GetUser();

            if (study.WholeDeck)
            {
                study.TimeOf = DateTime.Now;
                studySql.LogStudySession(study);
            }
            return View("StudyComplete", study);
        }


        public ActionResult StudyRedo(string redo, string deckID, bool frontFirst)
        {
            string userID = GetUser();

            Deck deck = deckSql.GetDeckByID(deckID);
            List<Card> redoCards = CardsToRedo(redo);
            redoCards.Shuffle();

            Study study = new Study
            {
                DeckID = deckID,
                UserID = userID,
                Cards = redoCards,
                FrontFirst = frontFirst,
                WholeDeck = false
            };
            return View("Study", study);
        }



        public ActionResult TagView(string userID)
        {
            GetUser();

            User user = userSql.GetUserByID(userID);
            List<Tag> userTags = user.Tags();

            return View("TagView", userTags);
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