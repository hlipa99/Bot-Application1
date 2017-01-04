using NLPtest.WorldObj;

namespace NLPtest.view
{
    internal class multyPersoneObject : PersonObject
    {
        private WorldObject[] persons;
        int number;

        public multyPersoneObject(WorldObject[] persons) : base("multy")
        {
            this.persons = persons;
            number = persons.Length;
        }
    }
}