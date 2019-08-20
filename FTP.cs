using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectManager.ClassFolder 
{
    /// <summary>
    /// Класс содержит информацию для авторизации 
    /// </summary>
    public class FtpUserInfoVo
    {
        public string Host { get; set; }
        public string DownloadPath { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// Метод читает данны из файла конфигурации ftp
        /// </summary>
        /// <returns></returns>
        public bool ReadFile()
        {
            string ftpConfig = "a.txt";

            if (File.Exists(ftpConfig))
            {
                File.ReadAllLines(ftpConfig);
                return true;
            }
            else
            {
                MessageBox.Show(@"ERROR can't read: " + ftpConfig + @" file", "ERROR", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }
    }

    public class ftpServer2 
    {

        public struct FtpDataStuct
        {
            public string FtpPath;
            public bool Checked;

        }
        private bool stop = true;
        private bool flag = true;
        static List<string>tmplist = new List<string>();
        public List<FtpDataStuct> FtpDatalList = new List<FtpDataStuct>();

        #region Member declaration
        private readonly Queue<string> _downloadlist = new Queue<string>();
        private static NetworkCredential _networkCredential;
        private readonly object _sync = new object();
        private static FtpUserInfoVo _ftpUserInfo;
        #endregion

        public ftpServer2(FtpUserInfoVo ftpUserInfo)
        {
            _ftpUserInfo = ftpUserInfo;
            _networkCredential = new NetworkCredential(ftpUserInfo.UserName, ftpUserInfo.Password);
        }

        protected virtual async Task FtpFileCreationTime(Uri url)
        {
            try
            {
                var creationtime = WebRequest.Create(url) as FtpWebRequest;
                if (creationtime != null)
                {
                    creationtime.UsePassive = false;
                    creationtime.UseBinary = true;
                    creationtime.Headers["User-Agent"] = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5";
                    creationtime.Credentials = _networkCredential;
                    creationtime.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                    using (var filecreationtime = await creationtime.GetResponseAsync() as FtpWebResponse)
                    {
                       // return filecreationtime != null ? filecreationtime.LastModified : DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
           // return DateTime.Now;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private async void StartDownloading()
        {
            while (_downloadlist.Count > 0)
            {
                try
                {
                    #region File Path Configuration
                    var sourcefilepath = _downloadlist.Dequeue();
                    var destinationfilepath = string.Format("{0}\\{1}", _ftpUserInfo.DownloadPath,
                        Path.GetFileName(sourcefilepath));
                    #endregion
                    var request = WebRequest.Create(sourcefilepath) as FtpWebRequest;
                    if (request == null) continue;
                    request.UseBinary = true;
                    request.Timeout = 1200000;
                    request.ReadWriteTimeout = 1200000;
                    request.UsePassive = true;
                    request.KeepAlive = false;
                    request.EnableSsl = false;
                    request.Headers["User-Agent"] = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5";
                    request.Credentials = _networkCredential;
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    using (var response = (FtpWebResponse)await request.GetResponseAsync())
                    {
                        lock (_sync)
                        {
                            Console.WriteLine("Downloading file " + sourcefilepath);
                            if (response.ContentLength <= 0) continue;
                            using (var file = File.Create(destinationfilepath))
                            {
                                var responseStream = response.GetResponseStream();
                                if (responseStream == null) continue;
                                responseStream.CopyTo(file);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Ftp Host : {0}", _ftpUserInfo.Host));
                    Console.WriteLine(ex);
                }
            }
        }
        public void DownloadFiles(string directory)
        {
            try
            {
                var url = string.IsNullOrEmpty(directory) ? HostUrl : HostUrl + "/" + directory;
                var ftprequest = WebRequest.Create(url);
                ftprequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftprequest.Credentials = _networkCredential;
                ftprequest.Timeout = 1200000;
                using (var ftpresponse = (FtpWebResponse)ftprequest.GetResponse())
                {
                    var responcestream = ftpresponse.GetResponseStream();
                    if (responcestream != null)
                    {
                        using (var streamreader = new StreamReader(responcestream))
                        {
                            while (!streamreader.EndOfStream)
                            {
                                var file = streamreader.ReadLine();
                                if (file == null) continue;
                                var filename = file.Split('/').Last();
                                if (!file.Contains("."))
                                {
                                    DownloadFiles(directory + "/" + filename);
                                }
                                file = url + "/" + filename;
                                _downloadlist.Enqueue(file);
                            }
                        }
                    }
                }
                if (_downloadlist.Count > 0 && _downloadlist.ElementAt(0).Equals("access.log"))
                {
                    _downloadlist.Dequeue();
                }
                StartDownloading();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Connecting to Ftp Host : {0} with username : {1}",
                    _ftpUserInfo.Host, _networkCredential.UserName));
                Console.WriteLine(ex);
            }
        }
        public void UploadFile(string directory, string filepath)
        {
            try
            {
                var fs = new FileStream(filepath, FileMode.Open);
                var filename = Path.GetFileName(filepath);
                var url = string.IsNullOrEmpty(directory) ? HostUrl : HostUrl + "/" + directory + "/" + filename;
                var ftprequest = WebRequest.Create(url);
                ftprequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftprequest.Credentials = _networkCredential;
                ftprequest.Timeout = 1200000;
                using (var ftpstream = ftprequest.GetRequestStream())
                {
                    var buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    ftpstream.Write(buffer, 0, buffer.Length);
                    ftpstream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ftp Host : {0} with username : {1}",
                    _ftpUserInfo.Host, _networkCredential.UserName));
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Данный метод предназначен для работы с методом ScanFtp(), этот метод получает список файлов на FTP Сервере
        /// 
        /// </summary>
        /// <param name="directory">папка на FTP</param>
        /// <param name="ftpDatalList">Список куда записавются пути на FTP сервере</param>
        /// <param name="flag">флаг который отвечат за первое заполенение списка</param>
        private static  void ListDirectories(string directory, List<FtpDataStuct> ftpDatalList,bool flag)
        {
            try
            {
                var url = string.IsNullOrEmpty(directory) ? HostUrl : HostUrl + "/" + directory;
                var ftprequest = WebRequest.Create(url);
                //ftprequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftprequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftprequest.Credentials = _networkCredential;
                ftprequest.Timeout = 1200000;
                using (var ftpresponse = (FtpWebResponse) ftprequest.GetResponse())
                {
                    var responcestream = ftpresponse.GetResponseStream();
                    if (responcestream == null) return;
                    using (var streamreader = new StreamReader(responcestream))
                    {
                        tmplist = new List<string>();
                        while (!streamreader.EndOfStream)
                        {
                            var tmpDataStuct = new FtpDataStuct();
                            var file = streamreader.ReadLine();

                            if (file != null)
                                if (file.Contains("."))
                                {
                                    tmplist.Add(file);
                                    if (flag)
                                    {
                                        tmpDataStuct.FtpPath = file;
                                        tmpDataStuct.Checked = true;
                                        ftpDatalList.Add(tmpDataStuct);
                                    }
                                }
                                else
                                {
                                    //string tst = string.Empty;
                                    tmplist.Add(file);
                                    if (flag)
                                    {
                                        tmpDataStuct.FtpPath = file;
                                        tmpDataStuct.Checked = false;
                                        ftpDatalList.Add(tmpDataStuct);
                                    }
                                }
                        }
                    }
                }
            }
            catch (WebException e)
            {
                var status = ((FtpWebResponse) e.Response).StatusDescription;
                MessageBox.Show(status);
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Метод сканирует FTP сервер и заполняет список FtpDatalList кторый состоит из структуры FtpDataStuct
        /// </summary>
        public void ScanFtp()
        {
            try
            {
                
                string tst1 = string.Empty;//временная переменная 

                ListDirectories("", FtpDatalList, flag);// получаем файлы из Root папки
                flag = false; // ставим флаг который говорит от том что в дальнешем данные в FtpDatalList будут заполнятся тут
                do
                {
                    for (int i = 0; i < FtpDatalList.Count; i++)
                    {
                        if (FtpDatalList[i].Checked == false)// проверяем была ли проверена папка или файл
                        {
                            if (FtpDatalList[i].FtpPath.Contains('.') == false)// если не содержит точку значит файл
                            {
                                ftpServer2.ListDirectories(FtpDatalList[i].FtpPath, FtpDatalList, flag);// получаем список файлов по указанной сылке
                                if (tmplist.Count > 0)//список файлов по сылке не пустой
                                {
                                    for (int k = 0; k < tmplist.Count; k++)// для всех файлов в списке tmplist
                                    {
                                        if (tmplist[k].Contains('/'))
                                        {
                                            tst1 = "/" + tmplist[k].Split('/')[1];
                                        }
                                        string tempdata = FtpDatalList[i].FtpPath + tst1;
                                        FtpDataStuct tmps = new FtpDataStuct();
                                        tmps.FtpPath = tempdata;
                                        tst1 = string.Empty;
                                        FtpDatalList.Insert(i + 1, tmps);// добавляем данные из tmplist в  FtpDatalList
                                        tempdata = string.Empty;
                                    }
                                }
                                tmplist = new List<string>();// обнуляем данные tmplist
                            }

                            var ftpDataStuct = FtpDatalList[i];
                            ftpDataStuct.Checked = true; // ставим флаг что проверили текушею позицею
                            FtpDatalList[i] = ftpDataStuct;
                        }
                    }
                    Thread.Sleep(100);// ожидаем 100
                    stop = CheckToStop();
                }
                while (stop);
            }
            catch (WebException e)
            {
                String status = ((FtpWebResponse)e.Response).StatusDescription;
                System.Windows.Forms.MessageBox.Show(status.ToString());
            }
            
        }
        public static string HostUrl
        {
            get { return string.Format("ftp://{0}", _ftpUserInfo.Host); }
        }

        /// <summary>
        /// Метод проверят проверены ли папки и файлы в списке FtpDatalList если везде стоит true 
        /// он прервет выполнения цикла в ScanFtp()
        /// </summary>
        /// <returns>True or False</returns>
        private bool CheckToStop()
        {
            foreach (var VARIABLE in FtpDatalList)
            {
                if (VARIABLE.Checked == false)
                {
                    return true;
                }
            }
            return false;
        }
        

        public string[] Return(string filepath, string username, string password)
        {
            List<string> directories = new List<string>();
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(filepath);
                ftpRequest.Credentials = new NetworkCredential(username, password);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());
                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    directories.Add(line);
                    line = streamReader.ReadLine();
                }
            }
            catch (WebException e)
            {
                String status = ((FtpWebResponse)e.Response).StatusDescription;
                System.Windows.Forms.MessageBox.Show(status.ToString());
            }
            return directories.ToArray();
        }
        // In this part i create the sub-directories.   
        public void createdir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }  

       
    }

    
}
