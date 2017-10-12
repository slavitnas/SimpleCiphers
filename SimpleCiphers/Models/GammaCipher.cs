using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCiphers.Models
{
    public class GammaCipher : ICipher
    {
        public string Encrypt(string text, string key, string abc)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string text, string key, string abc)
        {
            throw new NotImplementedException();
        }

        public string[,] GetEncryptedAlphabet(string text, string key, string abc)
        {
            throw new NotImplementedException();
        }

        public string[] GetRowAlphabet(string key, string abc)
        {
            throw new NotImplementedException();
        }

        public string[] GetColAlphabet(string key, string abc)
        {
            throw new NotImplementedException();
        }
    }
}
