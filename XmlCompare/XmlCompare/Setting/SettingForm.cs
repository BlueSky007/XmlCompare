using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XmlCompare.StaticClass;

namespace XmlCompare.Setting
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
            this.Opacity = Convert.ToDouble(PublicClass.Diaphaneity) / 100.0;
        }
        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_HOR_POSITIVE = 0x0001;
        const int AW_HOR_NEGATIVE = 0x0002;
        const int AW_VER_POSITIVE = 0x0004;
        const int AW_VER_NEGATIVE = 0x0008;
        const int AW_CENTER = 0x0010;
        const int AW_HIDE = 0x10000;
        const int AW_ACTIVATE = 0x20000;
        const int AW_SLIDE = 0x40000;
        const int AW_BLEND = 0x80000;
        private void HideOvalShape_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 300, AW_HIDE | AW_VER_NEGATIVE);
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            //初始化调用不规则窗体生成代码
            BitmapRegion BitmapRegion = new BitmapRegion();//此为生成不规则窗体和控件的类
            //BitmapRegion.CreateControlRegion(this, EJZLibrary.Properties.ResourceBackgroundImage.configFormBackground_01);
            //打开窗体特效
            AnimateWindow(this.Handle, 1000, AW_CENTER | AW_ACTIVATE);

            tckback.Value = Convert.ToInt32(PublicClass.Diaphaneity);
        }

        private void SettingForm_LocationChanged(object sender, EventArgs e)
        {
            if ((this.Location.X - Form1._x) > 25 || (this.Location.Y - Form1._y > 25))
                Form1._Start = false;
            else if ((this.Location.X - Form1._x) >= 0 && (this.Location.X - Form1._x) <= 25)
            {
                Form1._Start = true;
                this.Location = new Point(Form1._x, Form1._y);
            }
        }


    }
}
