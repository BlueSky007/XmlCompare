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
    public partial class EditorGrid : Form
    {
        public delegate void ChangeXmlValueHandler(string value,int colIndex,int rowIndex);
        public ChangeXmlValueHandler ChangedXml;
        private int ColIndex;
        private int RowIndex;
        public EditorGrid(int colIndex,int rowIndex,string oldValue)
        {
            InitializeComponent();
            this.ColIndex = colIndex;
            this.RowIndex = rowIndex;
            this.EditorRichTextBox.Text = oldValue;
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            if(ChangedXml != null)
            this.ChangedXml(this.EditorRichTextBox.Text,this.ColIndex,this.RowIndex);
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
