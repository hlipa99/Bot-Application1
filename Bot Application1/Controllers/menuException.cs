using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Controllers
{
    [Serializable]
    internal class menuException : Exception
    {
        public menuException()
        {
        }

        public menuException(string message) : base(message)
        {
        }

        public menuException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected menuException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}