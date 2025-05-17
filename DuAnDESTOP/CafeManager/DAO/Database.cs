using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.DAO
{
    public class Database
    {
        private static Database instance;

        // get and set khác java
        public static Database Instance 
        { get { 
                if (Database.instance == null) 
                    Database.instance = new Database(); 
                return Database.instance; 
              }
          
            private set { Database.instance = value; }
        }

        private Database() { }

        private string con = "Data Source=DESKTOP-IIAQ9MJ\\SQLEXPRESS;Initial Catalog=CafeManager;Integrated Security=True;";


        // cơ bản 
        //public DataTable getDataTable(string query)
        //{
        //    DataTable dataTable = new DataTable();

        //    using (SqlConnection connection = new SqlConnection(con))
        //    {

        //        connection.Open();

        //        SqlCommand cmd = new SqlCommand(query, connection);

        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

        //        adapter.Fill(dataTable);

        //        connection.Close();
        //    }
        //    return dataTable;
        //}

        // truyên Parameters vào
        //public DataTable ExecuteQuery1(string query, string id)
        //{
        //    DataTable dataTable = new DataTable();

        //    using (SqlConnection connection = new SqlConnection(con))
        //    {

        //        connection.Open();

        //        SqlCommand cmd = new SqlCommand(query, connection);

        //        cmd.Parameters.AddWithValue("@userName", id);

        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

        //        adapter.Fill(dataTable);

        //        connection.Close();
        //    }
        //    return dataTable;
        //}

        // truyền nhiều parameters nâng cao

        public DataTable ExecuteQuery(string query, object[] parameters = null)
        {
            // lưu ý muốn sử dụng hàm này đúng thì chỗ @cot1 , @cot2 phải có định dạng như này 
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(con))
            {

                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);

                if(parameters != null)
                {
                    string[] listStr = query.Split(' ');
                    int i = 0;
                    foreach (string item in listStr)
                    {
                        if (item.Contains('@'))
                        {
                           cmd.Parameters.AddWithValue(item, parameters[i]);
                           i++;
                        }
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dataTable);

                connection.Close();
            }
            return dataTable;
        }

        public int ExecuteNonQuery(string query, object[] parameters = null)
        {
            // lưu ý muốn sử dụng hàm này đúng thì chỗ @cot1 , @cot2 phải có định dạng như này 
            int dataRowCount;

            using (SqlConnection connection = new SqlConnection(con))
            {

                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);

                if (parameters != null)
                {
                    string[] listStr = query.Split(' ');
                    int i = 0;
                    foreach (string item in listStr)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }
                // cmd.ExecuteNonQuery(); trả về số dòng bị ảnh hưởng (Insert, delete, update, creat table, dropTbale..)
                dataRowCount = cmd.ExecuteNonQuery();

                connection.Close();
            }
            return dataRowCount;
        }

        public object ExecuteScalar(string query, object[] parameters = null)
        {
            // lưu ý muốn sử dụng hàm này đúng thì chỗ @cot1 , @cot2 phải có định dạng như này 
            object dataRowCount = 0;

            using (SqlConnection connection = new SqlConnection(con))
            {

                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);

                if (parameters != null)
                {
                    string[] listStr = query.Split(' ');
                    int i = 0;
                    foreach (string item in listStr)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }
                //  cmd.ExecuteScalar(); trả về 1 cột và 1 dòng duy nhất ví dụ (COUNT, SUM,... trong sql)
                dataRowCount = cmd.ExecuteScalar();

                connection.Close();
            }
            return dataRowCount;
        }
    }
}
