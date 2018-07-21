using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Diagnostics;


namespace MainProject
{
    public partial class ERROR_component : Component
    {
        public ERROR_component()
        {
            InitializeComponent();
        }

        public ERROR_component(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        public void error_messege(Exception ex)//выдает ошибку название метода и класса где это произошло
        {
            
            StackFrame frame = new StackFrame(1);
            var method = frame.GetMethod();
            var Class_name = method.DeclaringType;
            var metod_name = method.Name;
            MessageBox.Show("ERROR " + "Дата:" + DateTime.Now.ToShortDateString() + " Время: " + DateTime.Now.ToShortTimeString() + "\n" + "Форма: " + Class_name + "\nМетод: " + metod_name + "\n" + ex);
        }
       
    }
}
