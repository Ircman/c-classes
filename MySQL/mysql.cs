using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ProjectManager.Forms;

namespace ProjectManager.ClassFolder
{
    class DbMySQL
    {
        private static string Host { get; set; }
        private static int Port { get; set; }
        private static string Database { get; set; }
        private static string Username { get; set; }
        private static string Password { get; set; }
        readonly WorkWithFiles _wf = new WorkWithFiles();
        /// <summary>
        /// Конструктор
        /// </summary>
        public DbMySQL()
        {
            ReadConnectionData();
        }
        /// <summary>
        /// Метод формирует строку соеденнения
        /// </summary>
        /// <returns>Возврашает String с параметрями для соеденения </returns>
        private static string GetConnection()
        {
            // Connection String.
            String connString = "Server=" + Host + ";Database=" + Database + ";port=" + Port + ";User Id=" + Username + ";password=" + Password;
            //MySqlConnection connection = new MySqlConnection(connString);
            return connString;
        }
        /// <summary>
        /// Метод формирует строку соеденнения
        /// </summary>
        /// <returns>Возврашает MySqlConnection с параметрями для соеденения </returns>
        private MySqlConnection GetConnectionM()
        {
            // Connection String.
            String connString = "Server=" + Host + ";Database=" + Database + ";port=" + Port + ";User Id=" + Username + ";password=" + Password;
           MySqlConnection connection = new MySqlConnection(connString);
            return  connection;
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
            Host = hostName;
            Port = portN;
            Database = databaseName;
            Username = usr;
            Password = pwd;
        }
        /// <summary>
        /// Метод считавает данные для соеденнения с MySQL из файла
        /// </summary>
        public string[] ReadConnectionData()
        {
            try
            {
                if (File.Exists(GlobalConfig.MySqlConfigFile))
                {
                    string[] mysqlConfData = new string[5];
                    mysqlConfData = File.ReadAllLines(GlobalConfig.MySqlConfigFile);
                    Host = mysqlConfData[0];
                    Port = Convert.ToInt32(mysqlConfData[1]);
                    Username = mysqlConfData[2];
                    Password = mysqlConfData[3];
                    Database = mysqlConfData[4];
                    return mysqlConfData;
                }
                return null;
            }
            catch (Exception e)
            {
                Error.Msg(e);
                return null;
            }
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
                    if (String.IsNullOrEmpty(Database))
                    {
                        connection.Close();
                        return false;
                    }
                }
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                Error.Msg(e);
                return false;
            }
        }
        /// <summary>
        /// Метод который делает запрос к базе MySQL
        /// </summary>
        /// <param name="query">строка запроса</param>
        /// <param name="columnCount">Кол-во возврашаемых параметров</param>
        /// <returns></returns>
        public List<string[]> QueryList(string query, int columnCount)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(GetConnection());
                List<string[]> responseSql = new List<string[]>();
                if (IsConnect())
                {
                    connection.Open();
                    var cmd = new MySqlCommand(query, connection);
                    //var cmd = connection.
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
                    connection.Close();
                }
                return responseSql;
            }
            catch (Exception e)
            {
                Error.Msg(e);
                return null;
            }
        }

        /// <summary>
        /// Метод удаляет все данные из таблице Test, а потом добавляет их заного из задоного списка
        /// </summary>
        /// <param name="data"></param>
        public void Insert(List<string>data,ProgressBar progress , Label lb_progress,Label lb_time)
        {
            try
            {
                int progressIndex = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                DeleteTable("smartcar_update_packages");
                Crc32 crc32 = new Crc32();
                MySqlConnection connection = new MySqlConnection(GetConnection());
                if (IsConnect())
                {
                     
                    progress.Maximum = data.Count;
                    connection.Open();
                    foreach (var path in data)
                    {
                       
                        string hash = crc32.GetFileCrc32(path);
                        string version = _wf.FileVersion(path);
                        string[] fileData = _wf.PakagesPathParser(path);
                        string pathUpdate = path.Replace("\\", "/");
                        string query = string.Format("INSERT INTO smartcar_update_packages (file_path,file_crc,file_name,file_type, file_version) VALUES ('{0}', '{1}', '{2}', '{3}','{4}' );", @pathUpdate, hash, fileData[1], fileData[0],version);
                        MySqlCommand cmd = connection.CreateCommand();
                        cmd.CommandText = query;
                        //cmd.Parameters.Add(@pathUpdate, hash);
                        cmd.ExecuteNonQuery();
                        lb_progress.Text = string.Format("Curent progress: {0}/{1}", progressIndex, data.Count);
                        lb_progress.Refresh();
                        var runTime = stopwatch.Elapsed;
                        lb_time.Text =string.Format("время сканирования :{0:hh\\:mm\\:ss}", runTime);
                        lb_time.Refresh();
                        progressIndex++;
                        progress.Value = progressIndex;
                        progress.Refresh();
                    }
                }
                ScanForm.ProgressIndex = 0;
                stopwatch.Stop();
                var time = stopwatch.Elapsed;
                MessageBox.Show(string.Format("Сканирование завершено:\n Файлов добавлено в базу:{0}\n время сканирования :{1:hh\\:mm\\:ss}", data.Count,time));
                connection.Close();
            }
            catch (Exception e)
            {
                Error.Msg(e);
            }
        }

        /// <summary>
        /// Метод должен быть уневерсальным но не доделаным (не доделан)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableColums"></param>
        /// <param name="values"></param>
        public void Insert1(string tableName, string[] tableColums ,string[] values)
        {
            try
            {
                Crc32 crc32 = new Crc32();
                MySqlConnection connection = new MySqlConnection(GetConnection());
                if (IsConnect())
                {
                    connection.Open();
                    string columnString = String.Empty;
                    string ValuString = "VALUES (";
                    for (int i = 0; i < tableColums.Length; i++)
                    {
                        if (i == 0)
                        {
                            columnString = string.Format("({0},", tableColums[0]);
                        }
                        else
                        {
                            if (i == tableColums.Length - 1)
                            {
                                columnString += tableColums[i] + ") ";
                            }
                            else
                            {
                                columnString += tableColums[i] + ",";
                            }
                        }
                    }
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (i == 0)
                        {
                            columnString = string.Format("({0},", tableColums[0]);
                        }
                        else
                        {
                            if (i == tableColums.Length - 1)
                            {
                                columnString += tableColums[i] + ")";
                            }
                            else
                            {
                                columnString += tableColums[i] + ",";
                            }
                        }
                    }
                    string query = string.Format("INSERT INTO {0} {1} {2};", tableName, columnString, ValuString);
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    //cmd.Parameters.Add(@pathUpdate, hash);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                Error.Msg(e);
            }
        }
        /// <summary>
        /// Метод добавляет новый файл в базу данных при обновлении 
        /// </summary>
        /// <param name="customer_id">ид клиента</param>
        /// <param name="project_id">ид проекта</param>
        /// <param name="file_path">путь к файлу(на FTP)</param>
        /// <param name="file_name">название файла</param>
        /// <param name="file_crc">хеш файла</param>
        /// <param name="file_size">размер файла</param>
        /// <param name="file_version">версия файла</param>
        /// <param name="file_type">тип файла</param>
        public void InsertNewFile(int customer_id, string project_id, string file_path, string file_name, string file_crc, long file_size, string file_version, string file_type)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(GetConnection());
                if (IsConnect())
                {
                    connection.Open();
                    //INSERT INTO smartcar_update_files (customer_id,project_id,file_path,file_name,file_crc,file_size,file_version,file_type) VALUES ('2', '2.91', '/2/91/conmandll.dll', 'conmandll.dll', 'E305F8A8' ,'3215151' , 'Version', 'kernel.xp');
                    string query = string.Format("INSERT INTO smartcar_update_files (customer_id,project_id,file_path,file_name,file_crc,file_size,file_version,file_type) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}' ,'{5}' , '{6}','{7}');", customer_id, project_id, file_path.ToLower(), file_name.ToLower(), file_crc, file_size, file_version,file_type);
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    //cmd.Parameters.Add(@pathUpdate, hash);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                Error.Msg(e);
            }
        }
        /// <summary>
        /// Метод добавляет ошибку в базу данных в таблицу `errors`
        /// </summary>
        /// <param name="Class">название класса где произошла ошибк</param>
        /// <param name="Method">название метода который вызвал ошибк</param>
        /// <param name="errorMsg">сообшение ошбк</param>
        /// <param name="innerExeption">сообшение что вызвало ошибку</param>
        /// <param name="date">дата время ошбк</param>
        public void ErrorMsg(string Class,string Method,string errorMsg,string innerExeption, int linenumber)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(GetConnection());
                if (IsConnect())
                {
                    connection.Open();
                    //INSERT INTO `errors` (Class,Method,ErrorMsg,InnerExeption,Date) VALUES	('asd', '123', '123', '123','12.09.2019') ;
                    string query = string.Format("INSERT INTO `errors` (Class,Method,ErrorMsg,InnerExeption,Line) VALUES ('{0}', '{1}', '{2}', '{3}','{4}');", Class, Method, errorMsg, innerExeption,linenumber);
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                Error.Msg(e);
            }
        }
        /// <summary>
        /// Метод обновляет данные в таблице smartcar_update_file для конкретного файла
        /// </summary>
        /// <param name="crc32">хеш файла</param>
        /// <param name="filesize">размер файла</param>
        /// <param name="version">версия файла</param>
        /// <param name="proectId">ид проекта</param>
        /// <param name="fileName">название файла</param>
        public void UpdateFile(string crc32,long filesize , string version, string proectId, string fileName)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(GetConnection());
                if (IsConnect())
                {
                    //UPDATE smartcar_update_files SET file_crc ='522F861A' , file_size='24793', file_version='NULL'  WHERE project_id = '2.91' AND file_name ='loader_driver.ini'
                    connection.Open();
                    string query = string.Format("UPDATE smartcar_update_files SET file_crc ='{0}' , file_size='{1}', file_version='{2}'  WHERE project_id = '{3}' AND file_name ='{4}';", crc32, filesize, version, proectId, fileName);
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception e)
            {
               Error.Msg(e);
            }
        }
        /// <summary>
        /// Метод удаляет данные из указанной таблицы
        /// </summary>
        /// <param name="table"></param>
        public void DeleteTable(string table)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(GetConnection());
                if (IsConnect())
                {
                    connection.Open();
                    string query = string.Format("DELETE  from {0} ;", table);
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
               Error.Msg(ex);
            }
        }
        /// <summary>
        /// Метод обновляет данные для файла Loader_driver.ini
        /// </summary>
        /// <param name="proectId"></param>
        /// <param name="crc32"></param>
        public void UpdateForIni(string proectId, string crc32)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(GetConnection());
                if (IsConnect())
                {
                    connection.Open();
                    string query =
                        string.Format("UPDATE smartcar_update  SET file_crc='{0}' WHERE project_id ='{1}' AND `comment`LIKE'%D%LOADER_DRIVER.INI';",crc32, proectId);
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                Error.Msg(e);
            }
        } 
        /// <summary>
        /// Метод обновляет Loader.exe в базе 
        /// </summary>
        /// <param name="proectId">ид проекта</param>
        /// <param name="crc32">хеш файла</param>
        public void UpdateForLoader(string proectId, string crc32)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(GetConnection());
                if (IsConnect())
                {
                    //UPDATE smartcar_update u SET u.file_crc='------' WHERE u.project_id = '2.91' AND u.`comment`LIKE 'D%LOADER.EXE';
                    connection.Open();
                    string query =string.Format("UPDATE smartcar_update u SET u.file_crc='{0}' WHERE u.project_id = '{1}' AND u.`comment`LIKE 'D%LOADER.EXE';", crc32, proectId);
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                Error.Msg(e);
            }
        }
        /// <summary>
        /// Метод обновляет Bloader.exe в базе 
        /// </summary>
        /// <param name="proectId">ид проекта</param>
        /// <param name="crc32">хеш файла</param>
        public void UpdateForLoaderBb(string proectId, string crc32)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(GetConnection());
                if (IsConnect())
                {
                    //UPDATE smartcar_update u SET u.file_crc='------' WHERE u.project_id = '2.91' AND u.`comment`LIKE 'D%LOADER.EXE';
                    connection.Open();
                    string query =string.Format("UPDATE smartcar_update u SET u.file_crc='{0}' WHERE u.project_id = '{1}' AND u.`comment`LIKE 'C%LOADER.EXE';", crc32, proectId);
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                Error.Msg(e);
            }
        }
        /// <summary>
        /// Метод проверяет есть ли такой тип в базе 
        /// </summary>
        /// <param name="type">тип</param>
        /// <returns>True если тип есть</returns>
        public bool IstypeExist(string type)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(GetConnection());
                List<string[]> responseSql = new List<string[]>();
                if (IsConnect())
                {
                    string query = string.Format("SELECT project_id from smartcar_update_files WHERE file_type ='{0}' ",type);
                    connection.Open();
                    var cmd = new MySqlCommand(query, connection);
                    //var cmd = connection.
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var sqlData = new string[1];
                        for (int i = 0; i < sqlData.Length; i++)
                        {
                            sqlData[i] = Convert.ToString(reader.GetValue(i));
                        }
                        responseSql.Add(sqlData);
                    }
                    reader.Close();
                    connection.Close();
                }

                if (responseSql.Count > 0)//если получили такой тип из базы
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Error.Msg(e);
                return false;
            }

        }
        /// <summary>
        /// Метод возврашает полный путь к файлу на ftp
        /// </summary>
        /// <param name="projectId">ид проекта</param>
        /// <param name="fileName">название файла</param>
        /// <param name="packType">тип файла</param>
        /// <returns></returns>
        public string GetFilePath(string projectId, string fileName, string packType)
        {
        //SELECT * FROM smartcar_update_files  WHERE project_id='3.10' AND file_name LIKE 'ok.png' AND (file_type='smartmedic' OR file_type IS NULL) 
            string query = string.Format("SELECT file_path FROM smartcar_update_files  WHERE project_id='{0}' AND file_name ='{1}' AND (file_type='{2}' OR file_type IS NULL)", projectId, fileName,packType);
            try
            {
                MySqlConnection connection = new MySqlConnection(GetConnection());
                var sqlData = new string[1];
                if (IsConnect())
                {
                    connection.Open();
                    var cmd = new MySqlCommand(query, connection);
                    //var cmd = connection.
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {                    
                        for (int i = 0; i < sqlData.Length; i++)
                        {
                            sqlData[i] = Convert.ToString(reader.GetValue(i));
                        }                       
                    }
                    reader.Close();
                    connection.Close();
                }
                if (sqlData.Length == 1)
                {
                    if (sqlData[0] != null)
                    {
                        return sqlData[0];
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Error.Msg(e);
                return null;
            }

           
        }
    }
}
