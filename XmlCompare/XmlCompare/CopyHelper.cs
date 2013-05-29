using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XmlCompare
{
    //Copy类的编写
    public sealed class CopyHelper
    {
        private CopyHelper()
        {
        }

        public static void Copy(string source, string destination)
        {
            CopyHelper.CopyProcess(source, destination, false, "", false);
        }

        public static void CopyAndReplace(string source, string destination, string replaceWithText, bool isWriteCopyLog)
        {
            CopyHelper.CopyProcess(source, destination, true, replaceWithText, false);
        }

        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static void CopyProcess(string source, string destination, bool needReplace, string replaceWithText, bool isWriteCopyLog)//source 是原文件夹的路径,destination是要复制到的目标文件夹路径
        {
            //如果源文件夹不存在，则创建
            CopyHelper.CreateDirectory(destination);

            //调用文件夹复制函数
            CopyHelper.CopyDirectoryAndReplaceBatFileText(source, destination, needReplace, replaceWithText);

            if (isWriteCopyLog)
            {
                //将原文件夹里的内容写进目标文件夹下的"记录.txt"里
                StreamWriter writer = new StreamWriter(destination + "\\CopyInfo.txt");
                writer.WriteLine("From: " + source);
                writer.WriteLine("TO   : " + destination);
                writer.WriteLine();
                //Directory.GetFileSystemEntries() 可以得到指定目录包含子文件夹以及文件名字的字符串数组
                string[] names = Directory.GetFileSystemEntries(source);
                //历遍names数组,把原文件那些名字全写进去
                foreach (string name in names)
                {
                    writer.WriteLine(name);
                }
                writer.Close();
            }
        }

        //文件夹复制函数编写
        private static void CopyDirectoryAndReplaceBatFileText(string source, string destination, bool needReplace, string replaceWithText)
        {
            DirectoryInfo info = new DirectoryInfo(source);
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                //目标路径destName = 目标文件夹路径 + 原文件夹下的子文件(或文件夹)名字
                //Path.Combine(string a ,string b) 为合并两个字符串
                String destName = Path.Combine(destination, fsi.Name);
                //如果是文件类,就复制文件
                if (fsi is System.IO.FileInfo)
                {
                    if (File.Exists(destName))
                    {
                        FileAttributes fileAttributes = File.GetAttributes(destName);
                        if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                        {
                            File.SetAttributes(destName, FileAttributes.Normal);
                        }
                    }
                    File.Copy(fsi.FullName, destName, true);
                }
                else //如果不是 则为文件夹,继续调用文件夹复制函数,递归
                {
                    Directory.CreateDirectory(destName);
                    CopyDirectoryAndReplaceBatFileText(fsi.FullName, destName, needReplace, replaceWithText);
                }
            }
        }
    }
}
