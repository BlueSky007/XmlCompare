namespace XmlCompare.Setting
{
    partial class SettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.tckback = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.HideOvalShape = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tckback)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.tckback);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(198, 102);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基本设置";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(84, 46);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(61, 17);
            this.radioButton2.TabIndex = 11;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "不启动";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(8, 44);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(73, 17);
            this.radioButton1.TabIndex = 10;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "开机启动";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // tckback
            // 
            this.tckback.AutoSize = false;
            this.tckback.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tckback.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tckback.Location = new System.Drawing.Point(28, 20);
            this.tckback.Maximum = 100;
            this.tckback.Minimum = 20;
            this.tckback.Name = "tckback";
            this.tckback.Size = new System.Drawing.Size(99, 16);
            this.tckback.SmallChange = 5;
            this.tckback.TabIndex = 7;
            this.tckback.TickFrequency = 10;
            this.tckback.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tckback.Value = 100;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(127, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "不透明";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "透明";
            // 
            // HideOvalShape
            // 
            this.HideOvalShape.BackColor = System.Drawing.Color.Transparent;
            this.HideOvalShape.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("HideOvalShape.BackgroundImage")));
            this.HideOvalShape.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.HideOvalShape.BorderColor = System.Drawing.Color.Transparent;
            this.HideOvalShape.BorderStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.HideOvalShape.FillColor = System.Drawing.Color.Transparent;
            this.HideOvalShape.FillGradientColor = System.Drawing.Color.Transparent;
            this.HideOvalShape.Location = new System.Drawing.Point(413, 4);
            this.HideOvalShape.Name = "HideOvalShape";
            this.HideOvalShape.SelectionColor = System.Drawing.Color.Transparent;
            this.HideOvalShape.Size = new System.Drawing.Size(15, 15);
            this.HideOvalShape.Click += new System.EventHandler(this.HideOvalShape_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.HideOvalShape});
            this.shapeContainer1.Size = new System.Drawing.Size(451, 151);
            this.shapeContainer1.TabIndex = 14;
            this.shapeContainer1.TabStop = false;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 151);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.shapeContainer1);
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.LocationChanged += new System.EventHandler(this.SettingForm_LocationChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tckback)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TrackBar tckback;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Microsoft.VisualBasic.PowerPacks.OvalShape HideOvalShape;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
    }
}