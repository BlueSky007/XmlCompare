using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace XmlCompare
{
    public class XmlHelper
    {
        public void UnZip(string[] args)
        {
            ZipInputStream s = new ZipInputStream(File.OpenRead(args[0]));

            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {

                string directoryName = Path.GetDirectoryName(args[1]);
                string fileName = Path.GetFileName(theEntry.Name);

                //生成解压目录
                Directory.CreateDirectory(directoryName);

                if (fileName != String.Empty)
                {
                    //解压文件到指定的目录
                    FileStream streamWriter = File.Create(args[1] + theEntry.Name);
                    //FileStream streamWriter = File.Create(args[1]);

                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }

                    streamWriter.Close();
                }
            }
            s.Close();
        }

        public static void GetAddNode(DataGridView grid,CheckBox chk)
        {
            DataTable dt = (DataTable)grid.DataSource;
            if (dt == null) return;
            foreach (DataRow row in dt.Rows)
            {
                if (row["Status"].ToString() == "Add")
                {
                    if (chk.Checked)
                    {
                        row["CombinKey"] = row["NewKey"];
                        row["CombinValue"] = row["NewValue"];
                    }
                    else
                    {
                        row["CombinKey"] = row["OldKey"];
                        row["CombinValue"] = row["OldValue"];
                    }
                }
            }
        }

        public static void GetModifyNode(DataGridView grid,CheckBox chk)
        {
            DataTable dt = (DataTable)grid.DataSource;
            if (dt == null) return;
            foreach (DataRow row in dt.Rows)
            {
                if (row["Status"].ToString() == "Modify")
                {
                    if (chk.Checked)
                    {
                        row["CombinKey"] = row["OldKey"];
                        row["CombinValue"] = row["OldValue"];
                    }
                    else
                    {
                        row["CombinKey"] = row["NewKey"];
                        row["CombinValue"] = row["NewValue"];
                    }
                }
            }
        }

        public static void DeleteNode(DataGridView grid, CheckBox chk)
        {
            DataTable dt = (DataTable)grid.DataSource;
            if (dt == null) return;
            foreach (DataRow row in dt.Rows)
            {
                if (row["Status"].ToString() == "Delete")
                {
                    if (chk.Checked)
                    {
                        row["CombinKey"] = row["NewKey"];
                        row["CombinValue"] = row["NewValue"];
                    }
                    else
                    {
                        row["CombinKey"] = row["OldKey"];
                        row["CombinValue"] = row["OldValue"];
                    }
                }
            }
        }

        public static DataTable GetBindTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("OldKey", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("OldValue", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("NewKey", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("NewValue", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CombinKey", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CombinValue", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Status", Type.GetType("System.String")));
            dt.Columns["OldKey"].ReadOnly = true;
            dt.Columns["OldValue"].ReadOnly = true;
            dt.Columns["NewKey"].ReadOnly = true;
            dt.Columns["NewValue"].ReadOnly = true;
            dt.Columns["Status"].ReadOnly = true;
            return dt;
        }

        public static Dictionary<string, string> GetConverterDictionary(DataSet dataSet)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (DataTable dt in dataSet.Tables)
            {
                string preNodeString = GetPreString(dataSet, dt);
                foreach (DataColumn column in dt.Columns)
                {
                    if (!column.ColumnName.Contains("_Id"))
                    {
                        string value = dt.Rows[0][column.ColumnName].ToString();
                        string key = string.IsNullOrEmpty(preNodeString) ? column.ColumnName : preNodeString + "." + column.ColumnName;
                        dictionary.Add(key, value);
                    }
                }
            }
            return dictionary;
        }

        public static string GetPreString(DataSet dataSet, DataTable dt)
        {
            string returnString = dt.TableName;
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName.Contains("_Id")
                    && column.ColumnName != dt.TableName + "_Id")
                {
                    returnString = column.ColumnName.Substring(0, column.ColumnName.Length - 3) + "." + returnString;
                    foreach (DataTable ds in dataSet.Tables)
                    {
                        if (column.ColumnName.Substring(0, column.ColumnName.Length - 3) == ds.TableName)
                        {
                            if (GetParentNode(dataSet, ds) != "")
                            {
                                returnString = GetParentNode(dataSet, ds) + "." + returnString;
                            }
                        }
                    }
                }
            }
            return returnString;
        }

        public static string GetParentNode(DataSet dataSet, DataTable dt)
        {
            string returnString = "";
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName.Contains("_Id")
                    && column.ColumnName != dt.TableName + "_Id")
                {
                    returnString = column.ColumnName.Substring(0, column.ColumnName.Length - 3);
                }
            }
            return returnString;
        }

        public static void CreateXml(XmlDocument doc, XmlElement root, string[] nodeList, string caption)
        {
            if (caption == "") caption = "    ";
            XmlElement parentNode = root;

            try
            {
                for (int i = 0; i < nodeList.Length; i++)
                {
                    string nodeName = nodeList[i];
                    if (parentNode.SelectSingleNode(nodeName) == null)
                    {
                        XmlElement newNode = doc.CreateElement(nodeName);
                        if (i == nodeList.Length - 1)
                        {
                            newNode.InnerText = caption;
                        }
                        parentNode.AppendChild(newNode);
                        parentNode = newNode;
                    }
                    else
                    {
                        parentNode = (XmlElement)parentNode.SelectSingleNode(nodeName);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public static DataSet GetCombinDataSet(DataGridView grid)
        {
            DataSet dataSet = new DataSet();
            dataSet.DataSetName = "Resourceses";
            DataTable dt = (DataTable)grid.DataSource;
            if (dt == null) return null;
            DataTable combinTable = dt.Copy();
            combinTable.TableName = "Resourcese";

            combinTable.Columns.Remove("OldKey");
            combinTable.Columns.Remove("OldValue");
            combinTable.Columns.Remove("NewKey");
            combinTable.Columns.Remove("NewValue");
            combinTable.Columns.Remove("Status");

            dataSet.Tables.Add(combinTable.Copy());

            return dataSet;
        }

        

    }
}
