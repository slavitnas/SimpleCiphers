using System;
using System.Linq;

namespace SimpleCiphers.Models
{
    public class PolybiusCipher : ICipher
    {
        public string Encrypt(string text, string key, string abc)
        {
            return Crypt(text, abc, true);
        }

        public string Decrypt(string text, string key, string abc)
        {
            return Crypt(text, abc, false);
        }

        public string[,] GetEncryptedAlphabet(string key, string abc)
        {
            int lengthArr = (int)Math.Ceiling(Math.Sqrt(abc.Length));

            if (lengthArr > 9)
            {
                throw new ArgumentException("Недопустимо большая длина входного алфавита.");
            }

            var abcArr = new string[lengthArr, lengthArr];

            int count = 0;

            for (int i = 0; i < abcArr.GetLength(0); i++)
            {
                for (int j = 0; j < abcArr.GetLength(1); j++)
                {
                    if (count == abc.Length)
                        abcArr[i, j] = string.Empty;
                    else
                        abcArr[i, j] = abc[count++].ToString();
                }
            }

            return abcArr;
        }

        public string[] GetRowAlphabet(string abc)
        {
            var arr = new string[abc.Length];
            for (int i = 0; i < abc.Length; i++)
            {
                arr[i] = $"{i + 1}";
            }
            return arr;
        }

        public string[] GetColAlphabet(string abc)
        {
            return GetRowAlphabet(abc);
        }

        public string Crypt(string text, string abc, bool encrypt)
        {
            text = text.ToLowerInvariant();
            // шифрованный алфавит
            var encAbc = GetEncryptedAlphabet("", abc);

            if (encrypt)
            {
                string check = string.Join("", abc.Union(text));
                if (check != abc)
                {
                    throw new ArgumentException("Текст содержит символы не из алфавита.");
                }
            }
            else
            {
                if (!text.All(char.IsDigit))
                {
                    throw new ArgumentException(
                        "Шифрованный текст должен состоять только из цифр.");
                }

                int num = text.Length % 2;
                if (num != 0)
                {
                    throw new ArgumentException(
                        "Длина шифрованного текста не кратна 2.\n" +
                        "Введите иди удалите одну цифру.");
                }
            }

            string result = "";

            if (encrypt)
            {
                foreach (char ch in text)
                {
                    if (ArrayOperations.ContainsIn(ch.ToString(), encAbc, out var x, out var y))
                    {
                        result += $"{x + 1}{y + 1}";
                    }
                }
            }
            else
            {
                for (int i = 0; i < text.Length - 1; i += 2)
                {
                    string temp = text.Substring(i, 2);
                    if (ArrayOperations.ContainsOut(temp, encAbc, out var x, out var y))
                    {
                        result += encAbc[x, y];
                    }
                    else
                    {
                        throw new ArgumentException(
                            $"Невозможно расшифровать {temp}.\n" +
                            "Данной комбинации нет в шифрованном алфавите.");
                    }
                }
            }
            return result;
        }
    }
}