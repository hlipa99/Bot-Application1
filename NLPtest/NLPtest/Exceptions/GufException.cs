using System;
using System.Runtime.Serialization;

namespace NLPtest.view
{
 
    internal class GufException : Exception
    {
        private personObject gufObject;

        public GufException()
        {
        }

        public GufException(string message) : base(message)
        {
        }

        public GufException(personObject gufObject)
        {
            this.gufObject = gufObject;
        }

        public GufException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GufException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}