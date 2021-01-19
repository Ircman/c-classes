    /// <summary>
    /// Форма номер 1
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Список содержит список всех студентов
        /// </summary>
        private List<Student> StudentsList;
        /// <summary>
        /// Переменная воторой формы
        /// </summary>
        CreateForm createFrom;
        /// <summary>
        /// конструктор первой формы 
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            //инизиализируем список студентов
            StudentsList = new List<Student>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //кнопка номер 1 октрываёт форму (CreateForm) где мы добавляем пользователя
        private void button1_Click(object sender, EventArgs e)
        {
            //инициализируем форму 2
            createFrom = new CreateForm();
            //подписаваемся на событые дабовление нового студента во второй форме 
            createFrom.OnStudentCreated += AddNewStudent;
            //показываем диалог второй формы
            createFrom.ShowDialog();
            //отписаваемся от события добавление нового студента 
            createFrom.OnStudentCreated -= AddNewStudent;

        }

        /// <summary>
        /// Метод добаляет нового пользователя в список
        /// </summary>
        private void AddNewStudent()
        {
            //проверка на null
            if (createFrom != null)
            {
                //проверка на null
                if (createFrom.CreatedStuden != null)
                {   //добаляем нового пользовтеля в список
                    StudentsList.Add(createFrom.CreatedStuden);
                }
            }
        }
    }
    
