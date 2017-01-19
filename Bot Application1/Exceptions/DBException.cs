using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Exceptions
{
    [Serializable]
    internal class DBException : Exception
    {
        public DBException()
        {
        }

        public DBException(string message) : base(message)
        {
        }

        public DBException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DBException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}