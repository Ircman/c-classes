using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace MainProject
{
    class File_copy_class
    {
        string dir;
        string main_folder = "Data";
        string file_old,file_new;
        bool flag;

        public string file_browse(string directory)//выбпор файла и папки для него 
        {
            try { 
            using (OpenFileDialog of = new OpenFileDialog() { Multiselect = false })
            {
                
                dir = directory;//directorija
                if (of.ShowDialog() == DialogResult.OK)
                {
                    file_old = of.FileName;//starij fail 
                    if (Directory.Exists(main_folder) == false)
                    {
                        Directory.CreateDirectory(main_folder);
                    }
                    if (Directory.Exists(main_folder+"\\"+directory) == false)
                    {
                        Directory.CreateDirectory(main_folder+"\\"+directory);
                        file_new = of.SafeFileName;
                        flag = true;
                       
                    }
                    else
                    {
                        file_new = of.SafeFileName;
                        if (File.Exists(main_folder + "\\"+dir + "\\" + file_new) == true)
                        {
                            if (MessageBox.Show("Фаил с таким названием уже сушетвует перезаписать его?", "Фаил сушетвует", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                flag = true;
                                
                                
                            }
                            else
                            {
                                flag = false;
                                MessageBox.Show("Файл небыл добавлен!!","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            flag = true;
                        }
                        

                        
                        
                    }

                }
            }
            return main_folder + "\\" + dir + "\\" + file_new;
            }
            catch (Exception ex)
            {

                MessageBox.Show("ERROR in :" + " File_copy_class " + "file_browse\n" + ex, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "ERROR";
            }
            }

        public void file_copy()//копируем и удаляем фаил после дабовления
        {
            try { 
            if (flag == true)
            {
                if (File.Exists(file_old))
                {
                    File.Copy(file_old, (main_folder + "\\" + dir + "\\" + file_new), true);
                    if (File.Exists(main_folder + "\\" + dir + "\\" + file_new) == true)
                    {
                        File.Delete(file_old); //delete after copy
                    }
                }
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show("ERROR in :" + " File_copy_class " + "file_copy\n" + ex, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
              
            }
            }

        public void file_start(string file_link)
        {
           
            if (string.IsNullOrEmpty(file_link) == false)
            {
                if (File.Exists(file_link) == true)
                {
                    Process.Start(file_link);
                }
                else
                {
                    MessageBox.Show("Файл не найден: \n" + file_link, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Файл не был добавлен в базу данных\n сылка на него Отсутствует ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        
        }

        public void Browse_foto(PictureBox picturebox)
        {
            
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG (* .jpg)|*.jpg", ValidateNames = true, Multiselect = false })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        
                        picturebox.Image = Image.FromFile(ofd.FileName);
                    }
                }
            
        
        }

        public byte[] foto_to_byte(Image image)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }


    }
}
