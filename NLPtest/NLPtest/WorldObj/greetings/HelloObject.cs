﻿

using System;

namespace NLPtest.WorldObj
{
    internal class HelloObject : WorldObject
    {
        private string type;

        public HelloObject()
        {
          
        }

        public HelloObject(string v)
        {
            this.type = v;
        }



        public override int GetHashCode()
        {
            if(type != null)
            {
              return 1000 + type.GetHashCode();
            }else
            {
                return 1000;
            }
        }

        internal string getObjectType()
        {
            return type;
        }
    }
}