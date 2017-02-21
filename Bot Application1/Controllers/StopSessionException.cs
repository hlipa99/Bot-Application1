using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Controllers
{
    [Serializable]
    internal class StopSessionException : Exception
    {
        public StopSessionException()
        {
        }

        public StopSessionException(string message) : base(message)
        {
        }

        public StopSessionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StopSessionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}