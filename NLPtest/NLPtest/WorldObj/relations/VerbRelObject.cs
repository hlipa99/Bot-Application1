namespace NLPtest.WorldObj
{
    internal class VerbRelObject : RelationObject
    {
        private int v;


        public VerbRelObject(WorldObject objective) : base(objective)
        {
   
         
            
        }
        public override IWorldObject Clone()
        {
            VerbRelObject res = new VerbRelObject(Objective);
            cloneBase(res);
            return res;
        }

    }

  

   
}

