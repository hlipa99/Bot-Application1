using NLPtest.WorldObj;

namespace NLPtest.view
{
    public class Context
    {
        WorldObject general = null;

        public WorldObject General
        {
            get
            {
                return general;
            }

            set
            {
                general = value;
            }
        }
    }
}