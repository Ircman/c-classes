using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testSQL
{
    class FilesToBinary
    {

        private string _filePatch;

        private string _fileName;

        //метод для открытия файла
        public void FileOpen()
        {
            //TODO:Проверить работает ли фильтр правельно !
            using (OpenFileDialog openFile =
                new OpenFileDialog()
                {
                    // Filter = "PDF (*.PDF)|*.pdf| WordFile (*.DOCX;*.DOC)|*.docx;*.doc",
                    Filter = "Documents (*.PDF;*.DOCX;*.DOC)|*.pdf;*.docx;*.doc",
                    ValidateNames = true,
                    Multiselect = false
                })
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    _filePatch = openFile.FileName;
                    _fileName = openFile.SafeFileName;
                }



        }

        //переводит файл в бинарный код и возврашает результат (вставляется в Адаптер Insert)
        public byte[] ConvertFileToBytes()
        {

            FileStream fStream = File.OpenRead(_filePatch);
            byte[] contents = new byte[fStream.Length];
            fStream.Read(contents, 0, (int) fStream.Length);
            fStream.Close();
            return contents;

        }

        //сохраняет  бинарный файл из даты грида 
        // DataGridView dataGridView --> указать датагрид
        //string datagrivewColumName --> указать название столбца датагрида
        //int cellIndex --> указать индех в котором хранится бинараный файл
        public void SaveBinaryFileFromDataGrid(DataGridView dataGridView, string datagrivewColumName, int cellIndex,
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;
            if (dataGridView.Columns[e.ColumnIndex].Name == datagrivewColumName)
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                // _fileName название файла 
                //TODO: название файла должно браться тоже из DataGrid
                // saveFile.FileName = _fileName;
                saveFile.ShowDialog();
                byte[] buffer = (byte[]) dataGridView.Rows[e.RowIndex].Cells[cellIndex].Value;
                FileStream fs = new FileStream(saveFile.FileName, FileMode.Create);
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();

            }


        }
    }
}
