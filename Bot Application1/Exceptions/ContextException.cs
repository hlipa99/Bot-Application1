﻿using System;
using System.Runtime.Serialization;

namespace Bot_Application1.Exceptions
{
    [Serializable]
    internal class ContextException : Exception
    {
        public ContextException()
        {
        }

        public ContextException(string message) : base(message)
        {
        }

        public ContextException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ContextException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}