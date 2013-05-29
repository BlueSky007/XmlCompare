using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XmlCompare
{
    //最好能用到正则表达式来代换
    //ReplaceTextHelper类的编写
    public sealed class ReplaceTextHelper
    {
        private ReplaceTextHelper()
        {
        }

        public static void ReplaceAllFiles(string folder, string replaceWithText)
        {
            DirectoryInfo info = new DirectoryInfo(folder);
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                if (fsi is System.IO.FileInfo)
                {
                    string fileName = fsi.FullName;
                    if (fileName.EndsWith(".bat", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ReplaceTextHelper.Replace(fileName, replaceWithText);
                    }
                }
            }
        }

        public static void Replace(string fileName, string replaceWithText)
        {
            FileAttributes fileAttributes = File.GetAttributes(fileName);
            if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(fileName, FileAttributes.Normal);
            }

            StreamReader sr = new StreamReader(fileName);
            string text = sr.ReadToEnd();
            sr.Close();
            text = string.Format(text, replaceWithText);
            //text = Regex.Replace(text, replacedText, replaceWithText, RegexOptions.IgnoreCase);
            StreamWriter sw = new StreamWriter(fileName);
            sw.WriteLine(text);
            sw.Close();

            File.SetAttributes(fileName, fileAttributes);
        }
    }
}
