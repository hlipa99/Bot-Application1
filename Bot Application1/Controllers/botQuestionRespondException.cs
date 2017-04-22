using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Controllers
{
    [Serializable]
    internal class botQuestionRespondException : Exception
    {
        public botQuestionRespondException()
        {
        }

        public botQuestionRespondException(string message) : base(message)
        {
        }

        public botQuestionRespondException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected botQuestionRespondException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}