using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XmlCompare
{
    public enum QueryType
    {
        Blur,
        Complete
    }
    public class GridQuery
    {
        private DataGridView _XmlGrid = null;     //查找对象
        private string _QueryStr = string.Empty;  //模糊查询字符串
        private QueryType _QueryType = QueryType.Blur;

        private int maxColIndex = 0; // DataGridView 总列数。
        private int maxRowIndex = 0; // DataGridView 总行数。

        private int _CurrentCoIndex = 0;
        private int _CurrentRowIndex = 0;
        private int _StartColIndex = 0;  // 查找的起始列序号。
        private int _StartRowIndex = 0;  // 查找的其实行序号。

        private bool _IsFond = false;

        public GridQuery(DataGridView grid)
        {
            this._XmlGrid = grid;
            this.maxColIndex = grid.Columns.Count - 1;
            this.maxRowIndex = grid.Rows.Count - 1;

            this._StartRowIndex = grid.CurrentCell.RowIndex;
            this._StartColIndex = grid.CurrentCell.ColumnIndex;
            this._CurrentCoIndex = grid.CurrentCell.ColumnIndex;
            this._CurrentRowIndex = grid.CurrentCell.RowIndex;

            this._XmlGrid.CellClick +=new DataGridViewCellEventHandler(_XmlGrid_CellClick);
        }

        private void _XmlGrid_CellClick(object sender,DataGridViewCellEventArgs e)
        {
            this._StartColIndex = this._XmlGrid.CurrentCell.ColumnIndex;
            this._StartRowIndex = this._XmlGrid.CurrentCell.RowIndex;
            this._CurrentCoIndex = this._XmlGrid.CurrentCell.ColumnIndex;
            this._CurrentRowIndex = this._XmlGrid.CurrentCell.RowIndex;
        }
        public QueryType QueryType
        {
            set
            {
                if (this._QueryType != value)
                {
                    this._QueryType = value;
                }
            }
        }
        /// <summary>
        /// 设置要查找的字符串（只写）。
        /// </summary>
        public string QueryStr
        {
            set
            {
                if (this._QueryStr != value)
                {
                    this._QueryStr = value;
                    this._IsFond = false;
                }
            }
        }
        //查询当前列
        public void Query2()
        {
            int colIndex = this._CurrentCoIndex;
            int rowIndex = this._CurrentRowIndex;

            this.Next(ref rowIndex);
            this.Query2(this._CurrentCoIndex, rowIndex);
            this._XmlGrid.CurrentCell = this._XmlGrid[this._CurrentCoIndex, this._CurrentRowIndex];
        }

        public void Query()
        {
            int colIndex = this._CurrentCoIndex;
            int rowIndex = this._CurrentRowIndex;

            this.Next(ref colIndex, ref rowIndex);
            this.Query(colIndex, rowIndex);
            this._XmlGrid.CurrentCell = this._XmlGrid[this._CurrentCoIndex,this._CurrentRowIndex];
        }

        private void Query(int colIndex, int rowIndex)
        {
            if (colIndex == this._StartColIndex && rowIndex == this._StartRowIndex)
            {
                if (this._IsFond == true ||
                    this.Match(this._XmlGrid[colIndex, rowIndex].Value.ToString(), this._QueryStr))
                {
                    MessageBox.Show("查找达到了搜索的起始点。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("未找到以下指定文本：\n\n" + this._QueryStr, "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                this._CurrentCoIndex = colIndex;
                this._CurrentRowIndex = rowIndex;
                return;
            }

            string str = Convert.ToString(this._XmlGrid[colIndex, rowIndex].Value);

            if (str == null || str.Length == 0)
            { }
            else
            {
                if (this._XmlGrid.Columns[colIndex].Visible && 
                    this._XmlGrid.Rows[rowIndex].Visible &&// 仅查找可见列。
                    //this.Match(this.dgv[colIndex, rowIndex].Value.ToString(), this.lookupStr))
                    this.Match(str, this._QueryStr))
                {

                    this._IsFond = true;

                    this._CurrentCoIndex = colIndex;
                    this._CurrentRowIndex = rowIndex;
                    return;
                }
            }

            Next(ref colIndex, ref rowIndex);

            this.Query(colIndex, rowIndex);
        }

        private void Query2(int colIndex, int rowIndex)
        {
            if (colIndex == this._StartColIndex && rowIndex == this._StartRowIndex)
            {
                if (this._IsFond == true ||
                    this.Match(this._XmlGrid[colIndex, rowIndex].Value.ToString(), this._QueryStr))
                {
                    MessageBox.Show("查找达到了搜索的起始点。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("未找到以下指定文本：\n\n" + this._QueryStr, "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                this._CurrentCoIndex = colIndex;
                this._CurrentRowIndex = rowIndex;
                return;
            }

            string str = Convert.ToString(this._XmlGrid[colIndex, rowIndex].Value).Trim();

            if (str == null || str.Length == 0)
            { }
            else
            {
                if (this._XmlGrid.Columns[colIndex].Visible &&
                    this._XmlGrid.Rows[rowIndex].Visible && // 仅查找可见列。
                    //this.Match(this.dgv[colIndex, rowIndex].Value.ToString(), this.lookupStr))
                    this.Match(str, this._QueryStr))
                {

                    this._IsFond = true;

                    this._CurrentCoIndex = colIndex;
                    this._CurrentRowIndex = rowIndex;
                    return;
                }
            }

            Next(ref rowIndex);
            this.Query2(colIndex, rowIndex);
        }

        private void Next(ref int rowIndex)
        {
            rowIndex++;
            if (rowIndex > maxRowIndex)
            {
                rowIndex = 0;
            }
        }

        private void Next(ref int colIndex, ref int rowIndex)
        {
            colIndex = colIndex + 1;

            if (colIndex > this.maxColIndex)
            {
                //从第一列开始查询
                colIndex = 6;
                rowIndex++;
                if (rowIndex > maxRowIndex)
                {
                    rowIndex = 0;
                }
            }
        }
        // sourceStr是否包含matchStr子串。
        private bool Match(string sourceStr, string matchStr)
        {
            if (matchStr.Trim().Length > sourceStr.Trim().Length) return false;
            if (this._QueryType == QueryType.Blur)
            {
                return sourceStr.Contains(matchStr);
            }
            else
            {
                return sourceStr == matchStr;
            }
            
        }

    }
}
