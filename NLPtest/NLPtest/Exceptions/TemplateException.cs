using System;
using System.Runtime.Serialization;

namespace NLPtest.Exceptions
{

    internal class TemplateException : Exception
    {
        private string[] template;

        public TemplateException()
        {
        }

        public TemplateException(string message) : base(message)
        {
        }

        public TemplateException(string[] template)
        {
            this.template = template;
        }

        public TemplateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TemplateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}