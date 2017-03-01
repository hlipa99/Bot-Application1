using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Exceptions
{
    [Serializable]
    internal class PhraseFormatException : Exception
    {
        public PhraseFormatException()
        {
        }

        public PhraseFormatException(string message) : base(message)
        {
        }

        public PhraseFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PhraseFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}