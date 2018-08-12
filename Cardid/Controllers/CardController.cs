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


        public ActionResult EditCardInit(string cardID, string deckID, string searchString)
        {
            GetUser();

            Card card = cardSql.GetCardByID(cardID);

            if (deckID != null)
            {
                card.CurrentDeckID = deckID;
            }
            else if (searchString != null)
            {
                card.CurrentSearchString = searchString;
            }
            return View("EditCard", card);
        }


        [HttpPost]
        public ActionResult EditCardSubmit(Card newValues)
        {
            GetUser();

            cardSql.EditCard(newValues);

            return RedirectToAction("EditCardInit", new { cardID = newValues.CardID });
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