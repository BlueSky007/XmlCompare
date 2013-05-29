using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace XmlCompare
{
    class ZipFile
    {
        /// <summary>
        /// 生成压缩文件
        /// </summary>
        /// <param name="strZipPath">生成的zip文件的路径</param>
        /// <param name="strZipTopDirectoryPath">源文件的上级目录</param>
        /// <param name="intZipLevel">T压缩等级</param>
        /// <param name="strPassword">压缩包解压密码</param>
        /// <param name="filesOrDirectoriesPaths">源文件路径</param>
        /// <returns></returns>
        public static bool Zip(string strZipPath, string strZipTopDirectoryPath, int intZipLevel, string strPassword, string[] filesOrDirectoriesPaths)
        {
            try
            {
                List<string> AllFilesPath = new List<string>();
                if (filesOrDirectoriesPaths.Length > 0) // get all files path
                {
                    for (int i = 0; i < filesOrDirectoriesPaths.Length; i++)
                    {
                        if (File.Exists(filesOrDirectoriesPaths[i]))
                        {
                            AllFilesPath.Add(filesOrDirectoriesPaths[i]);
                        }
                        else if (Directory.Exists(filesOrDirectoriesPaths[i]))
                        {
                            GetDirectoryFiles(filesOrDirectoriesPaths[i], AllFilesPath);
                        }
                    }
                }

                if (AllFilesPath.Count > 0)
                {

                    ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(strZipPath));
                    zipOutputStream.SetLevel(intZipLevel);
                    zipOutputStream.Password = strPassword;

                    for (int i = 0; i < AllFilesPath.Count; i++)
                    {
                        string strFile = AllFilesPath[i].ToString();
                        try
                        {
                            if (strFile.Substring(strFile.Length - 1) == "") //folder
                            {
                                string strFileName = strFile.Replace(strZipTopDirectoryPath, "");
                                if (strFileName.StartsWith(""))
                                {
                                    strFileName = strFileName.Substring(1);
                                }
                                ZipEntry entry = new ZipEntry(strFileName);
                                entry.DateTime = DateTime.Now;
                                zipOutputStream.PutNextEntry(entry);
                            }
                            else //file
                            {
                                FileStream fs = File.OpenRead(strFile);

                                byte[] buffer = new byte[fs.Length];
                                fs.Read(buffer, 0, buffer.Length);

                                string strFileName = strFile.Replace(strZipTopDirectoryPath, "");
                                if (strFileName.StartsWith(""))
                                {
                                    strFileName = strFileName.Substring(0);
                                }
                                ZipEntry entry = new ZipEntry(strFileName);
                                entry.DateTime = DateTime.Now;
                                zipOutputStream.PutNextEntry(entry);
                                zipOutputStream.Write(buffer, 0, buffer.Length);

                                fs.Close();
                                fs.Dispose();
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    zipOutputStream.Finish();
                    zipOutputStream.Close();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the directory files.
        /// </summary>
        /// <param name="strParentDirectoryPath">源文件路径</param>
        /// <param name="AllFilesPath">所有文件路径</param>
        private static void GetDirectoryFiles(string strParentDirectoryPath, List<string> AllFilesPath)
        {
            string[] files = Directory.GetFiles(strParentDirectoryPath);
            for (int i = 0; i < files.Length; i++)
            {
                AllFilesPath.Add(files[i]);
            }
            string[] directorys = Directory.GetDirectories(strParentDirectoryPath);
            for (int i = 0; i < directorys.Length; i++)
            {
                GetDirectoryFiles(directorys[i], AllFilesPath);
            }
            if (files.Length == 0 && directorys.Length == 0) //empty folder
            {
                AllFilesPath.Add(strParentDirectoryPath);
            }
        }


    }

    class ZipClass
    {
        public void ZipFile(string FileToZip, string ZipedFile, int CompressionLevel, int BlockSize)
        {
            //如果文件没有找到，则报错
            if (!System.IO.File.Exists(FileToZip))
            {
                throw new System.IO.FileNotFoundException("The specified file " + FileToZip + " could not be found. Zipping aborderd");
            }

            System.IO.FileStream StreamToZip = new System.IO.FileStream(FileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.FileStream ZipFile = System.IO.File.Create(ZipedFile);
            ZipOutputStream ZipStream = new ZipOutputStream(ZipFile);
            ZipEntry ZipEntry = new ZipEntry("ZippedFile");
            ZipStream.PutNextEntry(ZipEntry);
            ZipStream.SetLevel(CompressionLevel);
            byte[] buffer = new byte[BlockSize];
            System.Int32 size = StreamToZip.Read(buffer, 0, buffer.Length);
            ZipStream.Write(buffer, 0, size);
            try
            {
                while (size < StreamToZip.Length)
                {
                    int sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                    ZipStream.Write(buffer, 0, sizeRead);
                    size += sizeRead;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            ZipStream.Finish();
            ZipStream.Close();
            StreamToZip.Close();
        }

        public void ZipFileMain(string[] args)
        {
            string[] filenames = Directory.GetFiles(args[0]);

            Crc32 crc = new Crc32();
            ZipOutputStream s = new ZipOutputStream(File.Create(args[1]));

            s.SetLevel(6); // 0 - store only to 9 - means best compression

            foreach (string file in filenames)
            {
                //打开压缩文件
                FileStream fs = File.OpenRead(file);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                ZipEntry entry = new ZipEntry(file);

                entry.DateTime = DateTime.Now;

                // set Size and the crc, because the information
                // about the size and crc should be stored in the header
                // if it is not set it is automatically written in the footer.
                // (in this case size == crc == -1 in the header)
                // Some ZIP programs have problems with zip files that don't store
                // the size and crc in the header.
                entry.Size = fs.Length;
                fs.Close();

                crc.Reset();
                crc.Update(buffer);

                entry.Crc = crc.Value;

                s.PutNextEntry(entry);

                s.Write(buffer, 0, buffer.Length);

            }

            s.Finish();
            s.Close();
        }
    }
}

