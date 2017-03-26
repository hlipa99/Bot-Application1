using System;

namespace Bot_Application1.Models

{
    [Serializable]
    public class UserContext
    {
        internal string dialog;
        internal DateTime? lastSeen;

        public UserContext(string d)
        {
            dialog = d;
        }




    }
}