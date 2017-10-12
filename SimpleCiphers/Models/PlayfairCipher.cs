using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCiphers.Models
{
    public class PlayfairCipher : ICipher
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
            if (string.IsNullOrEmpty(key))
            {
                throw new CipherException("Необходимо задать ключ для шифра Плейфера.");
            }

            int lengthArr = (int) Math.Ceiling(Math.Sqrt(abc.Length));

            int highLength = lengthArr * lengthArr;
            int lowLength = (lengthArr - 1) * (lengthArr - 1);

            if (highLength != abc.Length)
            {
                throw new CipherException($"Длина алфавита составляет {abc.Length}.\n" +
                                            $"Необходимо либо увеличить ее до {highLength}, " +
                                            $"либо уменьшить до {lowLength}.");
            }

            string slogan = string.Join("", key.Distinct());
            string check = string.Join("", slogan.Intersect(abc));
            if (check != slogan)
            {
                throw new CipherException("Лозунг содержит символы не из алфавита.");
            }

            // зашифрованный алфавит в виде матрицы
            var abcArr = new string[lengthArr, lengthArr];

            // зашифрованный алфавит в виде строки
            var encAbc = string.Join("", slogan.Union(abc));

            int count = 0;

            for (int i = 0; i < abcArr.GetLength(0); i++)
            {
                for (int j = 0; j < abcArr.GetLength(1); j++)
                {
                    abcArr[i, j] = encAbc[count++].ToString();
                }
            }
            return abcArr;
        }

        public string[] GetRowAlphabet(string key, string abc)
        {
            return null;
        }

        public string[] GetColAlphabet(string key, string abc)
        {
            // если null, то колонки заполняются CO C1 C2 ...
            int lengthArr = (int) Math.Ceiling(Math.Sqrt(abc.Length));
            return new string[lengthArr];
        }

        public string Crypt(string text, string key, string abc, bool encrypt)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new CipherException("Необходимо задать ключ для шифра Плейфера.");
            }

            string check = string.Join("", abc.Union(key));

            //string slogan = string.Join("", key.Distinct());

           // string check = string.Join("", key.Intersect(abc));
            if (check != abc)
            {
                throw new CipherException("Лозунг содержит символы не из алфавита.");
            }

            string checkText = string.Join("", abc.Union(text));
            if (checkText != abc)
            {
                throw new CipherException("Текст содержит символы не из алфавита.");
            }

            // удвоенные буквы
            for (int i = 0; i < text.Length - 1; i += 2)
            {
                if (text[i] == text[i + 1])
                {
                    throw new CipherException(
                        $"Необходимо поставить символ между \"{text[i]}{text[i + 1]}\".");
                }
            }

            if (text.Length % 2 != 0)
            {
                throw new CipherException(
                    "Текст содержит нечетное количество символов.\n" +
                    "Добавьте или удалите один символ.");
            }

            var encAbc = GetEncryptedAlphabet(null, key, abc);

            string result = "";

            // rows == cols
            int len = encAbc.GetLength(0);

            for (int i = 0; i < text.Length - 1; i += 2)
            {
                var first = text[i].ToString();
                var second = text[i + 1].ToString();

                if (ArrayOperations.ContainsIn(first, encAbc, out var x1, out var y1) &&
                    ArrayOperations.ContainsIn(second, encAbc, out var x2, out var y2))
                {
                    // если в одной строке
                    if (x1 == x2)
                    {
                        if (encrypt)
                        {
                            result += $"{encAbc[x1, (y1 + 1) % len]}" +
                                      $"{encAbc[x2, (y2 + 1) % len]}";
                        }
                        else
                        {
                            result += $"{encAbc[x1, (y1 - 1 + len) % len]}" +
                                      $"{encAbc[x2, (y2 - 1 + len) % len]}";
                        }
                    }
                    // если в одном столбце
                    else if (y1 == y2)
                    {
                        if (encrypt)
                        {
                            result += $"{encAbc[(x1 + 1) % len, y1]}" +
                                      $"{encAbc[(x2 + 1) % len, y2]}";
                        }
                        else
                        {
                            result += $"{encAbc[(x1 - 1 + len) % len, y1]}" +
                                      $"{encAbc[(x2 - 1 + len) % len, y2]}";
                        }
                    }
                    // иначе
                    else
                    {
                        result += $"{encAbc[x1, y2]}" +
                                  $"{encAbc[x2, y1]}";
                    }
                }
            }
            return result;
        }
    }
}