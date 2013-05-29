using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace XmlCompare
{
    public class PathManager
    {
        //获取压缩包文件名
        public static string GetZipFileName(int index)
        {
            if (index == 0)
            {
                return "TradingConsole.Language.jar";
            }
            else
            {
                return "TradingConsole.Configuration.jar";
            }
        }

        //获取Xml文件根节点名称
        public static string GetXmlRootName(int index)
        {
            if (index == 0)
            {
                return "Languages"; 
            }
            else if (index == 1)
            {
                return "configuration"; 
            }
            else if (index == 2)
            {
                return "Settings";
            }
            return null;
        }

        //获取zip包输入路径的父文件夹名称
        public static string GetParentFolderName(int index)
        {
            if (index == 0)
            {
                return "Language";
            }
            else if (index == 1)
            {
                return "Configuration";
            }
            else if (index == 2)
            {
                return "Configuration";
            }
            else if (index == 3)
            {
                return "Configuration";
            }
            else if (index == 4)
            {
                return "Configuration";
            }
            return null;
        }

        public static string GetCompareFileName(int index)
        {
            if (index == 0)
            {
                return "Default.xml";
            }
            else if (index == 1)
            {
                return "TradingConsole.config";
            }
            else if (index == 2)
            {
                return "Settings.xml";
            }
            else if (index == 3)
            {
                return "ServiceWsdl.xml";
            }
            else if (index == 4)
            {
                return "AuthenticationWsdl.xml";
            }
            return null;
        }

        //判断文件路径
        public static bool ValidFileExist(string path)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("Xml文件不存在！" + path, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        //获取silverlight多语言名
        public static string GetSLLanguage(int index)
        {
            if (index == 0)
            {
                return "en-US";
            }
            else if (index == 1)
            {
                return "zh-Hans";
            }
            else
            {
                return "zh-Hant";
            }
        }

        public static void CreateFileFolder(string path)
        {
            string[] PathList = path.Split('\\');
            string FilePath = PathList[0] + "\\";
            for (int i = 1; i < PathList.Length; i++)
            {
                FilePath = Path.Combine(FilePath, PathList[i]);
                DirectoryInfo TheFolder = new DirectoryInfo(FilePath);
                if (!TheFolder.Exists)
                {
                    TheFolder.Create();
                }
            }
        }

    }
}
