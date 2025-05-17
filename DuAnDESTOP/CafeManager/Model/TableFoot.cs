using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Model
{
    public class TableFoot
    {
        private int _id;
        private string _name;
        private string _status;
        
        // constucter rỗng
        public TableFoot() { }
        
        // constructer
        public TableFoot(int id, string name, string status)
        {
            this._id = id;
            this._name = name;
            this._status = status;
        }
        // constucter sử lý ép kiểu DataRow
        public TableFoot(DataRow row)
        {
            this._id = (int)row["id"];
            this._name = (string)row["name"];
            this.Status = (string)row["status"];
        }
        // get and set 
        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
