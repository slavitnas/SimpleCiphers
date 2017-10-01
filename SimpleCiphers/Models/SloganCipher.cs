using System;
using System.Linq;

namespace SimpleCiphers.Models
{
    // Лозунговый шифр
    public class SloganCipher : ICipher
    {
        public string Encrypt(string text, string key, string abc)
        {
            return Crypt(text, key, abc, true);
        }

        public string Decrypt(string text, string key, string abc)
        {
            return Crypt(text, key, abc, false);
        }

        public string[,] GetEncryptedAlphabet(string key, string abc)
        {
            // пустой лозунг
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            // лозунг содержит символы не из алфавита.
            string slogan = string.Join("", key.ToLower().Distinct());
            string check = string.Join("", slogan.Intersect(abc));
            if (check != slogan)
            {
                return null;
            }

            string encAbc = Crypt(abc, key, abc, true);
            string[,] arr = new string[1, encAbc.Length];
            for (int i = 0; i < encAbc.Length; i++)
            {
                arr[0, i] = encAbc[i].ToString();
            }
            return arr;
        }

        public string[] GetRowAlphabet(string abc)
        {
            return null;
        }

        public string[] GetColAlphabet(string abc)
        {
            var arr = new string[abc.Length];
            for (int i = 0; i < abc.Length; i++)
            {
                arr[i] = abc[i].ToString();
            }
            return arr;
        }

        public string Crypt(string text, string key, string abc, bool encryptOrDecrypt)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(
                    "Необходимо задать ключ для лозунгового шифра.");
            }

            text = text.ToLowerInvariant();
            key = key.ToLowerInvariant();
            string slogan = string.Join("", key.Distinct());

            string check = string.Join("", slogan.Intersect(abc));
            if (check != slogan)
            {
                throw new ArgumentException("Лозунг содержит символы не из алфавита.");
            }

            var cryptLang = string.Join("", slogan.Union(abc));

            string result = "";

            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < abc.Length; j++)
                {
                    // для шифрования
                    if (encryptOrDecrypt && text[i] == abc[j])
                    {
                        result += cryptLang[j];
                        break;
                    }

                    // для дешифрования
                    if (!encryptOrDecrypt && text[i] == cryptLang[j])
                    {
                        result += abc[j];
                        break;
                    }
                    // если символа нет в алфавите, то добавить как есть
                    if (j == abc.Length - 1)
                        result += text[i];
                }
            }
            return result;
        }
    }
}