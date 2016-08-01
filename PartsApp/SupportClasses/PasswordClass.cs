using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace PartsApp.SupportClasses
{
    public class PasswordClass
    {

        /// <summary>
        /// Возвращает hash введенной строки.
        /// </summary>
        /// <param name="password">Строка которую необх-мо зашифровать.</param>
        /// <returns></returns>  
        public static string GetHashString(string password)
        {
            //переводим строку в байт-массим  
            byte[] bytes = Encoding.Unicode.GetBytes(password);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
                hash += String.Format("{0:x2}", b);

            return hash;
        }//GetHashString
    }//PasswordClass

}//namespace
