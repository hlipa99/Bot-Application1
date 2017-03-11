using System;
using System.Runtime.Serialization;

namespace NLP.Exceptions
{
    [Serializable]
    internal class UnknownEncodingException : Exception
    {
        private object charsetName;
        private ArgumentException ex;
        private object p;

        public UnknownEncodingException()
        {
        }

        public UnknownEncodingException(string message) : base(message)
        {
        }

        public UnknownEncodingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnknownEncodingException(object charsetName, object p, ArgumentException ex)
        {
            this.charsetName = charsetName;
            this.p = p;
            this.ex = ex;
        }

        protected UnknownEncodingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}