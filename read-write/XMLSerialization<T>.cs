/// <summary>
    /// Класс для XML сериализации/десериализации 
    /// </summary>
    /// <typeparam name="T">Любой тип класса</typeparam>
    class XmlSerialization<T>
    {
        private Object _obj = typeof(T);
        /// <summary>
        /// Метод десриализует файл в формате XML
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="obj">Переменная класса</param>
        /// <returns>возврашает object который надо приводить к типу класса (ClassType)</returns>
        public Object ReadData(string filePath, Object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer xser = new XmlSerializer(typeof(T));
                    obj = (T)xser.Deserialize(fs);
                    fs.Close();
                }
                return obj;
            }   
                throw new Exception("file does not exist");
        }
        /// <summary>
        /// Метод сериализует класс в формат XML и пишет его в файл
        /// </summary>
        /// <param name="filePath">путь к файлу для записи</param>
        /// <param name="obj">Переменная класса</param>
        public void WriteData(string filePath, Object obj)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                XmlSerializer xser = new XmlSerializer(typeof(T));
                xser.Serialize(fs, obj);
                fs.Close();
            }
        }
    }
    
#read 
