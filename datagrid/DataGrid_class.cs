using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics;
using System.IO;
namespace MainProject
{
    class DataGrid_class
    {
        ERROR_component error = new ERROR_component();
        public static string TempData;
        string asd;
        //записавает значение из датыгрида с указаным индехсом TempData;
        public void popupData_Grid_valut_to_textbox(DataGridView dataGridView, DataGridViewCellEventArgs e, int cellindex, TextBox text)
        {
            


                if (e.RowIndex == -1) return;
                text.Text = dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString();
                dataGridView.Visible = false;
        
        }
        //при нажати на столбец берет значение  с этого столбца
        public void popupData_Grid_texbox(DataGridView dataGridView, DataGridViewCellEventArgs e, int cellindex, TextBox texbox, string ROW_NAME)
        {
           

                if (e.ColumnIndex == -1) return;
                if (dataGridView.Columns[e.ColumnIndex].Name == ROW_NAME)
                {
                    if (e.RowIndex == -1) return;
                    texbox.Text = dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString();
                }
                   
                dataGridView.Visible = false;
        
        }

        public void popupData_Grid_file_open(DataGridView dataGridView, DataGridViewCellEventArgs e, int File_cellindex, string ROW_NAME)
        {
           
                if (e.RowIndex == -1) { return; }
                else
                {
                    if (e.ColumnIndex == -1) return;
                    if (dataGridView.Columns[e.ColumnIndex].Name == ROW_NAME)
                    {
                        string test = dataGridView.Rows[e.RowIndex].Cells[File_cellindex].Value.ToString();
                        if (File.Exists(dataGridView.Rows[e.RowIndex].Cells[File_cellindex].Value.ToString()))
                        {
                            Process.Start(dataGridView.Rows[e.RowIndex].Cells[File_cellindex].Value.ToString());
                        }
                        else
                        {
                            MessageBox.Show("Запрашиваемый вами файл не найден!\n Название файла :  " + dataGridView.Rows[e.RowIndex].Cells[File_cellindex].Value.ToString(), "Файла нет", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                }
        
        }

        public void data_to_texbox_groupbox(DataGridView dataGridView, DataGridViewCellEventArgs e, int cellindex, TextBox texbox, GroupBox groupbox, int widht, int height)
        {
           
                if (e.RowIndex == -1) return;
                texbox.Text = dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString();
                dataGridView.Visible = false;
                groupbox.Size = new Size(widht, height);
        
        }
        public void data_to_texbox(DataGridView dataGridView, DataGridViewCellEventArgs e, int cellindex, TextBox texbox)
        {
            
                if (e.RowIndex == -1) return;
                texbox.Text = dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString();
         

        }

        //data v string
        public void data_to_string(DataGridView dataGridView, DataGridViewCellEventArgs e, int cellindex, string stringa)
        {
           
                if (e.RowIndex == -1) return;
                //if (dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString()!=null)
                //  string_name = dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString();
                stringa = dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString();
                dataGridView.Visible = false;
          
        }
        public string data_to_string(DataGridView dataGridView, DataGridViewCellEventArgs e, int cellindex)
        {

            
                //if (dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString()!=null)
                //  string_name = dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString();
                dataGridView.Visible = false;
                if (e.RowIndex == -1) { return "No_file"; }
                else
                {
                    return dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString();
                }
          
        }
        //просто возврашает заначение из даты грида
        public string data_to_string_simple(DataGridView dataGridView, DataGridViewCellEventArgs e, int cellindex)
        {
            
                if (e.RowIndex == -1)
                {
                    return "0";
                }
                else
                {
                    return dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString();
                }
       
        }
        public string data_to_string(DataGridView dataGridView, DataGridViewCellEventArgs e, int cellindex, GroupBox groubox)
        {
           
                if (e.RowIndex == -1) return "No_file";
                //if (dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString()!=null)
                //  string_name = dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString();
                groubox.Visible = false;
                return dataGridView.Rows[e.RowIndex].Cells[cellindex].Value.ToString();
      
        }
        //показывает или уберает датагрид меню по нажатию текстбокса
        public void showhide_menu(DataGridView dataGridView)
        {
            
                if (dataGridView.Visible == false)
                {
                    dataGridView.Visible = true;
                }
                else
                {
                    dataGridView.Visible = false;
                }
       
        }
        //записават в текстокс текс из TempData
        public void textTOtextbox(TextBox text)
        {
            text.Text = TempData;
        }
        //удаляет выбраную строку !!! дата грид                  событие наж клетку         (РаботаBindingSoruce)  | название кнопки  | индех информации каторую вывести при удалении
        public void delete_row(DataGridView dataGridView, DataGridViewCellEventArgs e, BindingSource method, string info, string name_row_delete, int info_text_index)
        {
            


                if (e.ColumnIndex == -1) return;
                if (dataGridView.Columns[e.ColumnIndex].Name == name_row_delete)
                {
                    if (e.RowIndex == -1) return;
                    if (MessageBox.Show("Вы действительно хотите удалить эту запись?\n" + info + ": " + dataGridView.Rows[e.RowIndex].Cells[info_text_index].Value.ToString(), "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        method.RemoveCurrent();
                        //method.EndEdit();

                    }


                }
          
        }

        public string edit_row(DataGridView dataGridView, DataGridViewCellEventArgs e, string name_row_delete, int index)
        {
           
                if (e.ColumnIndex == -1) { return "ERROR"; }
                else
                {
                    if (dataGridView.Columns[e.ColumnIndex].Name == name_row_delete)
                    {
                        if (e.RowIndex == -1) return "error";
                        return dataGridView.Rows[e.RowIndex].Cells[index].Value.ToString();


                    }
                }
                return dataGridView.Rows[e.RowIndex].Cells[index].Value.ToString();
                
        
        }


        public void ENTER(TextBox textbox, DataGridView datagrid, KeyEventArgs e, int cellindex)
        {
           
                if (e.KeyCode == Keys.Enter)
                {
                    textbox.Text = datagrid.Rows[0].Cells[cellindex].Value.ToString();
                    datagrid.Visible = false;
                }
        
        }


        public void data_Print(DataGridView dataGridView, DataGridViewCellEventArgs e, string name_row, int cellindex_work_type, string content_id, string data_type)
        {
            
                if (e.ColumnIndex == -1) { return; }
                if (dataGridView.Columns[e.ColumnIndex].Name == name_row)
                {
                    MessageBox.Show("rabotaet");
                    if (e.RowIndex == -1) { return; }
                    else
                    {
                        data_type = dataGridView.Rows[e.RowIndex].Cells[cellindex_work_type].Value.ToString();
                        content_id = dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                    }
                }
       


        }
    }
}

