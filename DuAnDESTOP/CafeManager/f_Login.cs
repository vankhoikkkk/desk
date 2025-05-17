using CafeManager.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManager
{
    public partial class f_Login : Form
    {
        public f_Login()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string userName = textBoxUserName.Text;
            string passWord = textBoxPass.Text;
            bool check = AccountDAO.Instance.CheckLogin(userName, passWord);

            if (check)
            {
                f_TableManager ftm = new f_TableManager();
                this.Hide();
                ftm.ShowDialog(); // model top: có nghĩa là phải sử lí tk này rồi mới sứ lí được tk bên dưới (ví dụ : thông báo thoát trong chương trình hỏi bạn có chắc chắc thoát không thì phải sử lí cái thông báo này mới được mới tới tk khác được)
                this.Show();
            }else
            {
                MessageBox.Show("Thông tin tài khoản không chính xác");
            }

        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void f_Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            // giá trị thứ 3 là hiện thị nút ok và cancel và messageBox trả về giá trị DialogResult nên ta so sánh với System.Windows.Forms.DialogResult.OK nếu mà form hiện ok thì thoát
            if (MessageBox.Show("Bạn có chắc chắn thoát ?", "Thông Báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                // huỷ hành động đóng form
                e.Cancel = true;
            }
        }
    }
}
