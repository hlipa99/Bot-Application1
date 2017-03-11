using Bot_Application1.Controllers;
using Model;
using Model.dataBase;
using Model.Models;
using Moq;
using NLP;
using NLP.Controllers;
using NLP.HebWords;
using NLP.Models;
using NLP.NLP;
using NLP.WorldObj;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static NLP.WorldObj.personObject;

namespace UnitTestProject1
{
    public class ObjectTestBase
    {
       public ConversationController  ConvCtrl = new ConversationController ();
  //      public EducationController  EduCtrl = new EducationController();
        public NLPControler  NLPCtrl = new NLPControler ();
        public MorfAnalizer  MorfAnalizer = new MorfAnalizer ();

        public OuterAPIController  OuterAPICtrl = new OuterAPIController ();


        public IUser  UserMus = new User ();
        public IUser  UserFem = new User ();
        public IStudySession  SStudySession = new StudySession ();
       public IQuestion  Question1 = new Question ();
       public IQuestion  Question2 = new Question ();
       public IQuestion  Question3 = new Question ();
       public ISubQuestion  SubQqestion1 = new SubQuestion ();
        public ISubQuestion  SubQqestion2 = new SubQuestion ();
        public ISubQuestion  SubQqestion3 = new SubQuestion ();
        public DataBaseController  DB = new DataBaseController ();
        public Ientity  Entity1 = new entity ();
        public Ientity  Entity2 = new entity ();
        public Ientity  Entity3 = new entity ();
        public Ientity  Entity4 = new entity ();
        //public Ientity  Entity5 = new entity ();
        //public Ientity  Entity6 = new entity ();
        //public Ientity  Entity7 = new entity ();

        public WordObject  WordObject1 = new WordObject ();
        public WordObject  WordObject2 = new WordObject ();
        public WordObject  WordObject3 = new WordObject ();
        public WordObject  WordObject4 = new WordObject ();

        public WordObject  WordGufObjectShe = new WordObject ();
        public WordObject  WordGufObjectThey = new WordObject ();
        public WordObject  WordGufObjectHe = new WordObject ();
        public WordObject  WordGufObjectIt = new WordObject ();


        //public List<List<WordObject   wordObjectsListList;
        //public List<WordObject  wordObjectsList;

        public EventObject eventO = new EventObject("הכרזת העצמאות");

        public OrganizationObject orgO = new OrganizationObject("אום");
        public PersonObject persO = new PersonObject("בן גוריון");
        public ConceptObject concO = new ConceptObject("מנדט");
        //public List<WorldObject  list1 = new List<WorldObject ();
        //public List<WorldObject  list2 = new List<WorldObject ();
        public string userAnswerSubQuestion1;


        internal void initializeObject()
        {
            SubQqestion1 = new SubQuestion();
            SubQqestion1.questionText = "הצג כיצד הסתיימה מלחמת העצמאות ואילו בעיות נותרו בלתי פתורות בעקבותיה.";
            SubQqestion1.answerText = "בעיית הפליטים - כ500 אלף ערבים עזבו את בתיהם והפכו לפליטים. רובם לא קיבלו אזרחות במדינות אליהם ברחו והם שואפים לחזור לבתיהם - זכות השיבה | ירושליים נותרה מחולקת בשלטון הירדנים;";
            SubQqestion1.flags = "needAll";

            userAnswerSubQuestion1 = "ירושלים נותרה מחולקת, והרבה ערבים נותרו כפליטים במדינות השכנות";

            WordObject1.Text = "ערבי";
            WordObject1.Lemma = "ערבי";
            WordObject1.WordT = WordObject.WordType.organizationWord;
            WordObject1.Gender = genderType.masculine;
            WordObject1.Amount = amountType.plural;


            WordGufObjectThey = new WordObject();
            WordGufObjectThey.WordT = WordObject.WordType.gufWord;
            WordGufObjectThey.Gender = genderType.masculine;
            WordGufObjectThey.Amount = amountType.plural;


            Entity1.entityValue = "ערבי";
            Entity1.entityType = "organizationWord";

            WordObject2.Text = "ירושלים";
            WordObject2.WordT = WordObject.WordType.locationWord;

            Entity2.entityValue = "ירושלים";
            Entity2.entityType = "locationWord";

            WordObject3.Text = "פליט";
            WordObject3.WordT = WordObject.WordType.conceptWord;
            WordObject3.Gender = genderType.masculine;
            WordObject3.Amount = amountType.plural;

            Entity3.entityValue = "פליט";
            Entity3.entityType = "conceptWord";

            WordObject4.Text = "ירדן";
            WordObject4.WordT = WordObject.WordType.organizationWord;
            WordObject4.Gender = genderType.feminine;

            Entity4.entityValue = "ירדן";
            Entity4.entityType = "locationWord";






        }
        public string EnumVal(Pkey key)
        {
            return Enum.GetName(typeof(Pkey), key);
        }
    }
}
