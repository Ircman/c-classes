 public class Student
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public int Age { get; set; }


        public Student(string name,string surname,int age)
        {
            this.Name = name;
            this.SurName = surname;
            this.Age = age;
        }
    }
