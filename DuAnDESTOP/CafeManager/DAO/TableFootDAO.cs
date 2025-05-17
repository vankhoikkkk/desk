using CafeManager.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.DAO
{
    public  class TableFootDAO
    {
        private static TableFootDAO instance;

        public static int Width = 80;
        public static int Height = 80;

        public static TableFootDAO Instance 
        { get { if (instance == null) instance = new TableFootDAO(); return instance; }
           private set => instance = value; 
        }


        public List<TableFoot> loadTableFootList()
        {
            List<TableFoot> list = new List<TableFoot>();
            string query = "EXEC dbo.USP_GetAllTableFoot";
            DataTable dataTable = Database.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                TableFoot tableFoot = new TableFoot(row);
                list.Add(tableFoot);
            }

            return list;
        }
       
    }
}
