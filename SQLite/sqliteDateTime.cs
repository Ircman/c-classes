 class SQLliteDateTime
    {
        private DateTime _curDateTime;

        public SQLliteDateTime()
        {
            _curDateTime = DateTime.Now;
        }


        /// <summary>
        /// <para>0=YYYY-MM-DD HH:MM:SS</para>
        /// <para>1=YYYY-MM-DD</para>
        /// <para>2=HH:MM:SS</para>
        /// <para>3=HH:MM</para>
        /// </summary>
        /// <param name="choice"></param>
        /// <returns></returns>
        public string GetDateTime(int choice)
        {
            switch (choice)
            {
                case 0:
                {
                    UpdateDate();
                    return string.Format("{0}-{1}-{2} {3}:{4}:{5}", _curDateTime.Year, Convert(_curDateTime.Month),
                        Convert(_curDateTime.Day), _curDateTime.Hour, Convert(_curDateTime.Minute), Convert(_curDateTime.Second));
                }
                case 1:
                {
                    UpdateDate();
                    return string.Format("{0}-{1}-{2}", _curDateTime.Year, Convert(_curDateTime.Month),
                        Convert(_curDateTime.Day));
                }
                case 2:
                {
                    UpdateDate();
                    return string.Format("{0}:{1}:{2}",_curDateTime.Hour, Convert(_curDateTime.Minute), Convert(_curDateTime.Second));
                }
                case 3:
                {
                    UpdateDate();
                    return string.Format("{0}:{1}", _curDateTime.Hour, Convert(_curDateTime.Minute));
                }

                default:
                {
                    UpdateDate();
                    return string.Format("{0}-{1}-{2} {3}:{4}:{5}", DateTime.Now.Year, Convert(DateTime.Now.Month),
                        Convert(DateTime.Now.Day), DateTime.Now.Hour, Convert(DateTime.Now.Minute), Convert(DateTime.Now.Second));
                }
            }
        }

        public void AddDays(int days)
        {
           _curDateTime = DateTime.Now.AddDays(days);
        }

        private void UpdateDate()
        {
            _curDateTime = DateTime.Now;
        }

        public void AddMonths(int Month)
        {
            _curDateTime = DateTime.Now.AddMonths(Month);
        }

        public void AddMinutes(int Minutes)
        {
            _curDateTime = DateTime.Now.AddMinutes(Minutes);
        }

        private string Convert(int number)
        {

            if (number < 10)
            {
                return string.Format("0{0}", number);
            }
            else
            {
                return number.ToString();
            }
        }
    }
