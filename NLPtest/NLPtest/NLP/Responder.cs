using NLPtest.view;
using NLPtest.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest
{

    public class Responder
    {
        public ContentTurn respone(ContentTurn input,ConversationContext context)
        {

            ContentTurn res = new ContentTurn();
            while (!input.empty())
            {
                var o = input.pop();
                if(o is HelloObject)
                {
                    var h = o as HelloObject;
                    if(h.getObjectType() == "intergection")
                    {
                        res.Add(new HelloObject());
                    }
                    else if (h.getObjectType() == "inquisive")
                    {
                        res.Add(new MyStatusObject());
                    }
                }
                else
                {
                    throw new CantResponeException(o);
                }
            }

            return res;
        } 

    }
}
