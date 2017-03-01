using System;

namespace Bot_Application1.Models

{
    [Serializable]
    public class UserContext
    {
        internal string dialog;

        public UserContext(string d)
        {
            dialog = d;
        }
    }
}