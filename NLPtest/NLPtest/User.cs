using System;

namespace NLPtest
{

    [Serializable]
    public class User
    {
        public string gender;
        public string userClass;
        public string userName;

        public User(string v)
        {
            this.userName = v;
        }

        public User()
        {
        }

        internal string getUserName()
        {
            return userName;
        }

        internal string getGender()
        {
            return gender;
        }
    }
}