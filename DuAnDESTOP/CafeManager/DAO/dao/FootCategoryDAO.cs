using CafeManager.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.DAO
{
    public class FootCategoryDAO
    {
        private static FootCategoryDAO instance;

        public static FootCategoryDAO Instance {
            get {
                if (instance == null)
                {
                    instance = new FootCategoryDAO();
                }
                return instance;
            }
            private set => instance = value; 
        }

    public List<FoodCategory> getListFootCategory()
        {
            List<FoodCategory> listCategory = new List<FoodCategory>();
            string query = "SELECT * FROM FoodCategory";
            DataTable dataTable = Database.Instance.ExecuteQuery(query);
            foreach (DataRow row in dataTable.Rows)
            {
                FoodCategory foodCategory = new FoodCategory(row);
                listCategory.Add(foodCategory);

            }
            return listCategory;
        }



    }
}
