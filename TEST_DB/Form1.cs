using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TEST_DB.Classes;

namespace TEST_DB
{
    public partial class Form1 : Form
    {
        BackgroundWorker workerThread = null;

        public delegate void UpdateLabel(string label);

        bool _keepRunning = false;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InstantiateWorkerThread();
        }
        /// <summary>
        /// инициализация 
        /// </summary>
        private void InstantiateWorkerThread()
        {
            workerThread = new BackgroundWorker();
            workerThread.ProgressChanged += WorkerThread_ProgressChanged;
            workerThread.DoWork += WorkerThread_DoWork;
            workerThread.RunWorkerCompleted += WorkerThread_RunWorkerCompleted;
            workerThread.WorkerReportsProgress = true;
            workerThread.WorkerSupportsCancellation = true;

        }

        private void WorkerThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
          //  lb_test.Text = e.UserState.ToString();// показывает прогресс
        }

        private void WorkerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
          //      lb_test.Text = "Cancelled";
            }
            else
            {
           //     lb_test.Text = "Stopped";
            }
        }

        private void WorkerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime startTime = DateTime.Now;


            _keepRunning = true;

            while (_keepRunning)
            {
                Thread.Sleep(1000);

                string timeElapsedInstring = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss");
                //lb_time.Invoke(new UpdateLabel(UpdateUI), timeElapsedInstring); // получаем управление

                workerThread.ReportProgress(0, timeElapsedInstring);// отправляем результат работы

                if (workerThread.CancellationPending)
                {
                    // this is important as it set the cancelled property of RunWorkerCompletedEventArgs to true
                    e.Cancel = true;
                    break;
                }
            }


        }

        private void bt_start_Click(object sender, EventArgs e)
        {
            workerThread.RunWorkerAsync();
        }

        private void bt_stop_Click(object sender, EventArgs e)
        {
            _keepRunning = false;
        }

        private void bt_cancel_Click(object sender, EventArgs e)
        {
            workerThread.CancelAsync();
        }

        public void asdddd()
        {
            Thread t3 = new Thread(p =>
            {
                int numberOfSeconds = 0;
                while (numberOfSeconds < Convert.ToInt32(p))
                {
                    Thread.Sleep(1000);

                    numberOfSeconds++;
                }
                Console.WriteLine("I ran for {0} seconds", numberOfSeconds);
            });
        }

        

        private void UpdateUI( string labelText)// управление на лайбел
        {
           // lb_time.Text = labelText;
        }

       
        



    }
}

