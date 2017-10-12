using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCiphers.Models
{
    [Serializable]
    public class CipherException : Exception
    {
        public CipherException()
        {
        }

        public CipherException(string message) : base(message)
        {
        }

        public CipherException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CipherException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}