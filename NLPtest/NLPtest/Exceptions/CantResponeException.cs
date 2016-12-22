using System;
using System.Runtime.Serialization;
using NLPtest.WorldObj;

namespace NLPtest
{
    [Serializable]
    internal class CantResponeException : Exception
    {
        private WorldObject o;

        public CantResponeException()
        {
        }

        public CantResponeException(string message) : base(message)
        {
        }

        public CantResponeException(WorldObject o)
        {
            this.o = o;
        }

        public CantResponeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CantResponeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}