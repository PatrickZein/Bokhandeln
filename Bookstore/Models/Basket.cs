using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    class Basket
    {
        Book orderedBook { get; set; }
        int orderedCopies { get; set; }
        decimal orderedAtPrice { get; set; }

        public Basket(Book book, int copies, decimal atPrice)
        {
            this.orderedBook = book;
            this.orderedCopies = copies;
            this.orderedAtPrice = atPrice;
        }
    }
}
