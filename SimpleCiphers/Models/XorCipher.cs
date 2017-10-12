using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCiphers.Models
{
    public class XorCipher : ICipher
    {
        public string Encrypt(string text, string key, string abc)
        {
            return Crypt(text, key, abc, true);
        }

        public string Decrypt(string text, string key, string abc)
        {
            return Crypt(text, key, abc, false);
        }

        public string[,] GetEncryptedAlphabet(string text, string key, string abc)
        {
            string[,] arr = new string[5, abc.Length];

            for (int i = 0; i < abc.Length; i++)
            {
                arr[0, i] = i.ToString();
            }

            for (int i = 0; i < text.Length; i++)
            {
                char temp = key[i % key.Length];
                arr[1, i] = text[i].ToString();
                arr[2, i] = abc.IndexOf(text[i]).ToString();
                arr[3, i] = temp.ToString();
                arr[4, i] = abc.IndexOf(temp).ToString();
            }

            return arr;
        }

        public string[] GetRowAlphabet(string key, string abc)
        {
            return null;
        }

        public string[] GetColAlphabet(string key, string abc)
        {
            return abc.Select(x => $"{x}").ToArray();
        }

        public string Crypt(string text, string key, string abc, bool encrypt)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Необходимо задать ключ.");
            }

            string checkText = string.Join("", abc.Union(text));
            if (checkText != abc)
            {
                throw new ArgumentException("Текст содержит символы не из алфавита.");
            }

            int len = abc.Length;

            string result = "";

            int cnt = 0;
            foreach (char t in text)
            {
                int index = abc.IndexOf(t);
                int keyIndex = abc.IndexOf(key[cnt++]);
                if (encrypt)
                {
                    result += $"{abc[(index + keyIndex) % len]}";
                }
                else
                {
                    result += $"{abc[(index + len - keyIndex) % len]}";
                }
                if (cnt == key.Length - 1)
                    cnt = 0;
            }
            return result;
        }
    }
}