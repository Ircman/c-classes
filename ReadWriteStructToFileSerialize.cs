using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using DocumentFormat.OpenXml.Office2010.PowerPoint;


namespace testingTSI
{
    //Класс сериализует и записавает стуктуру в хмл формат 
    public class StructWriteBinary
    {
        private string _path = "data.struct";

        [Serializable]
        public struct TestStruct
        {
            public int Value1 { get; set; }
            public int Value2 { get; set; }
            public string Value3 { get; set; }
            public double Value4 { get; set; }
            public string Value5 { get; set; }
        }
        public TestStruct t1 = new TestStruct();
        public List<TestStruct> ListTestStructs = new List<TestStruct>();  

        /// <summary>
        /// Метод добовляет в список данные 
        /// </summary>
        /// <param name="textBox1"></param>
        /// <param name="textBox2"></param>
        /// <param name="textBox3"></param>
        /// <param name="textBox4"></param>
        /// <param name="textBox5"></param>
        public void AddData(TextBox textBox1,TextBox textBox2,TextBox textBox3,TextBox textBox4,TextBox textBox5)
        {
            t1.Value1 = Convert.ToInt32(textBox1.Text);
            t1.Value2 = Convert.ToInt32(textBox2.Text);
            t1.Value3 = textBox3.Text;
            t1.Value4 = Convert.ToDouble(textBox4.Text);
            t1.Value5 = textBox5.Text;

            ListTestStructs.Add(t1);

            SaveToFile(_path);
        }
        /// <summary>
        /// метод удаляет все стуктуры где datatoremove поподает под значение
        /// </summary>
        /// <param name="datatoremove">Инт который подходит ишем в первом параметре стуктуры</param>
        public void RemoveAllData(int datatoremove)
        {
      
            ListTestStructs.RemoveAll(i => i.Value1 == datatoremove);
        }
        /// <summary>
        /// Метод сохраняет в файл структуру
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        /// <returns></returns>
        public bool SaveToFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = _path;
            }
            FileStream stream = File.Open(filePath, FileMode.Create,FileAccess.Write);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<TestStruct>));
                serializer.Serialize(stream, ListTestStructs);

                stream.Close();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            finally
            {
                stream.Close();
            }
        }
        /// <summary>
        /// Метод считавет данные из файла
        /// </summary>
        /// <param name="filePath">Путь к файлу </param>
        /// <returns></returns>
        public bool ReadFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = _path;
            }
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                

                XmlSerializer serializer = new XmlSerializer(typeof(List<TestStruct>));
                ListTestStructs = (List<TestStruct>) serializer.Deserialize(stream);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                stream.Close();
            }
        }

       

    }
}
