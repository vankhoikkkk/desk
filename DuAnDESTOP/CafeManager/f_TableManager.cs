using CafeManager.DAO;
using CafeManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManager
{
    public partial class f_TableManager : Form
    {
        public f_TableManager()
        {
            InitializeComponent();
            loadTable();
            loadFoodCategory();
        }

        #region Methon
        void loadFoodCategory()
        {
            List<FoodCategory> listFootCategory = FootCategoryDAO.Instance.getListFootCategory();
            comboBoxFoodCategory.DataSource = listFootCategory;
            comboBoxFoodCategory.DisplayMember = "Name";
        }

        void loadFoodByIdCategory(int id)
        {
            List<Food> listFood = FoodDAO.Instance.getListFoodByIdCategory(id);
            comboBoxFood.DataSource = listFood;
            comboBoxFood.DisplayMember = "Name";
        }

    

        void loadTable()
        {
            flpTable.Controls.Clear();
            List<TableFoot> footList = TableFootDAO.Instance.loadTableFootList();

            foreach (TableFoot foot in footList)
            {
                Button btn = new Button() { Width = TableFootDAO.Width, Height = TableFootDAO.Height };
                btn.Text = foot.Name + "\n" + foot.Status;
                // gán foot vào tag để lấy id
                btn.Tag = foot;
                // gán sự kiện click lên nút
                btn.Click += btn_Click;
                switch (foot.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.DarkOliveGreen;
                        break;
                    default:
                        btn.BackColor = Color.Aqua;
                        break;
                }

                flpTable.Controls.Add(btn);
            }
                
        }
        private void showBill(int id)
        {
            listViewBill.Items.Clear();
            List<BillAndFootFromat> list = BillAndFootFromatDAO.Instance.getListMenuOfTableId(id);
            double tatolPrice = 0;
            foreach(BillAndFootFromat item in list)
            {
                ListViewItem lvBill = new ListViewItem(item.Name.ToString());
                lvBill.SubItems.Add(item.Count.ToString());
                lvBill.SubItems.Add(item.Price.ToString());
                lvBill.SubItems.Add(item.TotalPrice.ToString());
                listViewBill.Items.Add(lvBill);

                tatolPrice += item.TotalPrice;

            }
            CultureInfo culture = new CultureInfo("vi-VN");
            //Thread.CurrentThread.CurrentCulture = culture;
            textBoxTotalPrice.Text = tatolPrice.ToString("c", culture);

        }

     
        #endregion


        #region Event
        private void btn_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if(button != null)
            {
                TableFoot tableFoot = button.Tag as TableFoot;
                if(tableFoot != null)
                {
                    int table_id = tableFoot.Id;
                    listViewBill.Tag = tableFoot;
                    buttonCheckOut.Tag = tableFoot;
                    showBill(table_id);
                }
            }
        }

        private void buttonAddFoot_Click(object sender, EventArgs e)
        {
            
            TableFoot tableFoot = listViewBill.Tag as TableFoot;
            int idBill = BillAndFootFromatDAO.Instance.checkBillExistByIdTable(tableFoot.Id);
            int id_foot = (int)(comboBoxFood.SelectedItem as Food).Id;
            int count = (int)(nmFootCount.Value);
            if (count != 0)
            {
                if (idBill < 0)
                {
                    BillAndFootFromatDAO.Instance.insertBill(tableFoot.Id);
                    BillAndFootFromatDAO.Instance.insertBillInfo(BillAndFootFromatDAO.Instance.getIdBillMax(), id_foot, count);
                }
                else
                {
                    BillAndFootFromatDAO.Instance.CheckInsertBillInfo(idBill, id_foot, count);
                }
            }
            showBill(tableFoot.Id);
            loadTable();
        }

   

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            f_AccountProfile f_AccountProfile = new f_AccountProfile();
            f_AccountProfile.ShowDialog();
            this.Show();

        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f_Admin admin = new f_Admin();
            this.Hide();
            admin.ShowDialog();
            this.Show();

        }
        #endregion


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
             ComboBox comboBox = (ComboBox)sender;
             FoodCategory foodCategory = (FoodCategory)comboBox.SelectedItem;
             int id = foodCategory.Id;
             loadFoodByIdCategory(id);
        }

        private void buttonCheckOut_Click(object sender, EventArgs e)
        {
            Button btnCheckOut = (Button)sender;
            TableFoot tableFoot = (TableFoot)btnCheckOut.Tag;
            int id_bill = BillAndFootFromatDAO.Instance.getBillbyIdTableUnCheck(tableFoot.Id);
            string input = textBoxTotalPrice.Text;
            var match = Regex.Match(input, "[-+]?[0-9]*\\.?[0-9]*");
            float totalPrice = 0;
            if (match.Success)
            {
                totalPrice = float.Parse(match.Value);
            }
            string price = textBoxTotalPrice.Text;
            if (MessageBox.Show("Bạn có chắc là thanh toán bàn " + tableFoot.Name + " với giá tiến thanh toán là" +
                " " + price + "", "Thông báo", MessageBoxButtons.OKCancel)
                == System.Windows.Forms.DialogResult.OK)
            {
                BillAndFootFromatDAO.Instance.checkOut(id_bill ,tableFoot.Id, totalPrice);
                loadTable();
                showBill(tableFoot.Id);
            }
        }
    }
}
