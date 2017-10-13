using System.Linq;

namespace SimpleCiphers.Models
{
    public class CaesarCipher : ICipher
    {
        public string Encrypt(string text, string key, string abc) => Crypt(text, key, abc, true);

        public string Decrypt(string text, string key, string abc) => Crypt(text, key, abc, false);

        public string[,] GetEncryptedAlphabet(string text, string key, string abc)
        {
            var encAbc = Crypt(abc, key, abc, true).Select(x => $"{x}").ToArray();
            return ArrayOperations.Turn1DTo2D(encAbc);
        }

        public string[] GetRowAlphabet(string key, string abc) => null;

        public string[] GetColAlphabet(string key, string abc) => abc.Select(x => $"{x}").ToArray();

        public string Crypt(string text, string key, string abc, bool encrypt)
        {
            Checker.KeyNull(key);
            var intKey = Checker.GetKeyInt(key);
            Checker.TextNull(text);
            Checker.TextContain(text, abc);

            // to avoid overflows
            intKey %= abc.Length;

            if (!encrypt)
            {
                intKey *= -1;
            }

            // otherwise invalid index
            if (intKey < 0)
            {
                intKey += abc.Length;
            }

            var result = "";

            foreach (var ch in text)
            {
                for (var j = 0; j < abc.Length; j++)
                {
                    if (ch == abc[j])
                    {
                        var temp = (j + intKey) % abc.Length;
                        result += abc[temp];
                        break;
                    }
                }
            }
            return result;
        }
    }
}