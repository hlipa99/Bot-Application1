using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Exceptions
{
    [Serializable]
    internal class unknownUserException : Exception
    {
        public unknownUserException()
        {
        }

        public unknownUserException(string message) : base(message)
        {
        }

        public unknownUserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected unknownUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}