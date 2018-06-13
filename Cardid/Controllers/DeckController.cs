using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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


        public ActionResult Index()
        {
            if (Session["userid"] == null)
            {
                Session["anon"] = "Deck";
                return RedirectToAction("Login", "Home");
            }

            string userID = Session["userid"].ToString();
            ViewBag.UserID = userID;
            ViewBag.TagsByName = tagSql.GetAllTagsByName();
            ViewBag.TagsByPopularity = tagSql.GetAllTagsByPopularity();
            List<Deck> allDecks = deckSql.GetAllDecks(userID);
            return View("MainDeckView", allDecks);
        }


        public ActionResult AddDeckTag(string deckID, string tagID)
        {
            Deck deck = deckSql.GetDeckByID(deckID);
            Tag tag = tagSql.GetTagByID(tagID);
            tagSql.AddTagToDeck(deckID, tagID);

            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult CardsInDeck(string deckID)
        {
            Deck deck = deckSql.GetDeckByID(deckID);
            return View(deck);
        }


        public ActionResult ChooseDecksInit(string cardID)
        {
            Card card = cardSql.GetCardByID(cardID);
            List<Deck> availableDecks = deckSql.DecksNotWithCard(card);

            ViewBag.Card = card;
            return View("ChooseDecks", availableDecks);
        }


        public ActionResult ChooseDecksSubmit(string cardID, string deckID)
        {
            Card card = cardSql.GetCardByID(cardID);
            cardSql.AddCardToDeck(card, deckID);

            TempData["addedCard"] = card;
            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult ChooseOtherDeck(string cardID)
        {
            string userID = Session["userid"].ToString();
            Card publicCard = cardSql.GetCardByID(cardID);

            Card userCard = new Card();
            userCard.Front = publicCard.Front;
            userCard.Back = publicCard.Back;
            userCard = cardSql.CreateCard(userCard, userID);
            return RedirectToAction("ChooseDecksInit", new { cardID = userCard.CardID });
        }


        public ActionResult CreateDeckInit()
        {
            return View("CreateDeck");
        }


        [HttpPost]
        public ActionResult CreateDeckSubmit(Deck newDeck)
        {
            string userID = Session["userid"].ToString();

            newDeck = deckSql.CreateDeck(newDeck.Name, userID);

            return RedirectToAction("EditDeck", new { deckID = newDeck.DeckID });
        }


        public ActionResult CreateTag(Deck deck)
        {
            Tag newTag = deck.NewTag;
            deck = deckSql.GetDeckByID(deck.DeckID);
            newTag = tagSql.CreateTag(newTag.TagName, deck.UserID);
            tagSql.AddTagToDeck(deck.DeckID, newTag.TagID);

            return RedirectToAction("EditDeck", new { deckID = deck.DeckID });
        }


        [HttpPost]
        public ActionResult DeleteDeck(string deckID)
        {
            Deck deck = deckSql.GetDeckByID(deckID);

            List<Card> cardsInDeck = cardSql.GetCardsByDeckID(deck.DeckID);
            List<Card> cardsOnlyInDeck = new List<Card>();
            foreach (Card card in cardsInDeck)
            {
                if (card.Decks.Count == 1)
                {
                    cardsOnlyInDeck.Add(card);
                }
            }
            deckSql.DeleteDeck(deckID, cardsOnlyInDeck);

            return RedirectToAction("Index");
        }


        public ActionResult EditDeck(string deckID)
        {
            Deck deck = deckSql.GetDeckByID(deckID);

            ViewBag.OtherTagsByName = tagSql.GetOtherTagsByName(deckID);
            ViewBag.OtherTagsByPopularity = tagSql.GetOtherTagsByPopularity(deckID);
            return View(deck);
        }


        public ActionResult MakeDeckPrivate(string deckID)
        {
            deckSql.MakeDeckPrivate(deckID);
            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult MakeDeckPublic(string deckID)
        {
            deckSql.MakeDeckPublic(deckID);
            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult RemoveCardFromDeck(string cardID, string deckID)
        {
            deckSql.RemoveCardFromDeck(cardID, deckID);

            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult RemoveDeckTag(string deckID, string tagID)
        {
            Deck deck = deckSql.GetDeckByID(deckID);
            Tag tag = tagSql.GetTagByID(tagID);
            tagSql.RemoveTagFromDeck(deckID, tagID);

            return RedirectToAction("EditDeck", new { deckID });
        }


        public ActionResult SearchDeckNames(string searchString)
        {
            ViewBag.UserID = Session["userid"].ToString();
            ViewBag.TagsByName = tagSql.GetAllTagsByName();
            ViewBag.TagsByPopularity = tagSql.GetAllTagsByPopularity();

            List<Deck> matchingDecks = deckSql.SearchDecksByName(searchString);
            ViewBag.SearchName = searchString;
            return View("MainDeckView", matchingDecks);
        }


        public ActionResult SearchDeckTags(string searchString)
        {
            ViewBag.UserID = Session["userid"].ToString();
            ViewBag.TagsByName = tagSql.GetAllTagsByName();
            ViewBag.TagsByPopularity = tagSql.GetAllTagsByPopularity();

            List<Deck> matchingDecks = deckSql.SearchDecksByTag(searchString);
            ViewBag.SearchTag = searchString;
            return View("MainDeckView", matchingDecks);
        }


        public ActionResult StudyBegin(string deckID)
        {
            Deck deck = deckSql.GetDeckByID(deckID);
            ViewBag.Deck = deck;

            //randomize cards based on bool parameter from view

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
            if (wholeDeck)
            {
                studySql.LogStudySession(study);
            }
            return View("StudyComplete", study);
        }


        public ActionResult StudyRedo(string redo, string deckID)
        {
            Deck deck = deckSql.GetDeckByID(deckID);
            ViewBag.Deck = deck;

            List<string> redoList = redo.Split(',').ToList();
            List<Card> redoCards = new List<Card>();
            foreach (string cardID in redoList)
            {
                Card card = cardSql.GetCardByID(cardID);
                redoCards.Add(card);
            }

            //randomize cards based on bool parameter from view

            Study study = new Study
            {
                DeckID = deckID,
                UserID = Session["userid"].ToString()
            };

            ViewBag.Redo = redoCards;
            return View("Study", study);
        }

    }
}