using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    class Utilities
    {
        static void Main(string[] args)
        {
            // Denna metod använde jag initialt för att testa sökfunktionen på console.

            string searchString = "e";  // Tips: Strängarna "e" och "i" listar alla titlar.

            while (!searchString.Equals(""))
            {
                Console.Write("Ange en söksträng: ");
                searchString = Console.ReadLine();
                if (!searchString.Equals(""))
                {
                    /* var a = ConsoleSearchAsync(searchString, 1, 5); // Detta anrop kan används för att testa IBookstoreService på console. */
                    var boklista = BookSearchAsync(searchString, 1, 5);
                    Thread.Sleep(1000);

                    foreach (Book bokObjekt in boklista.Result)
                    {
                        if ((bokObjekt.Title.Equals("count")) && (bokObjekt.Price == 0))
                        {
                            Console.WriteLine("Det finns " + bokObjekt.Counter + " böcker som matchar din sökning.");
                        }
                        else
                        {
                            Console.WriteLine(bokObjekt.Counter + ": " + bokObjekt.Author + ", " + bokObjekt.Title + ", " + String.Format("{0:C}", bokObjekt.Price));
                        }
                    }
                }
            }
        }

        static async Task ConsoleSearchAsync(string searchString, int startCount, int maxCount)
        {
            // Denna metod använde jag initialt för att testa sökfunktionen på console.
            int count = 0;
            bool nextPage = false;

            // Hämta en lista över böcker som matchar en angiven söksträng.
            var webStore = new Bookservice();
            var webStoreList = await webStore.GetBooksAsync(searchString);

            // OBS: Om det finns fler än ett visst antal matchande böcker, så bör de delas upp på flera sidor! 
            // Angående test: Prova olika söksträngar som ger noll böcker, ett fåtal böcker respektive mer än en sida med böcker!

            foreach (Book book in webStoreList)
            {
                count++;
                if ((count >= startCount) && (count < startCount + maxCount))
                {
                    Console.WriteLine(book.Counter + ": " + book.Author + ", " + book.Title + ", " + String.Format("{0:C}", book.Price));
                }
                if (count == startCount + maxCount)
                {
                    nextPage = true;
                }
            }

            Console.WriteLine();
            if (count == 0)
            {
                Console.WriteLine("Urvalet gav inga träffar.");
            }

            if (nextPage == true)
            {
                Console.WriteLine("Det finns fler böcker som matchar urvalet. Vill du se dem?");
                Console.WriteLine("Eller vill du göra en ny sökning?");
            }
            else
            {
                Console.WriteLine("Vill du göra en ny sökning?");
            }
            Console.WriteLine("--------------------------------");
        }

        public static async Task<IEnumerable<Book>> BookSearchAsync(string searchString, int startCount, int maxCount)
        {
            // Denna metod returnerar en lista med ett urval av böcker.
            int count = 0;
            int lastBook = 0;

            // Hämta en lista över böcker som matchar en angiven söksträng.
            var webStore = new Bookservice();
            var webStoreList = await webStore.GetBooksAsync(searchString);
            var bookSelection = new List<Book>();

            // OBS: Om det finns fler än ett visst antal matchande böcker, så bör de delas upp på flera sidor! 
            // Angående test: Prova olika söksträngar som ger noll böcker, ett fåtal böcker respektive mer än en sida med böcker!

            // Spara det utvalda intervallet av böcker.
            foreach (Book book in webStoreList)
            {
                count++;
                if ((count >= startCount) && (count < startCount + maxCount))
                {
                    // Denna bok får stanna kvar i listan.
                    bookSelection.Add(book);
                    lastBook = count;
                }
            }
            // Lägg till en post på slutet för att ange hur många böcker som kunde hittas, oavsett hur många som listats.
            bookSelection.Add(new Book(count, "count", "---", 0, lastBook));

            return bookSelection;
        }
    }
}
