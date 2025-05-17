using CafeManager.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        { get
            {
                if(instance == null)
                {
                    instance = new FoodDAO();
                }
                return instance;
            }
            private set => instance = value; 
        }

        public List<Food> getListFoodByIdCategory(int id)
        {
            List<Food> listFood = new List<Food>();

            string query = "SELECT * FROM Food WHERE idCategory = "+ id + "";
            DataTable dataTable = Database.Instance.ExecuteQuery(query);
            foreach (DataRow row in dataTable.Rows)
            {
                Food food = new Food(row);
                listFood.Add(food);
            }
            return listFood;
        }

        public List<Food> getListAllFoot()
        {
            List<Food> listFood = new List<Food>();

            string query = "select * from food";
            DataTable dataTable = Database.Instance.ExecuteQuery(query);
            foreach(DataRow row in dataTable.Rows)
            {
                Food food = new Food(row);
                listFood.Add(food);
            }

            return listFood;
        }


    }
}
