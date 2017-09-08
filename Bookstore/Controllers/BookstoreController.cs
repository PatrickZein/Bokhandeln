using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookstore.Models;
using System.Threading.Tasks;

namespace Bookstore.Controllers
{
    public class BookstoreController : Controller
    {
        // GET: Bookstore
        public ActionResult Index()
        {
            return View();
        }

        // GET: Bookstore
        public async Task<ActionResult> BookSearch(string searchString, int startPos = 1, int maxCount = 0)
        {
            var bookSelection = await PerformBookSearch(searchString, startPos, maxCount);
            return View();
        }

        public async Task<IEnumerable<Book>> TestBookSearch(string searchString, int startPos = 1, int maxCount = 0)
        {
            // Denna metod har skapats endast för systemtest via UnitTest
            var bookSelection = await PerformBookSearch(searchString, startPos, maxCount);
            var bookList = new List<Book>();
            if (Session["MemBookSearch"] != null)
            {
                bookList = Session["MemBookSearch"] as List<Book>;
            }
            if(bookList.Count == 0)
            {
                bookList.Add(new Book(1, "---", "---", 0, 0));
            }
            return bookList;
        }

        public async Task<IEnumerable<Order>> TestBookOrder(string searchString, int startPos = 1, int maxCount = 0)
        {
            // Denna metod har skapats endast för systemtest via UnitTest

            // Finns det några tidigare beställnignar sparade i session?
            var bookSelection = await PerformBookSearch(searchString, startPos, maxCount);
            var orderedBooks = new List<Order>();
            if (Session["MemOrderedBooks"] != null)
            {
                orderedBooks = Session["MemOrderedBooks"] as List<Order>;
            }
            return orderedBooks;
        }

        private async Task<ActionResult> PerformBookSearch(string searchString, int startPos = 1, int maxCount = 0)
        {
            if ((searchString != null) && (searchString.Equals("#order#")))
            {
                // Det handlar om en beställning!
                var orderedBooks = new List<Order>();
                var bookList = Session["MemBookSearch"] as List<Book>;

                // Finns det några tidigare beställnignar sparade i session?
                if (Session["MemOrderedBooks"] != null)
                {
                    orderedBooks = Session["MemOrderedBooks"] as List<Order>;
                }

                // Lägg till info om den nya beställningen i listan.
                // Så länge antalet är större än noll!
                if(maxCount > 0)
                {
                    foreach (Book bookObject in bookList)
                    {
                        if ((bookObject.Counter == startPos) && (!bookObject.Author.Equals("---")))
                        {
                            // Vi har hittat boken!
                            orderedBooks.Add(new Order(bookObject, maxCount));

                            // Spara info om alla beställningar i session.
                            Session["MemOrderedBooks"] = orderedBooks;
                        }
                    }
                    // Glöm bort sökningen.
                    Session["MemSearch"] = null;
                    Session["MemBookSearch"] = null;
                }
            }
            else
            {
                // Nej, det handlar om en sökning.
                // Har användaren valt att bläddra sin tidigare sökning?
                if ((maxCount == 0) && (Session["MemSearch"] != null))
                {
                    searchString = (string) Session["MemSearch"];
                }
                maxCount = 5;
                if (searchString != null && searchString.Length > 0)
                { 
                    var bookList = await Utilities.BookSearchAsync(searchString, startPos, maxCount);

                    // Spara denna sökning i session!
                    Session["MemSearch"] = searchString;
                    Session["MemStartPos"] = startPos;
                    Session["MemBookSearch"] = bookList;
                }
            }
            return View();
        }

        // GET: Bookstore
        public ActionResult CheckBasket(string action, string author, string title)
        {
            if (action.Equals("remove"))
            {
                var orderedBooks = new List<Order>();

                if(Session["MemOrderedBooks"] != null)
                { 
                    orderedBooks = Session["MemOrderedBooks"] as List<Order>;
                }

                if(orderedBooks != null)
                { 
                    for (int i=0;i < orderedBooks.Count;)
                    {
                        if ((orderedBooks.ElementAt(i).Author.Equals(author)) && (orderedBooks.ElementAt(i).Title.Equals(title)))
                        {
                            // Ta bort beställningsraden!
                            orderedBooks.Remove(orderedBooks.ElementAt(i));
                        }
                        else i++;
                    }
                }

                // Spara info om alla beställningar i session.
                Session["MemOrderedBooks"] = orderedBooks;
            }
            return View();
        }

        // GET: Bookstore
        public async Task<ActionResult> CheckOut()
        {
            // Läs in all beställningar ur session controller.
            var orderedBooks = new List<Order>();

            // Finns det några tidigare beställnignar sparade i session?
            if (Session["MemOrderedBooks"] != null)
            {
                orderedBooks = Session["MemOrderedBooks"] as List<Order>;

                // Kontrollera hur många av de beställda böckerna som kan levereras

                foreach (Order orderObject in orderedBooks)
                {
                    // Leta upp boken i databasen, föt att se hur många böcker som finns på lager
                    var bookList = await Utilities.BookSearchAsync(orderObject.Author, 1, 100);

                    foreach (Book bookObject in bookList)
                    { 
                        if (bookObject.Author.Equals(orderObject.Author) &&
                            bookObject.Title.Equals(orderObject.Title) &&
                            bookObject.Price == orderObject.Price)
                        {
                            // Uppdatera orderraden
                            if (bookObject.InStock >= orderObject.Copies)
                            {
                                orderObject.Verified = orderObject.Copies;
                            }
                            else
                            {
                                orderObject.Verified = bookObject.InStock;
                            }// Beräkna den verifierade beställningens pris
                            orderObject.VerCost = orderObject.Verified * orderObject.Price;
                        }
                    }
                }
                // Spara uppdaterad info om alla beställningar i session.
                Session["MemOrderedBooks"] = orderedBooks;
            }
            return View();
        }

        public async Task<IEnumerable<Order>> TestCheckOut()
        {
            // Denna metod har skapats endast för systemtest via UnitTest

            await CheckOut();
            var orderedBooks = new List<Order>();
            if (Session["MemOrderedBooks"] != null)
            {
                orderedBooks = Session["MemOrderedBooks"] as List<Order>;
            }
            return orderedBooks;
        }
    }
}