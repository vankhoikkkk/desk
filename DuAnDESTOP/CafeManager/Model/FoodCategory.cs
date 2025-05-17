using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Model
{
    public class FoodCategory
    {
        private int id;
        private string name;


        public FoodCategory(DataRow row)
        {
            this.id = (int)row["id"];
            this.name = (string)row["name"];
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
    }
}
