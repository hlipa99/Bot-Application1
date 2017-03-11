using System;
using System.Runtime.Serialization;

namespace NLP.Exceptions
{
    [Serializable]
    internal class DBDataException : Exception
    {
        public DBDataException()
        {
        }

        public DBDataException(string message) : base(message)
        {
        }

        public DBDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DBDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}