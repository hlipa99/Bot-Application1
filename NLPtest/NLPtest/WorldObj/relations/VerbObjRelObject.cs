namespace NLPtest.WorldObj
{
    internal class VerbObjRelObject : RelationObject
    {
        private int v;

        public VerbObjRelObject(WorldObject objective) : base(objective)
        {
   
         
            
        }

        public override IWorldObject Clone()
        {
            VerbObjRelObject res = new VerbObjRelObject(Objective);
            cloneBase(res);
            return res;
        }

    }




}

