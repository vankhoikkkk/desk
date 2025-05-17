using CafeManager.DAO;
using CafeManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManager
{
    public partial class f_Admin : Form
    {
        public f_Admin()
        {
            InitializeComponent();
            loadListFoot();
            addListFootDataBinding();
            loadListDropDowCategory();
        }

        #region Methon
        void loadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dataGridViewBillDate.DataSource = BillAndFootFromatDAO.Instance.getListBillbyDate(checkIn, checkOut);
            float total = BillAndFootFromatDAO.Instance.getTotalPriceByListBillDate(BillAndFootFromatDAO.Instance.getListBillbyDate(checkIn, checkOut));
            CultureInfo culture = new CultureInfo("vi-VN");
            //Thread.CurrentThread.CurrentCulture = culture;
            textBoxTotalPriceDate.Text = total.ToString("c", culture);
        }

        void loadListFoot()
        {
            dataGridViewFoot.DataSource = FoodDAO.Instance.getListAllFoot();
        }

        void loadListDropDowCategory()
        {
            comboBoxFootCategory.DataSource = FootCategoryDAO.Instance.getListFootCategory();
            comboBoxFootCategory.DisplayMember = "Name";
        }

        void addListFootDataBinding()
        {
            textBoxFootID.DataBindings.Add(new Binding("Text", dataGridViewFoot.DataSource, "ID"));
            textBoxShowFootName.DataBindings.Add(new Binding("Text", dataGridViewFoot.DataSource, "Name"));
            numericUpDownPricePage.DataBindings.Add(new Binding("Value", dataGridViewFoot.DataSource, "Price"));
        }


        #endregion

        #region Event

        private void buttonViewBillDate_Click(object sender, EventArgs e)
        {
            loadListBillByDate(dateTimePickerCheckIn.Value, dateTimePickerCheckOut.Value);
        }

        private void buttonViewFoot_Click(object sender, EventArgs e)
        {
            loadListFoot();
        }

        private void textBoxFootID_TextChanged(object sender, EventArgs e)
        {
            // số lượng ô đang được chọn trong lúc này
            if (dataGridViewFoot.SelectedCells.Count > 0)
            {
                int id_FoodCategero = (int)dataGridViewFoot.SelectedCells[0].OwningRow.Cells["IdCategory"].Value;

                FoodCategory category = FootCategoryDAO.Instance.getFoodCategory(id_FoodCategero);

                int i = 0;
                int indexCbFC = -1;
                foreach(FoodCategory item in comboBoxFootCategory.Items)
                {
                    if(item.Id == id_FoodCategero)
                    {
                        indexCbFC = i;
                        break;
                    }
                    i++;
                }
                // làm như này vì chỉ có thể so sanh bằng index với combox không thể so sanh trực tiệp category của combox với category lấy ra được
                comboBoxFootCategory.SelectedIndex = indexCbFC;  
            }
        }



        #endregion
    }
}
