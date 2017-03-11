namespace NLP.WorldObj
{
    internal class VerbObjRelObject : RelationObject
    {
        private int v;

        public VerbObjRelObject(IWorldObject objective) : base(objective)
        {
   
         
            
        }

        public override IWorldObject Clone()
        {
            VerbObjRelObject res = new VerbObjRelObject(Objective);
            res.Copy(this);
            return res;
        }

    }




}

