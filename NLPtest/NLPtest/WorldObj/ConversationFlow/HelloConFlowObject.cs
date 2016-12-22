using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.WorldObj.ConversationFlow
{
    class HelloConFlowObject : ConversationFlowObject
    {
        private HelloObject introjection;
        private HelloObject inquary;

        public HelloConFlowObject(HelloObject introjection, HelloObject inquary)
        {
            this.introjection = introjection;
            this.inquary = inquary;
        }


        public override ContentTurn getResponse()
        {
            ContentTurn res = new ContentTurn();

          
            if (introjection != null)
            {
                res.Add(new HelloObject());
            }

            if (inquary != null)
            {
                res.Add(new MyStatusObject());
            }

            if(introjection == null && inquary == null)
            {
                throw new EmptyWorldObjectException("helloObject");
            }


            return res;
        }
    }
}
