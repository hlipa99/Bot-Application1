using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Controllers
{
    [Serializable]
    internal class sessionBreakException : Exception
    {
        public sessionBreakException()
        {
        }

        public sessionBreakException(string message) : base(message)
        {
        }

        public sessionBreakException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected sessionBreakException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}