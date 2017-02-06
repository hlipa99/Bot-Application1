using System;
using System.Runtime.Serialization;

namespace NLPtest
{
    [Serializable]
    internal class DictionaryException : Exception
    {
        public DictionaryException()
        {
        }

        public DictionaryException(string message) : base(message)
        {
        }

        public DictionaryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DictionaryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}