using CafeManager.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManager.DAO
{
    public class BillAndFootFromatDAO
    {
        private static BillAndFootFromatDAO instance;

        public static BillAndFootFromatDAO Instance 
        { get {
                if(instance == null)
                {
                    instance = new BillAndFootFromatDAO();
                }
                return instance;
            }
        private set => instance = value; }


        public List<BillAndFootFromat> getListMenuOfTableId(int id_table)
        {
            List<BillAndFootFromat>  list = new List<BillAndFootFromat> ();
            string query = "EXEC dbo.USP_GetBillAndFootFromat @idTable";
            DataTable dataTable = Database.Instance.ExecuteQuery (query, new object[] {id_table});

            foreach (DataRow row in dataTable.Rows)
            {
                BillAndFootFromat billAndFootFromat = new BillAndFootFromat(row);
                list.Add(billAndFootFromat);
            }

            return list;
        }

        public int checkBillExistByIdTable(int id_table)
        {
            int check = -1;
            string query = "EXEC dbo.USP_GetBillUnpaid @idTable";
            DataTable dataTable  = Database.Instance.ExecuteQuery(query, new object[] { id_table });
            if (dataTable.Rows.Count > 0)
            {
                
                return (int)dataTable.Rows[0]["id"];
            }
            return check;
        }

        // Thêm mới bill
        public void insertBill(int id_table)
        {
            string query = "INSERT INTO Bill (DateCheckIn, DateCheckOut, idTable, status) VALUES (GETDATE(), null, " + id_table + ", 0)";
            Database.Instance.ExecuteNonQuery(query);
        }

        // lấy bill mới nhất được thêm vào để insert vào billInfo
        public int getIdBillMax()
        {
            int idMax;
            try
            {
                string query = "SELECT MAX(id) FROM Bill";
                idMax = (int)Database.Instance.ExecuteScalar(query);
            }
            catch (Exception ex)
            {
                return 1;
            }
            return idMax;
        }

        public void insertBillInfo(int id_bill, int id_foot, int count)
        {
            string query = "INSERT INTO BillInfo(idBill, idFood, count) VALUES (" + id_bill + ", " + id_foot + ", " + count + ")";
            Database.Instance.ExecuteNonQuery(query);
        }

        //public int getBillByIdTable(int id_table)
        //{
        
        //    string query = "SELECT id FROM bill WHERE idTable = " +id_table+ " AND status = 0";
        //    DataTable dataTable = Database.Instance.ExecuteQuery(query);
        //    lấy dòng đầu tiên cột id;
        //    return (int)dataTable.Rows[0]["id"];
        //}

        public void CheckInsertBillInfo(int id_bill, int id_foot, int count)
        {
            string query = "EXEC USP_CheckInsertBillInfo @idBill , @idFood , @CountAdd ";
            Database.Instance.ExecuteNonQuery(query, new object[] { id_bill, id_foot, count });
        }

        public void checkOut(int idBill,int id_Table, float totalPrice)
        {
            string query = "UPDATE Bill SET DateCheckOut = GETDATE(), status = 1, totalPrice = "+ totalPrice +" WHERE idTable = " + id_Table + " AND id = "+idBill+";";
            Database.Instance.ExecuteNonQuery(query);
        }

        public int getBillbyIdTableUnCheck(int id_table)
        {
            string query = "select id from bill where idTable = " + id_table + " and status = 0 ";
            
           DataTable dataTable = Database.Instance.ExecuteQuery(query);

            int id_bill = (int)dataTable.Rows[0]["id"];
            return id_bill;

        }

        public void chuyenBanDAO(int id_Table1, int id_Table2)
        {
            
        }

        public DataTable getListBillbyDate(DateTime checkIn, DateTime checkOut)
        {
            string query = "EXEC USP_GETBillByDate @DateCheckIn , @DateCheckOut ";
            DataTable dataTable = Database.Instance.ExecuteQuery(query, new object[] { checkIn, checkOut });

            return dataTable;
        }

        public float getTotalPriceByListBillDate(DataTable dataTable)
        {
            float total = 0;

            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    try
                    {
                        total += Convert.ToSingle(row["totalPrice"]);
                    }
                    catch (InvalidCastException)
                    {
                        total = -10000;
                    }
                }
            }

            return total;
        }

    }
}
