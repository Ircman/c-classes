using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace TestDB1111
{
    public class SQLite
    {

        private SQLiteConnection _connection;
        /// <summary>
        /// Путь к базе данных (полный или из папки с программой)
        /// </summary>
        private static string _dbPath = @"tetpwd.db";
        /// <summary>
        /// версия базы данных
        /// </summary>
        private static string _version = "3";
        /// <summary>
        /// Пароль если он нужен
        /// </summary>
        private static string _password="123456";
        /// <summary>
        /// Метод возврашает строку для соеденнения с базой SQLite
        /// </summary>
        /// <returns></returns>
        private static string GetConnection()
        {
            if (string.IsNullOrWhiteSpace(_password))
            {
                return string.Format("Data Source={0};{1};",_dbPath,_version);
            }
            else
            {
                return string.Format("Data Source={0};Version={1};password={2};", _dbPath, _version, _password);
            }
        }
        /// <summary>
        /// Метод проверяет открыто ли соеденение с базой
        /// </summary>
        /// <returns></returns>
        private bool IsConnected()
        {
            if (_connection.State.ToString() == "Open")
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Метод который делает запрос к базе MySQL
        /// </summary>
        /// <param name="query">строка запроса</param>
        /// <param name="columnCount">Кол-во возврашаемых параметров</param>
        /// <returns>List(string[])</returns>
        public List<string[]> QueryList(string query, int columnCount)
        {
            try
            {
                _connection = new SQLiteConnection(GetConnection());
                _connection.Open();
                if (IsConnected())
                {
                    List<string[]> responseSql = new List<string[]>();

                    var cmd = _connection.CreateCommand();
                    cmd.CommandText = query;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var sqlData = new string[columnCount];
                        for (int i = 0; i < sqlData.Length; i++)
                        {
                            sqlData[i] = Convert.ToString(reader.GetValue(i));
                        }
                        responseSql.Add(sqlData);
                    }
                    reader.Close();
                    _connection.Close();
                    return responseSql;
                }
                return null;
            }
            catch (SQLiteException e)
            {
                return null;
            }
            finally
            {
                if (_connection != null)
                {
                    _connection.Close();
                }
            }
        }

        /// <summary>
        /// Выполнить команду в базе Update/delete/insert (НЕ Запрос не чего не возврашает)
        /// </summary>
        /// <param name="query">Запрос</param>
        public void SingleQuery(string query)
        {
            try
            {
                _connection = new SQLiteConnection(GetConnection());
                _connection.Open();
                if (IsConnected())
                {
                    var cmd = _connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    _connection.Close();
                }
            }
            catch (SQLiteException e)
            {
              
            }
            finally
            {
                if (_connection != null)
                {
                    _connection.Close();
                }
            }
        }

        /// <summary>
        /// Метод добавляет данные в базу данных 
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="dictionary">словарь где Key= column , value = value</param>
        public void Insert(string table, Dictionary<string, string> dictionary)
        {
            
            try
            {
                _connection = new SQLiteConnection(GetConnection());
              
                    var cmd = _connection.CreateCommand();

                    string column = "";
                    string value = "";
                    string querty = "";

                    for (int i = 0; i < dictionary.Count; i++)
                    {
                        if (i == 0)
                        {
                            column += "(";
                            column += dictionary.ElementAt(i).Key;
                            value += "(";
                            value += "@" + dictionary.ElementAt(i).Key;
                            cmd.Parameters.AddWithValue(string.Format("@{0}", dictionary.ElementAt(i).Key), dictionary.ElementAt(i).Value);
                            continue;
                        }
                        if (i == dictionary.Count - 1)
                        {
                            column += ",";
                            column += dictionary.ElementAt(i).Key;
                            column += ")";
                            value += ",";
                            value += "@" + dictionary.ElementAt(i).Key;
                            value += ")";
                            cmd.Parameters.AddWithValue(string.Format("@{0}", dictionary.ElementAt(i).Key), dictionary.ElementAt(i).Value);
                        }
                        else
                        {
                            column += ",";
                            column += dictionary.ElementAt(i).Key;
                            value += ",";
                            value += "@"+dictionary.ElementAt(i).Key ;
                            cmd.Parameters.AddWithValue(string.Format("@{0}", dictionary.ElementAt(i).Key), dictionary.ElementAt(i).Value);
                        }

                    }
                    querty = string.Format("INSERT INTO {0} {1} VALUES {2};",table,column,value);
                    cmd.CommandText = querty;
                    _connection.Open();
                    cmd.ExecuteNonQuery();
                    _connection.Close(); 
            }
            catch (SQLiteException e)
            {

            }
        }
        /// <summary>
        /// Метод удаляет данные из указанной таблицы
        /// </summary>
        /// <param name="table">Название таблицы</param>
        public void ClearTable(string table)
        {
            try
            {
                _connection = new SQLiteConnection(GetConnection());
                _connection.Open();
                if (IsConnected())
                {

                    string query = string.Format("DELETE  from {0} ;", table);
                    var cmd = _connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    _connection.Close();
                }
            }
            catch (SQLiteException e)
            {

            }
            finally
            {
                if (_connection!=null)
                {
                    _connection.Close();
                }
            }
        }

    }
}
