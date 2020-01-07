Добавить из ToolBox
        /// <summary>
        /// Уберает форму при -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Deactivate(object sender, EventArgs e)
        {
           
                if (this.WindowState == FormWindowState.Minimized) 
                { 
                    this.ShowInTaskbar = false; 
                    notifyIcon1.Visible = true; 
                } 
               
        }
        //показывает форму 
        private void notifyIcon2_Click(object sender, EventArgs e)
        {
            
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }
