using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlCompare
{
    public class AutoSaveHelper
    {
        //解压
        public static bool UnZipOldFile(string oldChangeSet,string companyName,List<string> zipNames)
        {
            string err;
            foreach(string zipName in zipNames)
            {
                string preZipPath = System.Configuration.ConfigurationSettings.AppSettings["OldZipFromPath"];
                string UnZipPath = System.Configuration.ConfigurationSettings.AppSettings["OldZipToPath"];
                preZipPath = string.Format(preZipPath, oldChangeSet, companyName, zipName);
                UnZipPath = string.Format(UnZipPath, oldChangeSet, companyName);
                if (!PathManager.ValidFileExist(preZipPath)) return false;
                UnZipClass.UnZipFile(preZipPath, UnZipPath, out err);
            }
            return true;
        }

        public static bool UnZipNewFile(string oldChangeSet, string companyName, List<string> zipNames)
        {
            string err;
            foreach (string zipName in zipNames)
            {
                string preZipPath = System.Configuration.ConfigurationSettings.AppSettings["NewZipFromPath"];
                string UnZipPath = System.Configuration.ConfigurationSettings.AppSettings["NewZipToPath"];
                preZipPath = string.Format(preZipPath, zipName);
                UnZipPath = string.Format(UnZipPath, zipName.Split('.')[1]);
                if (!PathManager.ValidFileExist(preZipPath)) return false;

                PathManager.CreateFileFolder(UnZipPath);
                UnZipClass.UnZipFile(preZipPath, UnZipPath, out err);  
            }
            return true;
        }
    }
}
