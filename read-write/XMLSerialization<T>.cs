   //блилиотеки
   using System;
   using System.IO;
   using System.Xml.Serialization;

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
        /// <returns>возврашает object приведеный к типу</returns>
        public T ReadData(string filePath)
        {
           
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer xser = new XmlSerializer(typeof(T));
                    _obj = (T)xser.Deserialize(fs);
                    fs.Close();
                }
                return (T)Convert.ChangeType(_obj, typeof(T));
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
    
// Пример: 

// класс
public class TestClass// должен быть публичным 
    {
        public int inttt { get; set; } // поля должны быть публичными 
        public string str { get; set; } // поля должны быть публичными 
        public bool b { get; set; } // поля должны быть публичными 


        public TestClass(int inttt, string str, bool b)
        {
            this.inttt = inttt;
            this.str = str;
            this.b = b;
        }

        public TestClass() // должен быть констуктор без параметров
        {
            
        }
    }

// пример использования запись в файл :
XmlSerialization<TestClass> xmlSerialization = new XmlSerialization<TestClass>();
TestClass tsClass = new TestClass(1,"string",true);
xmlSerialization.WriteData("asd.txt",tsClass);
// пример использования чтение из файла :
XmlSerialization<TestClass> xmlSerialization = new XmlSerialization<TestClass>();
TestClass tsClass = new TestClass();
tsClass = xmlSerialization.ReadData("asd.txt");




