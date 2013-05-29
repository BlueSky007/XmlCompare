namespace XmlCompare
{
    partial class QueryLine
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.RowLine = new System.Windows.Forms.TextBox();
            this.ColumnLine = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CancleBtn = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CurrentPageRadio = new System.Windows.Forms.RadioButton();
            this.AllDataSource = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "行：";
            // 
            // RowLine
            // 
            this.RowLine.Location = new System.Drawing.Point(12, 35);
            this.RowLine.Name = "RowLine";
            this.RowLine.Size = new System.Drawing.Size(83, 20);
            this.RowLine.TabIndex = 1;
            // 
            // ColumnLine
            // 
            this.ColumnLine.Location = new System.Drawing.Point(12, 99);
            this.ColumnLine.Name = "ColumnLine";
            this.ColumnLine.Size = new System.Drawing.Size(83, 20);
            this.ColumnLine.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "列：";
            // 
            // CancleBtn
            // 
            this.CancleBtn.Location = new System.Drawing.Point(216, 135);
            this.CancleBtn.Name = "CancleBtn";
            this.CancleBtn.Size = new System.Drawing.Size(64, 23);
            this.CancleBtn.TabIndex = 5;
            this.CancleBtn.Text = "取消";
            this.CancleBtn.UseVisualStyleBackColor = true;
            this.CancleBtn.Click += new System.EventHandler(this.CancleBtn_Click);
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(137, 135);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(64, 23);
            this.OkButton.TabIndex = 6;
            this.OkButton.Text = "确定";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AllDataSource);
            this.groupBox1.Controls.Add(this.CurrentPageRadio);
            this.groupBox1.Location = new System.Drawing.Point(111, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(169, 107);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询方式";
            // 
            // CurrentPageRadio
            // 
            this.CurrentPageRadio.AutoSize = true;
            this.CurrentPageRadio.Checked = true;
            this.CurrentPageRadio.Location = new System.Drawing.Point(26, 26);
            this.CurrentPageRadio.Name = "CurrentPageRadio";
            this.CurrentPageRadio.Size = new System.Drawing.Size(109, 17);
            this.CurrentPageRadio.TabIndex = 0;
            this.CurrentPageRadio.TabStop = true;
            this.CurrentPageRadio.Text = "只查询显示页面";
            this.CurrentPageRadio.UseVisualStyleBackColor = true;
            // 
            // AllDataSource
            // 
            this.AllDataSource.AutoSize = true;
            this.AllDataSource.Location = new System.Drawing.Point(26, 73);
            this.AllDataSource.Name = "AllDataSource";
            this.AllDataSource.Size = new System.Drawing.Size(109, 17);
            this.AllDataSource.TabIndex = 1;
            this.AllDataSource.Text = "查询当前数据源";
            this.AllDataSource.UseVisualStyleBackColor = true;
            // 
            // QueryLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 160);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.CancleBtn);
            this.Controls.Add(this.ColumnLine);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RowLine);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QueryLine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Go To";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox RowLine;
        private System.Windows.Forms.TextBox ColumnLine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CancleBtn;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton AllDataSource;
        private System.Windows.Forms.RadioButton CurrentPageRadio;
    }
}