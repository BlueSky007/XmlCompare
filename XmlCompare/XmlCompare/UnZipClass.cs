using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace XmlCompare
{
    public class UnZipClass
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

        //方法二：
        public static bool UnZipFile(string zipFilePath, string unZipDir, out string err)
        {
            err = "";
            if (zipFilePath == string.Empty)
            {
                err = "压缩文件不能为空！";
                return false;
            }
            if (!File.Exists(zipFilePath))
            {
                err = "压缩文件不存在！";
                return false;
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("//"))
                unZipDir += "//";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {

                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(unZipDir + directoryName);
                        }
                        if (!directoryName.EndsWith("//"))
                            directoryName += "//";
                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                            {

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
                            }
                        }
                    }//while
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return false;
            }
            return true;
        }
    }
}
