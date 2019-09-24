using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Dropbox.Api;
using Dropbox.Api.FileRequests;
using Dropbox.Api.Sharing;

namespace WindowsFormsApplication1
{
    class Testing
    {

        public async Task asd(ProgressBar asdProgressBar,string token)
        {
            string folderName=String.Empty;
              string fileName="test.txt";
            using (var dbx = new DropboxClient(token))
            {
                var response = await dbx.Files.DownloadAsync(folderName + "/" + fileName);
                ulong fileSize = response.Response.Size;
                const int bufferSize = 1024 * 1024;

                var buffer = new byte[bufferSize];

                using (var stream = await response.GetContentAsStreamAsync())
                {
                    using (var file = new FileStream(fileName, FileMode.CreateNew))
                    {
                        var length = stream.Read(buffer, 0, bufferSize);

                        while (length>0)
                        {

                            file.Write(buffer, 0, length);
                            var percentage = 100 * (ulong) file.Length / fileSize;
                            // UpdateFileRequestDeadline.Update progress bar with the percentage.
                            asdProgressBar.Value = (int) percentage;
                            asdProgressBar.Update();

                        }


                    }
                }
            }
        }
    }
}
