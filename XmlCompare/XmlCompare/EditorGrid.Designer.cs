namespace XmlCompare
{
    partial class EditorGrid
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
            this.EditorRichTextBox = new System.Windows.Forms.RichTextBox();
            this.OKbutton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EditorRichTextBox
            // 
            this.EditorRichTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.EditorRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditorRichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditorRichTextBox.ForeColor = System.Drawing.Color.Blue;
            this.EditorRichTextBox.Location = new System.Drawing.Point(2, -1);
            this.EditorRichTextBox.Name = "EditorRichTextBox";
            this.EditorRichTextBox.Size = new System.Drawing.Size(453, 120);
            this.EditorRichTextBox.TabIndex = 1;
            this.EditorRichTextBox.Text = "";
            // 
            // OKbutton
            // 
            this.OKbutton.Location = new System.Drawing.Point(300, 125);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(75, 23);
            this.OKbutton.TabIndex = 2;
            this.OKbutton.Text = "确定";
            this.OKbutton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(380, 125);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "取消";
            this.CancelButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // EditorGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(457, 150);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKbutton);
            this.Controls.Add(this.EditorRichTextBox);
            this.Name = "EditorGrid";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑Grid";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox EditorRichTextBox;
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Button CancelButton;
    }
}