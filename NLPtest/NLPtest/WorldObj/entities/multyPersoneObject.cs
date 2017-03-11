using NLP.WorldObj;

namespace NLP.view
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

        public override IWorldObject Clone()
        {
            multyPersoneObject res = new multyPersoneObject(persons);
            res.Copy(this);
            return res;
        }
    }
}