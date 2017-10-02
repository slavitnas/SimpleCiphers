using System;
using System.Linq;

namespace SimpleCiphers.Models
{
    // Шифр Цезаря
    public class CaesarCipher : ICipher
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
                throw new ArgumentException("Необходимо задать ключ для шифра Цезаря.");
            }

            if (!int.TryParse(key, out var temp))
            {
                throw new ArgumentException("Ключ для шифра Цезаря должен состоять " +
                                            "только из целых чисел в пределах int.");
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
            return abc.Select(x => $"{x}").ToArray();
        }

        public string Crypt(string text, string key, string abc, bool encrypt)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Необходимо задать ключ для шифра Цезаря.");
            }

            bool check = int.TryParse(key, out var intKey);

            if (!check)
            {
                throw new ArgumentException("Ключ для шифра Цезаря должен состоять " +
                                            "только из целых чисел в пределах int.");
            }

            string checkText = string.Join("", abc.Union(text));
            if (checkText != abc)
            {
                throw new ArgumentException("Текст содержит символы не из алфавита.");
            }

            // для исключения переполнения
            intKey %= abc.Length;

            text = text.ToLowerInvariant();

            // если дешифрование, то надо отнимать
            if (!encrypt)
                intKey *= -1;

            // иначе будет обращаться к недопустимому индексу
            if (intKey < 0)
            {
                intKey += abc.Length;
            }

            string result = "";

            foreach (char ch in text)
            {
                for (int j = 0; j < abc.Length; j++)
                {
                    if (ch == abc[j])
                    {
                        var temp = (j + intKey) % abc.Length;
                        result += abc[temp];
                        break;
                    }
                }
            }
            return result;
        }
    }
}