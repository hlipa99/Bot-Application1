using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Controllers
{
    [Serializable]
    internal class insertIntrestingException : Exception
    {
        public insertIntrestingException()
        {
        }

        public insertIntrestingException(string message) : base(message)
        {
        }

        public insertIntrestingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected insertIntrestingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}