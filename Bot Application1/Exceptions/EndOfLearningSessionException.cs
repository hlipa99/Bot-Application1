using System;
using System.Runtime.Serialization;

namespace Bot_Application1.IDialog
{
    [Serializable]
    internal class EndOfLearningSessionException : Exception
    {
        public EndOfLearningSessionException()
        {
        }

        public EndOfLearningSessionException(string message) : base(message)
        {
        }

        public EndOfLearningSessionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EndOfLearningSessionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}