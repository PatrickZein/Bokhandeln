using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    interface IBook
    {
        string Title { get; }
        string Author { get; }
        decimal Price { get; }
        int InStock { get; set; }

        // Vi behöver kunna plocka ut ex ur lager:
        void PickFromShelf(int nr);

        // Vi behöver kunna returnera ex till lager:
        void ReturnToShelf(int nr);

        /* Personligen skulle jag även vilja lägga till ett antal fler fält med grundläggande info:
        string ISBN { get; } // unik identifiering är alltid bra
        string category { get; } // roman, deckare, kokbok, barnbok, skolbok...
        string media { get; } // inbunden bok, pocketbok, e-bok, CD, DVD...
        string description { get; } // vad handlar boken om? */

        /* Det känns angeläget att lägga till en unik identifierare för varje artikel i databasen,
        eftersom man då t.ex. inte behöver belasta servern med att spara en massa information om de böcker som en kund tittar på. */
    }
}
