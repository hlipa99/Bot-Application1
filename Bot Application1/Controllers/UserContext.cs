using System;

namespace Bot_Application1.Controllers
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