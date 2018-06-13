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
    public class CardController : Controller
    {
        CardSqlDAL cardSql = new CardSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);
        DeckSqlDAL deckSql = new DeckSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);

        public ActionResult Index()
        {
            if (Session["userid"] == null)
            {
                Session["anon"] = "Card";
                return RedirectToAction("Login", "Home");
            }
            string userID = Session["userid"].ToString();

            List<Card> userCards = cardSql.GetCardsByUserID(userID);
            ViewBag.DecksCount = deckSql.GetDecksByUserID(userID).Count;
            return View("MainCardView", userCards);
        }


        //add cards to deck
        public ActionResult ChooseCardsInit(string deckID)
        {
            Deck deck = deckSql.GetDeckByID(deckID);

            List<Card> availableCards = cardSql.CardsNotWithDeck(deck);

            ViewBag.Deck = deck;
            return View("ChooseCards", availableCards);
        }


        public ActionResult ChooseCardsSubmit(string cardID, string deckID)
        {
            Card card = cardSql.GetCardByID(cardID);
            cardSql.AddCardToDeck(card, deckID);

            TempData["addedCard"] = card;
            return RedirectToAction("EditDeck", new { deckID });
        }


        //create cards
        public ActionResult CreateCardInit()
        {
            return View("CreateCard");
        }


        [HttpPost]
        public ActionResult CreateCardSubmit(Card newCard)
        {
            string userID = Session["userid"].ToString();

            newCard = cardSql.CreateCard(newCard, userID);

            List<Card> userCards = cardSql.GetCardsByUserID(userID);
            ViewBag.DecksCount = deckSql.GetDecksByUserID(userID).Count;
            return View("MainCardView", userCards);
        }


        [HttpPost]
        public ActionResult DeleteCard(string cardID)
        {
            cardSql.DeleteCard(cardID);

            string userID = Session["userid"].ToString();
            List<Card> userCards = cardSql.GetCardsByUserID(userID);

            return RedirectToAction("Index", userCards);
        }


        //edit card contents from card view
        public ActionResult EditCardInit(string cardID)
        {
            Session["currentdeck"] = null;

            Card card = cardSql.GetCardByID(cardID);

            ViewBag.FormAction = "EditCardSubmit";
            return View("EditCard", card);
        }


        [HttpPost]
        public ActionResult EditCardSubmit(Card newValues)
        {
            cardSql.EditCard(newValues);

            return RedirectToAction("EditCardInit", new { cardID = newValues.CardID });
        }


        //edit card contents from deck view
        public ActionResult EditCardInitD(string cardID, string deckID)
        {
            Session["currentdeck"] = deckSql.GetDeckByID(deckID);

            Card card = cardSql.GetCardByID(cardID);

            ViewBag.FormAction = "EditCardSubmitD";
            return View("EditCard", card);
        }


        [HttpPost]
        public ActionResult EditCardSubmitD(Card newValues)
        {
            Deck deck = Session["currentdeck"] as Deck;

            cardSql.EditCard(newValues);

            return RedirectToAction("EditCardInitD", new { cardID = newValues.CardID, deckID = deck.DeckID });
        }


        public ActionResult SearchCards(string searchString)
        {
            string userID = Session["userid"].ToString();
            ViewBag.DecksCount = deckSql.GetDecksByUserID(userID).Count;

            List<Card> matchingCards = cardSql.SearchCardsForText(searchString);
            ViewBag.SearchString = searchString;
            return View("MainCardView", matchingCards);
        }

    }
}