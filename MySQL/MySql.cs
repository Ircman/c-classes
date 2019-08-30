 using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ProjectManager.ClassFolder
{
    class DbMySQL
    {
        private static string host = "192.168.100.9";
        private static int port = 3306;
        private static string database = "itdb_update";
        private static string username = "kirils.iovenko";
        private static string password = "tuMEdIWo";

        /// <summary>
        /// Метод формирует строку соеденнения
        /// </summary>
        /// <returns>Возврашает String с параметрями для соеденения </returns>
        private static string GetConnection()
        {
            // Connection String.
            String connString = "Server=" + host + ";Database=" + database + ";port=" + port + ";User Id=" + username + ";password=" + password;
            //MySqlConnection connection = new MySqlConnection(connString);
            return connString;
        }
        /// <summary>
        /// Метод задает параметры для соедененния с базой MySQL
        /// </summary>
        /// <param name="hostName">Адресс</param>
        /// <param name="portN">порт</param>
        /// <param name="databaseName">База данных</param>
        /// <param name="usr">Логин</param>
        /// <param name="pwd">пароль</param>
        public static void SetConnection(string hostName, int portN, string databaseName, string usr, string pwd)
        {
            host = hostName;
            port = portN;
            database = databaseName;
            username = usr;
            password = pwd;
        }

        /// <summary>
        /// метот проверяет установлено соеденнение или нет
        /// </summary>
        /// <returns>TRUE or FALSE</returns>
        public bool IsConnect()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(GetConnection());
                connection.Open();
               var s = connection.State.ToString();
                if (s == "Open")
                {
                    if (String.IsNullOrEmpty(database))
                        return false;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        /// <summary>
        /// Метод закрывает соеденнение 
        /// </summary>
        public void Close()
        {
            
        }

        /// <summary>
        /// Метод который делает запрос к базе MySQL
        /// </summary>
        /// <param name="query">строка запроса</param>
        /// <param name="columnCount">Кол-во возврашаемых параметров</param>
        /// <returns></returns>
        public List<string[]> QueryList(string query, int columnCount)
        {
            List<string[]> responseSQL = new List<string[]>();
            if (IsConnect())
            {
                string[] sqlData;
             //   var cmd = new MySqlCommand(query, GetConnection());
              //  var reader = cmd.ExecuteReader();
           //     while (reader.Read())
                {
                    sqlData = new string[columnCount];
                    for (int i = 0; i < sqlData.Length; i++)
                    {
                //        sqlData[i] = Convert.ToString(reader.GetValue(i));
                    }
                    responseSQL.Add(sqlData);
                }
            //    reader.Close();
            }
            Close();
            return responseSQL;
        }

        public void Insert(List<string>data)
        {
            Crc32  crc32 = new Crc32();
            
            if (IsConnect())
            {
                MySqlConnection connection = new MySqlConnection(GetConnection());
                connection.Open();
                foreach (var path in data)
                {
                    string hash = crc32.GetFileCrc32(path);
                    string query = string.Format("INSERT INTO test (filename,crc) VALUES ('{0}', '{1}');",path,hash);
                   

                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(path, hash);
                    cmd.ExecuteNonQuery();
                }
            }
            Close();
        }
    }
}
