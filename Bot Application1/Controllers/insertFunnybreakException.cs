using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Controllers
{
    [Serializable]
    internal class insertFunnybreakException : Exception
    {
        public insertFunnybreakException()
        {
        }

        public insertFunnybreakException(string message) : base(message)
        {
        }

        public insertFunnybreakException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected insertFunnybreakException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}