using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProjectManager.ClassFolder
{
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
                MessageBox.Show(String.Format("Произошла ошибка\n Название класса: {0}\nНазвание метода:{1}\nПричина:{2},\n номер линии :{4} \nвнешние причины:{3}", className, metodName, msg, innerEx, line), @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(3);
            }
        }
        /// <summary>
        /// Метод выдает ошибку и записавает ее базу данных а также может завершать работу программы
        /// </summary>
        /// <param name="ex">exeption</param>
        /// <param name="killProgramm">Если True программа будет завершать работу</param>
        public static void Msg(Exception ex,bool killProgramm)
        {
            StackFrame frame = new StackFrame(1);
            var method = frame.GetMethod();
            if (method.DeclaringType != null)
            {
                var className = method.DeclaringType.ToString();
                var metodName = method.Name;
                //  DateTime date = DateTime.Now;
                string innerEx = string.Empty;
                int line = GetLineNumber(ex);
                var msg = ex.Message;
                if (ex.InnerException != null)
                {
                    innerEx = ex.InnerException.ToString();

                }
                MySql.ErrorMsg(className, metodName, msg, innerEx, line);
                MessageBox.Show(String.Format("Произошла ошибка\n Название класса: {0}\nНазвание метода:{1}\nПричина:{2},\n номер линии :{4} \nвнешние причины:{3}", className, metodName, msg, innerEx, line), @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (killProgramm)
                {
                    Environment.Exit(3);
                }
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
            var index = ex.StackTrace.LastIndexOf(lineSearch, StringComparison.Ordinal);
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
}
