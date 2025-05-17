using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance 
        { get { if (instance == null) instance = new AccountDAO(); return instance; }
          private set { instance = value; }
        }

        public bool CheckLogin(string username, string password)
        {   // không có tính bảo mật
            //string query = "SELECT COUNT(*) FROM Account WHERE UserName = N'" + username + "' and PassWord = N'" + password + "'";
            // cách 1 dùng COUNT(*) đưa vào ExecuteScalar lấy cột đếm được 
            string query = "EXEC dbo.USP_CHECK_AccountByUserNameAndPassword @userName , @passWord ";
            object check = Database.Instance.ExecuteScalar(query, new object[] {username, password});
            // cách 2 dùng select bình thường
            //string query2 = "SELECT * FROM Account WHERE UserName = N'" + username + "' and PassWord = N'" + password + "'"; Nên viết bằng PROC để bảo mật dữ liệu
            //DataTable data = Database.Instance.ExecuteQuery(query2);
            //int count = data.Rows.Count; dùng hàm đếm dòng trong dataTable



            return Convert.ToInt32(check) > 0 ? true : false;
        }
    }
}
