using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManager.Model
{
    public class Food
    {
        private int id;
        private string name;
        private int idCategory;
        private double price;

        public Food(DataRow row)
        {
            this.id = (int)row["id"];
            this.name = (string)row["name"];
            this.idCategory = (int)row["idCategory"];
            this.price = (double)row["price"];
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int IdCategory { get => idCategory; set => idCategory = value; }
        public double Price { get => price; set => price = value; }
    }
}
