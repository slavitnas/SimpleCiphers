using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCiphers.Models
{
    // Шифр вертикальной перестановки
    public class ColumnarTransCipher : ICipher
    {
        public string Encrypt(string text, string key, string abc)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Необходимо задать ключ.");
            }

            var encAbc = GetEncryptedAlphabet(text, key, abc);
            int[] indexes = GetIndexes(key);

            string result = "";

            for (int i = 0; i < key.Length; i++)
            {
                int index = Array.IndexOf(indexes, i);
                for (int j = 0; j < encAbc.GetLength(0); j++)
                {
                    result += encAbc[j, index];
                }
            }
            return result;
        }

        public string Decrypt(string text, string key, string abc)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Необходимо задать ключ.");
            }

            int cols = key.Length;
            int textLen = text.Length;

            if (cols > textLen)
            {
                throw new ArgumentException("Длина ключа больше чем шифрованный текст.");
            }

            int rows = textLen / cols;

            if (textLen % cols > 0)
                rows += 1;

            var textArr = new string[rows, cols];

            int count = 0;
            int[] indexes = GetIndexes(key);

            for (int i = 0; i < key.Length; i++)
            {
                int index = Array.IndexOf(indexes, i);
                for (int j = 0; j < textArr.GetLength(0); j++)
                {
                    if (count == text.Length)
                        textArr[j, index] = "•";
                    else
                        textArr[j, index] = text[count++].ToString();
                }
            }

            var result = string.Join("", textArr.Cast<string>());

//            string result = "";
//
//            for (int i = 0; i < textArr.GetLength(0); i++)
//            {
//                for (int j = 0; j < textArr.GetLength(1); j++)
//                {
//                    result += textArr[i, j];
//                }
//            }

            return result;
        }

        public string[,] GetEncryptedAlphabet(string text, string key, string abc)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Необходимо задать исходный текст.");
            }

            int textLen = text.Length;
            int cols = key.Length;
            int rows = textLen / cols;

            if (textLen % cols > 0)
                rows += 1;

            var textArr = new string[rows, cols];

            int count = 0;

            for (int i = 0; i < textArr.GetLength(0); i++)
            {
                for (int j = 0; j < textArr.GetLength(1); j++)
                {
                    if (count == text.Length)
                        textArr[i, j] = "•";
                    else
                        textArr[i, j] = text[count++].ToString();
                }
            }
            return textArr;
        }

        public string[] GetRowAlphabet(string key, string abc)
        {
            return null;
        }

        public string[] GetColAlphabet(string key, string abc)
        {
            var arr = GetIndexes(key).Select(x => $"{x + 1}").ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = $"{key[i]} ({arr[i]})";
            }
            return arr;
        }

        private static int[] GetIndexes(string key)
        {
            int keyLength = key.Length;
            int[] indexes = new int[keyLength];
            List<KeyValuePair<int, char>> sortedKey = new List<KeyValuePair<int, char>>();
            int i;

            for (i = 0; i < keyLength; i++)
                sortedKey.Add(new KeyValuePair<int, char>(i, key[i]));

            sortedKey.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            for (i = 0; i < keyLength; ++i)
                indexes[sortedKey[i].Key] = i;

            return indexes;
        }
    }
}