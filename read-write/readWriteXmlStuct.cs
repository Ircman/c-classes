  public struct WorkerDataStruct
        {
           public string Name;
           public string Surname;
           public string Email;
           public string Password;
           public bool Childrens;
        }
 
 public  void ReadData()
        {
            //todo read user data from file
            if (File.Exists(_filePach))
            {
                using (FileStream fs = new FileStream(_filePach, FileMode.Open))
                {
                    XmlSerializer xser = new XmlSerializer(typeof(List<WorkerDataStruct>));
                    WorkerDataList = (List<WorkerDataStruct>) xser.Deserialize(fs);
                    fs.Close();
                }
            }
            else
            {
               //  MessageBox.Show(@"Не все поля заполнены", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         private void WriteData()
        {
            //todo write user data to file
            if (File.Exists(_filePach)) File.Delete(_filePach);
            using (FileStream fs = new FileStream(_filePach, FileMode.Create))
            {
                XmlSerializer xser = new XmlSerializer(typeof(List<WorkerDataStruct>));
                xser.Serialize(fs, WorkerDataList);
                fs.Close();
            }
        }
