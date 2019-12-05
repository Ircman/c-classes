if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        bt_startUpdate.Visible = true;
                        infoForm.Hide();
                    }));
                }


----------------------------------------------------- второй способ
   delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        /// <summary>
        /// Metoд позволяет получать контроль над элементами в другом процессе
        /// </summary>
        /// <param name="oControl">тип(TextBox)</param>
        /// <param name="propName">Свойство (Text)</param>
        /// <param name="propValue">Новое значение</param>
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)

        {
            if (oControl.InvokeRequired)

            {
                SetControlValueCallback d = SetControlPropertyValue;

                oControl.Invoke(d, oControl, propName, propValue);
            }

            else

            {
                var t = oControl.GetType();

                var props = t.GetProperties();

                foreach (var p in props)

                    if (p.Name.ToUpper() == propName.ToUpper())

                        p.SetValue(oControl, propValue, null);
            }
        }
