using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Model
{
    public class BillAndFootFromat
    {
        private string name;
        private int count;
        private double price;
        private double totalPrice;

        public BillAndFootFromat (DataRow row) 
        {
            this.name = (string)row["name"];
            this.count = (int)row["count"];
            this.price = (double)row["price"];
            this.totalPrice = (double)row["TotalPrice"];
        }


        public string Name { get => name; set => name = value; }
        public int Count { get => count; set => count = value; }
        public double Price { get => price; set => price = value; }
        public double TotalPrice { get => totalPrice; set => totalPrice = value; }
    }
}
