


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSI.Classes
{  
   internal static class  RSAcrypt
   {
       private static RSAParameters _publickey;
       private static RSAParameters _privatekey;
       /// <summary>
       /// Публичный код для шифрования
       /// </summary>
       private static string _publicKey;
       /// <summary>
       /// Частный ключ для дешефровки
       /// </summary>
       private static string _privateKey;


       public enum keySizes
       {
           Size512 = 512,
           Size1024 = 1024,
           Size2048 = 2048,
           Size952 = 952,
           Size136 = 136
       };

       /// <summary>
       /// Частный ключ для дешефровки
       /// </summary>
       public static string PrivateKey
       {
           get { return _privateKey; }
           set { _privateKey = value; }
       }

       /// <summary>
       /// Публичный код для шифрования
       /// </summary>
       public static string PublicKey
       {
           get { return _publicKey; }
           set { _publicKey = value; }
       }

       /// <summary>
       /// Метод генерирует два ключа публичный и часный и сохраняет их файлы
       /// </summary>
       public static void GenerateKeys() //создание ключей 
       {
           try
           {
               if (Directory.Exists(GlobalConfig.TempFolder) == false)
               {
                   Directory.CreateDirectory(GlobalConfig.Data);
                   Directory.CreateDirectory(GlobalConfig.TempFolder);
               }
               using (var rsa = new RSACryptoServiceProvider((int) keySizes.Size2048))
               {
                   if (File.Exists(GlobalConfig.PublicKey))
                       File.Delete(GlobalConfig.PublicKey);
                   if (File.Exists(GlobalConfig.PrivateKey))
                       File.Delete(GlobalConfig.PrivateKey);

                   rsa.PersistKeyInCsp = false;
                   PublicKey = rsa.ToXmlString(false);
                   File.WriteAllText(GlobalConfig.PublicKey, PublicKey);

                   PrivateKey = rsa.ToXmlString(true);
                   File.WriteAllText(GlobalConfig.PrivateKey, PrivateKey);
               }
           }
           catch (Exception e)
           {
               //TODO:: обработчик ошибок
           }
       }

       public static void GetKeys()
       {
           if (File.Exists(GlobalConfig.PublicKey))
           {
               PublicKey = File.ReadAllText(GlobalConfig.PublicKey);
           }
           if (File.Exists(GlobalConfig.PrivateKey))
           {
               PrivateKey = File.ReadAllText(GlobalConfig.PrivateKey);
             //  File.Delete(GlobalConfig.PrivateKey);
           }   

              
       }

       /// <summary>
       /// Метод дешифрует используя Часный ключ
       /// </summary>
       /// <param name="text">текс для шифрования</param>
       /// <returns></returns>
       public static string Decrypt(string text)
       {
           try
           {
               byte[] decrContent = null;

               RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
               rsa.FromXmlString(PrivateKey);
               decrContent = rsa.Decrypt(Convert.FromBase64String(text), true);
               return _toString(decrContent);
           }
           catch (Exception e)
           {
               MessageBox.Show(e.Message, @"Ошибка дешифровки", MessageBoxButtons.OK, MessageBoxIcon.Error);
               throw;
           }
       }
       /// <summary>
       /// Метод меняет кодировку
       /// </summary>
       /// <param name="decrContent">массив байтов</param>
       /// <returns></returns>
       private static string _toString(byte[] decrContent)
       {
           return Encoding.UTF8.GetString(decrContent);
       }
       /// <summary>
       /// Метод переводит String в байты
       /// </summary>
       /// <param name="text"></param>
       /// <returns></returns>
       private static byte[] _toByte(string text)
       {
           return Encoding.UTF8.GetBytes(text);
       }

       /// <summary>
       /// Метод шифрудет данные с указаным публичным  ключем
       /// </summary>
       /// <param name="text">Текс для расшидрования</param>
       /// <returns></returns>
       public static string Encrypt(string text)
       {
           try
           {
               byte[] encContent = null;

               RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
               rsa.FromXmlString(PublicKey);
               encContent = rsa.Encrypt(_toByte(text),true);
               return Convert.ToBase64String(encContent);
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               throw;
           }
       }

   }
}
