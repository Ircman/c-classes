using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dropbox.Api;
using Dropbox.Api.Files;

#pragma warning disable 1587
///youtube https://www.youtube.com/watch?v=mMBx9vDp5sk
#pragma warning restore 1587

namespace WindowsFormsApplication1
{//"m0u-KlF_3iAAAAAAAAAACNVB9DXCkjVsNprBtbH8PpUi0YyDJ2Q6N2ZRBXQNr7wG"
    class DropBoxApiClass
    {
        //получаем id , email, name с дроп бокса
        private async Task DbGetId(string token)
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
        public async Task DbGetFolderslist(string token)
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
        public async Task DbGetFilelist(string token)
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
        //скачивание файла с дропбокса
        public async Task DbdownloadFile(string token, string fileName, string folderName)
        {
           

            using (var dbx = new DropboxClient(token))
            {
                var download = await dbx.Files.DownloadAsync(folderName+ "/" + fileName );//путь к файлу
                var buffer = download.GetContentAsByteArrayAsync();
                buffer.Wait();
                var d = buffer.Result;
                File.WriteAllBytes(fileName,d);
            }

        }
        //загружаем файл на дропбокс
        public async Task DbUploaddFile(string token)
        {
            using (var dbx = new DropboxClient(token))
            {
            string filePath = ChouseFile();//путь к файлу на компьютере
            string url = string.Empty;
            string fileName=@"test.txt";//название файла которое запишется на дропбох
            string folderName= String.Empty;//"/foldername";//папка на дроп боксе 
            
                using (var buffer = new MemoryStream(File.ReadAllBytes(filePath)))
                {
                    var upload =  await dbx.Files.UploadAsync(folderName + "/" + fileName, WriteMode.Overwrite.Instance,body: buffer);
                    var tx = dbx.Sharing.CreateSharedLinkWithSettingsAsync(folderName + "/" + fileName);//создаем сылку на файл
                    tx.Wait();
                    url = tx.Result.Url;//Получаем сылку на файл
                }
            }

       

        }
        //выбераем файл 
        public string ChouseFile()
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
    }
}
