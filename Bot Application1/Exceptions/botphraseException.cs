using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Controllers
{
    [Serializable]
    internal class botphraseException : Exception
    {
        public botphraseException()
        {
        }

        public botphraseException(string message) : base(message)
        {
        }

        public botphraseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected botphraseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}