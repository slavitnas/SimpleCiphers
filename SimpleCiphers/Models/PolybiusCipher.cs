using System;
using System.Linq;

namespace SimpleCiphers.Models
{
    public class PolybiusCipher : ICipher
    {
        public string Encrypt(string text, string key, string abc) => Crypt(text, abc, true);

        public string Decrypt(string text, string key, string abc) => Crypt(text, abc, false);

        public string[,] GetEncryptedAlphabet(string text, string key, string abc)
        {
            var lengthArr = (int) Math.Ceiling(Math.Sqrt(abc.Length));

            if (lengthArr > 9)
            {
                throw new CipherException("Недопустимо большая длина входного алфавита.");
            }

            var abcArr = new string[lengthArr, lengthArr];

            var count = 0;

            for (var i = 0; i < abcArr.GetLength(0); i++)
            {
                for (var j = 0; j < abcArr.GetLength(1); j++)
                {
                    if (count == abc.Length)
                    {
                        abcArr[i, j] = "";
                    }
                    else
                    {
                        abcArr[i, j] = abc[count++].ToString();
                    }
                }
            }
            return abcArr;
        }

        public string[] GetRowAlphabet(string key, string abc) => abc.Select(x => $"{abc.IndexOf(x) + 1}").ToArray();

        public string[] GetColAlphabet(string key, string abc) => GetRowAlphabet(null, abc);

        public string Crypt(string text, string abc, bool encrypt)
        {
            var encAbc = GetEncryptedAlphabet(null, null, abc);

            Checker.TextNull(text);

            if (encrypt)
            {
                Checker.TextContain(text, abc);
            }
            else
            {
                Checker.TextEncDigit(text);

                if (text.Length % 2 != 0)
                {
                    throw new CipherException("Длина шифрованного текста не кратна 2.\n" +
                                              "Добавьте иди удалите одну цифру.");
                }
            }

            var result = "";

            if (encrypt)
            {
                foreach (var ch in text)
                {
                    if (ArrayOperations.ContainsIn(ch.ToString(), encAbc, out var x, out var y))
                    {
                        result += $"{x + 1}{y + 1}";
                    }
                }
            }
            else
            {
                for (var i = 0; i < text.Length - 1; i += 2)
                {
                    var temp = text.Substring(i, 2);
                    if (ContainsOut(temp, encAbc, out var x, out var y))
                    {
                        result += encAbc[x, y];
                    }
                    else
                    {
                        throw new CipherException($"Невозможно расшифровать {temp}.\n" +
                                                  "Данной комбинации нет в шифрованном алфавите.");
                    }
                }
            }
            return result;
        }

        // Содержится ли text на измерениях массива encAbc с нумерацией 1 2 3 ...
        //   1 2     text = 11
        // 1 a b     return x = 0, y = 0
        // 2 c d     abc[0,0] = 11 - расшифрованный
        //           encAbc[0,0] = a - зашифрованный
        public static bool ContainsOut(string text, string[,] encAbc, out int x, out int y)
        {
            for (var i = 0; i < encAbc.GetLength(0); i++)
            {
                for (var j = 0; j < encAbc.GetLength(1); j++)
                {
                    if (text == $"{i + 1}{j + 1}" && encAbc[i, j] != "")
                    {
                        x = i;
                        y = j;
                        return true;
                    }
                }
            }
            x = -1;
            y = -1;
            return false;
        }
    }
}