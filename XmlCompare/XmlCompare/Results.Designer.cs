namespace XmlCompare
{
    partial class Results
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
            this.lvDestination = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvSource = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvDestination
            // 
            this.lvDestination.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader3});
            this.lvDestination.FullRowSelect = true;
            this.lvDestination.HideSelection = false;
            this.lvDestination.LabelEdit = true;
            this.lvDestination.Location = new System.Drawing.Point(507, 12);
            this.lvDestination.MultiSelect = false;
            this.lvDestination.Name = "lvDestination";
            this.lvDestination.Size = new System.Drawing.Size(513, 680);
            this.lvDestination.TabIndex = 4;
            this.lvDestination.UseCompatibleStateImageBehavior = false;
            this.lvDestination.View = System.Windows.Forms.View.Details;
            this.lvDestination.SelectedIndexChanged += new System.EventHandler(this.lvDestination_SelectedIndexChanged);
            this.lvDestination.Resize += new System.EventHandler(this.lvDestination_Resize);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Text (Destination)";
            this.columnHeader4.Width = 450;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Line";
            this.columnHeader3.Width = 50;
            // 
            // lvSource
            // 
            this.lvSource.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader1});
            this.lvSource.FullRowSelect = true;
            this.lvSource.HideSelection = false;
            this.lvSource.Location = new System.Drawing.Point(2, 12);
            this.lvSource.MultiSelect = false;
            this.lvSource.Name = "lvSource";
            this.lvSource.Size = new System.Drawing.Size(499, 680);
            this.lvSource.TabIndex = 3;
            this.lvSource.UseCompatibleStateImageBehavior = false;
            this.lvSource.View = System.Windows.Forms.View.Details;
            this.lvSource.SelectedIndexChanged += new System.EventHandler(this.lvSource_SelectedIndexChanged);
            this.lvSource.Resize += new System.EventHandler(this.lvSource_Resize);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Line";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Text (Source)";
            this.columnHeader2.Width = 450;
            // 
            // Results
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 514);
            this.Controls.Add(this.lvDestination);
            this.Controls.Add(this.lvSource);
            this.Name = "Results";
            this.Text = "Results";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Resize += new System.EventHandler(this.Results_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvDestination;
        public System.Windows.Forms.ColumnHeader columnHeader4;
        public System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ListView lvSource;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}