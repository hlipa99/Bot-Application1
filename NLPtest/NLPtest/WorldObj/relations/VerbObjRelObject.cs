namespace NLPtest.WorldObj
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
           res.cloneBase(this);
            return res;
        }

    }




}

