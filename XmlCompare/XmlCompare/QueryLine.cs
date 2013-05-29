using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XmlCompare
{
    public partial class QueryLine : Form
    {
        public delegate void QueryLineHandler(int rowIndex, int ColIndex);
        public QueryLineHandler QueryLineHand;
        public QueryLine()
        {
            InitializeComponent();
        }

        private void CancleBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.RowLine.Text) || string.IsNullOrEmpty(this.ColumnLine.Text))
            {
                MessageBox.Show("请输入行列值", "XmlCompare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.QueryLineHand(Convert.ToInt32(this.RowLine.Text), Convert.ToInt32(this.ColumnLine.Text));
            this.Close();
        }
    }
}
