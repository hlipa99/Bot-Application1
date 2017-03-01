using System;

namespace NLPtest.MorfObjects
{
    internal class MessageObject
    {
        private string tags;
        private string vars;
        private string content;

        public MessageObject(string content)
        {
            this.content = content;
        }

        public MessageObject(string content,string vars)
        {
            this.vars = vars;
            this.content = content;

        }

        public MessageObject(string content, string vars, string tags) : this(content, vars)
        {
            this.content = content;
            this.tags = tags;
            this.vars = vars;
        }

        internal string GetVars()
        {
            return vars;
        }

        internal string getString()
        {
            return content;
        }
    }
}