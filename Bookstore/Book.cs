using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class Book : IBook
    {
        private int _counter;
        private string _title;
        private string _author;
        private decimal _price;
        private int _inStock;

        public int Counter { get { return _counter; } set { _counter = value; } }
        public string Title { get { return _title; } }
        public string Author { get { return _author; } }
        public decimal Price { get { return _price; } }
        public int InStock { get { return _inStock; } set { _inStock = value; } }

        public Book(int counter, string title, string author, decimal price, int inStock)
        {
            _counter = counter;
            _title = title;
            _author = author;
            _price = price;
            _inStock = inStock;
        }

        public void PickFromShelf(int nr)
        {
            // Reservera ett antal exemplar till en kund.
            _inStock = _inStock - nr;
        }

        public void ReturnToShelf(int nr)
        {
            // Ta bort böcker från kundens beställning.
            _inStock = _inStock + nr;
        }
    }
}
