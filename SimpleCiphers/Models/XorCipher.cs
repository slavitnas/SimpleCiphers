using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCiphers.Models
{
    public class XorCipher : ICipher
    {
        public string Encrypt(string text, string key, string abc) => Crypt(text, key, abc, true);

        public string Decrypt(string text, string key, string abc) => Crypt(text, key, abc, false);

        public string[,] GetEncryptedAlphabet(string text, string key, string abc)
        {
            var arr = abc.Select(x => $"{abc.IndexOf(x)}").ToArray();
            return ArrayOperations.Turn1DTo2D(arr);
        }

        public string[] GetRowAlphabet(string key, string abc) => null;

        public string[] GetColAlphabet(string key, string abc) => abc.Select(x => $"{x}").ToArray();

        public string Crypt(string text, string key, string abc, bool encrypt)
        {
            Checker.KeyNull(key);
            Checker.KeyContain(key, abc);
            Checker.TextNull(text);
            Checker.TextContain(text, abc);

            var len = abc.Length;
            var result = "";
            // index of the key symbol
            var count = 0;
            foreach (var t in text)
            {
                var index = abc.IndexOf(t);
                var keyIndex = abc.IndexOf(key[count]);
                if (encrypt)
                {
                    result += $"{abc[(index + keyIndex) % len]}";
                }
                else
                {
                    result += $"{abc[(index + len - keyIndex) % len]}";
                }
                count = (count + 1) % key.Length;
            }
            return result;
        }
    }
}