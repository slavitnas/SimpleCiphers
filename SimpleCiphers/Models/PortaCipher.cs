using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCiphers.Models
{
    public class PortaCipher : ICipher
    {
        public string Encrypt(string text, string key, string abc)
        {
            return Crypt(text, abc, true);
        }

        public string Decrypt(string text, string key, string abc)
        {
            return Crypt(text, abc, false);
        }

        public string[,] GetEncryptedAlphabet(string text, string key, string abc)
        {
            int len = abc.Length;
            string leadZeros = $"D{Math.Floor(Math.Log10(len * len) + 1)}";
            var abcArr = new string[len, len];
            int count = 1;

            for (int i = 0; i < abcArr.GetLength(0); i++)
            {
                for (int j = 0; j < abcArr.GetLength(1); j++)
                {
                    abcArr[i, j] = (count++).ToString(leadZeros);
                }
            }
            return abcArr;
        }

        public string[] GetRowAlphabet(string key, string abc)
        {
            return abc.Select(x => $"{x}").ToArray();
        }

        public string[] GetColAlphabet(string key, string abc)
        {
            return GetRowAlphabet(null, abc);
        }

        public string Crypt(string text, string abc, bool encrypt)
        {
            int len = abc.Length;
            // число значущих цифр при шифровании
            int count = (int) Math.Floor(Math.Log10(len * len) + 1);

            if (encrypt)
            {
                string check = string.Join("", abc.Union(text));
                if (check != abc)
                {
                    throw new ArgumentException("Текст содержит символы не из алфавита.");
                }

                if (text.Length % 2 != 0)
                {
                    throw new ArgumentException(
                        "Текст содержит нечетное количество символов.\n" +
                        "Добавьте или удалите один символ для шифрования.");
                }
            }
            else
            {
                if (!text.All(char.IsDigit))
                {
                    throw new ArgumentException(
                        "Шифрованный текст должен состоять только из цифр.");
                }

                int num = text.Length % count;
                if (num != 0)
                {
                    throw new ArgumentException(
                        $"Длина шифрованного текста не кратна {count}.\n" +
                        $"Введено {num} из {count} чисел.");
                }
            }

            // шифрованный алфавит
            var encAbc = GetEncryptedAlphabet(null, null, abc);
            string result = "";

            if (encrypt)
                for (int i = 0; i < text.Length - 1; i += 2)
                {
                    int row = abc.IndexOf(text[i]);
                    int col = abc.IndexOf(text[i + 1]);
                    result += $"{encAbc[row, col]}";
                }
            else
            {
                for (int i = 0; i < text.Length - count + 1; i += count)
                {
                    string temp = text.Substring(i, count);
                    if (ArrayOperations.ContainsIn(temp, encAbc, out var x, out var y))
                    {
                        result += $"{abc[x]}{abc[y]}";
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