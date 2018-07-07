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

        private string GetUser()
        {
            return Session["userid"].ToString();
        }



        public ActionResult Index()
        {
            if (Session["userid"] == null)
            {
                Session["anon"] = "Card";
                return RedirectToAction("Login", "Home");
            }
            string userID = GetUser();

            List<Card> userCards = cardSql.GetCardsByUserID(userID);
            TempData["deckscount"] = deckSql.GetDecksByUserID(userID).Count;
            return View("MainCardView", userCards);
        }


        [HttpPost]
        public ActionResult CardToNewDeck(Deck deck)
        {
            string userID = GetUser();

            Card newCard = new Card()
            {
                UserID = userID,
                Front = deck.NewCardFront,
                Back = deck.NewCardBack
            };

            newCard = cardSql.CreateCard(newCard, userID);
            cardSql.AddCardToDeck(newCard, deck.DeckID);

            deck = deckSql.GetDeckByID(deck.DeckID);

            return RedirectToAction("CreateDeckContinue", "Deck", new { deckID = deck.DeckID });
        }


        public ActionResult ChooseCardsInit(string deckID)
        {
            GetUser();

            Deck deck = deckSql.GetDeckByID(deckID);
            return View("ChooseCards", deck);
        }


        public ActionResult ChooseCardsSubmit(string cardID, string deckID)
        {
            GetUser();

            Card card = cardSql.GetCardByID(cardID);
            cardSql.AddCardToDeck(card, deckID);

            TempData["card-added"] = card;
            return RedirectToAction("EditDeck", "Deck", new { deckID });
        }


        public ActionResult CreateCardInit(string deckID)
        {
            GetUser();

            ViewBag.Deck = deckSql.GetDeckByID(deckID);
            return View("CreateCard");
        }


        [HttpPost]
        public ActionResult CreateCardSubmit(Card newCard, string deckID)
        {
            GetUser();

            string userID = Session["userid"].ToString();

            newCard = cardSql.CreateCard(newCard, userID);
            cardSql.AddCardToDeck(newCard, deckID);

            TempData["card-added"] = newCard;
            return RedirectToAction("EditDeck", "Deck", new { deckID });
        }


        public ActionResult DeleteCard(string cardID)
        {
            string userID = GetUser();

            cardSql.DeleteCard(cardID);

            List<Card> userCards = cardSql.GetCardsByUserID(userID);

            TempData["card-deleted"] = true;
            return RedirectToAction("Index", userCards);
        }


        //edit card contents from card view
        public ActionResult EditCardInit(string cardID)
        {
            GetUser();

            Session["currentdeck"] = null;

            Card card = cardSql.GetCardByID(cardID);

            ViewBag.FormAction = "EditCardSubmit";
            return View("EditCard", card);
        }


        [HttpPost]
        public ActionResult EditCardSubmit(Card newValues)
        {
            GetUser();

            cardSql.EditCard(newValues);

            return RedirectToAction("EditCardInit", new { cardID = newValues.CardID });
        }


        //edit card contents from deck view
        public ActionResult EditCardInitD(string cardID, string deckID)
        {
            GetUser();

            Session["currentdeck"] = deckSql.GetDeckByID(deckID);

            Card card = cardSql.GetCardByID(cardID);

            ViewBag.FormAction = "EditCardSubmitD";
            return View("EditCard", card);
        }


        [HttpPost]
        public ActionResult EditCardSubmitD(Card newValues)
        {
            GetUser();

            Deck deck = Session["currentdeck"] as Deck;

            cardSql.EditCard(newValues);

            return RedirectToAction("EditCardInitD", new { cardID = newValues.CardID, deckID = deck.DeckID });
        }


        public ActionResult SearchCards(string searchString)
        {
            string userID = GetUser();

            TempData["deckscount"] = deckSql.GetDecksByUserID(userID).Count;

            List<Card> matchingCards = cardSql.SearchCardsForText(searchString);
            ViewBag.SearchString = searchString;
            return View("MainCardView", matchingCards);
        }

    }
}