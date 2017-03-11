
namespace NLP.WorldObj
{
    public class EntityObject : NounObject
    {

        public EntityObject(string word) : base(word)
        {
        
        }
        public override IWorldObject Clone()
        {
            EntityObject res = new EntityObject(Word);
            res.Copy(this);
            return res;
        }


    }
}