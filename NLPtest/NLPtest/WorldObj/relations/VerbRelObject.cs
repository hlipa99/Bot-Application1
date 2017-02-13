namespace NLPtest.WorldObj
{
    internal class VerbRelObject : RelationObject
    {
        private int v;


        public VerbRelObject(IWorldObject objective) : base(objective)
        {
   
         
            
        }
        public override IWorldObject Clone()
        {
            VerbRelObject res = new VerbRelObject(Objective);
           res.cloneBase(this);
            return res;
        }

    }

  

   
}

