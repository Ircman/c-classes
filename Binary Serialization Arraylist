using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace L4
{
    class Program
    {
        [Serializable]
        public struct Person
        {
            public string Name;
            public string Surname;
            public int Age;

        }

        static ArrayList persons = new ArrayList();

        static Person temPerson = new Person();

        static void Main(string[] args)
        {

            DirectoryInfo dir1 = new DirectoryInfo(@".\PERSON");
            if (!dir1.Exists)
            {
                Console.WriteLine("Folder PERSON does not exist!\nCreating folder PERSON");
                dir1.Create();
            }
            else Console.WriteLine("The folder PERSON exist!");


            FileInfo file = new FileInfo(@".\PERSON\persons.dat");
            if (!file.Exists)
            {
                Console.WriteLine("The file persons.dat does not exist!\nCreating file persons.dat");
                file.Create();
            }
            else
            {
                Console.WriteLine("The file persons.dat exist!");
                Console.WriteLine("***************************");
                Console.WriteLine("File name: {0}", file.Name);
                Console.WriteLine("File size: {0}", file.Length);
                Console.WriteLine("Creation: {0}", file.CreationTime);
                Console.WriteLine("Attributes: {0}", file.Attributes);
                Console.WriteLine("***************************\n\n");

                ShowPerson();


            }
            AddPersons();
            // 4) Заполните ArrayList объектами Person (значение для полей объектов задает
            //  пользователь, количество объектов задает пользователь)
            // 5) Запишите в файл созданный Вами ArrayList с помощью BinaryFormatter:

            BinaryFormatter binFormat = new BinaryFormatter();
            FileStream fStream = new FileStream(@".\PERSON\persons.dat",
                FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            binFormat.Serialize(fStream, persons);
            fStream.Close();

            Console.ReadLine();

        }

        static void AddPersons()
        {
            Console.WriteLine("Сколько надо людей добавить ?");

            var count = Convert.ToInt32(Console.ReadLine());



            for (int i = 0; i < count; i++)
            {
                Console.Write("Ведите имя => ");
                temPerson.Name = Console.ReadLine();
                Console.WriteLine("\n");

                Console.Write("Ведите Фамилию => ");
                temPerson.Surname = Console.ReadLine();
                Console.WriteLine("\n");

                Console.Write("Ведите Возраст => ");
                temPerson.Age = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("\n");

                persons.Add(temPerson);
               
            }


        }

        static void ShowPerson()
        {

            FileInfo fileInfo = new FileInfo(@".\PERSON\persons.dat");
            if (fileInfo.Length <= 0) return;
            BinaryFormatter binFormat = new BinaryFormatter();
            FileStream fStream = new FileStream(@".\PERSON\persons.dat",
                FileMode.Open, FileAccess.ReadWrite, FileShare.None);

 
                persons = (ArrayList) binFormat.Deserialize(fStream);


            fStream.Close();

            Console.WriteLine("Список ранне добавленых рабочих");
            Console.Write(" Кол-во работников в базе => ");
            Console.Write(persons.Count);
            Console.WriteLine();

            int index = 1;
            foreach (Person p in persons)
            {
                Console.Write(index);
                Console.Write("Имя: ");
                Console.Write(p.Name);
                Console.Write("   Фамилия: ");
                Console.Write(p.Surname);
                Console.Write("   Возраст: ");
                Console.Write(p.Age);
                index++;
                Console.WriteLine();
            }
            //for (int i = 0; i < persons.Count; i++)
            //{
            //    Console.WriteLine(persons[i].ToArray());
            //}
            //Console.WriteLine("\n");

          
        }
    }
}
