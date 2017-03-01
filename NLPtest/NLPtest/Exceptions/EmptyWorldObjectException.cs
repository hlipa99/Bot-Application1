using System;
using System.Runtime.Serialization;

namespace NLPtest.Exceptions
{
    [Serializable]
    internal class EmptyWorldObjectException : Exception
    {
        public EmptyWorldObjectException()
        {
        }

        public EmptyWorldObjectException(string message) : base(message)
        {
        }

        public EmptyWorldObjectException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmptyWorldObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}