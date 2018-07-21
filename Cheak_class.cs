using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
//using System.Data.Sql;

namespace MainProject
{
    class Cheak_class //класс провеки на правельный вод
    {

       public  Cheak_class()// konstructor 
        {

        }
       public void Only_Leters(KeyPressEventArgs e)// проверка вода в текстбох можно только буквы
       {
       
              
       
           if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))// только буквы
           {
               e.Handled = true;
           }
           if (!char.IsControl(e.KeyChar) && (e.KeyChar == (char)Keys.Space))//не разрешается водить пробел
           {
               e.Handled = true;
           }
        
        }
       public void LatersANDdigitsIntpucCheak(KeyPressEventArgs e) //проверка вода в текстбох можно только буквы и цыфры
       {
           

               if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar))// только буквы и цыфтры
               {
                   e.Handled = true;
               }
               if (!char.IsControl(e.KeyChar) && (e.KeyChar == (char)Keys.Space))//не разрешается водить пробел
               {
                   e.Handled = true;
               }
        
       }
       public void Only_dictimal(object sender, KeyPressEventArgs e)// проверка только цифры и  одна запятая (заменяет  точку на запятую )
       {
           
           if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar!='.') && (e.KeyChar)!=',')// только чилса , и вод через запятую их
           {
               e.Handled = true;
           }
           if ((e.KeyChar == '.'))
           {
               e.KeyChar = ',';
           }
          
          if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))// только 1 запятая на тексбох
           {
               e.Handled = true;
           }
        
            }
       public bool empy_cheakbox_error(TextBox texbox,Label name)//нельзя оставлять поле пустым выдаст ошибку подсветит лайбел
       {
          
           if (string.IsNullOrEmpty(texbox.Text))
           {
               name.ForeColor = System.Drawing.Color.Red;
               MessageBox.Show("Данное поле не может быть пустым : " + name.Text, "Ошибка ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               texbox.Focus();
               return false ;
           }
           else
           {
               name.ForeColor = System.Drawing.Color.Black;
               return true;
           }
           
         
       }
       public bool empy_ComboBox_error(ComboBox Combo, Label name)//нельзя оставлять поле пустым выдаст ошибку подсветит лайбел
       {
          
           
           if (string.IsNullOrEmpty (Combo.Text))
           {
               name.ForeColor = System.Drawing.Color.Red;
               MessageBox.Show("Данное поле не может быть пустым : " + name.Text, "Ошибка ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               Combo.Focus();
               return false;
           }
           else
           {
               name.ForeColor = System.Drawing.Color.Black;
               return true;
           }
        
       }
       
       public void Only_digits(KeyPressEventArgs e)// проверка вода в текстбох можно только цифры
       {
         
           if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))// только цифры
           {
               e.Handled = true;
           }
           if (!char.IsControl(e.KeyChar) && (e.KeyChar == (char)Keys.Space))//не разрешается водить пробел
           {
               e.Handled = true;
           }
      
         }
       public void test()
       {

       }
    }
}
