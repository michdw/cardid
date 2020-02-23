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
        readonly CardSqlDAL cardSql = new CardSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);
        readonly DeckSqlDAL deckSql = new DeckSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);
        readonly StudySqlDAL studySql = new StudySqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);
        readonly TagSqlDAL tagSql = new TagSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);
        readonly UserSqlDAL userSql = new UserSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);

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

        private string GetBackground()
        {
            Background bg = new Background();
            return bg.Path;
        }

        private string GetUserID()
        {
            return Session["userid"].ToString();
        }



        public ActionResult Index()
        {
            if (Session["userid"] == null)
            {
                Session["anon"] = "Deck";
                return RedirectToAction("Login", "Home");
            }
            string userID = GetUserID();

            ViewBag.UserID = userID;
            ViewBag.TagsByName = tagSql.GetAllTagsByName();
            ViewBag.TagsByPopularity = tagSql.GetAllTagsByPopularity();

            List<Deck> displayDecks = new List<Deck>();
            if (Session["display-decks"] == null)
            {
                displayDecks = deckSql.GetAllDecks(userID);
            }
            else
            {
                displayDecks = Session["display-decks"] as List<Deck>;
            }

            if (Session["search-deckname"] != null)
            {
                ViewBag.SearchName = Session["search-deckname"].ToString();
            }
            if (Session["search-decktag"] != null)
            {
                ViewBag.SearchTag = Session["search-decktag"].ToString();
            }

            Session["background"] = GetBackground();
            return View("MainDeckView", displayDecks);
        }


        public ActionResult AddDeckTag(string deckID, string tagID)
        {
            GetUserID();

            tagSql.AddTagToDeck(deckID, tagID);

            Tag tag = tagSql.GetTagByID(tagID);
            TempData["tag-added"] = tag;

            return RedirectToAction("EditDeck", new { deckID, newBackground = false });
        }


        public ActionResult ChangeDeckName(Deck deck)
        {
            GetUserID();

            Deck currentDeck = deckSql.GetDeckByID(deck.DeckID);
            if (deck.DeckName != currentDeck.DeckName)
            {
                deckSql.ChangeDeckName(deck.DeckName, deck.DeckID);
                TempData["deckname-changed"] = true;
            }

            return RedirectToAction("EditDeck", new { deckID = deck.DeckID, newBackground = false });
        }


        public ActionResult CreateDeckInit()
        {
            string userID = GetUserID();
            Deck newDeck = new Deck
            {
                UserID = userID
            };

            Session["background"] = GetBackground();
            return View("CreateDeck", newDeck);
        }


        [HttpPost]
        public ActionResult CreateDeckSubmit(Deck deck)
        {
            string userID = GetUserID();

            if (deck.DeckName == null)
            {
                TempData["deckname-missing"] = true;
                return View("CreateDeck");
            }

            deck = deckSql.CreateDeck(deck.DeckName, userID);
            return View("EditDeck", deck);
        }


        [HttpPost]
        public ActionResult CreateTagForDeck(Deck deck)
        {
            GetUserID();

            Tag newTag = deck.NewTag;
            string tagName = newTag.TagName;
            tagName = tagName.ToLower();
            if (tagName == null || tagName.Length == 0)
            {
                TempData["tagname-missing"] = true;
                return RedirectToAction("EditDeck", new { deckID = deck.DeckID, newBackground = false });
            }

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
            return RedirectToAction("EditDeck", new { deckID = deck.DeckID, newBackground = false });
        }


        [HttpPost]
        public ActionResult DeleteDeck(string deckID)
        {
            GetUserID();

            Deck deck = deckSql.GetDeckByID(deckID);
            List<Card> cardsInDeck = cardSql.GetCardsByDeckID(deck.DeckID);

            deckSql.DeleteDeck(deckID, cardsInDeck);
            TempData["deck-deleted"] = true;
            return RedirectToAction("Index");
        }


        public ActionResult EditDeck(string deckID, bool newBackground)
        {
            string userID = GetUserID();

            Deck deck = deckSql.GetDeckByID(deckID);

            if (deck.UserID != userID)
            {
                return RedirectToAction("Index", "Error");
            }

            if (newBackground)
            {
                Session["background"] = GetBackground();
            }
            return View(deck);
        }


        public ActionResult AccountToDeck(string deckID)
        {
            string userID = GetUserID();
            Deck deck = deckSql.GetDeckByID(deckID);

            if (deck.UserID == userID)
            {
                return RedirectToAction("EditDeck", new { deckID, newBackground = true });
            }
            else
            {
                return RedirectToAction("ViewDeck", new { deckID });
            }
        }


        public ActionResult MakeDeckPrivate(string deckID)
        {
            GetUserID();

            deckSql.MakeDeckPrivate(deckID);

            return RedirectToAction("EditDeck", new { deckID, newBackground = false });
        }


        public ActionResult MakeDeckPublic(string deckID)
        {
            GetUserID();

            deckSql.MakeDeckPublic(deckID);

            return RedirectToAction("EditDeck", new { deckID, newBackground = false });
        }


        public ActionResult RemoveDeckTag(string deckID, string tagID)
        {
            GetUserID();

            tagSql.RemoveTagFromDeck(deckID, tagID);

            TempData["tag-removed"] = true;

            return RedirectToAction("EditDeck", new { deckID, newBackground = false });
        }


        public ActionResult SearchDeckNames(string searchString)
        {
            string userID = GetUserID();

            if (searchString.Length == 0)
            {
                TempData["searchstring-missing"] = true;
                return RedirectToAction("Index");
            }

            List<Deck> displayDecks = deckSql.SearchDecksByName(searchString, userID);
            Session["display-decks"] = displayDecks;

            Session["search-deckname"] = searchString;
            return RedirectToAction("Index");
        }


        public ActionResult SearchDeckTags(string searchString)
        {
            string userID = GetUserID();

            List<Deck> displayDecks = deckSql.SearchDecksByTag(searchString, userID);
            Session["display-decks"] = displayDecks;

            Session["search-decktag"] = searchString;
            return RedirectToAction("Index");
        }


        public ActionResult ShowAllDecks()
        {
            Session["search-deckname"] = null;
            Session["search-decktag"] = null;
            Session["display-decks"] = null;
            return RedirectToAction("Index");
        }


        public ActionResult StudyBegin(string deckID, bool frontFirst)
        {
            string userID = GetUserID();

            Deck deck = deckSql.GetDeckByID(deckID);
            List<Card> cards = deck.Cards;
            cards.Shuffle();

            Study study = new Study
            {
                DeckID = deckID,
                UserID = userID,
                Cards = cards,
                FrontFirst = frontFirst,
                WholeDeck = true
            };

            Session["background"] = GetBackground();
            return View("Study", study);
        }


        public ActionResult StudyComplete(Study study)
        {
            return View(study);
        }


        [HttpPost]
        public ActionResult StudyLog(Study study)
        {
            GetUserID();

            if (study.WholeDeck)
            {
                study.TimeOf = DateTime.Now;
                studySql.LogStudySession(study);
            }
            return RedirectToAction("StudyComplete", study);
        }


        public ActionResult StudyRedo(string redo, string deckID, bool frontFirst)
        {
            string userID = GetUserID();

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


        public ActionResult ViewDeck(string deckID)
        {
            GetUserID();

            Deck deck = deckSql.GetDeckByID(deckID);

            if (deck.IsPublic == false)
            {
                return RedirectToAction("Index", "Error");
            }

            Session["background"] = GetBackground();
            return View(deck);
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