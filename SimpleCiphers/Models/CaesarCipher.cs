using System;

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
            // ключ только из целых чисел
            if (!int.TryParse(key, out var temp))
            {
                return null;
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
            var arr = new string[abc.Length];
            for (int i = 0; i < abc.Length; i++)
            {
                arr[i] = abc[i].ToString();
            }
            return arr;
        }

        public string Crypt(string text, string key, string abc, bool encryptOrDecrypt)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(
                    "Необходимо задать ключ для шифра Цезаря.");
            }

            text = text.ToLowerInvariant();

            bool check = int.TryParse(key, out var intKey);

            if (!check)
            {
                throw new ArgumentException(
                    "Ключ для шифра Цезаря должен состоять только из целых чисел.");
            }

            // для исключения переполнения
            intKey %= abc.Length;

            if (!encryptOrDecrypt)
                intKey *= -1;

            // иначе будет обращаться к недопустимому индексу
            if (intKey < 0)
            {
                intKey += abc.Length;
            }

            string result = "";

            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < abc.Length; j++)
                {
                    if (text[i] == abc[j])
                    {
                        // раньше тут было переполнение
                        var temp = Math.Abs((j + intKey) % abc.Length);
                        result += abc[temp];
                        break;
                    }
                    // если символа нет в алфавите, то добавить как есть
                    if (j == abc.Length - 1)
                        result += text[i];
                }
            }

            return result;
        }
    }
}