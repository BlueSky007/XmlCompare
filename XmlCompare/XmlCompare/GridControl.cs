using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;

namespace XmlCompare
{
    public class GridControl
    {
        public static void AddButtonColumn(DataGridView grid,bool IsAddColumn)
        {
            if (!IsAddColumn)
            {
                System.Windows.Forms.DataGridViewCellStyle buttonCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
                buttonCellStyle.BackColor = System.Drawing.Color.Silver;

                DataGridViewButtonColumn getNewColumn = new DataGridViewButtonColumn();
                getNewColumn.DefaultCellStyle = buttonCellStyle;
                getNewColumn.HeaderText = "取新";
                getNewColumn.Name = "NewButton";
                getNewColumn.UseColumnTextForButtonValue = true;
                getNewColumn.Text = "取新";
                getNewColumn.Width = 60;
                grid.Columns.Add(getNewColumn);

                DataGridViewButtonColumn getOldColumn = new DataGridViewButtonColumn();
                getOldColumn.DefaultCellStyle = buttonCellStyle;
                getOldColumn.Name = "OldButton";
                getOldColumn.UseColumnTextForButtonValue = true;
                getOldColumn.HeaderText = "取旧";
                getOldColumn.Text = "取旧";
                getOldColumn.Width = 60;
                grid.Columns.Add(getOldColumn);
            }
        }

        public static void SetGridColumnWidth(DataGridView grid)
        {
            grid.Columns[0].Width = 70;
            grid.Columns[1].Width = 70;
            grid.Columns[2].Width = 120;
            grid.Columns[3].Width = 120;
            grid.Columns[4].Width = 0;
            grid.Columns[4].Visible = false;
            grid.Columns[5].Width = 120;
            grid.Columns[6].Width = 120;
            grid.Columns[7].Width = 240;
            grid.Columns[8].Width = 80;

            grid.Columns[0].HeaderText = "取新版本";
            grid.Columns[1].HeaderText = "取旧版本";
            grid.Columns[2].HeaderText = "旧版本Key";
            grid.Columns[3].HeaderText = "旧版本Value";
            grid.Columns[4].HeaderText = "新版本Key";
            grid.Columns[5].HeaderText = "新版本Value";
            grid.Columns[6].HeaderText = "合并版本Key";
            grid.Columns[6].ReadOnly = true;
            grid.Columns[7].HeaderText = "合并版本Value";
            grid.Columns[8].HeaderText = "状态";

        }

        public static void SetWsdlGridColumnWidth(DataGridView grid)
        {
            grid.Columns[0].Width = 50;
            grid.Columns[1].Width = 50;
            grid.Columns[2].Width = 50;
            grid.Columns[3].Width = 220;
            grid.Columns[4].Width = 0;
            grid.Columns[4].Visible = false;
            grid.Columns[5].Width = 250;
            grid.Columns[6].Width = 250;
            grid.Columns[6].Visible = false;
            grid.Columns[7].Width = 280;
            grid.Columns[8].Width = 60;

            grid.Columns[0].HeaderText = "取新";
            grid.Columns[1].HeaderText = "取旧";
            grid.Columns[2].HeaderText = "第几行";
            grid.Columns[3].HeaderText = "旧版本";
            grid.Columns[4].HeaderText = "新版本";
            grid.Columns[5].HeaderText = "新版本";
            grid.Columns[6].HeaderText = "合并版本";
            grid.Columns[6].ReadOnly = true;
            grid.Columns[7].HeaderText = "合并版本";
            grid.Columns[8].HeaderText = "状态";

            for (int i = 0; i < grid.Columns.Count; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public static void SetGridDifferBackColor(DataGridView grid)
        {
            if (grid.CurrentCell == null) return;
            grid.CurrentCell.Style.BackColor = System.Drawing.Color.Cyan;
            int i = 0;
            foreach (DataGridViewRow row in grid.Rows)
            {
                row.Cells[7].Style.BackColor = Color.YellowGreen;
                if (i == grid.Rows.Count - 1) return;
                row.Cells[0].Style.BackColor = Color.LightBlue;
                row.Cells[1].Style.BackColor = Color.LightBlue;

                row.Cells[8].Style.ForeColor = System.Drawing.Color.DimGray;
                row.Cells[8].Style.Font = new Font("宋体", 8, FontStyle.Bold);

                if (row.Cells[3].Value == row.Cells[5].Value)
                {
                    row.Cells[0].ReadOnly = true;
                    row.Cells[0].Style.BackColor = Color.SlateGray;
                    row.Cells[0].Style.ForeColor = System.Drawing.Color.White;
                    row.Cells[1].ReadOnly = true;
                    row.Cells[1].Style.BackColor = Color.SlateGray;
                    row.Cells[1].Style.ForeColor = System.Drawing.Color.White;
                }
                if (row.Cells[3].Value != row.Cells[5].Value)
                {
                    row.Cells[2].Style.BackColor = Color.Pink;
                    row.Cells[3].Style.BackColor = Color.Pink;
                    row.Cells[4].Style.BackColor = Color.Pink;
                    row.Cells[5].Style.BackColor = Color.Pink;
                    row.Cells[2].Style.ForeColor = Color.Red;
                    row.Cells[3].Style.ForeColor = Color.Red;
                    row.Cells[4].Style.ForeColor = Color.Red;
                    row.Cells[5].Style.ForeColor = Color.Red;
                }
                i++;
            }
        }

        public static void ShowAllRecord(DataGridView grid)
        {
            grid.CurrentCell = null;

            DataTable dt = (DataTable)grid.DataSource;
            if (dt == null) return;
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Cells[8].Value == null) return;
                row.Visible = true;
                grid.CurrentCell = row.Cells[7];
            }
        }
        public static void ShowDifferItem(DataGridView grid)
        {
            grid.CurrentCell = null;
            DataTable dt = (DataTable)grid.DataSource;
            if (dt == null) return;
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Cells[8].Value == null) return;
                if (row.Cells[8].Value.ToString() != "Unchanged")
                {
                    row.Visible = true;
                    grid.CurrentCell = row.Cells[7];
                }
                else
                {
                    row.Visible = false;
                }
            }
        }
        //显示所有相同项
        public static void ShowSameItem(DataGridView grid)
        {
            grid.CurrentCell = null;

            DataTable dt = (DataTable)grid.DataSource;
            if (dt == null) return;
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Cells[8].Value == null) return;
                if (row.Cells[8].Value.ToString() == "Unchanged")
                {
                    row.Visible = true;
                    grid.CurrentCell = row.Cells[7];
                }
                else
                {
                    row.Visible = false ;
                }
            }
        }

        public static void HideRow(DataGridView grid,CheckBox chk1,CheckBox chk2)
        {
            bool isSetCurrentCell = false;
            grid.CurrentCell = null;
            for (int i = 0; i < grid.Rows.Count - 1; i++)
            {
                if ((chk1.Checked
                    && grid.Rows[i].Cells[8].Value.ToString() == "Unchanged")
                    || (chk2.Checked
                    && grid.Rows[i].Cells[8].Value.ToString() == "Modify"))
                {
                    grid.Rows[i].Visible = false;
                }
                else
                {
                    if (!isSetCurrentCell)
                    {
                        grid.CurrentCell = grid[7, i];
                        isSetCurrentCell = true;
                    }
                }
            }
        }

        public static void SetForeColor(TextBox textBox,bool IsDIffer)
        {
            if (IsDIffer)
            {
                textBox.ForeColor = Color.Red;
                textBox.BackColor = Color.Pink;
            }
            else
            {
                textBox.ForeColor = Color.Black;
                textBox.BackColor = Color.LightGray;
            }
        }

        //查找上一个不同的行
        public static void GetPreviouseDifferRow(DataGridView grid)
        {
            int startIndex = grid.CurrentCell.RowIndex;
            DataTable dt = (DataTable)grid.DataSource;
            if (dt == null) return;

            bool preSectionDiffer = false;
            if (grid.CurrentRow.Cells[3].Value == grid.CurrentRow.Cells[5].Value)
            {
                preSectionDiffer = true;
            }
            startIndex--; //从当前行下一行查找
            if (startIndex < 0)
            {
                MessageBox.Show("没有上一个不同的区域！", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = startIndex; i >= 0; i--)
            {
                if (grid.CurrentCell.RowIndex == 0)
                {
                    MessageBox.Show("没有上一个不同的区域！", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DataGridViewRow row = grid.Rows[i];
                if (row.Visible)
                {
                    if (row.Cells[3].Value != row.Cells[5].Value)
                    {
                        if (preSectionDiffer)
                        {
                            grid.CurrentCell = grid[7, i];
                            return;
                        }
                    }
                    else
                    {
                        preSectionDiffer = true;
                    }
                }
            }
        }

        private void Next(int maxRowIndex,ref int rowIndex)
        {
            rowIndex++;
            if (rowIndex > maxRowIndex)
            {
                rowIndex = 0;
            }
        }

        //查找下一个不同的行区域
        public static void GetNextDifferSectionRow(DataGridView grid)
        {
            int startIndex = grid.CurrentCell.RowIndex;
            DataTable dt = (DataTable)grid.DataSource;
            if(dt==null)return;

            bool preSectionDiffer = false;
            if (grid.CurrentRow.Cells[3].Value == grid.CurrentRow.Cells[5].Value)
            {
                preSectionDiffer = true;
            }
            startIndex++; //从当前行下一行查找
            if (startIndex >= grid.Rows.Count - 1) return;

            for (int i = startIndex; i < grid.Rows.Count; i++)
            {
                if (i == grid.Rows.Count - 1)
                {
                    MessageBox.Show("没有下一个不同的区域！", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                DataGridViewRow row = grid.Rows[i];
                if (row.Visible)
                {
                    if (row.Cells[3].Value != row.Cells[5].Value)
                    {
                        if (preSectionDiffer)
                        {
                            grid.CurrentCell = grid[7, i];
                            return;
                        }
                    }
                    else
                    {
                        preSectionDiffer = true;
                    }
                }
            }
        }
    }
}
