using System;
using System.Runtime.Serialization;
using NLPtest.WorldObj;

namespace NLPtest
{
    [Serializable]
    internal class WorldObjectException : Exception
    {
        private WorldObject key;
        private Word word;

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

        public WorldObjectException(Word word)
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