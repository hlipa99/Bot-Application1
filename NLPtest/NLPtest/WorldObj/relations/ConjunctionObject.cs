using NLPtest.WorldObj;

namespace NLPtest
{
    internal class ConjunctionRelObject : RelationObject
    {
        private ConjunctionType type;

        public ConjunctionRelObject(IWorldObject objective) : base(objective)
        {
            this.Type = ConjunctionType.unknownConjunction;
        }

        public ConjunctionRelObject(WorldObject objective, ConjunctionType type) : base(objective)
        {

            this.Type = type;

        }

        private ConjunctionType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }


        public override string ToString()
        {
            return type.ToString();
        }
        public override IWorldObject Clone()
        {
            ConjunctionRelObject res = new ConjunctionRelObject(null);
            res.Copy(this);
            return res;
        }
        public enum ConjunctionType
        {
            andConjunction,
            orConjunction,
            forConjunction,
            butConjunction,
            asConjunction,
            likeConjunction,
            notConjunction,
            unknownConjunction
        }
    }
}