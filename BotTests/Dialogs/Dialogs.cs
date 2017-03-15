using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Bot.Connector.DirectLine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Model;
using Bot_Application1.Controllers;
using Model.dataBase;

namespace UnitTestProject1
{
    public class DialogsTestsBase
    {
        
        string ConvID = "";
        DirectLineClient client = null;
        Task<List<string>> task;
        string[] options = null;
        List<string> response = null;
        DataBaseController db;

        public DirectLineClient Client
        {
            get
            {
                return client;
            }

            set
            {
                client = value;
            }
        }

        public string ConvID1 { get => ConvID; set => ConvID = value; }

        public DialogsTestsBase()
        {
            db = DataBaseController.getInstance();
            createNewClientConversation(out ConvID);
            Client = createNewClientConversation(out ConvID);
        
        }


        public string[] DBbotPhrase(Pkey key)
        {
            return db.getBotPhrase(Pkey.letsLearn, new string[] { }, new string[] { });
        }


        public bool Contains(List<string> response, string[] options)
        {
            string res = "";
            foreach (var r in response)
            {
                res += r;
            }
 
                foreach (var o in options)
                {
                    var pStr = o;
                    while(pStr.Contains("<") && pStr.Contains(">"))
                    {
                        pStr = pStr.Remove(pStr.IndexOf("<"), pStr.IndexOf(">") - pStr.IndexOf("<"));
                    }

                    foreach (var w in pStr.Split(' '))
                    {
                        if (!res.Contains(w)) return false;
                       
                    }
                       
                }
            
            return true;
        }

        public string[] getOptions(string response)
        {
            List < string > res = new List<string>();
            string[] result = null;
            dynamic stuff = JsonConvert.DeserializeObject(response);
            res.Add(stuff["text"].ToString());
            foreach(var b in stuff["buttons"])
            {
                res.Add(b["value"].ToString());
            }
            return res.ToArray();
        }

        public List<string> sendBot(string test)
        {
            var task = sendMessage(test);
            task.Wait();
            var response = task.Result;
            return response;
        }

        public List<string> sendBot(string test1, string test2)
        {
            var task1 = sendMessage(test1);
            var task2 = sendMessage(test2);
            task1.Wait();
            var response1 = task1.Result;
            var response2 = task1.Result;
            response1.AddRange(response2);
            return response1;
        }

        public DirectLineClient createNewClientConversation(out string convID)
        {
            string secret = "IK1o_f1fBYc.cwA.LZs.ZrbtNGSTAYzcLqc6DPbq_0bBOV_zzyMSYn7KCaoVdpI";
            Uri uri = new Uri("https://directline.botframework.com");
            var creds = new DirectLineClientCredentials(secret);

            DirectLineClient client = new DirectLineClient(uri, creds);
            Conversations convs = new Conversations(client);

            var conv = convs.StartConversation();
            convID = conv.ConversationId;
            return client;
        }


        public async Task<List<string>> sendMessage(string message)
        {
           
            Activity userMessage = new Activity
            {
                From = new ChannelAccount("testDirectApi"),
                Text = message,
                Type = ActivityTypes.Message
            };
            return await sendActivity(userMessage);

        }

        public async Task<List<string>> sendConversationContantUpdated(string action)
        {
            Activity activity = new Activity
            {
                From = new ChannelAccount("testDirectApi"),
                Type = ActivityTypes.ContactRelationUpdate,
                Action = action

            };
            return await sendActivity(activity);
       }


        public async Task<List<string>> sendActivity(Activity activity)
        {
            var i = client.Conversations.GetActivities(ConvID1).Activities.Count;
            await client.Conversations.PostActivityAsync(ConvID1, activity);
         
            var activities = client.Conversations.GetActivities(ConvID1);
            i++;
            List<string> resualtTest = new List<string>();

          

            for (; i < activities.Activities.Count; i++)
            {
                if(activities.Activities[i].Text != null)
                {
                    if (activities.Activities[i].Text.Trim() != ""){
                             resualtTest.Add(activities.Activities[i].Text);

                   }
                }

                if(activities.Activities[i].Attachments != null)
                {
                    foreach (var a in activities.Activities[i].Attachments)
                    {
                        resualtTest.Add(a.Content.ToString());
                    }
                }

            }

            return resualtTest;

        }


        public List<string> endConversation()
        {
            var res = sendBot("ביי");
            res = sendBot("יום טוב");
            return res;
        }

        public List<string> createUser(string v1, string v2, string v3)
        {
            var res = sendBot("היי");
            res = sendBot("יוחאי");
            res = sendBot("בן");
            res = sendBot("יא");
            return res;
        }


    }
}
    
