using System;
using System.Runtime.Serialization;
using NLP.WorldObj;
using NLP.HebWords;

namespace NLP.Exceptions
{
    [Serializable]
    internal class WorldObjectException : Exception
    {
        private WorldObject key;
        private WordObject word;

        public WorldObjectException()
        {
        }

        public WorldObjectException(WorldObject key)
        {
            this.key = key;
        }

        public WorldObjectException(string message) : base(message)
        {
        }

        public WorldObjectException(WordObject word)
        {
            this.word = word;
        }

        public WorldObjectException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WorldObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}