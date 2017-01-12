using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Exceptions
{
    [Serializable]
    internal class CategoryOutOfQuestionException : Exception
    {
        public CategoryOutOfQuestionException()
        {
        }

        public CategoryOutOfQuestionException(string message) : base(message)
        {
        }

        public CategoryOutOfQuestionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CategoryOutOfQuestionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}