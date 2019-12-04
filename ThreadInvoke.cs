if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        bt_startUpdate.Visible = true;
                        infoForm.Hide();
                    }));
                }
