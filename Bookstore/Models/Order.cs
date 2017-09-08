using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class Order
    {
        private string _title { get; set; }
        private string _author { get; set; }
        private decimal _price { get; set; }
        private int _copies { get; set; }
        private int _verified { get; set; }
        private decimal _totalCost { get; set; }
        private decimal _verCost { get; set; }

        public string Title { get { return _title; } }
        public string Author { get { return _author; } }
        public decimal Price { get { return _price; } }
        public int Copies { get { return _copies; } }
        public int Verified { get { return _verified; } set { _verified = value;  } }
        public decimal TotalCost { get { return _totalCost; } set { _totalCost = value; } }
        public decimal VerCost { get { return _verCost; } set { _verCost = value; } }

        public Order(Book book, int copies)
        {
            if (book == null)
                throw new ArgumentException("Parametern kan inte vara null", "book");
            if (copies < 1)
                throw new ArgumentOutOfRangeException("Parametern kan inte vara mindre än 1", "copies");

            this._title = book.Title;
            this._author = book.Author;
            this._price = book.Price;
            this._copies = copies;
            this._verified = 0;
            this._totalCost = book.Price * copies;
            this._verCost = 0;
        }
    }
}