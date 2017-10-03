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
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Необходимо задать ключ для лозунгового шифра.");
            }

            string slogan = string.Join("", key.Distinct());
            string check = string.Join("", slogan.Intersect(abc));
            if (check != slogan)
            {
                throw new ArgumentException("Лозунг содержит символы не из алфавита.");
            }

            string encAbc = string.Join("", slogan.Union(abc));
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
            return abc.Select(x => $"{x}").ToArray();
        }

        public string Crypt(string text, string key, string abc, bool encrypt)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Необходимо задать ключ для лозунгового шифра.");
            }

            string slogan = string.Join("", key.Distinct());

            string check = string.Join("", slogan.Intersect(abc));
            if (check != slogan)
            {
                throw new ArgumentException("Лозунг содержит символы не из алфавита.");
            }

            string checkText = string.Join("", abc.Union(text));
            if (checkText != abc)
            {
                throw new ArgumentException("Текст содержит символы не из алфавита.");
            }

            var encAbc = string.Join("", slogan.Union(abc));

            string result = "";

            if (encrypt)
            {
                foreach (char ch in text)
                {
                    for (int j = 0; j < abc.Length; j++)
                    {
                        if (ch == abc[j])
                        {
                            result += encAbc[j];
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (char ch in text)
                {
                    for (int j = 0; j < abc.Length; j++)
                    {
                        if (ch == encAbc[j])
                        {
                            result += abc[j];
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}