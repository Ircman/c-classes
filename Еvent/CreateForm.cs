/// <summary>
    /// Форма 2 создание студента
    /// </summary>
    public partial class CreateForm : Form
    {
        //делагат 
        public delegate void StudentCreated();
        //событие создание нового студента
        public event StudentCreated OnStudentCreated;
        //класс-переменная студента
        public Student CreatedStuden;
        
        public CreateForm()
        {
            InitializeComponent();
        }


        private void CreateForm_Load(object sender, EventArgs e)
        {
           
        }

        //кнопка создать нового пользователя
        private void btn_create_Click(object sender, EventArgs e)
        {
            //проверяем все ли поля заполнил пользователь 
            if(tx_name.Text.Length>0 && tx_surname.Text.Length>0 && tx_age.Text.Length>0)
            {   
                string name = tx_name.Text;
                string surname = tx_surname.Text;
                int age = 0;
                //проверям вел ли пользователь цифры 
                if (Int32.TryParse(tx_age.Text, out age))
                {

                    CreatedStuden = new Student(name, surname, age);
                    OnStudentCreated();
                    Hide();
                }
                else//ошибка пользователь ввел в место возраста что то другое
                {
                    MessageBox.Show("Age field can contain only digits", "Error message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else//ошибка не все поля были заполнены верно
            {
                MessageBox.Show("Can't create new user one of fields is empty", "Error message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
