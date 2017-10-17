using System;
using System.Linq;

namespace SimpleCiphers.Models
{
    public static class Checker
    {
        public static void KeyNull(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new CipherException("Необходимо задать ключ.");
            }
        }

        public static void KeyContain(string key, string abc)
        {
            var exclude = string.Join("", key.Except(abc));
            if (exclude == string.Empty) return;
            throw new CipherException($"Ключ содержит символы не из алфавита:\n{exclude}.");
        }

        public static void TextNull(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new CipherException("Необходимо задать исходный текст.");
            }
        }

        public static void TextContain(string text, string abc)
        {
            var exclude = string.Join("", text.Except(abc));
            if (exclude == string.Empty) return;
            throw new CipherException($"Текст содержит символы не из алфавита:\n{exclude}");
        }

        public static int GetKeyInt(string key)
        {
            var check = int.TryParse(key, out var intKey);
            if (!check)
            {
                throw new CipherException("Ключ должен состоять только из целых чисел.");
            }
            return intKey;
        }

        public static void TextEncDigit(string text)
        {
            if (!text.All(char.IsDigit))
            {
                throw new CipherException("Шифрованный текст должен состоять только из цифр.");
            }
        }
    }
}