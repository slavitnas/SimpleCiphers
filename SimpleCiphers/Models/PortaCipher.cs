using System;
using System.Linq;

namespace SimpleCiphers.Models
{
    public class PortaCipher : ICipher
    {
        public string Encrypt(string text, string key, string abc) => Crypt(text, abc, true);

        public string Decrypt(string text, string key, string abc) => Crypt(text, abc, false);

        public string[,] GetEncryptedAlphabet(string text, string key, string abc)
        {
            var len = abc.Length;
            var leadZeros = $"D{Math.Floor(Math.Log10(len * len) + 1)}";
            var abcArr = new string[len, len];
            var count = 1;

            for (var i = 0; i < abcArr.GetLength(0); i++)
            {
                for (var j = 0; j < abcArr.GetLength(1); j++)
                {
                    abcArr[i, j] = (count++).ToString(leadZeros);
                }
            }
            return abcArr;
        }

        public string[] GetRowAlphabet(string key, string abc) => abc.Select(x => $"{x}").ToArray();

        public string[] GetColAlphabet(string key, string abc) => GetRowAlphabet(null, abc);

        public string Crypt(string text, string abc, bool encrypt)
        {
            Checker.TextNull(text);

            var len = abc.Length;
            // the number of significant digits
            var count = (int) Math.Floor(Math.Log10(len * len) + 1);

            if (encrypt)
            {
                Checker.TextContain(text, abc);

                if (text.Length % 2 != 0)
                {
                    throw new CipherException("Длина текста для шифрования не кратна 2.\n" +
                                              "Добавьте или удалите один символ.");
                }
            }
            else
            {
                Checker.TextEncDigit(text);

                var num = text.Length % count;
                if (num != 0)
                {
                    throw new CipherException($"Длина шифрованного текста не кратна {count}.\n" +
                                              $"Введено {num} из {count} чисел.");
                }
            }

            var encAbc = GetEncryptedAlphabet(null, null, abc);
            var result = "";

            if (encrypt)
                for (var i = 0; i < text.Length - 1; i += 2)
                {
                    var row = abc.IndexOf(text[i]);
                    var col = abc.IndexOf(text[i + 1]);
                    result += $"{encAbc[row, col]}";
                }
            else
            {
                for (var i = 0; i < text.Length - count + 1; i += count)
                {
                    var temp = text.Substring(i, count);
                    if (ArrayOperations.ContainsIn(temp, encAbc, out var x, out var y))
                    {
                        result += $"{abc[x]}{abc[y]}";
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
    }
}