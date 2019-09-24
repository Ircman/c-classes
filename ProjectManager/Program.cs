using ProjectManager.Forms;
using System;
using System.Windows.Forms;
using ProjectManager.ClassFolder;

namespace ProjectManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            }
            catch (Exception e)
            {
              Error.Msg(e);
            }
        }
    }
}
