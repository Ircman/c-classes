using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEST_DB.Classes
{
    /// <summary>
    /// Класс для работы с файлами
    /// </summary>
    class WorkWithFiles
    {
        private string[] _fileData;

        public struct CleanData
        {
            public string PathLocal;
            public string PathRemote;
            public string Hash;
        }

        private int _filesCount = 0;
        private int _folderCount = 0;

        public static List<CleanData> CleanDataList=new List<CleanData>();

        public string[] ReadFile(string filePath)
        {
            if (File.Exists(filePath))
            {
               _fileData = File.ReadAllLines(filePath);
                return _fileData;
            }
            return null;
        }

        public List<CleanData> GetCleanData()
        {
            for (int i = 0; i < _fileData.Length; i++)
            {
                CleanData tmCleanData = new CleanData();
                char delim = ':';
                char delim2 = ',';
                var tmp = _fileData[i];
                var lastIndexOf = tmp.LastIndexOf('=');
                if (lastIndexOf >= 0)
                {

                    if (tmp.Contains(":")&&tmp.Contains(","))
                    {
                        lastIndexOf += 1;
                        var del1 = tmp.IndexOf(delim);
                        var del2 = tmp.IndexOf(delim2);
                        
                        tmCleanData.PathLocal = tmp.Substring(lastIndexOf, del2-lastIndexOf);
                        tmCleanData.PathRemote = tmp.Substring(del1 + 1, del2 - del1-1);
                        tmCleanData.Hash = tmp.Substring(del2 + 1, tmp.Length - del2 - 1);
                        
                   
                           
                        CleanDataList.Add(tmCleanData);

                    }
                }
                

            }

            return null;
        }

        /// <summary>
        /// создание файлов для теста
        /// </summary>
        public void CreateFiles()
        {
            for (int i = 0; i < WorkWithFiles.CleanDataList.Count; i++)
            {
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaasdasdasdasfsdgewr2423412321343256576ijhdfv sdt34567ujhvsdvxcvb5w4w6yuj7ue5q3rt3q3423r2";
                Random asd = new Random();
                var next = asd.Next(1, 100000);
                List<int> rnd = new List<int>();



                string[] liStrings = new string[next];

                for (int j = 0; j < (next); j++)
                {
                    liStrings[j] = data;
                }

                if (WorkWithFiles.CleanDataList[i].PathRemote.Contains("service"))
                {
                    i++;
                }
                string path = "project" + WorkWithFiles.CleanDataList[i].PathRemote;
                FileStream fileStream = File.Open(path, FileMode.Create, FileAccess.Write);
                StreamWriter fileWriter = new StreamWriter(fileStream);
                foreach (var s in liStrings)
                {
                    fileWriter.Write(s);
                }
                //fileWriter.Write(liStrings);
                fileWriter.Close();
                //File.WriteAllLines(path,liStrings);

                //File.WriteAllText(path,data);
            } 
        }


        public List<string> files = new List<string>();

        public void DirSearch(string sDir)
        {
            try
            {
                foreach (var f in Directory.GetFiles(sDir))
                {
                    _filesCount++;
                    files.Add(f);
                }
                foreach (var d in Directory.GetDirectories(sDir))
                {
                    _folderCount++;
                    DirSearch(d);
                }
            }
            catch (Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        public List<string> HideRootPath(string rootPath)
        {
            if (files.Count>0)
            {
                List<string> tmpList = new List<string>();
                foreach (var VARIABLE in files)
                {
                    if (IsDir(VARIABLE))
                    {
                        _folderCount++;
                    }
                    else
                    {
                        _filesCount++;
                    }
                    tmpList.Add(VARIABLE.Remove(0,rootPath.Length));
                }
                return tmpList;
            }
            return null;
        }

        public bool IsDir(string @path)
        {
            FileAttributes attr = File.GetAttributes(@path);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return true;
            }
            return false;
        }

        public void ShowCountFoldersAndFiles(Label lb_folders,Label lb_files)
        {
            lb_folders.Text = _folderCount.ToString();
            lb_files.Text = _filesCount.ToString();
            lb_files.Refresh();
            lb_folders.Refresh();
        }

        public string[] PathParser(string Path)
        {
            string tmpPath = Path;
            string[] strings = new string[3];
            int pos = 0;

            //level1
            pos = tmpPath.IndexOf('\\');
            tmpPath = tmpPath.Remove(pos, 1);

            pos = tmpPath.IndexOf('\\');
            strings[0] = tmpPath.Substring(0, pos);




            //level2
            pos = tmpPath.IndexOf('\\');
            tmpPath = tmpPath.Remove(0,pos+1);
            pos = tmpPath.IndexOf('\\');
            strings[1]=tmpPath.Substring(0, pos);





            //level 3
            tmpPath = tmpPath.Remove(0, pos);
            pos = tmpPath.IndexOf('\\');
            strings[2] = tmpPath.Remove(0, 1);

            return strings;
        }
    } 
}
