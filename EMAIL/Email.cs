using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace OXYGEN
{
    class Email
    {
        private string _sendermail = "em";
        private string _mailTo = "";
        private string _subject="";
        private string _mainBodyText="";
        private string _filename;
        /// <summary>
        /// конфигурация сервера STMP 
        /// </summary>
        SmtpClient smtp = new SmtpClient();
        /// <summary>
        /// кокнфигурация отправлений и вложений
        /// </summary>
        MailMessage MailDetails = new MailMessage();
        /// <summary>
        /// Метод для конфигурации сервера STMP 
        /// </summary>
        /// <param name="port">порт</param>
        /// <param name="host">хост</param>
        /// <param name="SSL">использовать SSL TRUE = да</param>
        public void STMPconfig(int port,string host, bool SSL)
        {
            smtp.Port = port;
            smtp.Host = host;
            smtp.EnableSsl = SSL;
        }
        /// <summary>
        /// Метод для конфигурации сообшения
        /// </summary>
        /// <param name="senderEmail">Адресс отправителя</param>
        /// <param name="mailTo">Адресс получателя</param>
        /// <param name="subject">Тема сообшения</param>
        /// <param name="isBodyHtml">Включить подержку HTML TRUE = да </param>
        /// <param name="mainBodyText">Основной текст сообшения</param>
        public void MailDetailsConfig(string senderEmail, string mailTo, string subject, bool isBodyHtml, string mainBodyText)
        {
            try
            {
                if (string.IsNullOrEmpty(senderEmail) == false)
                    _sendermail = senderEmail;
                if (string.IsNullOrEmpty(mailTo) == false)
                    _mailTo = mailTo;
                if (string.IsNullOrEmpty(subject) == false)
                    _subject = subject;
                if (string.IsNullOrEmpty(mainBodyText) == false)
                    _mainBodyText = mainBodyText;

                MailDetails.From = new MailAddress(_sendermail);
                MailDetails.To.Add(_mailTo);
                MailDetails.Subject = _subject;
                MailDetails.IsBodyHtml = isBodyHtml;
                MailDetails.Body = _mainBodyText;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Метод используется если надо добавить больше одного получателя
        /// </summary>
        /// <param name="mailTo"></param>
        public void AddMassMailTo(string mailTo)
        {
            if (string.IsNullOrEmpty(mailTo) == false)
                MailDetails.To.Add(mailTo);
        }
        /// <summary>
        /// Метод прикрепляет файлы к письму
        /// </summary>
        public void Addattachment()
        {
            try
            {
                using (OpenFileDialog opf = new OpenFileDialog())
                {
                    if (opf.ShowDialog() == DialogResult.OK)
                    {
                        _filename = opf.FileName;
                    }
                    if (string.IsNullOrEmpty(_filename) == false)
                    {
                        Attachment attachment = new Attachment(_filename);
                        MailDetails.Attachments.Add(attachment);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Метод отравляет почту
        /// </summary>
        public void SendMail()
        {
            try
            {
                smtp.Send(MailDetails);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
    }
}
