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
            try
            {
                string ftpConfig = GlobalConfig.FtpConfigFile;
                if (File.Exists(ftpConfig))
                {
                    var ftpdata = File.ReadAllLines(ftpConfig);
                    Host = ftpdata[0];
                    UserName = ftpdata[1];
                    Password = ftpdata[2];
                    Port = ftpdata[3];
                    return true;
                }
                else
                {
                    MessageBox.Show(@"ERROR can't read: " + ftpConfig + @" file", @"ERROR", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception e)
            {
                Error.Msg(e);
                return false;
            }
        }
    }
    public class FtpServer2
    {
        public struct FtpDataStuct
        {
            public string FtpPath;
            public bool Checked;
        }
        private bool _stop = true;
        private bool _flag = true;
        static List<string> _tmplist = new List<string>();
        public List<FtpDataStuct> FtpDatalList = new List<FtpDataStuct>();
        #region Member declaration
        private readonly Queue<string> _downloadlist = new Queue<string>();
        private static NetworkCredential _networkCredential;
        private readonly object _sync = new object();
        private static FtpUserInfoVo _ftpUserInfo;
        #endregion
        /// <summary>
        /// Конструктор 
        /// </summary>
        public FtpServer2()
        {
            try
            {
                FtpUserInfoVo condata = new FtpUserInfoVo();
                condata.ReadFile();
                _ftpUserInfo = condata;
                _networkCredential = new NetworkCredential(condata.UserName, condata.Password);
            }
            catch (Exception e)
            {
                Error.Msg(e);
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        private async void StartDownloading()
        {
            try
            {
                while (_downloadlist.Count > 0)
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
                                Console.WriteLine(@"Downloading file " + sourcefilepath);
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
            }
            catch (Exception e)
            {
                Error.Msg(e);
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
                Error.Msg(ex);
            }
        }
        /// <summary>
        /// Метод скачивает один файл с ftp во временную папку в программе
        /// </summary>
        /// <param name="downloadFrom">путь к файлу на FTP (от корня /)</param>
#pragma warning disable 1998
        public async Task SingleDownload(string downloadFrom)
#pragma warning restore 1998
        {
            try
            {
                string serverPath = string.Format("ftp://{0}/{1}",_ftpUserInfo.Host, downloadFrom);
                string destinationFile = GlobalConfig.TmpDir + Path.GetFileName(downloadFrom);
                FtpWebRequest request =  (FtpWebRequest)WebRequest.Create(serverPath);
                request.KeepAlive = true;
                request.UsePassive = true;
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = _networkCredential;
                // Read the file from the server & write to destination                
                using (var ftpStream = request.GetResponse().GetResponseStream())
                using (Stream fileStream = File.Create(destinationFile))
                {
                    var buffer = new byte[10240];
                    int read;
                    while (ftpStream != null && (read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                        fileStream.Write(buffer, 0, read);
                }
                
            }
            catch (Exception e)
            {
                Error.Msg(e);
            }
            
        }


#pragma warning disable 1998
        /// <summary>
        /// Метод скачивает файл с FTP в папку
        /// </summary>
        /// <param name="downloadFrom">полный путь к файлу</param>
        /// <param name="downloadTo">папка куда скачать файл</param>
        /// <returns></returns>
        public async Task Download(string downloadFrom , string downloadTo)
#pragma warning restore 1998
        {
//            FtpWebRequest request =(FtpWebRequest)WebRequest.Create("ftp://ftp.example.com/remote/path/file.zip");
//            request.Credentials = new NetworkCredential("username", "password");
//            request.Method = WebRequestMethods.Ftp.DownloadFile;
//
//            using (Stream ftpStream = request.GetResponse().GetResponseStream())
//            using (Stream fileStream = File.Create(@"C:\local\path\file.zip"))
//            {
//                byte[] buffer = new byte[10240];
//                int read;
//                while ((read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
//                {
//                    fileStream.Write(buffer, 0, read);
//                    Console.WriteLine("Downloaded {0} bytes", fileStream.Position);
//                }
//            }



            try
            {
                var serverPath = string.Format("ftp://{0}/{1}", _ftpUserInfo.Host, downloadFrom);
                var destinationFile = GlobalConfig.TmpDir + Path.GetFileName(downloadFrom);
                var request = (FtpWebRequest) WebRequest.Create(serverPath);
                request.KeepAlive = true;
                request.UsePassive = true;
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = _networkCredential;
                // Read the file from the server & write to destination                
                using (var ftpStream = request.GetResponse().GetResponseStream())
                using (Stream fileStream = File.Create(destinationFile))
                {
                    var buffer = new byte[10240];
                    int read;
                    while (ftpStream != null && (read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                        fileStream.Write(buffer, 0, read);
                }

            }
            catch (Exception e)
            {
                Error.Msg(e);
            }

        }

        /// <summary>
        /// Метод переименовавает файл
        /// </summary>
        /// <param name="fullPathWithFileName">полны путь до файла на FTP </param>
        /// <param name="renameToFilename">название файла на что переименовать</param>
        public void RenameFile(string fullPathWithFileName, string renameToFilename)
        {

            FtpWebResponse ftpResponse = null;
            FtpWebRequest request = null;
            string serverPath = string.Format("ftp://{0}/{1}", _ftpUserInfo.Host, fullPathWithFileName);
            request = (FtpWebRequest)WebRequest.Create(serverPath);
            request.Credentials = _networkCredential;
            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = true;
            request.Method = WebRequestMethods.Ftp.Rename;
            request.RenameTo = "serverPath";//rename to nado ukzatj na kakoj fajl
            ftpResponse = (FtpWebResponse)request.GetResponse();
            ftpResponse.Close();
  
        }
        /// <summary>
        /// Метод загружает файлы на FTP сервер
        /// </summary>
        /// <param name="directory">папка на ftp куда загружать файл</param>
        /// <param name="filepath">путь к файлу который загружаем</param>
        public void UploadFile(string directory, string filepath)
        {
            Stream fs=null;
            Stream ftpstream=null;
            try
            {
                 fs = new FileStream(filepath, FileMode.Open);
                var filename = Path.GetFileName(filepath);
                var url = string.IsNullOrEmpty(directory) ? HostUrl : HostUrl + "/" + directory.ToLower() + "/" + filename.ToLower();
                var ftprequest = WebRequest.Create(url);
                ftprequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftprequest.Credentials = _networkCredential;
                ftprequest.Timeout = 1200000;
                using (ftpstream = ftprequest.GetRequestStream())
                {
                    var buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    ftpstream.Write(buffer, 0, buffer.Length);
                    ftpstream.Close();
                    ftpstream.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("File unavailable")) //обрабатываем ошибку если папки нету на FTP
                {
                   if (fs != null) fs.Close(); // закрываем поток
                    if (ftpstream != null) //закрываем поток
                    {
                        ftpstream.Close();
                    }
                    if (directory != null)
                    {
                       MkDirFullPath(directory.ToLower()); //создаем папку
                       UploadFile(directory, filepath); //повторяем загрузку
                    }
                }
                else
                {
                    Error.Msg(ex);
                }
            }
            finally
            {
                if (fs != null) fs.Close(); // закрываем поток
                if (ftpstream != null) //закрываем поток
                {
                   ftpstream.Close();
                    ftpstream.Dispose();
                }
            }
        }
        /// <summary>
        /// Данный метод предназначен для работы с методом ScanFtp(), этот метод получает список файлов на FTP Сервере
        /// 
        /// </summary>
        /// <param name="directory">папка на FTP</param>
        /// <param name="ftpDatalList">Список куда записавются пути на FTP сервере</param>
        /// <param name="flag">флаг который отвечат за первое заполенение списка</param>
        private static void ListDirectories(string directory, List<FtpDataStuct> ftpDatalList, bool flag)
        {
            try
            {
                var url = string.IsNullOrEmpty(directory) ? HostUrl : HostUrl + "/" + directory;
                var ftprequest = WebRequest.Create(url);
                //ftprequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftprequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftprequest.Credentials = _networkCredential;
                ftprequest.Timeout = 1200000;
                using (var ftpresponse = (FtpWebResponse)ftprequest.GetResponse())
                {
                    var responcestream = ftpresponse.GetResponseStream();
                    if (responcestream == null) return;
                    using (var streamreader = new StreamReader(responcestream))
                    {
                        _tmplist = new List<string>();
                        while (!streamreader.EndOfStream)
                        {
                            var tmpDataStuct = new FtpDataStuct();
                            var file = streamreader.ReadLine();
                            if (file != null)
                                if (file.Contains("."))
                                {
                                    _tmplist.Add(file);
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
                                    _tmplist.Add(file);
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
                Error.Msg(e);
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
                ListDirectories("", FtpDatalList, _flag);// получаем файлы из Root папки
                _flag = false; // ставим флаг который говорит от том что в дальнешем данные в FtpDatalList будут заполнятся тут
                do
                {
                    for (int i = 0; i < FtpDatalList.Count; i++)
                    {
                        if (FtpDatalList[i].Checked == false)// проверяем была ли проверена папка или файл
                        {
                            if (FtpDatalList[i].FtpPath.Contains('.') == false)// если не содержит точку значит файл
                            {
                                ListDirectories(FtpDatalList[i].FtpPath, FtpDatalList, _flag);// получаем список файлов по указанной сылке
                                if (_tmplist.Count > 0)//список файлов по сылке не пустой
                                {
                                    for (int k = 0; k < _tmplist.Count; k++)// для всех файлов в списке tmplist
                                    {
                                        if (_tmplist[k].Contains('/'))
                                        {
                                            tst1 = "/" + _tmplist[k].Split('/')[1];
                                        }
                                        string tempdata = FtpDatalList[i].FtpPath + tst1;
                                        FtpDataStuct tmps = new FtpDataStuct();
                                        tmps.FtpPath = tempdata;
                                        tst1 = string.Empty;
                                        FtpDatalList.Insert(i + 1, tmps);// добавляем данные из tmplist в  FtpDatalList
                                        
                                    }
                                }
                                _tmplist = new List<string>();// обнуляем данные tmplist
                            }
                            var ftpDataStuct = FtpDatalList[i];
                            ftpDataStuct.Checked = true; // ставим флаг что проверили текушею позицею
                            FtpDatalList[i] = ftpDataStuct;
                        }
                    }
                    Thread.Sleep(100);// ожидаем 100
                    _stop = true;
                }
                while (_stop);
            }
            catch (WebException e)
            {
                Error.Msg(e);
            }
        }
        public static string HostUrl
        {
            get { return string.Format("ftp://{0}", _ftpUserInfo.Host); }
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
                Error.Msg(e);
            }
            return directories.ToArray();
        }
        /// <summary>
        /// Метод создает полный путь 
        /// </summary>
        /// <param name="path"></param>
        public void MkDirFullPath(string path)
        {
            var tmpFolders = path.Split('/');
            string createPath=string.Empty;
            int i = 0;
            foreach (var folder in tmpFolders)
            {
                if (string.IsNullOrEmpty(folder)==false)
                {
                    if (i == 0)
                    {
                        i++;
                        createPath = "/" + folder + "/";
                        MkDir(createPath);
                    }
                    else
                    {
                        i++;
                        createPath += folder+"/";
                        MkDir(createPath);
                    }
                }
                
            }





        }
        /// <summary>
        /// Метод создает папку
        /// </summary>
        /// <param name="folder"></param>
        public void MkDir(string folder)
        {

               FtpWebRequest ftp_web_request = null;
               FtpWebResponse ftp_web_response = null;
                string ftp_path = HostUrl + folder;
            try
            {
                ftp_web_request = (FtpWebRequest) WebRequest.Create(ftp_path);
                ftp_web_request.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftp_web_request.Credentials = _networkCredential;
                ftp_web_response = (FtpWebResponse) ftp_web_request.GetResponse();
                ftp_web_response.Close();
            }
            catch (WebException Ex)
            {
                FtpWebResponse response = (FtpWebResponse) Ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close(); //папка сушетвует
                }
                else
                {
                    response.Close();
                    MkDir(folder);
                }
            }
            finally
            {
                if (ftp_web_response != null) ftp_web_response.Close();
            }
        }
    }
}
