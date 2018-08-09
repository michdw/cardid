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
            GetUser();
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult AddCardToDeck(Deck deck)
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

            TempData["card-added"] = true;
            return RedirectToAction("EditDeck", "Deck", new { deckID = deck.DeckID });
        }


        public ActionResult ChooseDeckInit(string cardID)
        {
            string userID = GetUser();

            Card card = cardSql.GetCardByID(cardID);
            if (card.UserID != userID)
            {
                ViewBag.UserDecks = deckSql.GetDecksByUserID(userID);
            }
            return View("ChooseDecks", card);
        }


        public ActionResult ChooseDeckSubmit(string cardID, string deckID)
        {
            GetUser();

            Card card = cardSql.GetCardByID(cardID);
            cardSql.AddCardToDeck(card, deckID);

            TempData["card-added"] = card;
            return RedirectToAction("EditDeck", "Deck", new { deckID });
        }


        public ActionResult EditCardInit(string cardID, string deckID)
        {
            GetUser();

            Session["currentdeck"] = deckSql.GetDeckByID(deckID);

            Card card = cardSql.GetCardByID(cardID);

            ViewBag.FormAction = "EditCardSubmit";
            return View("EditCard", card);
        }


        [HttpPost]
        public ActionResult EditCardSubmit(Card newValues)
        {
            GetUser();

            Deck deck = Session["currentdeck"] as Deck;

            cardSql.EditCard(newValues);

            return RedirectToAction("EditCardInit", new { cardID = newValues.CardID, deckID = deck.DeckID });
        }


        public ActionResult RemoveCardFromDeck(string cardID, string deckID)
        {
            GetUser();

            cardSql.RemoveCardFromDeck(cardID, deckID);

            TempData["card-removed"] = true;
            return RedirectToAction("EditDeck", "Deck", new { deckID });
        }

    }
}