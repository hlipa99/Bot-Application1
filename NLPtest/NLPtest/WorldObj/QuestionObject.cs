

namespace NLP.WorldObj
{
    internal class QuestionObject : WorldObject
    {
        private QuestionType isIt;
        private QuestionType questionType;


        public QuestionObject(string word) : base(word)
        {
         
        }

        public QuestionObject(QuestionType isIt)
        {
            this.isIt = isIt;
        }


        public override IWorldObject Clone()
        {
            QuestionObject res = new QuestionObject(Word);
            res.Copy(this);
            return res;
        }

        public enum QuestionType
        { What, When, Why, Whom, Where, HowMatch, How, IsIt };
    }
}