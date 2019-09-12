 public static class Error
    {
        static readonly DbMySQL MySql = new DbMySQL();
        /// <summary>
        /// Метод выдает ошибку и записавает ее базу данных
        /// </summary>
        /// <param name="ex">exeption</param>
        public static void Msg(Exception ex)
        {
            StackFrame frame = new StackFrame(1);
            var method = frame.GetMethod();
            if (method.DeclaringType != null)
            {
                var className = method.DeclaringType.ToString();
                var metodName = method.Name;
              //  DateTime date = DateTime.Now;
                string innerEx=string.Empty;
                int line = GetLineNumber(ex);
                var msg = ex.Message;
                if (ex.InnerException != null)
                {
                    innerEx = ex.InnerException.ToString();
                   
                }

                MySql.ErrorMsg(className, metodName, msg, innerEx,line);
                MessageBox.Show(String.Format("Произошла ошибка\n Название класса: {0}\nНазвание метода:{1}\nПричина:{2},\n номер линии :{4} \nчто вызвало:{3}", className, metodName, msg, innerEx, line), @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Метод получает номер строки
        /// </summary>
        /// <param name="ex">exeption</param>
        /// <returns>int line number</returns>
        private static int GetLineNumber(Exception ex)
        {
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = ex.StackTrace.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
            }
            return lineNumber;
        }

    
    }
