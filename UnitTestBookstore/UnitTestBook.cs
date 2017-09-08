using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Bookstore.Models;
using Bookstore.Controllers;

namespace UnitTestBookstore
{
    [TestClass]
    public class UnitTestBook
    {
        [TestMethod]
        public void TestBook()
        {
            // ### Arrange ###
            BookstoreController statController = new BookstoreController();
            if (statController == null)
            {
                // ERROR: Ingen boklista kunde hittas!
                Assert.Fail("10: Uppstarten fungerade inte.");
            }
            var bookListRaw = statController.TestBookSearch("e", 1, 5); // Försök läsa in max 5 böcker som matchar sökbegreppet "a".
            List<Book> bookListProp = new List <Book>();
            if (bookListRaw == null)
            {
                // ERROR: Ingen boklista kunde hittas!
                Assert.Fail("20: Ingen boklista funnen.");
            }
            else
            {
                foreach (Book book in bookListRaw.Result)
                {
                    bookListProp.Add(book);
                }
                if (bookListProp == null || bookListProp.Count == 0)
                {
                    // ERROR: Ingen boklista kunde hittas!
                    Assert.Fail("30: Inga böcker funna i boklistan.");
                }
                else bookListProp.Add(new Book(100, "Testbok 100", "Testförfattare 100", (decimal)10.95, 5)); // Infoga en testbok med rad nr 100.
            }

            // ### Act ###
            bookListRaw = statController.TestBookSearch("#order#", 100, 10); // Beställ 10 ex av testboken, även om det bara finns 5 på lager.
            bookListProp = new List<Book>();
            foreach (Book book in bookListRaw.Result)
            {
                bookListProp.Add(book);
            }
            List<Order> orderList = (List <Order>) statController.TestCheckOut().Result; // "Gå till kassan" --> En order ska skapas.

            // ### Assert ###
            if (orderList == null || orderList.Count != 1)
            {
                // ERROR: Ingen order har noterats!
                Assert.Fail("40: Orderlistan är tom.");
            }
            else
            {
                Boolean testSvar = false;
                foreach (Order order in orderList)
                {
                    if (order.Author.Equals("Testförfattare 100") &&
                        order.Title.Equals("Testbok 100") &&
                        order.Verified == 5 &&
                        order.VerCost == (decimal)10.95 * 5)
                        testSvar = true;
                }
                if (testSvar)
                {
                    // SUCCESS! Det finns exakt en färdigbehandlad order-post och den visar 5 böcker till totalpriset 5 * 10.95 kr.
                }
                else
                {
                    // ERROR: En betällning finns, men något har gått fel!
                    Assert.Fail("50: Inkorrekta data i order-posten.");
                }
            }
        }
    }
}