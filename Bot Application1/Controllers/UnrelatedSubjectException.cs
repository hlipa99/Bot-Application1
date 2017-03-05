using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Controllers
{
    [Serializable]
    internal class UnrelatedSubjectException : Exception
    {
        public UnrelatedSubjectException()
        {
        }

        public UnrelatedSubjectException(string message) : base(message)
        {
        }

        public UnrelatedSubjectException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnrelatedSubjectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}