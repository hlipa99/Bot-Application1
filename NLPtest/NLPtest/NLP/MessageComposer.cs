using NLP.MorfObjects;
using NLP.WorldObj;
using System;
using System.Collections.Generic;

namespace NLP.NLP
{

    public class MessageComposer
    {

        MessageDictionary messageDictionary = new MessageDictionary();


        public MessageComposer()
        {
            messageDictionary.Add("NLPtest.WorldObj.HelloObject", new MessageObject("היי, טוב לפגוש אותך <username>","<username>"));
            messageDictionary.Add("NLPtest.WorldObj.HelloObject", new MessageObject("שלום <username>,", "<username>"));
            messageDictionary.Add("NLPtest.WorldObj.HelloObject", new MessageObject("<timeOfDay> טוב <username>,", "<username>|<timeOfDay>", ""));            
                                
            messageDictionary.Add("NLPtest.WorldObj.HelloObject", new MessageObject("מה קורה?"));
            messageDictionary.Add("NLPtest.WorldObj.HelloObject", new MessageObject("מה המצב אצלך?"));
            messageDictionary.Add("NLPtest.WorldObj.HelloObject", new MessageObject("איך הולך?"));
                              
                              
            messageDictionary.Add("NLPtest.WorldObj.MyStatusObject", new MessageObject("אצלי הכל בסדר"));
            messageDictionary.Add("NLPtest.WorldObj.MyStatusObject", new MessageObject("אני יודע.. חיים"));
            messageDictionary.Add("NLPtest.WorldObj.MyStatusObject", new MessageObject("הכל טוב. מסתדרים"));
            messageDictionary.Add("NLPtest.WorldObj.MyStatusObject", new MessageObject("בסדר. אין תלונות"));

        }

        //public List<string> compose(ContentList content,UserObject user)
        //{
        //    List<string> res = new List<string>();
        //    while (!content.empty())
        //    {
        //        res.Add(createMessage(content.pop(), user));
        //    }
        //    return res;
        //}

            private string createMessage(WorldObject worldObject,UserObject user)
            {

                var en = messageDictionary.getRandomEnumerator(worldObject.GetType().ToString());
            en.MoveNext(); //get first
            //do
            //{
            //choose message by flages - time, feelings, personallity, etc.
            //}
            //while (en.Current != null);
            var message = en.Current; //temp TODO ^
                var messageString = message.getString(); ;
                var vars = message.GetVars();
            if (vars != null)
            {
                foreach (var s in vars.Split('|'))
                {
                    if (s == "<username>")
                    {
                        messageString = messageString.Replace(s, user.getUserName());
                    }
                    else if (s == "<timeOfDay>")
                    {
                        messageString = messageString.Replace(s, messageDictionary.getTimeOfDay());
                    }
                }
            }


                return messageString;

            }
        }
    }
