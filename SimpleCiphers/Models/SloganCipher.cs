using System.Linq;

namespace SimpleCiphers.Models
{
    public class SloganCipher : ICipher
    {
        public string Encrypt(string text, string key, string abc) => Crypt(text, key, abc, true);

        public string Decrypt(string text, string key, string abc) => Crypt(text, key, abc, false);

        public string[,] GetEncryptedAlphabet(string text, string key, string abc)
        {
            Checker.KeyNull(key);
            Checker.KeyContain(key, abc);

            var encAbc = string.Join("", key.Union(abc));
            return ArrayOperations.Turn1DTo2D(encAbc);
        }

        public string[] GetRowAlphabet(string key, string abc) => null;

        public string[] GetColAlphabet(string key, string abc) => abc.Select(x => $"{x}").ToArray();

        public string Crypt(string text, string key, string abc, bool encrypt)
        {
            Checker.KeyNull(key);
            Checker.KeyContain(key, abc);
            Checker.TextNull(text);
            Checker.TextContain(text, abc);

            var encAbc = string.Join("", key.Union(abc));
            var result = "";

            foreach (var ch in text)
            {
                for (var j = 0; j < abc.Length; j++)
                {
                    if (encrypt)
                    {
                        if (ch == abc[j])
                        {
                            result += encAbc[j];
                            break;
                        }
                    }
                    else
                    {
                        if (ch == encAbc[j])
                        {
                            result += abc[j];
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}