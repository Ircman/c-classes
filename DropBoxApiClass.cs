using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Stone;

#pragma warning disable 1587
///youtube https://www.youtube.com/watch?v=mMBx9vDp5sk
#pragma warning restore 1587

namespace WindowsFormsApplication1
{//"m0u-KlF_3iAAAAAAAAAACNVB9DXCkjVsNprBtbH8PpUi0YyDJ2Q6N2ZRBXQNr7wG"
    class DropBoxApiClass
    {
        //получаем id , email, name с дроп бокса
        public async Task GetId(string token)
        {

            using (var dbx = new DropboxClient(token))
            {
               var id = await dbx.Users.GetCurrentAccountAsync();
               string ide = id.Name.DisplayName;
               string conuntry = id.Country;
               string email = id.Email;
            }
           
        }
        //получаем список папок
        public async Task GetFolderslist(string token)
        {
            using (var dbx = new DropboxClient(token))
            {
                var list = await dbx.Files.ListFolderAsync(String.Empty);//название папки => /folder
                foreach (var folder in list.Entries.Where(i=>i.IsFolder))
                {
                    string folderName = folder.Name;//получаем список папок
                }
            }

        }
        //получаем список файлов 
        public async Task GetFilelist(string token)
        {
            using (var dbx = new DropboxClient(token))
            {
                var list = await dbx.Files.ListFolderAsync(String.Empty);//название папки => /folder
                foreach (var file in list.Entries.Where(i => i.IsFile))
                {
                    string fileName = file.Name;//получаем список файлов
                    var fileSize = file.AsFile.Size;//получаем размер файлов
                }
            }

        }

#region Download Скачивание файлов
        // скачиваем файл и обновляем прогрессбар
        public async Task DownloadWithProgressBar(ProgressBar progressBar, string token, string folderName, string fileName)
        {
            //string folderName = String.Empty;
            //string fileName = "test.rar";
            using (var dbx = new DropboxClient(token))
            {
                var response = await dbx.Files.DownloadAsync(folderName + "/" + fileName);
                ulong fileSize = response.Response.Size;
                const int bufferSize = 1024 * 1024;
                var buffer = new byte[bufferSize];
                using (var stream = await response.GetContentAsStreamAsync())
                {
                    using (var file = new FileStream(ChouseFiletoDownload(fileName), FileMode.OpenOrCreate))
                    {
                        var length = stream.Read(buffer, 0, bufferSize);

                        while (length > 0)
                        {
                            file.Write(buffer, 0, length);
                            var percentage = 100 * (ulong)file.Length / fileSize;
                            //Update progress bar with the percentage.
                            progressBar.Value = (int)percentage;
                            length = stream.Read(buffer, 0, bufferSize);
                        }
                    }
                }
            }
            progressBar.Value = 0;
        }
        //скачивание файла с дропбокса без файла загрушика
        public async Task DownloadFile(string token, string fileName, string folderName)
        {
            using (var dbx = new DropboxClient(token))
            {
                var download = await dbx.Files.DownloadAsync(folderName + "/" + fileName);//путь к файлу
                var buffer = download.GetContentAsByteArrayAsync();
                buffer.Wait();
                var d = buffer.Result;
                string pathToDownload = ChouseFiletoDownload(fileName);
                File.WriteAllBytes(pathToDownload, d);
            }

        }
        //сохраняем файл в указаную папку
        private string ChouseFiletoDownload(string fileName)
        {

            using (SaveFileDialog sfd = new SaveFileDialog() { })
            {
                sfd.FileName = fileName;
                if (sfd.ShowDialog() == DialogResult.OK)
                {

                    return sfd.FileName;
                }
                return sfd.FileName;
            }
        }

#endregion

#region Upload Загрузка файлов 
        //загружаем файл на дропбокс
        public async Task UploaddFile(string token)
        {
            using (var dbx = new DropboxClient(token))
            {
               
            string filePath = ChouseFiletoUpload();//путь к файлу на компьютере
            string url = string.Empty;
            string fileName=@"test.txt";//название файла которое запишется на дропбох
            string folderName= String.Empty;//"/foldername";//папка на дроп боксе 
               
                using ( MemoryStream buffer = new MemoryStream(File.ReadAllBytes(filePath)))
                {
                   

                    var size = dbx.Files.UploadAsync(folderName + "/" + fileName, WriteMode.Overwrite.Instance, body: buffer).Result.Size;
                    var upload =  await dbx.Files.UploadAsync(folderName + "/" + fileName, WriteMode.Overwrite.Instance,body: buffer);
                    var tx = dbx.Sharing.CreateSharedLinkWithSettingsAsync(folderName + "/" + fileName);//создаем сылку на файл
                    tx.Wait();
                    url = tx.Result.Url;//Получаем сылку на файл
                }
            }

       

        }
        //выбераем файл для загрузки
        private string ChouseFiletoUpload()
        {

            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    return ofd.FileName;
                }
                return ofd.FileName;
            }
        }


        public async Task Upload(string token, string folderName)// загрузка файла без Progressbar
        {
            using (var dbx = new DropboxClient(token))
            {
                string localFilepath = ChouseFiletoUpload();
                FileInfo f = new FileInfo(localFilepath);
                string fileName = f.Name;
                using (MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(localFilepath)))
                {
                    await dbx.Files.UploadAsync(folderName + "/" + fileName, WriteMode.Overwrite.Instance,
                        body: memoryStream);

                }


            }
        }

        public async Task UploadWithProgressbarr(string token, ProgressBar progressBar)// загрузка файлов
        {
            string localPath = ChouseFiletoUpload();
            FileInfo f = new FileInfo(localPath);

            string remotePath = "/" + f.Name;
            const int ChunkSize = 4096 * 1024;
            DropboxClient client = new DropboxClient(token);
            using (var fileStream = File.Open(localPath, FileMode.Open))
            {
                if (fileStream.Length <= ChunkSize)
                {
                    await client.Files.UploadAsync(remotePath, body: fileStream);
                }
                else
                {
                    await ChunkUpload(remotePath, fileStream, (int)ChunkSize, token, progressBar);
                }
            }
        }

        private async Task ChunkUpload(String path, FileStream stream, int chunkSize, string token, ProgressBar progressBar)
        {
            DropboxClient client = new DropboxClient(token);
            ulong numChunks = (ulong)Math.Ceiling((double)stream.Length / chunkSize);
            byte[] buffer = new byte[chunkSize];
            string sessionId = null;
            for (ulong idx = 0; idx < numChunks; idx++)
            {
                var byteRead = stream.Read(buffer, 0, chunkSize);
                double percentage = 0;
                using (var memStream = new MemoryStream(buffer, 0, byteRead))
                {
                    if (idx == 0)
                    {
                        var result = await client.Files.UploadSessionStartAsync(false, memStream);
                        sessionId = result.SessionId;
                    }
                    else
                    {
                        var cursor = new UploadSessionCursor(sessionId, (ulong)chunkSize * idx);
                        percentage = 100 * ((chunkSize * (int)idx) / (double)stream.Length);
                        progressBar.Value = (int)Math.Ceiling((decimal)percentage);

                        if (idx == numChunks - 1)
                        {
                            FileMetadata fileMetadata = await client.Files.UploadSessionFinishAsync(cursor, new CommitInfo(path), memStream);
                            percentage = 100 * ((chunkSize * (int)idx) / (double)stream.Length);
                            progressBar.Value = (int)Math.Ceiling((decimal)percentage);
                            //Console.WriteLine(fileMetadata.PathDisplay);
                        }
                        else
                        {
                            await client.Files.UploadSessionAppendV2Async(cursor, false, memStream);
                            percentage = 100 * ((chunkSize * (int)idx) / (double)stream.Length);
                            progressBar.Value = (int)Math.Ceiling((decimal)percentage);

                        }
                    }

                }


                progressBar.Value = 0;
            }
            stream.Close();
        }


#endregion
        
    }
}
