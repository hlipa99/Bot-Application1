using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Controllers
{
    [Serializable]
    internal class swearWordException : Exception
    {
        public swearWordException()
        {
        }

        public swearWordException(string message) : base(message)
        {
        }

        public swearWordException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected swearWordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}