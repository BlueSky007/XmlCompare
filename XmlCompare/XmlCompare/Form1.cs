using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Xml;
using System.Configuration;
using System.Drawing.Drawing2D;
using XmlCompare.Setting;
using XmlCompare.StaticClass;
using System.Collections;
using DifferenceEngine;
using System.Threading;

namespace XmlCompare
{
    public partial class Form1 : Form
    {
        private bool IsAddButtonColumn = false;
        private bool IsAddButtonWsdlColumn = false;
        private string _OldUnZipFromPath;
        private string _OldUnZipToPath;
        private string _NewUnZipFromPath;
        private string _NewUnZipToPath;
        private string _OldJavaXmlPath;
        private string _NewJavaXmlPath;
        private string _OldSLXmlPath;
        private string _NewSLXmlPath;
        private string _CombinXmlPath;
        private string _SLCombinXmlPath;
        //压缩
        private string _CompressInputPath;
        private string _CompressOutputPath;

        public EditorGrid _EditorGridForm;

        SettingForm form = new SettingForm();
        public static bool _Start = true;     //定义一个变量_start为true；
        public static int _x, _y;             //定义两个int变量实现x轴y轴计算；
        public List<string> _HaveCompareFiles = new List<string>();
        public Form1()
        {
            InitializeComponent();
            this.QueryTypeComBox.SelectedIndex = 0;
            this.BindComboBox();
            this.GetConfigSetting();
        }

        private void SettingCombinValue(string newValue,int colIndex,int rowIndex)
        {
            this.OldXmlGrid[colIndex, rowIndex].Value = newValue;
        }
        private void SettingCombinValueCallBack(string newValue, int colIndex, int rowIndex)
        {
            this.WsdlGrid[colIndex, rowIndex].Value = newValue;
        }

        private void GetConfigSetting()
        {
            this._OldUnZipFromPath = this.OldFromPathComBox.Text = ConfigurationSettings.AppSettings["OldZipFromPath"];
            this._OldUnZipToPath = this.OldToPathComBox.Text = ConfigurationSettings.AppSettings["OldZipToPath"];
            this._NewUnZipFromPath = this.NewFromPathComBox.Text = ConfigurationSettings.AppSettings["NewZipFromPath"];
            this._NewUnZipToPath = this.NewToPathComBox.Text = ConfigurationSettings.AppSettings["NewZipToPath"];
            //xml文件路径 = 压缩包输出根目录 +　文件路径
            this._OldJavaXmlPath = this.OldXmlPathComBox.Text = this._OldUnZipToPath + "\\{2}\\{3}\\{4}";
            this._NewJavaXmlPath = this.NewXmlPathComBox.Text = this._NewUnZipToPath + "\\{1}\\{2}\\{3}";

            this._OldSLXmlPath = this.OldSLXmlPathComBox.Text = ConfigurationSettings.AppSettings["OldSLXmlPath"];
            this._NewSLXmlPath = this.NewSLXmlPathComBox.Text = ConfigurationSettings.AppSettings["NewSLXmlPath"];

            this._CombinXmlPath = this.CombinXmlPathComBox.Text = this._NewUnZipToPath + "\\{1}\\{2}\\{3}";
            this._SLCombinXmlPath = this.SLCombinXmlPathComBox.Text = ConfigurationSettings.AppSettings["SLCombinXmlPath"];

            //压缩
            this._CompressInputPath = this.InputPathComBox.Text = ConfigurationSettings.AppSettings["CompressInputPath"];
            this._CompressOutputPath = this.OutputPathComBox.Text = ConfigurationSettings.AppSettings["CompressOutPutPath"];

            //ServiceWsdl路径
            this.OldServiceWsdCmb.Text = this._OldUnZipToPath + "\\{2}\\{3}";
            this.NewServiceWsdCmb.Text = this._NewUnZipToPath + "\\{1}\\{2}";
            this.SaveWsdlPathCmb.Text = this._NewUnZipToPath +  "\\{1}\\{2}";
        }

        private void DefaultSettingBtn_Click(object sender, EventArgs e)
        {
            this.GetConfigSetting();
        }

        private void CompanyComBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string companies = ConfigurationSettings.AppSettings["CompanyList"];
            List<string> CompanyList = new List<string>();
            List<string> LanguageList = new List<string>();
            string[] companyArray = companies.Split('|');
            for (int i = 0; i < companyArray.Length; i++)
            {
                if (this.CompanyComBox.Text == companyArray[i].Split(';')[0].Trim())
                {
                    string[] languageArray = companyArray[i].Split(';')[1].Split(',');
                    for (int j = 0; j < languageArray.Length; j++)
                    {
                        LanguageList.Add(languageArray[j]);
                    }
                }
            }
            this.LanguageComBox.DataSource = LanguageList;
        }

        private void BindComboBox()
        {
            string companies = ConfigurationSettings.AppSettings["CompanyList"];
            List<string> CompanyList = new List<string>();
            List<string> LanguageList = new List<string>();
            string[] companyArray = companies.Split('|');
            for (int i = 0; i < companyArray.Length; i++)
            {
                CompanyList.Add(companyArray[i].Split(';')[0].Trim());
            }

            
            List<string> FileList = new List<string>() { "多语言文件", "配置文件", "Settings配置"};
            //List<string> FileList = new List<string>() { "多语言文件", "配置文件", "Settings配置", "ServiceWsdl配置", "AuthenticationWsdl配置" };
            
            this.CompanyComBox.DataSource = CompanyList;
            this.FileTypeComBox.DataSource = FileList;
        }

        #region ....解压文件
        //获取解压文件zip路径
        private void OpenOldFileBtn_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Title = "XML Compare";
            this.openFileDialog1.InitialDirectory = "c:\\";
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this._OldUnZipFromPath = this.openFileDialog1.FileName;
            }
            this.OldFromPathComBox.Text = this._OldUnZipFromPath;
        }

        private void UnzipOldPathBtn_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this._OldUnZipToPath = this.folderBrowserDialog1.SelectedPath;
            }
            this.OldToPathComBox.Text = this._OldUnZipToPath;
        }
        private void OpenNewFileBtn_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "解压Zip文件(*.zip) |*.zip";
            this.openFileDialog1.InitialDirectory = "c:\\";
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this._NewUnZipFromPath = this.openFileDialog1.FileName;
            }
            this.NewFromPathComBox.Text = this._OldUnZipFromPath;
        }
        private void UnzipNewPathBtn_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this._NewUnZipToPath = this.folderBrowserDialog1.SelectedPath;
            }
            this.NewToPathComBox.Text = this._OldUnZipToPath;
        }

        private void OpenOldXmlBtn_Click(object sender, EventArgs e)
        {
            //this.openFileDialog1.Filter = "旧版本Java.Xml文件(*.xml) |*.xml";
            this.openFileDialog1.InitialDirectory = "c:\\";
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this._OldJavaXmlPath = this.openFileDialog1.FileName;
                this.OldXmlPathComBox.Text = this.openFileDialog1.FileName;
            }
        }

        private void OpenNewXmlBtn_Click(object sender, EventArgs e)
        {
           // this.openFileDialog1.Filter = "新版本Java.Xml文件(*.xml) |*.xml";
            this.openFileDialog1.InitialDirectory = "c:\\";
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this._NewJavaXmlPath = this.openFileDialog1.FileName;
                this.NewXmlPathComBox.Text = this.openFileDialog1.FileName;
            }
        }

        private void OpenOldSLXmlBtn_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "旧版本Silverlight.Xml文件(*.xml) |*.xml";
            this.openFileDialog1.InitialDirectory = "c:\\";
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this._OldSLXmlPath = this.openFileDialog1.FileName;
                this.OldSLXmlPathComBox.Text = this.openFileDialog1.FileName;
            }
        }

        private void OpenNewSLXmlPathBtn_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "新版本Silverlight.Xml文件(*.xml) |*.xml";
            this.openFileDialog1.InitialDirectory = "c:\\";
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this._NewSLXmlPath = this.openFileDialog1.FileName;
                this.NewSLXmlPathComBox.Text = this.openFileDialog1.FileName;
            }
        }

        private void OpenCombinXmlBtn_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this._CombinXmlPath = this.folderBrowserDialog1.SelectedPath;
            }
            this.CombinXmlPathComBox.Text = this._CombinXmlPath;
        }

        
        private string GetUnZipToPathName(int index)
        {
            if (index == 0)
            {
                return "Language";
            }
            else
            {
                return "Configuration";
            }
        }
        private void UpZipButton_Click(object sender, EventArgs e)
        {
            string err;
            string unZipName = PathManager.GetZipFileName(this.FileTypeComBox.SelectedIndex);
            if (this.OldFromPathComBox.Text.Contains("{0}"))
            { 
                this._OldUnZipFromPath = string.Format(this.OldFromPathComBox.Text, this.ChangeSetTextBox.Text, this.CompanyComBox.Text, unZipName);
                this._OldUnZipToPath = string.Format(this.OldToPathComBox.Text, this.ChangeSetTextBox.Text, this.CompanyComBox.Text);
            }

            string preZipPath = this._OldUnZipFromPath == null ? System.Configuration.ConfigurationSettings.AppSettings["OldZipFromPath"] : this._OldUnZipFromPath;
            string UnZipPath = this._OldUnZipToPath == null ? System.Configuration.ConfigurationSettings.AppSettings["OldZipToPath"] : this._OldUnZipToPath;

            if (!PathManager.ValidFileExist(this._OldUnZipFromPath)) return;

            if (UnZipClass.UnZipFile(preZipPath, UnZipPath, out err))
            {
                MessageBox.Show("文件" + unZipName + "成功解压到路径:" + this._OldUnZipToPath, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
        }
        //解压新文件
        private void UnZipNewBtn_Click(object sender, EventArgs e)
        {
            string err;
            string unZipName = PathManager.GetZipFileName(this.FileTypeComBox.SelectedIndex);
            if (this.NewToPathComBox.Text.Contains("{0}"))
            {
                this._NewUnZipFromPath = string.Format(this.NewFromPathComBox.Text, unZipName);
                //新包解压到临时文件夹
                this._NewUnZipToPath = string.Format(this.NewToPathComBox.Text,this.GetUnZipToPathName(this.FileTypeComBox.SelectedIndex));
            }
            string preZipPath = this._NewUnZipFromPath;
            string UnZipPath = this._NewUnZipToPath;

            if (!PathManager.ValidFileExist(this._NewUnZipFromPath)) return;
            this.CreateFileFolder(this._NewUnZipToPath);

            if (UnZipClass.UnZipFile(preZipPath, UnZipPath, out err))
            {
                MessageBox.Show("文件" + unZipName + "成功解压到路径:" + this._NewUnZipToPath, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
        }
        #endregion
        

        #region ....压缩文件

        private void CompressFileButton_Click(object sender, EventArgs e)
        {
            int index = this.FileTypeComBox.SelectedIndex;
            string ZipFileName = PathManager.GetZipFileName(index);

            if (string.IsNullOrEmpty(this.PublishChangeSetTextBox.Text))
            {
                MessageBox.Show("请输入要发布的版本号！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.InputPathComBox.Text.Contains("{0}"))
            {
                this._CompressInputPath = string.Format(this.InputPathComBox.Text, PathManager.GetParentFolderName(index));
                this._CompressOutputPath = string.Format(this.OutputPathComBox.Text, this.PublishChangeSetTextBox.Text, this.CompanyComBox.Text, ZipFileName);
            }
            string outputPath = this._CompressOutputPath;
            string inputPath = this._CompressInputPath;
            int intZipLevel = 2;
            string strPassword = "";

            DirectoryInfo TheFolder = new DirectoryInfo(inputPath);
            DirectoryInfo PublishFolder = new DirectoryInfo(outputPath);
            List<string> fileNameList = new List<string>();
            //创建发布路径（如果不存在）
            if (!PublishFolder.Parent.Exists)
            {
                this.CreateFileFolder(outputPath.Substring(0, outputPath.Length - ZipFileName.Length));
            }
            //遍历文件夹
            if (!TheFolder.Exists)
            {
                this.CreateFileFolder(inputPath);
                TheFolder = new DirectoryInfo(inputPath);
            }
            if (TheFolder.GetDirectories().Length == 0
                && TheFolder.GetFiles().Length == 0)
            {
                MessageBox.Show("要压缩的文件夹:" + this._CompressInputPath + "没有文件！", "警告!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {
                fileNameList.Add(Path.Combine(inputPath, NextFolder.Name));
            }

            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                fileNameList.Add(Path.Combine(inputPath, NextFile.Name));
            }

            string[] filesOrDirectoriesPaths = new string[fileNameList.Count];
            filesOrDirectoriesPaths = fileNameList.ToArray();
            if (ZipFile.Zip(outputPath, inputPath, intZipLevel, strPassword, filesOrDirectoriesPaths))
            {
                MessageBox.Show("压缩文件成功! 输出路径：" + outputPath, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
        }
       
        private void InPutPathButton_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.Description = "选择要压缩文件对象";
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this._CompressInputPath = this.folderBrowserDialog1.SelectedPath;
                this.InputPathComBox.Text = this._CompressInputPath;
            }
        }

        private void OutPutPathButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this._CompressOutputPath = this.folderBrowserDialog1.SelectedPath;
                this.OutputPathComBox.Text = this._CompressOutputPath;
            }
        }
       #endregion

        private bool ValidFile(string fname)
        {
            if (fname != string.Empty)
            {
                if (File.Exists(fname))
                {
                    return true;
                }
            }
            return false;
        }
        private void ExportJavaData(string oldFilePath, string newFilePath, out bool isHasAddRecord, out bool isHasModifyRecord)
        {
            isHasAddRecord = isHasModifyRecord = false;
            int UnchangeCount = 0;
            int index = this.FileTypeComBox.SelectedIndex;
            DataTable dt = XmlHelper.GetBindTable();

            Dictionary<string, string> NewDictionary = new Dictionary<string, string>();
            Dictionary<string, string> OldDictionary = new Dictionary<string, string>();

            if (!PathManager.ValidFileExist(oldFilePath)
                || !PathManager.ValidFileExist(newFilePath)) return;

            DataSet dataSet = new DataSet();
            DataSet dataSet2 = new DataSet();
            dataSet.ReadXml(oldFilePath);
            dataSet2.ReadXml(newFilePath);
            NewDictionary = XmlHelper.GetConverterDictionary(dataSet2);
            OldDictionary = XmlHelper.GetConverterDictionary(dataSet);

            this.Cursor = Cursors.WaitCursor;
            foreach (KeyValuePair<string, string> pair in NewDictionary)
            {
                DataRow dr = dt.NewRow();
                dr["NewKey"] = pair.Key;
                dr["NewValue"] = pair.Value;
                dr["CombinKey"] = pair.Key;
                if (OldDictionary.ContainsKey(pair.Key) && OldDictionary.ContainsValue(pair.Value))
                {
                    dr["OldKey"] = pair.Key;
                    dr["OldValue"] = pair.Value;
                    dr["CombinValue"] = pair.Value;
                    dr["Status"] = "Unchanged";
                    UnchangeCount++;
                }
                else if (OldDictionary.ContainsKey(pair.Key) && !OldDictionary.ContainsValue(pair.Value))
                {
                    string combinValue;
                    dr["OldKey"] = pair.Key;
                    if (this.ModifyCheckBox.Checked)
                    {
                        OldDictionary.TryGetValue(pair.Key, out combinValue);
                        dr["CombinValue"] = combinValue;
                        dr["OldValue"] = combinValue;
                    }
                    else
                    {
                        OldDictionary.TryGetValue(pair.Key, out combinValue);
                        dr["CombinValue"] = pair.Value;
                        dr["OldValue"] = combinValue;
                    }
                    dr["Status"] = "Modify";
                    isHasModifyRecord = true;
                }
                else if (!OldDictionary.ContainsKey(pair.Key))
                {
                    dr["OldKey"] = "";
                    dr["OldValue"] = "";
                    dr["Status"] = "Add";
                    dr["CombinValue"] = this.GetNewCheckBox.Checked ? pair.Value : "";
                    isHasAddRecord = true;
                }
                dt.Rows.Add(dr);
            }
            foreach (KeyValuePair<string, string> pair in OldDictionary)
            {
                DataRow dr = dt.NewRow();
                if (!NewDictionary.ContainsKey(pair.Key))
                {
                    dr["NewKey"] = "";
                    dr["NewValue"] = "";
                    dr["OldKey"] = pair.Key;
                    dr["OldValue"] = pair.Value;
                    dr["CombinKey"] = "";
                    dr["CombinValue"] = "";
                    dr["Status"] = "Delete";
                    dt.Rows.Add(dr);
                }
            }
            this.DifferStatusText.Text = (OldDictionary.Count - UnchangeCount).ToString() + "条不同记录";
            GridControl.AddButtonColumn(this.OldXmlGrid, this.IsAddButtonColumn);
            this.IsAddButtonColumn = true;
            this.OldXmlGrid.DataSource = dt;

            GridControl.HideRow(this.OldXmlGrid, this.UnchangedCheckBox, this.NoContainModifyCheckBox);
            GridControl.SetGridDifferBackColor(this.OldXmlGrid);
            GridControl.SetGridColumnWidth(this.OldXmlGrid);
            this.CheckDataButton.Enabled = true;

            if (this.OldXmlGrid.CurrentCell != null)
            {
                this.CurrentOldXmlTextBox.Text = dt.Rows[this.OldXmlGrid.CurrentCell.RowIndex]["OldValue"].ToString();
                this.CurrentNewXmlText.Text = dt.Rows[this.OldXmlGrid.CurrentCell.RowIndex]["NewValue"].ToString();
            }


            this.Cursor = Cursors.Default;
        }

        //导入Xml文件
        private void GetDataButton_Click(object sender, EventArgs e)
        {
            int UnchangeCount = 0;
            int index = this.FileTypeComBox.SelectedIndex;
            DataTable dt = XmlHelper.GetBindTable();

            Dictionary<string, string> NewDictionary = new Dictionary<string, string>();
            Dictionary<string, string> OldDictionary = new Dictionary<string, string>();

            if (this.JavaXmlOption.Checked)
            {
                if (this.OldXmlPathComBox.Text.Contains("{0}"))
                {
                    string parentFileName = this.GetUnZipToPathName(index);
                    string compareFileName = PathManager.GetCompareFileName(this.FileTypeComBox.SelectedIndex);
                    string languageStr = index == 0 ? this.LanguageComBox.Text : null;

                    this._OldJavaXmlPath = string.Format(this.OldXmlPathComBox.Text, this.ChangeSetTextBox.Text, this.CompanyComBox.Text, parentFileName, languageStr, compareFileName);
                    this._NewJavaXmlPath = string.Format(this.NewXmlPathComBox.Text, parentFileName, parentFileName, languageStr, compareFileName);
                    this._OldJavaXmlPath = this._OldJavaXmlPath.Replace("\\\\", "\\");
                    this._NewJavaXmlPath = this._NewJavaXmlPath.Replace("\\\\", "\\");
                }

                if (!PathManager.ValidFileExist(this._OldJavaXmlPath)
                    || !PathManager.ValidFileExist(this._NewJavaXmlPath)) return;
                
                DataSet dataSet = new DataSet();
                DataSet dataSet2 = new DataSet();
                dataSet.ReadXml(this._OldJavaXmlPath);
                dataSet2.ReadXml(this._NewJavaXmlPath);
                NewDictionary = XmlHelper.GetConverterDictionary(dataSet2);
                OldDictionary = XmlHelper.GetConverterDictionary(dataSet);
            }
            else
            {
                //Silverlight 版本导入xml
                if (string.IsNullOrEmpty(this.ChangeSetTextBox.Text))
                {
                    MessageBox.Show("请输入上一次发布的版本号！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                XmlDocument newXmlDoc = new XmlDocument();
                XmlDocument oldXmlDoc = new XmlDocument();

                if (this.OldSLXmlPathComBox.Text.Contains("{0}"))
                {
                    string languageStr = PathManager.GetSLLanguage(this.LanguageComBox.SelectedIndex);
                    this._OldSLXmlPath = string.Format(this.OldSLXmlPathComBox.Text,this.ChangeSetTextBox.Text,languageStr,languageStr);
                    this._NewSLXmlPath = string.Format(this.NewSLXmlPathComBox.Text, languageStr, languageStr);
                }
                if(!File.Exists(this._OldSLXmlPath))
                {
                    MessageBox.Show("Silverlight旧版本Xml文件不存在! 路径：" + this._OldSLXmlPath, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!File.Exists(this._NewSLXmlPath))
                {
                    MessageBox.Show("Silverlight新版本Xml文件不存在! 路径：" + this._NewSLXmlPath, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                newXmlDoc.Load(this._OldSLXmlPath);
                oldXmlDoc.Load(this._NewSLXmlPath);
                XmlNodeList newNodeList = newXmlDoc.SelectSingleNode("Resourceses").ChildNodes;
                XmlNodeList oldNodeList = oldXmlDoc.SelectSingleNode("Resourceses").ChildNodes;
                NewDictionary = this.GetDictionary(newNodeList);
                OldDictionary = this.GetDictionary(oldNodeList);
            }
            this.Cursor = Cursors.WaitCursor;
            foreach (KeyValuePair<string, string> pair in NewDictionary)
            {
                DataRow dr = dt.NewRow();
                dr["NewKey"] = pair.Key;
                dr["NewValue"] = pair.Value;
                dr["CombinKey"] = pair.Key;
                if (OldDictionary.ContainsKey(pair.Key) && OldDictionary.ContainsValue(pair.Value))
                {
                    dr["OldKey"] = pair.Key;
                    dr["OldValue"] = pair.Value;
                    dr["CombinValue"] = pair.Value;
                    dr["Status"] = "Unchanged";
                    UnchangeCount++;
                }
                else if (OldDictionary.ContainsKey(pair.Key) && !OldDictionary.ContainsValue(pair.Value))
                {
                    string combinValue;
                    dr["OldKey"] = pair.Key;
                    if (this.ModifyCheckBox.Checked)
                    {
                        OldDictionary.TryGetValue(pair.Key, out combinValue);
                        dr["CombinValue"] = combinValue;
                        dr["OldValue"] = combinValue;
                    }
                    else
                    {
                        OldDictionary.TryGetValue(pair.Key, out combinValue);
                        dr["CombinValue"] = pair.Value;
                        dr["OldValue"] = combinValue;
                    }
                    dr["Status"] = "Modify";  
                }
                else if (!OldDictionary.ContainsKey(pair.Key))
                {
                    dr["OldKey"] = "";
                    dr["OldValue"] = "";
                    dr["Status"] = "Add";
                    dr["CombinValue"] = this.GetNewCheckBox.Checked ? pair.Value : "";
                }
                dt.Rows.Add(dr);
            }
            foreach (KeyValuePair<string, string> pair in OldDictionary)
            {
                DataRow dr = dt.NewRow();
                if (!NewDictionary.ContainsKey(pair.Key))
                {
                    dr["NewKey"] = "";
                    dr["NewValue"] = "";
                    dr["OldKey"] = pair.Key;
                    dr["OldValue"] = pair.Value;
                    dr["CombinKey"] = "";
                    dr["CombinValue"] = "";
                    dr["Status"] = "Delete";
                    dt.Rows.Add(dr);
                }
            }
            this.DifferStatusText.Text = (OldDictionary.Count - UnchangeCount).ToString() + "条不同记录";
            GridControl.AddButtonColumn(this.OldXmlGrid,this.IsAddButtonColumn);
            this.IsAddButtonColumn = true;
            this.OldXmlGrid.DataSource = dt;

            GridControl.HideRow(this.OldXmlGrid, this.UnchangedCheckBox, this.NoContainModifyCheckBox);
            GridControl.SetGridDifferBackColor(this.OldXmlGrid);
            GridControl.SetGridColumnWidth(this.OldXmlGrid);
            this.CheckDataButton.Enabled = true;

            if (this.OldXmlGrid.CurrentCell != null)
            {
                this.CurrentOldXmlTextBox.Text = dt.Rows[this.OldXmlGrid.CurrentCell.RowIndex]["OldValue"].ToString();
                this.CurrentNewXmlText.Text = dt.Rows[this.OldXmlGrid.CurrentCell.RowIndex]["NewValue"].ToString();
            }
            
            
            this.Cursor = Cursors.Default;
        }

        public void CreateJavaDifferFile(string rootName, bool IsCreateFileTitel)
        {
            XmlDocument doc = new XmlDocument();
            if (IsCreateFileTitel)
            {
                XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", null, null);
                doc.AppendChild(xmlDec);
            }
            XmlElement root = doc.CreateElement(rootName);
            doc.AppendChild(root);
        }

        public Dictionary<string, string> GetDictionary(XmlNodeList nodeList)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (XmlNode node in nodeList)
            {
                XmlElement xe = (XmlElement)node;
                string key = xe.GetAttribute("key");
                string value = xe.GetAttribute("value");
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, value);
                }
            }
            return dictionary;
        }

        private void SaveJava(string filePath,string rootName)
        {
            if (rootName == "")
            {
                MessageBox.Show("ddd");
            }
            Dictionary<string, string> xmlDictionary = new Dictionary<string, string>();
            try
            {
                DataSet dataSet = XmlHelper.GetCombinDataSet(this.OldXmlGrid);
                if (dataSet == null) return;

                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement(rootName);
                XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", null, null);
                doc.AppendChild(xmlDec);

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    if (row["CombinKey"].ToString() == "") continue;
                    xmlDictionary.Add(row["CombinKey"].ToString(), row["CombinValue"].ToString());
                }
                doc.AppendChild(root);

                List<string> CreatedNode = new List<string>();

                foreach (KeyValuePair<string, string> pair in xmlDictionary)
                {
                    string[] nodeList = pair.Key.ToString().Split('.');
                    XmlHelper.CreateXml(doc, root, nodeList, pair.Value.ToString());
                }

                this.CreateFilePath(filePath);
                doc.Save(filePath);
                MessageBox.Show("Java文件保存成功保存路径:!" + filePath, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
            }
        }


        //保存合并xml文件
        private void SaveButton_Click(object sender, EventArgs e)
        {
            int index = this.FileTypeComBox.SelectedIndex;
            Dictionary<string, string> xmlDictionary = new Dictionary<string, string>();
            try
            {
                DataSet dataSet = XmlHelper.GetCombinDataSet(this.OldXmlGrid);
                if (dataSet == null) return;

                if (this.JavaXmlOption.Checked)
                {
                    XmlDocument doc = new XmlDocument();
                    string rootName = PathManager.GetXmlRootName(index);
                    XmlElement root = doc.CreateElement(rootName);

                    switch (this.FileTypeComBox.SelectedIndex)
                    {
                        case 0:
                            XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", null, null);
                            doc.AppendChild(xmlDec);
                            break;
                        case 2:
                            XmlDeclaration xmlDec2 = doc.CreateXmlDeclaration("1.0", null, null);
                            doc.AppendChild(xmlDec2);
                            break;
                    }

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        if (row["CombinKey"].ToString() == "") continue;
                        xmlDictionary.Add(row["CombinKey"].ToString(), row["CombinValue"].ToString());
                    }
                    doc.AppendChild(root);
                    
                    List<string> CreatedNode = new List<string>();

                    foreach (KeyValuePair<string, string> pair in xmlDictionary)
                    {
                        string[] nodeList = pair.Key.ToString().Split('.');
                        XmlHelper.CreateXml(doc, root, nodeList, pair.Value.ToString());
                    }
                    if (this.CombinXmlPathComBox.Text.Contains("{0}"))
                    {
                        string parentFileName = PathManager.GetParentFolderName(index);
                        string languageStr = index == 0 ? this.LanguageComBox.Text : null;
                        this._CombinXmlPath = string.Format(this.CombinXmlPathComBox.Text, parentFileName,parentFileName, languageStr, PathManager.GetCompareFileName(index));
                        this._CombinXmlPath = this._CombinXmlPath.Replace("\\\\", "\\");
                    }
                    this.CreateFilePath(this._CombinXmlPath);
                    doc.Save(this._CombinXmlPath);
                    MessageBox.Show("Java文件保存成功保存路径:!" + this._CombinXmlPath, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (string.IsNullOrEmpty(this.PublishChangeSetTextBox.Text))
                    {
                        MessageBox.Show("请输入Silverlight版本发布版本号！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    XmlDocument doc = new XmlDocument();
                    XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                    doc.AppendChild(xmlDec);
                    XmlElement root = doc.CreateElement("Resourceses");

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        string key = row["CombinKey"].ToString();
                        if (string.IsNullOrEmpty(key)) continue;
                        XmlElement node = doc.CreateElement("Resource");
                        node.SetAttribute("value", row["CombinValue"].ToString());
                        node.SetAttribute("key", key);
                        root.AppendChild(node);
                    }
                    doc.AppendChild(root);

                    if (this.SLCombinXmlPathComBox.Text.Contains("{0}"))
                    {
                        string languageStr = PathManager.GetSLLanguage(index);
                        this._SLCombinXmlPath = string.Format(this.SLCombinXmlPathComBox.Text,this.PublishChangeSetTextBox.Text,languageStr,languageStr);
                    }
                    this.CreateFilePath(this._SLCombinXmlPath);
                    
                    doc.Save(this._SLCombinXmlPath);
                    MessageBox.Show("Silverlight文件保存成功保存路径:!" + this._SLCombinXmlPath, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
            }
        }

        private void CreateFileFolder(string path)
        {
            string[] PathList = path.Split('\\');
            string FilePath = PathList[0] + "\\";
            for (int i = 1; i < PathList.Length; i++)
            {
                FilePath = Path.Combine( FilePath ,PathList[i]);
                DirectoryInfo TheFolder = new DirectoryInfo(FilePath);
                if (!TheFolder.Exists)
                {
                    TheFolder.Create();
                }
            }
        }

        private void CreateFilePath(string path)
        {
            string[] PathList = path.Split('\\');
            string FilePath = PathList[0] + "\\";
            for (int i = 1; i < PathList.Length-1; i++)
            {
                FilePath = Path.Combine(FilePath, PathList[i]);
                DirectoryInfo TheFolder = new DirectoryInfo(FilePath);
                if (!TheFolder.Exists)
                {
                    TheFolder.Create();
                }
            }
        }
        

        //************************Java 版本xml文件转换******************************
       
        private bool IsHasChildNode(DataTable dt)
        {
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName == column.ColumnName + "_Id")
                {
                    return true;
                }
            }
            return false;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void OldXmlGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            GridControl.SetGridDifferBackColor(this.OldXmlGrid);
        }

        private void OldXmlGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            GridControl.SetGridDifferBackColor(this.OldXmlGrid);
        }

        private void OldXmlGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex == this.OldXmlGrid.Rows.Count - 1) return;
            int RowIndex = e.RowIndex;
            DataTable dt = (DataTable)this.OldXmlGrid.DataSource;
            DataRow row = dt.Rows[e.RowIndex];
            DataGridView dgv = (DataGridView)sender;

            //如果是"Button"列，按钮被点击
            if (dgv.Columns[e.ColumnIndex].Name == "NewButton" &&
                dgv.Rows[RowIndex].Cells[0].Style.BackColor != Color.SlateGray)
            {
                if (MessageBox.Show("您确定要获取新版本吗?", "系统提示!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                {
                    dgv.Rows[RowIndex].Cells[6].Value = dgv.Rows[RowIndex].Cells[4].Value;
                    dgv.Rows[RowIndex].Cells[7].Value = dgv.Rows[RowIndex].Cells[5].Value;
                }
            }
            if (dgv.Columns[e.ColumnIndex].Name == "OldButton" &&
                dgv.Rows[RowIndex].Cells[0].Style.BackColor != Color.SlateGray)
            {
                if (MessageBox.Show("您确定要获取旧版本吗?", "系统提示!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                {
                    dgv.Rows[RowIndex].Cells[6].Value = dgv.Rows[RowIndex].Cells[2].Value;
                    dgv.Rows[RowIndex].Cells[7].Value = dgv.Rows[RowIndex].Cells[3].Value;
                }
            }
        }

        private void GetAddNodeButton_Click(object sender, EventArgs e)
        {
            XmlHelper.GetAddNode(this.OldXmlGrid, this.IsGetAllNewNodeCheck);
        }

        private void KeepModifyNodeButton_Click(object sender, EventArgs e)
        {
            XmlHelper.GetModifyNode(this.OldXmlGrid, this.KeepModifyNodeCheck);
        }

        private void DeleteNodeButton_Click(object sender, EventArgs e)
        {
            XmlHelper.DeleteNode(this.OldXmlGrid, this.DeleteOldNodeCheck);
        }

        private void QueryButton_Click(object sender, EventArgs e)
        {
            if (this.OldXmlGrid.DataSource == null) return;
            string queryString = this.QueryStrTextBox.Text;
            GridQuery gridQuery = new GridQuery(this.OldXmlGrid);
            gridQuery.QueryStr = queryString;
            gridQuery.QueryType = this.BlurQueryRadioButton.Checked ? QueryType.Blur : QueryType.Complete;
            if (this.QueryTypeComBox.SelectedIndex == 0)
            {
                gridQuery.Query();
            }
            else if (this.QueryTypeComBox.SelectedIndex == 1
                || this.QueryTypeComBox.SelectedIndex == 2)
            {
                gridQuery.Query2();
            }
        }

        private void QueryStrTextBox_TextChanged(object sender, EventArgs e)
        {
            string queryString = this.QueryStrTextBox.Text;
            GridQuery gridQuery = new GridQuery(this.OldXmlGrid);
            gridQuery.QueryStr = queryString;
            gridQuery.QueryType = this.BlurQueryRadioButton.Checked ? QueryType.Blur : QueryType.Complete;
            if (this.QueryTypeComBox.SelectedIndex == 0)
            {
                gridQuery.Query();
            }
            else if (this.QueryTypeComBox.SelectedIndex == 1
                || this.QueryTypeComBox.SelectedIndex == 2)
            {
                gridQuery.Query2();
            }
        }

        private void QueryTypeComBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.OldXmlGrid.DataSource == null) return;
            int rowIndex = this.OldXmlGrid.CurrentCell.RowIndex;
            if (this.QueryTypeComBox.SelectedIndex == 1)
            {
                this.OldXmlGrid.CurrentCell = this.OldXmlGrid[6, rowIndex];
            }
            else if (this.QueryTypeComBox.SelectedIndex == 2)
            {
                this.OldXmlGrid.CurrentCell = this.OldXmlGrid[7, rowIndex];
            }
        }

        private void NewToPathComBox_TextChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            this._CompressInputPath = this.InputPathComBox.Text = cmb.Text;
        }

        private void OldXmlGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                string value = this.OldXmlGrid[e.ColumnIndex, e.RowIndex].Value.ToString();

                this._EditorGridForm = new EditorGrid(e.ColumnIndex, e.RowIndex, value);
                this._EditorGridForm.ChangedXml = new EditorGrid.ChangeXmlValueHandler(SettingCombinValue);
                this._EditorGridForm.Show();
            }
           // Clipboard.SetDataObject(value);
        }

        //设置Tab背景色
        private void CompareTab_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle endPageRect = CompareTab.GetTabRect(CompareTab.TabPages.Count - 1); //最后一个标题栏的范围            
            Rectangle HeaderBackRect = Rectangle.Empty;             //背景区域
            Rectangle TitleRect = CompareTab.GetTabRect(e.Index);  //当前标题栏的范围
            Brush brBack, brText = new SolidBrush(Color.Black);// 背景刷子
            Color info = Color.FromArgb(0xE1, 0X80, 0X80, 0X70);

            switch (e.Index) // 不同的选项卡刷不同的背景色
            {
                case 0: brBack = new SolidBrush(info); break;
                case 1: brBack = new SolidBrush(info); break;
                default: brBack = new SolidBrush(info); break;
            }
            switch (CompareTab.Alignment)
            {
                case TabAlignment.Top:
                    HeaderBackRect = new Rectangle(new Point(endPageRect.X + endPageRect.Width, endPageRect.Y),
                        new Size(Width - endPageRect.X - endPageRect.Width, endPageRect.Height));
                    break;
                case TabAlignment.Bottom:
                    HeaderBackRect = new Rectangle(new Point(endPageRect.X + endPageRect.Width, endPageRect.Y),
                        new Size(Width - endPageRect.X - endPageRect.Width, endPageRect.Height));
                    break;
                case TabAlignment.Left:
                    HeaderBackRect = new Rectangle(new Point(endPageRect.X, endPageRect.Y + endPageRect.Height),
                        new Size(endPageRect.Width, Height - endPageRect.Y - endPageRect.Height));
                    break;
                case TabAlignment.Right:
                    HeaderBackRect = new Rectangle(new Point(endPageRect.X, endPageRect.Y + endPageRect.Height),
                        new Size(endPageRect.Width, Height - endPageRect.Y - endPageRect.Height));
                    break;
            }

            Font font = new Font("Verdana", 8F); // 字体
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Color fontcolor = Color.Black;

            if (this.CompareTab.SelectedIndex == e.Index)
            {
                g.DrawRectangle(new Pen(Color.Black), TitleRect);    //消除选中标题的矩形方框
                font = new Font("Verdana", 8F, FontStyle.Bold); // 字体
                fontcolor = Color.Blue;
            }
            Brush fontbrush = new SolidBrush(fontcolor);
            e.Graphics.FillRectangle(brBack, TitleRect); // 用指定的颜色填充选项卡矩形区域
            //绘制标题文本
            g.DrawString(CompareTab.TabPages[e.Index].Text, font, fontbrush, TitleRect, sf);

            //绘制背景
            if (HeaderBackRect != Rectangle.Empty)
            {
                Brush HeaderBackBrush = new LinearGradientBrush(HeaderBackRect,Color.SlateGray, Color.Black,30); 
                g.FillRectangle(HeaderBackBrush, HeaderBackRect);
            }
        }

        #region Button MouseOver事件

        private void OpenOldFileBtn_MouseMove(object sender, MouseEventArgs e)
        {
            this.OpenOldFileBtn.FlatStyle = FlatStyle.Popup;
        }

        private void OpenOldFileBtn_MouseLeave(object sender, EventArgs e)
        {
            this.OpenOldFileBtn.FlatStyle = FlatStyle.Flat; ;
        }

        private void UnzipOldPathBtn_MouseMove(object sender, MouseEventArgs e)
        {
            this.UnzipOldPathBtn.FlatStyle = FlatStyle.Popup;
        }

        private void UnzipOldPathBtn_ChangeUICues(object sender, UICuesEventArgs e)
        {
            this.UnzipOldPathBtn.FlatStyle = FlatStyle.Popup;
        }

        private void OpenNewFileBtn_MouseMove(object sender, MouseEventArgs e)
        {
            this.OpenNewFileBtn.FlatStyle = FlatStyle.Popup;
        }

        private void OpenNewFileBtn_MouseLeave(object sender, EventArgs e)
        {
            this.OpenNewFileBtn.FlatStyle = FlatStyle.Flat;
        }

        private void UnzipNewPathBtn_MouseMove(object sender, MouseEventArgs e)
        {
            this.UnzipNewPathBtn.FlatStyle = FlatStyle.Popup;
        }

        private void UnzipNewPathBtn_MouseLeave(object sender, EventArgs e)
        {
            this.UnzipNewPathBtn.FlatStyle = FlatStyle.Flat;
        }

        private void OpenOldXmlBtn_MouseMove(object sender, MouseEventArgs e)
        {
            this.OpenOldXmlBtn.FlatStyle = FlatStyle.Popup;           
        }

        private void OpenOldXmlBtn_MouseLeave(object sender, EventArgs e)
        {
            this.OpenOldXmlBtn.FlatStyle = FlatStyle.Flat;
        }

        private void OpenNewXmlBtn_MouseMove(object sender, MouseEventArgs e)
        {
            this.OpenNewXmlBtn.FlatStyle = FlatStyle.Popup;
        }

        private void OpenNewXmlBtn_MouseLeave(object sender, EventArgs e)
        {
            this.OpenNewXmlBtn.FlatStyle = FlatStyle.Flat;
        }

        private void OpenOldSLXmlBtn_MouseMove(object sender, MouseEventArgs e)
        {
            this.OpenOldSLXmlBtn.FlatStyle = FlatStyle.Popup;
        }

        private void OpenOldSLXmlBtn_MouseLeave(object sender, EventArgs e)
        {
            this.OpenOldSLXmlBtn.FlatStyle = FlatStyle.Flat;
        }

        private void OpenNewSLXmlPathBtn_MouseMove(object sender, MouseEventArgs e)
        {
            this.OpenNewSLXmlPathBtn.FlatStyle = FlatStyle.Popup;
        }

        private void OpenNewSLXmlPathBtn_MouseLeave(object sender, EventArgs e)
        {
            this.OpenNewSLXmlPathBtn.FlatStyle = FlatStyle.Flat;
        }

        private void OpenCombinXmlBtn_MouseMove(object sender, MouseEventArgs e)
        {
            this.OpenCombinXmlBtn.FlatStyle = FlatStyle.Popup;
        }

        private void OpenCombinXmlBtn_MouseLeave(object sender, EventArgs e)
        {
            this.OpenCombinXmlBtn.FlatStyle = FlatStyle.Flat;
        }

        private void OpenSLCombinXmlBtn_MouseMove(object sender, MouseEventArgs e)
        {
            this.OpenSLCombinXmlBtn.FlatStyle = FlatStyle.Popup;
        }

        private void OpenSLCombinXmlBtn_MouseLeave(object sender, EventArgs e)
        {
            this.OpenSLCombinXmlBtn.FlatStyle = FlatStyle.Flat;
        }

        private void SaveButton_MouseMove(object sender, MouseEventArgs e)
        {
            this.SaveButton.FlatStyle = FlatStyle.Popup;
        }

        private void SaveButton_MouseLeave(object sender, EventArgs e)
        {
            this.SaveButton.FlatStyle = FlatStyle.Flat;
        }

        private void QueryButton_MouseMove(object sender, MouseEventArgs e)
        {
            this.QueryButton.FlatStyle = FlatStyle.Popup;
        }

        private void QueryButton_MouseLeave(object sender, EventArgs e)
        {
            this.QueryButton.FlatStyle = FlatStyle.Flat;
        }

        private void GetDataButton_MouseMove(object sender, MouseEventArgs e)
        {
            this.GetDataButton.FlatStyle = FlatStyle.Popup;
        }

        private void GetDataButton_MouseLeave(object sender, EventArgs e)
        {
            this.GetDataButton.FlatStyle = FlatStyle.Flat;
        }
        private void SaveNewFileBtn_MouseMove(object sender, MouseEventArgs e)
        {
            this.SaveNewFileBtn.FlatStyle = FlatStyle.Popup;
        }

        private void SaveNewFileBtn_MouseLeave(object sender, EventArgs e)
        {
            this.SaveNewFileBtn.FlatStyle = FlatStyle.Flat;
        }

        private void CopyOldButton_MouseLeave(object sender, EventArgs e)
        {
            this.CopyOldButton.FlatStyle = FlatStyle.Flat;
        }

        private void CopyNewButton_MouseMove(object sender, MouseEventArgs e)
        {
            this.CopyNewButton.FlatStyle = FlatStyle.Popup;
        }

        private void CopyNewButton_MouseLeave(object sender, EventArgs e)
        {
            this.CopyNewButton.FlatStyle = FlatStyle.Flat;
        }
        #endregion

        private void ovalShape_Click(object sender, EventArgs e)
        {
            if (form.IsDisposed)
            {
                form = new SettingForm();
                _x = this.Location.X;
                _y = this.Location.Y;
                int temp = form.Location.X;
                if ((temp - _x) <= 30 && (temp - _x) >= 0)
                    form.Location = new Point(_x, _y);
                if (!_Start)
                    return;
                form.Location = new Point(_x, _y);
            }
            form.Show();
            form.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //初始化调用不规则窗体生成代码
            BitmapRegion BitmapRegion = new BitmapRegion();//此为生成不规则窗体和控件的类
           // BitmapRegion.CreateControlRegion(this, EJZLibrary.Properties.ResourceBackgroundImage.MainFormBackground_01);

            //磁性窗体的页面加载事件《下》；
            _x = this.Location.X + this.Width;      //变量_x等于窗体x轴大小；
            _y = this.Location.Y;                   //变量_y等于窗体y轴大小
        }

        private void ovalShape_MouseDown(object sender, MouseEventArgs e)
        {
            ovalShape.BackgroundImage = imageList1.Images[2];
        }

        private void ovalShape_MouseUp(object sender, MouseEventArgs e)
        {
            ovalShape.BackgroundImage = imageList1.Images[3];
        }

       //检查多语言重复项
        private bool CheckData(string filePath, string rootName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNodeList nodeList = doc.SelectSingleNode(rootName).ChildNodes;
            XmlNode nodeName;
            if (CheckXmlNode(nodeList, out nodeName))
            {
                MessageBox.Show("有重复节点:" +  nodeName.ParentNode.Name + "." + nodeName.Name, "警告!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CheckXmlNode(XmlNodeList nodeList,out XmlNode theSameNodeName)
        {
            bool returnValue = false;
            theSameNodeName = null;
            Dictionary<string,string> list = new Dictionary<string,string>();
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].ChildNodes.Count <= 1)
                {
                    if (!list.ContainsKey(nodeList[i].Name))
                    {
                        list.Add(nodeList[i].Name, nodeList[i].Name);
                    }
                    else
                    {
                        theSameNodeName = nodeList[i];
                        return true;
                    }
                }
                else
                {
                    returnValue = CheckXmlNode(nodeList[i].ChildNodes, out theSameNodeName);
                    if (returnValue) return true;
                }
            }
            return returnValue;
        }

        private void CheckDataButton_Click(object sender, EventArgs e)
        {
            this.CheckData(this._OldJavaXmlPath, "Languages");
            this.CheckData(this._NewJavaXmlPath, "Languages");
        }


        //****************************ServiceWsdl******************************
        private void OpenServiceWsdBtn_Click(object sender, EventArgs e)
        {
            this.OldServiceWsdCmb.Text = this.GetFileName();
        }
        private void OpenNewServiceWsdBtn_Click(object sender, EventArgs e)
        {
            this.NewServiceWsdCmb.Text = this.GetFileName();
        }
        private void OpenTempButton_Click(object sender, EventArgs e)
        {
            this.TemplatePathCmb.Text = this.GetFileName();
        }

        private void OpenSaveWsdlPathBtn_Click(object sender, EventArgs e)
        {
            this.SaveWsdlPathCmb.Text = this.GetFileName();
        }
        public string GetFileName()
        {
            string fileName = string.Empty;
            this.openFileDialog1.InitialDirectory = "c:\\";
            this.openFileDialog1.Filter = "All Files (*.*)|*.*";
            this.openFileDialog1.FilterIndex = 1;
            this.openFileDialog1.RestoreDirectory = true;

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = this.openFileDialog1.FileName;
            }
            return fileName;
        }
        //判断文件路径是否存在
       
        private void CompareButton_Click(object sender, EventArgs e)
        {
            int typeIndex = this.FileTypeComBox.SelectedIndex;
            if (typeIndex != 3 && typeIndex != 4)
            {
                MessageBox.Show("请选择要对比文件类型", "警告!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string parentFileName = PathManager.GetParentFolderName(typeIndex);
            string compareFileName = PathManager.GetCompareFileName(typeIndex);
            string sFile = this.OldServiceWsdCmb.Text.Trim();
            string dFile = this.NewServiceWsdCmb.Text.Trim();
            if (sFile.Contains("{0}"))
            {
                sFile = string.Format(this.OldServiceWsdCmb.Text, this.ChangeSetTextBox.Text, this.CompanyComBox.Text, parentFileName, compareFileName);
            }
            if (dFile.Contains("{0}"))
            {
                dFile = string.Format(this.NewServiceWsdCmb.Text, parentFileName, parentFileName, compareFileName);
            }
            if (!PathManager.ValidFileExist(sFile) || !PathManager.ValidFileExist(dFile)) return;
            
            this.Cursor = Cursors.WaitCursor;

            DiffList_TextFile sLF = null;
            DiffList_TextFile dLF = null;
            try
            {
                sLF = new DiffList_TextFile(sFile);
                dLF = new DiffList_TextFile(dFile);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "File Error");
                return;
            }

            try
            {
                double time = 0;
                DiffEngine de = new DiffEngine();
                time = de.ProcessDiff(sLF, dLF, DiffEngineLevel.FastImperfect);

                ArrayList rep = de.DiffReport();
                this.BindData(sLF, dLF, rep);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                string tmp = string.Format("{0}{1}{1}***STACK***{1}{2}",ex.Message, Environment.NewLine,ex.StackTrace);
                MessageBox.Show(tmp, "Compare Error");
                return;
            }
            this.Cursor = Cursors.Default;
        }

        public void BindData(DiffList_TextFile source, DiffList_TextFile destination, ArrayList DiffLines)
        {
            DataTable dt = XmlHelper.GetBindTable();

            int cnt = 1;
            int i;

            foreach (DiffResultSpan drs in DiffLines)
            {
                switch (drs.Status)
                {
                    case DiffResultSpanStatus.DeleteSource:
                        for (i = 0; i < drs.Length; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["OldKey"] = cnt.ToString("00000");
                            dr["NewKey"] = cnt.ToString("00000");
                            dr["OldValue"] = ((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line;
                            dr["NewValue"] = "";
                            dr["CombinKey"] = cnt.ToString("00000");

                            dr["CombinValue"] = this.DeleteCheckBox.Checked ? "":((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line;
                            dr["Status"] = "Delete";

                            dt.Rows.Add(dr);
                            cnt++;
                        }
                        break;
                    case DiffResultSpanStatus.NoChange:
                        for (i = 0; i < drs.Length; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["OldKey"] = cnt.ToString("00000");
                            dr["NewKey"] = cnt.ToString("00000");
                            dr["OldValue"] = ((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line;
                            dr["NewValue"] = ((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line;
                            dr["Status"] = "Unchanged";
                            dr["CombinKey"] = cnt.ToString("00000");
                            dr["CombinValue"] = ((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line;
                            dt.Rows.Add(dr);
                            cnt++;
                        }
                        break;
                    case DiffResultSpanStatus.AddDestination:
                        for (i = 0; i < drs.Length; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["OldKey"] = cnt.ToString("00000");
                            dr["NewKey"] = cnt.ToString("00000");
                            dr["OldValue"] = "";
                            dr["NewValue"] = ((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line;
                            dr["Status"] = "Add";
                            dr["CombinKey"] = cnt.ToString("00000");
                            dr["CombinValue"] = this.GetNewCheckBox.Checked ? ((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line: "";

                            dt.Rows.Add(dr);
                        }
                        break;
                    case DiffResultSpanStatus.Replace:
                        for (i = 0; i < drs.Length; i++)
                        {

                            DataRow dr = dt.NewRow();
                            dr["OldKey"] = cnt.ToString("00000");
                            dr["NewKey"] = cnt.ToString("00000");
                            dr["OldValue"] = ((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line;
                            dr["NewValue"] = ((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line;
                            dr["Status"] = "Modify";
                            dr["CombinKey"] = cnt.ToString("00000");
                            dr["CombinValue"] = this.ModifyCheckBox.Checked ? ((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line : ((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line;

                            dt.Rows.Add(dr);

                            cnt++;
                        }
                        break;
                }
                
            }
            GridControl.AddButtonColumn(this.WsdlGrid, this.IsAddButtonWsdlColumn);
            this.WsdlGrid.DataSource = dt;
            this.IsAddButtonWsdlColumn = true;
            GridControl.SetGridDifferBackColor(this.WsdlGrid);
            GridControl.HideRow(this.WsdlGrid, this.UnchangedCheckBox, this.NoContainModifyCheckBox);
            GridControl.SetWsdlGridColumnWidth(this.WsdlGrid);

            if (this.WsdlGrid.CurrentCell != null)
            {
                this.ShowOldTextBox.Text = dt.Rows[this.WsdlGrid.CurrentCell.RowIndex]["OldValue"].ToString();
                this.ShowNewTextBox.Text = dt.Rows[this.WsdlGrid.CurrentCell.RowIndex]["NewValue"].ToString();
            } 
        }
        
        private void CompareFrom_Click(object sender, EventArgs e)
        {
            int typeIndex = this.FileTypeComBox.SelectedIndex;
            if (typeIndex != 3 && typeIndex != 4)
            {
                MessageBox.Show("请选择要对比文件类型", "警告!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string parentFileName = PathManager.GetParentFolderName(typeIndex);
            string compareFileName = PathManager.GetCompareFileName(typeIndex);
            string sFile = this.OldServiceWsdCmb.Text.Trim();
            string dFile = this.NewServiceWsdCmb.Text.Trim();
            if (sFile.Contains("{0}"))
            {
                sFile = string.Format(this.OldServiceWsdCmb.Text, this.ChangeSetTextBox.Text, this.CompanyComBox.Text, parentFileName, compareFileName);
            }
            if (dFile.Contains("{0}"))
            {
                dFile = string.Format(this.NewServiceWsdCmb.Text, parentFileName, parentFileName, compareFileName);
            }
            if (!PathManager.ValidFileExist(sFile) || !PathManager.ValidFileExist(dFile)) return;

            this.Cursor = Cursors.WaitCursor;

            DiffList_TextFile sLF = null;
            DiffList_TextFile dLF = null;
            try
            {
                sLF = new DiffList_TextFile(sFile);
                dLF = new DiffList_TextFile(dFile);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "File Error");
                return;
            }

            try
            {
                double time = 0;
                DiffEngine de = new DiffEngine();
                time = de.ProcessDiff(sLF, dLF, DiffEngineLevel.FastImperfect);

                ArrayList rep = de.DiffReport();
                Results dlg = new Results(sLF, dLF, rep);
                dlg.ShowDialog();
                dlg.Dispose();
                //this.BindData(sLF, dLF, rep);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                string tmp = string.Format("{0}{1}{1}***STACK***{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace);
                MessageBox.Show(tmp, "Compare Error");
                return;
            }
            this.Cursor = Cursors.Default;
        }


        private void CompareNewButton_Click(object sender, EventArgs e)
        {
            
        }

        //
        private void WsdlGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int colIndex = e.ColumnIndex;
            if (rowIndex <0 || rowIndex == this.WsdlGrid.Rows.Count -1) return;

            DataTable dt = (DataTable)this.WsdlGrid.DataSource;
            DataRow row = dt.Rows[e.RowIndex];

            DataGridView dgv = (DataGridView)sender;

            //如果是"Button"列，按钮被点击
            if (dgv.Columns[e.ColumnIndex].Name == "NewButton" &&
                dgv.Rows[rowIndex].Cells[0].Style.BackColor != Color.SlateGray)
            {
                if (MessageBox.Show("您确定要获取新版本吗?", "系统提示!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                {
                    dgv.Rows[rowIndex].Cells[7].Value = dgv.Rows[rowIndex].Cells[5].Value;
                }
            }
            if (dgv.Columns[e.ColumnIndex].Name == "OldButton" &&
                dgv.Rows[rowIndex].Cells[0].Style.BackColor != Color.SlateGray)
            {
                if (MessageBox.Show("您确定要获取旧版本吗?", "系统提示!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                {
                    dgv.Rows[rowIndex].Cells[7].Value = dgv.Rows[rowIndex].Cells[3].Value;
                }
            }
        }

        private void WsdlGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                string value = this.WsdlGrid[e.ColumnIndex, e.RowIndex].Value.ToString();
                this._EditorGridForm = new EditorGrid(e.ColumnIndex, e.RowIndex, value);
                this._EditorGridForm.ChangedXml = new EditorGrid.ChangeXmlValueHandler(SettingCombinValueCallBack);
                this._EditorGridForm.Show();
            }
        }

        private void AddNodeApplicationBtn_Click(object sender, EventArgs e)
        {
            XmlHelper.GetAddNode(this.WsdlGrid, this.GetAddNodeChk);
        }

        private void ModifyKeepBtn_Click(object sender, EventArgs e)
        {
            XmlHelper.GetModifyNode(this.WsdlGrid, this.KeepModifyNodeChk);
        }

        private void DeleteNodeBtn_Click(object sender, EventArgs e)
        {
            XmlHelper.DeleteNode(this.WsdlGrid, this.DeleteChk);
        }

        private void WsdlQueryButton_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.WsdlGrid.DataSource;
            if (dt == null) return;
            string queryString = this.WsdlQueryTextBox.Text;
            GridQuery gridQuery = new GridQuery(this.WsdlGrid);
            gridQuery.QueryStr = queryString;
            gridQuery.QueryType = QueryType.Blur;
            gridQuery.Query();     
        }

        //保存Wsdl类型XML文件
        private void SaveNewFileBtn_Click(object sender, EventArgs e)
        {
            int typeIndex = this.FileTypeComBox.SelectedIndex;
            try
            {
                string tempPath = this.TemplatePathCmb.Text.ToString().Trim();//保存临时路径

                DataSet dataSet = XmlHelper.GetCombinDataSet(this.WsdlGrid);
                if (dataSet == null) return;
                // 创建XmlTextWriter类的实例对象
                XmlTextWriter textWriter = new XmlTextWriter(tempPath, Encoding.UTF8);
                textWriter.Formatting = Formatting.Indented;
                // 开始写过程，调用WriteStartDocument方法
                textWriter.WriteStartDocument();
                int i = 0;
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    if (i == 0)
                    {
                        i++;
                        continue;
                    }
                    textWriter.WriteRaw(row["CombinValue"].ToString());
                    i++;
                }
                textWriter.Close();

                //转换XML格式
                string savePath = this.SaveWsdlPathCmb.Text.ToString().Trim();

                string parentFileName = PathManager.GetParentFolderName(typeIndex);
                string combinFileName = PathManager.GetCompareFileName(typeIndex);
                if (savePath.Contains("{0}"))
                {
                    savePath = string.Format(savePath, parentFileName, parentFileName, combinFileName);
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(tempPath);
                StringBuilder sub = new StringBuilder();
                StringWriter sw = new StringWriter(sub);
                XmlTextWriter xw = new XmlTextWriter(sw);
                xw.Formatting = Formatting.Indented;
                doc.WriteTo(xw);
                doc.Save(savePath);
                MessageBox.Show("成功保存Wsdl.xml文件：路径：" + savePath, "XmlCompare", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string tmp = string.Format("{0}{1}{1}***STACK***{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace);
                MessageBox.Show(tmp, "Save Error");
                return;
            }
        }

        private void FileTypeComBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.FileTypeComBox.SelectedIndex;
            if (index == 3 || index == 4)
            {
                this.CompareTab.SelectedIndex = 3;
                this.GetDataButton.Enabled = false;
            }
            else
            {
                this.GetDataButton.Enabled = true;
            }
        }

        private void CompareTab_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void WsdlGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            DataGridView grid = sender as DataGridView;
            if (e.RowIndex == -1 || e.RowIndex == grid.Rows.Count - 1) return;
            this.CurrentLineLable.Text = "当前行：" + e.RowIndex.ToString();
            DataTable dr = (DataTable)grid.DataSource;
            this.ShowOldTextBox.Text = dr.Rows[e.RowIndex]["OldValue"].ToString();
            this.ShowNewTextBox.Text = dr.Rows[e.RowIndex]["NewValue"].ToString();
            if (dr.Rows[e.RowIndex]["OldValue"] != dr.Rows[e.RowIndex]["NewValue"])
            {
                GridControl.SetForeColor(this.ShowOldTextBox, true);
                GridControl.SetForeColor(this.ShowNewTextBox, true);
            }
            else
            {
                GridControl.SetForeColor(this.ShowOldTextBox, false);
                GridControl.SetForeColor(this.ShowNewTextBox, false);
            }
        }

        private void CopyOldButton_Click(object sender, EventArgs e)
        {
            DataTable dr = (DataTable)this.WsdlGrid.DataSource;
            if (dr == null) return;
            dr.Rows[WsdlGrid.CurrentCell.RowIndex]["CombinKey"] = dr.Rows[WsdlGrid.CurrentCell.RowIndex]["OldKey"];
            dr.Rows[WsdlGrid.CurrentCell.RowIndex]["CombinValue"]= this.ShowOldTextBox.Text;
        }

        private void CopyNewButton_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.WsdlGrid.DataSource;
            if (dt == null) return;
            dt.Rows[this.WsdlGrid.CurrentCell.RowIndex]["CombinKey"] = dt.Rows[this.WsdlGrid.CurrentCell.RowIndex]["NewKey"];
            dt.Rows[this.WsdlGrid.CurrentCell.RowIndex]["CombinValue"] = this.ShowNewTextBox.Text;
        }

        private void CopyOldButton_MouseMove(object sender, MouseEventArgs e)
        {
            this.CopyOldButton.FlatStyle = FlatStyle.Popup;
        }

        private void OldXmlGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            int rowIndex = e.RowIndex;
            if (rowIndex < 0 || rowIndex == grid.Rows.Count - 1) return;
            DataTable dt = (DataTable)this.OldXmlGrid.DataSource;

            this.CurrentOldXmlTextBox.Text = dt.Rows[rowIndex]["OldValue"].ToString();
            this.CurrentNewXmlText.Text = dt.Rows[rowIndex]["NewValue"].ToString();
            if (dt.Rows[e.RowIndex]["OldValue"] != dt.Rows[e.RowIndex]["NewValue"])
            {
                GridControl.SetForeColor(this.CurrentOldXmlTextBox, true);
                GridControl.SetForeColor(this.CurrentNewXmlText, true);
                this.SameOrDifferStatusText.Text = "Important Difference";
            }
            else
            {
                GridControl.SetForeColor(this.CurrentOldXmlTextBox, false);
                GridControl.SetForeColor(this.CurrentNewXmlText, false);
                this.SameOrDifferStatusText.Text = "Same";
            }
        }


        #region ....工具栏事件

        private void NextDifferButton_Click(object sender, EventArgs e)
        {
            if(this.CompareTab.SelectedIndex == 1)
            {
                GridControl.GetNextDifferSectionRow(this.OldXmlGrid);
                DataTable dr = (DataTable)this.OldXmlGrid.DataSource;
                this.CurrentOldXmlTextBox.Text = dr.Rows[this.OldXmlGrid.CurrentCell.RowIndex]["OldValue"].ToString();
                this.CurrentNewXmlText.Text = dr.Rows[this.OldXmlGrid.CurrentCell.RowIndex]["NewValue"].ToString();
            }
            if (this.CompareTab.SelectedIndex == 3)
            {
                GridControl.GetNextDifferSectionRow(this.WsdlGrid);
                DataTable dr = (DataTable)this.WsdlGrid.DataSource;
                this.ShowOldTextBox.Text = dr.Rows[this.WsdlGrid.CurrentCell.RowIndex]["OldValue"].ToString();
                this.ShowNewTextBox.Text = dr.Rows[this.WsdlGrid.CurrentCell.RowIndex]["NewValue"].ToString();
            }  
        }

        private void PreviouseDifferButton_Click(object sender, EventArgs e)
        {
            if (this.CompareTab.SelectedIndex == 1)
            {
                GridControl.GetPreviouseDifferRow(this.OldXmlGrid);
                DataTable dr = (DataTable)this.OldXmlGrid.DataSource;
                this.CurrentOldXmlTextBox.Text = dr.Rows[this.OldXmlGrid.CurrentCell.RowIndex]["OldValue"].ToString();
                this.CurrentNewXmlText.Text = dr.Rows[this.OldXmlGrid.CurrentCell.RowIndex]["NewValue"].ToString();
            }
            if (this.CompareTab.SelectedIndex == 3)
            {
                GridControl.GetPreviouseDifferRow(this.WsdlGrid);
                DataTable dr = (DataTable)this.WsdlGrid.DataSource;
                this.ShowOldTextBox.Text = dr.Rows[this.WsdlGrid.CurrentCell.RowIndex]["OldValue"].ToString();
                this.ShowNewTextBox.Text = dr.Rows[this.WsdlGrid.CurrentCell.RowIndex]["NewValue"].ToString();
            }
        }

        private void SameRowButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (this.CompareTab.SelectedIndex == 1)
            {
                GridControl.ShowSameItem(this.OldXmlGrid);
            }
            if (this.CompareTab.SelectedIndex == 3)
            {
                GridControl.ShowSameItem(this.WsdlGrid);
            }
            this.Cursor = Cursors.Default;
        }

        private void DifferRowButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (this.CompareTab.SelectedIndex == 1)
            {
                GridControl.ShowDifferItem(this.OldXmlGrid);
            }
            if (this.CompareTab.SelectedIndex == 3)
            {
                GridControl.ShowDifferItem(this.WsdlGrid);
            }
            this.Cursor = Cursors.Default;
        }

        private void ShowAllButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (this.CompareTab.SelectedIndex == 1)
            {
                GridControl.ShowAllRecord(this.OldXmlGrid);
            }
            if (this.CompareTab.SelectedIndex == 3)
            {
                GridControl.ShowAllRecord(this.WsdlGrid);
            }
            this.Cursor = Cursors.Default;
        }
        #endregion

        private void QueryLineButton_Click(object sender, EventArgs e)
        {
            QueryLine queryForm = new QueryLine();
            queryForm.QueryLineHand = new QueryLine.QueryLineHandler(this.QueryGridLineCallBack);
            queryForm.Show();
        }

        public void QueryGridLineCallBack(int rowIndex, int ColIndex)
        {
            
            if (this.CompareTab.SelectedIndex == 1)
            {
                if ((rowIndex < 0 || rowIndex > this.OldXmlGrid.Rows.Count - 1)
                    || (ColIndex < 0 || ColIndex > this.OldXmlGrid.Columns.Count - 1)) return;
                this.OldXmlGrid.CurrentCell = this.OldXmlGrid[ColIndex, rowIndex];
                DataTable dt = (DataTable)this.OldXmlGrid.DataSource;
                this.CurrentOldXmlTextBox.Text = dt.Rows[this.OldXmlGrid.CurrentCell.RowIndex]["OldValue"].ToString();
                this.CurrentNewXmlText.Text = dt.Rows[this.OldXmlGrid.CurrentCell.RowIndex]["NewValue"].ToString();
            }
            if (this.CompareTab.SelectedIndex == 3)
            {
                if ((rowIndex < 0 || rowIndex > this.WsdlGrid.Rows.Count - 1)
                    || (ColIndex < 0 || ColIndex > this.WsdlGrid.Columns.Count - 1)) return;
                this.WsdlGrid.CurrentCell = this.WsdlGrid[ColIndex, rowIndex];
                DataTable dt = (DataTable)this.WsdlGrid.DataSource;
                this.ShowOldTextBox.Text = dt.Rows[this.WsdlGrid.CurrentCell.RowIndex]["OldValue"].ToString();
                this.ShowNewTextBox.Text = dt.Rows[this.WsdlGrid.CurrentCell.RowIndex]["NewValue"].ToString();
            }
        }

        private void OldXmlGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid.CurrentCell != null)
            {
                int rowIndex = grid.CurrentCell.RowIndex;
                this.CurrentOldXmlTextBox.Text = grid[3, rowIndex].Value.ToString();
                this.CurrentNewXmlText.Text = grid[5, rowIndex].Value.ToString();
            }
        }

        private void WsdlGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid.CurrentCell != null)
            {
                int rowIndex = grid.CurrentCell.RowIndex;
                this.ShowOldTextBox.Text = grid[3, rowIndex].Value.ToString();
                this.ShowNewTextBox.Text = grid[5, rowIndex].Value.ToString();
            }
        }


        #region ....自动化保存

        private string GetJavaXmlFilePath(PathJoin pathJoin)
        {
            string oldFilePath = ConfigurationSettings.AppSettings["OldZipToPath"] + "\\{2}\\{3}\\{4}";
            string newFilePath = ConfigurationSettings.AppSettings["NewZipToPath"] + "\\{1}\\{2}\\{3}";
            oldFilePath = string.Format(oldFilePath, pathJoin.ChangeSet, pathJoin.Company,pathJoin.ParentFileName,pathJoin.LanguageCode,pathJoin.XmlFileName);
            newFilePath = string.Format(newFilePath, pathJoin.ParentFileName, pathJoin.ParentFileName, pathJoin.LanguageCode,pathJoin.XmlFileName);
            oldFilePath = oldFilePath.Replace("\\\\", "\\");
            newFilePath = newFilePath.Replace("\\\\", "\\");
            return oldFilePath + "|" + newFilePath;
        }
        private void AutoSaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.PublishChangeSetTextBox.Text))
            {
                MessageBox.Show("请输入要发布的版本号！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //定义压缩路径
            string inputPath = ConfigurationSettings.AppSettings["CompressInputPath"];
            string outputPath = ConfigurationSettings.AppSettings["CompressOutPutPath"];
            this._HaveCompareFiles.Clear();
            //第一步.解压新旧文件
            List<string> zipNames = new List<string>() { "TradingConsole.Language.jar", "TradingConsole.Configuration.jar" };
            List<string> configFiles = new List<string>() { "TradingConsole.config", "Settings.xml"};
            if (AutoSaveHelper.UnZipOldFile(this.ChangeSetTextBox.Text, this.CompanyComBox.Text, zipNames))
            {
                MessageBox.Show("解压旧文件成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            if (AutoSaveHelper.UnZipNewFile(this.ChangeSetTextBox.Text, this.CompanyComBox.Text, zipNames))
            {
                MessageBox.Show("解压新文件成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            //第二步：Xml文件处理---JAVA多语言文件处理
            this.CompareTab.SelectedIndex = 1;
            if (MessageBox.Show("是否接续？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Cancel) return;
            string companies = ConfigurationSettings.AppSettings["CompanyList"];
            string[] companyLanguageArray = companies.Split('|');

            //第三步：对公司循环
            for (int i = 0; i < companyLanguageArray.Length; i++)
            {
                string companyName = companyLanguageArray[i].Split(';')[0].Trim();
                //test
                if (i >= 1) return;
                string[] languageArray = companyLanguageArray[i].Split(';')[1].Split(',');
                //对语言进行循环
                for (int j = 0; j < languageArray.Length; j++)
                {
                    bool isHasModifyRecord = false;
                    bool isHasAddRecord = false;
                    //获取文件路径
                    PathJoin pathJoin = new PathJoin();
                    pathJoin.ChangeSet = this.ChangeSetTextBox.Text;
                    pathJoin.Company = companyName;
                    pathJoin.LanguageCode = languageArray[j];
                    pathJoin.ParentFileName = "Language";
                    pathJoin.XmlFileName = "Default.xml";
                    string currentFileFullName = pathJoin.Company + pathJoin.ParentFileName + pathJoin.LanguageCode + pathJoin.XmlFileName;
                    string oldFilePath = this.GetJavaXmlFilePath(pathJoin).Split('|')[0];
                    string newFilePath = this.GetJavaXmlFilePath(pathJoin).Split('|')[1];
                    //导入xml数据
                    this.ExportJavaData(oldFilePath, newFilePath, out isHasAddRecord, out isHasModifyRecord);

                    if (isHasAddRecord || isHasModifyRecord)
                    {
                        if (MessageBox.Show("文件有新增或者修改记录,是否手动编辑？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                        {
                            this._HaveCompareFiles.Add(currentFileFullName);
                            return; 
                        }
                        //开始保存文件
                        this.SaveJava(newFilePath, "Languages");
                        this._HaveCompareFiles.Add(currentFileFullName);
                    }     
                }

                //循环配置jar文件
                foreach (string fileName in configFiles)
                {
                    bool isHasModifyRecord = false;
                    bool isHasAddRecord = false;
                    //获取文件路径
                    PathJoin pathJoin = new PathJoin();
                    pathJoin.ChangeSet = this.ChangeSetTextBox.Text;
                    pathJoin.Company = companyName;
                    pathJoin.LanguageCode = null;
                    pathJoin.ParentFileName = "Configuration";
                    pathJoin.XmlFileName = fileName;
                    string currentFileFullName = pathJoin.Company + pathJoin.ParentFileName + pathJoin.LanguageCode + pathJoin.XmlFileName;
                    string oldFilePath = this.GetJavaXmlFilePath(pathJoin).Split('|')[0];
                    string newFilePath = this.GetJavaXmlFilePath(pathJoin).Split('|')[1];
                    //导入xml数据
                    this.ExportJavaData(oldFilePath, newFilePath, out isHasAddRecord, out isHasModifyRecord);

                    if (isHasAddRecord || isHasModifyRecord)
                    {
                        if (MessageBox.Show("文件有新增或者修改记录,是否手动编辑？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                        {
                            this._HaveCompareFiles.Add(currentFileFullName);
                            return;
                        } 
                    }
                    //开始保存文件
                    this.SaveJava(newFilePath, "configuration");
                    this._HaveCompareFiles.Add(currentFileFullName);
                }


                //对该公司文件进行压缩

                this.CompressZip(string.Format(inputPath, "Language"),
                    string.Format(outputPath, this.PublishChangeSetTextBox.Text, companyName, "TradingConsole.Language.jar"), "TradingConsole.Language.jar");
                this.CompressZip(string.Format(inputPath, "Configuration"),
                    string.Format(outputPath, this.PublishChangeSetTextBox.Text, companyName, "TradingConsole.Configuration.jar"), "TradingConsole.Configuration.jar");
            } 
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            string copyFromPath = ConfigurationSettings.AppSettings["CopyFromPath"];
            string copyToPath = ConfigurationSettings.AppSettings["CopyToPath"];
            List<string> companyList = new List<string>() { "ACMTrader", "AG", "Boxim" };
            if (string.IsNullOrEmpty(this.PublishChangeSetTextBox.Text))
            {
                MessageBox.Show("请输入发布的版本号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                foreach (string company in companyList)
                {
                    copyFromPath = string.Format(copyFromPath, this.ChangeSetTextBox.Text);
                    copyToPath = string.Format(copyToPath, this.PublishChangeSetTextBox.Text);
                }
                CopyHelper.CopyAndReplace(copyFromPath, copyToPath, null, false);
                MessageBox.Show("复制文件成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                throw new Exception();
            }
            
        }

        //继续执行自动保存
        private void ContinueAutoSave()
        {
              //定义压缩路径
            string inputPath = ConfigurationSettings.AppSettings["CompressInputPath"];
            string outputPath = ConfigurationSettings.AppSettings["CompressOutPutPath"];
            //Xml文件处理---JAVA多语言文件处理
            this.CompareTab.SelectedIndex = 1;
            string companies = ConfigurationSettings.AppSettings["CompanyList"];
            List<string> configFiles = new List<string>() { "TradingConsole.config", "Settings.xml" };
            List<string> CompanyList = new List<string>();
            List<string> LanguageList = new List<string>();
            string[] companyLanguageArray = companies.Split('|');
            //第三步：对公司循环
            for (int i = 0; i < companyLanguageArray.Length; i++)
            {
                string companyName = companyLanguageArray[i].Split(';')[0].Trim();
                //test
                if (i >= 1) return;
                string[] languageArray = companyLanguageArray[i].Split(';')[1].Split(',');
                //对语言进行循环
                for (int j = 0; j < languageArray.Length; j++)
                {
                    bool isHasModifyRecord = false;
                    bool isHasAddRecord = false;
                    //获取文件路径
                    PathJoin pathJoin = new PathJoin();
                    pathJoin.ChangeSet = this.ChangeSetTextBox.Text;
                    pathJoin.Company = companyName;
                    pathJoin.LanguageCode = languageArray[j];
                    pathJoin.ParentFileName = "Language";
                    pathJoin.XmlFileName = "Default.xml";
                    string currentFileFullName = pathJoin.Company + pathJoin.ParentFileName + pathJoin.LanguageCode + pathJoin.XmlFileName;

                    if (this._HaveCompareFiles.Contains(currentFileFullName)) continue;
                    string oldFilePath = this.GetJavaXmlFilePath(pathJoin).Split('|')[0];
                    string newFilePath = this.GetJavaXmlFilePath(pathJoin).Split('|')[1];
                    //导入xml数据
                    this.ExportJavaData(oldFilePath, newFilePath, out isHasAddRecord, out isHasModifyRecord);

                    if (isHasAddRecord || isHasModifyRecord)
                    {
                        if (MessageBox.Show("文件有新增或者修改记录,是否手动编辑？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                        {
                            this._HaveCompareFiles.Add(currentFileFullName);
                            return;
                        }
                        //开始保存文件
                        this.SaveJava(newFilePath, "Languages");
                        this._HaveCompareFiles.Add(currentFileFullName);
                    }
                }

                //循环配置jar文件
                foreach (string fileName in configFiles)
                {
                    bool isHasModifyRecord = false;
                    bool isHasAddRecord = false;
                    //获取文件路径
                    PathJoin pathJoin = new PathJoin();
                    pathJoin.ChangeSet = this.ChangeSetTextBox.Text;
                    pathJoin.Company = companyName;
                    pathJoin.LanguageCode = null;
                    pathJoin.ParentFileName = "Configuration";
                    pathJoin.XmlFileName = fileName;
                    string currentFileFullName = pathJoin.Company + pathJoin.ParentFileName + pathJoin.LanguageCode + pathJoin.XmlFileName;
                    if (this._HaveCompareFiles.Contains(currentFileFullName)) continue;
                    string oldFilePath = this.GetJavaXmlFilePath(pathJoin).Split('|')[0];
                    string newFilePath = this.GetJavaXmlFilePath(pathJoin).Split('|')[1];
                    //导入xml数据
                    this.ExportJavaData(oldFilePath, newFilePath, out isHasAddRecord, out isHasModifyRecord);

                    if (isHasAddRecord || isHasModifyRecord)
                    {
                        if (MessageBox.Show("文件有新增或者修改记录,是否手动编辑？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                        {
                            this._HaveCompareFiles.Add(currentFileFullName);
                            return;
                        }
                    }
                    //开始保存文件
                    this.SaveJava(newFilePath, "configuration");
                    this._HaveCompareFiles.Add(currentFileFullName);
                         //对该公司文件进行压缩

                this.CompressZip(string.Format(inputPath, "Language"),
                    string.Format(outputPath, this.PublishChangeSetTextBox.Text, companyName, "TradingConsole.Language.jar"), "TradingConsole.Language.jar");
                this.CompressZip(string.Format(inputPath, "Configuration"),
                    string.Format(outputPath, this.PublishChangeSetTextBox.Text, companyName, "TradingConsole.Configuration.jar"), "TradingConsole.Configuration.jar");
                }
            } 
        }

        private void CompressZip(string inputPath,string outputPath,string zipFileName)
        {
            int intZipLevel = 2;
            string strPassword = "";

            DirectoryInfo TheFolder = new DirectoryInfo(inputPath);
            DirectoryInfo PublishFolder = new DirectoryInfo(outputPath);
            List<string> fileNameList = new List<string>();
            //创建发布路径（如果不存在）
            if (!PublishFolder.Parent.Exists)
            {
                this.CreateFileFolder(outputPath.Substring(0, outputPath.Length - zipFileName.Length));
            }
            //遍历文件夹
            if (!TheFolder.Exists)
            {
                this.CreateFileFolder(inputPath);
                TheFolder = new DirectoryInfo(inputPath);
            }
            if (TheFolder.GetDirectories().Length == 0
                && TheFolder.GetFiles().Length == 0)
            {
                MessageBox.Show("要压缩的文件夹:" + this._CompressInputPath + "没有文件！", "警告!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {
                fileNameList.Add(Path.Combine(inputPath, NextFolder.Name));
            }

            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                fileNameList.Add(Path.Combine(inputPath, NextFile.Name));
            }

            string[] filesOrDirectoriesPaths = new string[fileNameList.Count];
            filesOrDirectoriesPaths = fileNameList.ToArray();
            if (ZipFile.Zip(outputPath, inputPath, intZipLevel, strPassword, filesOrDirectoriesPaths))
            {
                MessageBox.Show("压缩文件成功! 输出路径：" + outputPath, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
        }

        private void NextAutoBtn_Click(object sender, EventArgs e)
        {
            ContinueAutoSave();
        }



       #endregion














    }
}
