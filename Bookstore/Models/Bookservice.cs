using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Bookstore.Models
{
    class Bookservice : IBookstoreService
    {
        public async Task<IEnumerable<IBook>> GetBooksAsync(string searchString)
        {
            // Hämta en boklista enligt angiven söksträng.
            var bookList = Task<IEnumerable<IBook>>.Factory.StartNew(() =>
            {
                string bookFile = GetBooksFromUrl("https://raw.githubusercontent.com/contribe/contribe/dev/arbetsprov-net/books.json");
                return BookSearch(bookFile, searchString);

            });
            return await bookList;
        }

        private string GetBooksFromUrl(string url)
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    // Read booklist from URL.
                    wc.Encoding = Encoding.UTF8;
                    var bookFile = wc.DownloadString(url);
                    return bookFile;
                }
                catch
                {
                    return null;
                }
            }

        }

        private static IEnumerable<IBook> BookSearch(string bookFile, string searchString)
        {
            // Denna metod listar böcker som matchar den angivna söksträngen.
            
            // Är bokfilen tom?
            if (bookFile == null)
            {
                return null;
            }

            // Konvertera json-fil till text
            JavaScriptSerializer js = new JavaScriptSerializer();
            dynamic bookList = js.DeserializeObject(bookFile);

            // Läs textfilen rad för rad och spara alla matchande rader i boklistan.
            var bookSelection = new List<Book>();
            int counter = 1;
            if (bookList != null && searchString != null)
            foreach(var item in bookList["books"])
            {
                // Om bokens titel eller författare matchar söksträngen, så lägg till den i listan.
                if (item["author"].ToLower().Contains(searchString.ToLower()) ||
                   item["title"].ToLower().Contains(searchString.ToLower()))
                {
                    // Eftersom jag jobbar med testdata, anser jag att jag inte behöver göra någon try/catch här,
                    // utan litar på att alla priser är skrivna i korrekt decimalformat, och att alla antal är integers.
                    var book = new Book(counter++,
                                        item["title"],
                                        item["author"],
                                        System.Convert.ToDecimal(item["price"]),
                                        System.Convert.ToInt16(item["inStock"])
                                        );
                    bookSelection.Add(book);
                }
            }

            return bookSelection;
        }
    }
}
