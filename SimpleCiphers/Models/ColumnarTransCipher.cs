using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCiphers.Models
{
    public class ColumnarTransCipher : ICipher
    {
        public string Encrypt(string text, string key, string abc) => Crypt(text, key, abc, true);

        public string Decrypt(string text, string key, string abc) => Crypt(text, key, abc, false);

        public string[,] GetEncryptedAlphabet(string text, string key, string abc)
        {
            Checker.KeyNull(key);
            Checker.TextNull(text);

            var textLen = text.Length;
            var cols = key.Length;
            var rows = textLen / cols;
            if (textLen % cols > 0)
                rows += 1;

            var textArr = new string[rows, cols];
            var count = 0;

            for (var i = 0; i < textArr.GetLength(0); i++)
            {
                for (var j = 0; j < textArr.GetLength(1); j++)
                {
                    if (count == text.Length)
                        textArr[i, j] = "•";
                    else
                        textArr[i, j] = text[count++].ToString();
                }
            }
            return textArr;
        }

        public string[] GetRowAlphabet(string key, string abc) => null;

        public string[] GetColAlphabet(string key, string abc)
        {
            var arr = GetIndexes(key).Select(x => $"{x + 1}").ToArray();
            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = $"{key[i]} ({arr[i]})";
            }
            return arr;
        }

        public string Crypt(string text, string key, string abc, bool encrypt)
        {
            Checker.KeyNull(key);
            Checker.TextNull(text);

            var indexes = GetIndexes(key);
            var result = "";

            if (encrypt)
            {
                var encAbc = GetEncryptedAlphabet(text, key, abc);

                for (var i = 0; i < key.Length; i++)
                {
                    var index = Array.IndexOf(indexes, i);
                    for (var j = 0; j < encAbc.GetLength(0); j++)
                    {
                        result += encAbc[j, index];
                    }
                }
            }
            else
            {
                var cols = key.Length;
                var textLen = text.Length;

                if (cols > textLen)
                {
                    throw new CipherException("Длина ключа больше чем шифрованный текст.");
                }

                var rows = textLen / cols;

                if (textLen % cols > 0)
                    rows += 1;

                var textArr = new string[rows, cols];
                var count = 0;

                for (var i = 0; i < key.Length; i++)
                {
                    var index = Array.IndexOf(indexes, i);
                    for (var j = 0; j < textArr.GetLength(0); j++)
                    {
                        if (count == text.Length)
                            textArr[j, index] = "•";
                        else
                            textArr[j, index] = text[count++].ToString();
                    }
                }
                result = string.Join("", textArr.Cast<string>());
            }
            return result;
        }

        // Get sorted indexes of the key
        private static int[] GetIndexes(string key)
        {
            var keyLength = key.Length;
            var indexes = new int[keyLength];
            var sortedKey = new List<KeyValuePair<int, char>>();
            int i;

            for (i = 0; i < keyLength; i++)
                sortedKey.Add(new KeyValuePair<int, char>(i, key[i]));

            sortedKey.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            for (i = 0; i < keyLength; i++)
                indexes[sortedKey[i].Key] = i;

            return indexes;
        }
    }
}