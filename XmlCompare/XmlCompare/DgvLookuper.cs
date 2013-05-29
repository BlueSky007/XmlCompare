using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XmlCompare
{
      public class DgvLookuper
    {
        private DataGridView dgv = null;        // 要在其中进行查找的 DataGridView。     
        private string lookupStr = string.Empty;// 要查找的字符串。

        private int maxColIndex = 0; // DataGridView 总列数。
        private int maxRowIndex = 0; // DataGridView 总行数。

        private int startColIndex = 0;  // 查找的起始列序号。
        private int startRowIndex = 0;  // 查找的其实行序号。

        private int currentColIndex = 0;// 当前列序号。
        private int currentRowIndex = 0;// 当前行序号。

        private bool isFound = false;   // 是否找到一个匹配。

        public DgvLookuper(DataGridView dgv)
        {
            this.dgv = dgv;

            this.maxColIndex = dgv.Columns.Count - 1;
            this.maxRowIndex = dgv.Rows.Count - 1;

            this.startColIndex = dgv.CurrentCell.ColumnIndex; //从当前列查询
            this.startRowIndex = dgv.CurrentCell.RowIndex;

            this.currentColIndex = dgv.CurrentCell.ColumnIndex;
            this.currentRowIndex = dgv.CurrentCell.RowIndex;

            this.dgv.CellClick += new DataGridViewCellEventHandler(dgv_CellClick);
        }

        // 如果用户点击了某个单元格，搜索起始点也随之改变。
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.startColIndex = dgv.CurrentCell.ColumnIndex;
            this.startRowIndex = dgv.CurrentCell.RowIndex;

            this.currentColIndex = dgv.CurrentCell.ColumnIndex;
            this.currentRowIndex = dgv.CurrentCell.RowIndex;
        }

        /// <summary>
        /// 设置要查找的字符串（只写）。
        /// </summary>
        public string LookupStr
        {
            set
            {
                if (this.lookupStr != value)
                {
                    this.lookupStr = value;
                    this.isFound = false;
                }
            }
        }

        /// <summary>
        /// 在 DataGridView 中查找，将匹配的单元格设置为当前单元格。
        /// </summary>
        public void Lookup()
        {
            int colIndex = this.currentColIndex;
            int rowIndex = this.currentRowIndex;

            Next(ref colIndex, ref rowIndex);

            Lookup(colIndex, rowIndex);

            this.dgv.CurrentCell = this.dgv[currentColIndex, currentRowIndex];
        }

        /// <summary>
        /// 在 DataGridView 中进行查找，直到找到匹配的单元格，或者达到搜索的起始点。
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="rowIndex"></param>
        private void Lookup(int colIndex, int rowIndex)
        {
            if (colIndex == this.startColIndex && rowIndex == this.startRowIndex)
            {
                if (this.isFound == true ||
                    Match(this.dgv[colIndex, rowIndex].Value.ToString(), this.lookupStr))
                {
                    MessageBox.Show("查找达到了搜索的起始点。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("未找到以下指定文本：\n\n" + this.lookupStr, "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                this.currentColIndex = colIndex;
                this.currentRowIndex = rowIndex;
                return;
            }

            string str = Convert.ToString(this.dgv[colIndex, rowIndex].Value);

            if (str == null || str.Length == 0)
            { }
            else
            {
                if (this.dgv.Columns[colIndex].Visible && // 仅查找可见列。
                    //this.Match(this.dgv[colIndex, rowIndex].Value.ToString(), this.lookupStr))
                    this.Match(str, this.lookupStr))
                {

                    this.isFound = true;

                    this.currentColIndex = colIndex;
                    this.currentRowIndex = rowIndex;
                    return;
                }
            }

            Next(ref colIndex, ref rowIndex);

            Lookup(colIndex, rowIndex);
        }

        // 下一个单元格序号。
        private void Next(ref int colIndex, ref int rowIndex)
        {
            colIndex = colIndex + 1;

            if (colIndex > this.maxColIndex)
            {
                colIndex = 0;
                rowIndex++;
                if (rowIndex > maxRowIndex)
                {
                    rowIndex = 0;
                }
            }
        }

        // matchStr 是否是 sourceStr 的子串。
        private bool Match(string sourceStr, string matchStr)
        {
            return sourceStr.Contains(matchStr);
        }
    }

}
