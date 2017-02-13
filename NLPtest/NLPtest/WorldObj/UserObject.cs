
using System;

namespace NLPtest.WorldObj
{

    public class UserObject : PersonObject
    {
        public string gender;
        public string userClass;
        private string userGender;

        public UserObject(string v) : base(v)
        {
        }

        public UserObject(string v, string userGender) : this(v)
        {
            this.userGender = userGender;
        }

        public string getUserName()
        {
            return Name;
        }

        internal string getGender()
        {
            return gender;
        }

        public override IWorldObject Clone()
        {
            UserObject res = new UserObject(Word);
           res.cloneBase(this);
            return res;
        }
    }
}