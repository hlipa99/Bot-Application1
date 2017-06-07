using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Bot.Connector.DirectLine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Model;
using Bot_Application1.Controllers;
using Model.dataBase;
using System.Threading;
using BotTests;
using System.Diagnostics;


namespace UnitTestProject1
{
    public class DialogsTestsBase
    {
        
        string convID = "";
        DirectLineClient client = null;
        Task<List<string>> task;
        string[] options = null;
        List<string> response = null;
        DataBaseController db;

        public Random rand;

        public void initRand()
        {
            Random seedRand = new Random();
            int seed = seedRand.Next();
            rand = new Random(seed);
            Console.WriteLine("seed:" + seed);
        }


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

        public string ConvID { get ; set; }

        public DialogsTestsBase()
        {
            db = new DataBaseController();
            createNewClientConversation(out convID);
            Client = createNewClientConversation(out convID);
            ConvID = convID;
        }


        public string[] DBbotPhrase(Pkey key)
        {
            return db.getBotPhrase(key, new string[] { }, new string[] { });
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

        public async Task<List<string>> sendBot(string test,bool userS = false, string user = null)
        {
            Debug.WriteLine("await sendBotStart:" + test);
            var task = userS ? await sendMessage(test, user) : await sendMessage(test);

            Console.WriteLine(test);
            Debug.WriteLine("await sendBotEnd:");
            //Thread.Sleep(500);
            printRes(task);
            return task;
        }

        public void printRes(List<string> res)
        {
            foreach (var s in res)
            {
                Debug.WriteLine(s);
                Console.WriteLine(s);
            }
        }

        public async Task<List<string>> getToLearningMenu()
        {
            var res = await sendBot("היי");
            res = await sendBot("יוחאי");
            res = await sendBot("בן");
            res = await sendBot("יא'");
            var options = getOptions(res[2]);
            res = await sendBot(options[1]);
            return res;
        }

        public void deleteProfile()
        {
            var task = sendMessage("/deleteprofile");
            var response = task;
            response.Wait();
            AssertNLP.contains(response.Result, "User profile deleted!");
        }


        public async Task<List<string>> sendBot(string test1, string test2)
        {
            var task1 = sendMessage(test1);
            var task2 = sendMessage(test2);
            task1.Wait();
            task2.Wait();
            var res = task1.Result;
            res.AddRange(task2.Result);
            return res;
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


        public async Task<List<string>> sendMessage(string message,string user = "testUser")
        {
           
            Activity userMessage = new Activity
            {
                From = new ChannelAccount(id: user),
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
            var iTask = client.Conversations.GetActivities(convID);


            var i = iTask.Activities.Count;
            await client.Conversations.PostActivityAsync(ConvID, activity);
            //Thread.Sleep(500);
            var activities = await client.Conversations.GetActivitiesAsync(ConvID);

            var activitiesRes = activities;

            i++;
            List<string> resualtTest = new List<string>();

          

            for (; i < activitiesRes.Activities.Count; i++)
            {
                if(activitiesRes.Activities[i].Text != null)
                {
                    if (activitiesRes.Activities[i].Text.Trim() != ""){
                             resualtTest.Add(activitiesRes.Activities[i].Text);

                   }
                }

                if(activitiesRes.Activities[i].Attachments != null)
                {
                    foreach (var a in activitiesRes.Activities[i].Attachments)
                    {
                        if (a.Content != null)
                        {
                            resualtTest.Add(a.Content.ToString());
                        }
                    }
                }

            }

            return resualtTest;

        }

        public async  Task<List<string>> endLearning()
        {
            var res = await sendBot("טוב מספיק");
            res = await sendBot("");
            return res;
        }

        public async  Task<List<string>> endConversation()
        {
            var res = await sendBot("ביי");
            res = await sendBot("מספיק");
            res = await sendBot("כן");

            res = await sendBot("ביי");
            res = await sendBot("לילה טוב");
            return res;
        }

        public async  Task<List<string>> createUser(string name, string gender, string classVal)
        {
            var res = await sendBot("היי");
            
            res = await sendBot(name);
            res = await sendBot(gender);
            res = await sendBot(classVal);
            return res;
        }





    }
}
    
