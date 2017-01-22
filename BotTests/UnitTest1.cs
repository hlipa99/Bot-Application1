using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Bot.Connector.DirectLine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Model;
using Bot_Application1.Controllers;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        
        string convID = "";
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

        [TestInitialize]
        public void TestInitializeAttribute()
        {
            db = new DataBaseController();
            createNewClientConversation(out convID);
            Client = createNewClientConversation(out convID);
            var task = sendMessage("/deleteprofile");
            task.Wait();
            var response = task.Result;
            Assert.IsTrue(Contains(response, new string[] { "User profile deleted!" }));
        }

        [TestCleanup]
        public void TestCleanup()
        {
             convID = "";
            // client = null;
        }

        [TestMethod]
        public void testALLDialogs()
        {
            assertNewUserDialog();
            assertLearningDialog();
        }

        private void assertLearningDialog()
        {
            options = getOptions(response[2]);
            response = sendBot(options[1]);   //learning options
            Assert.IsTrue(response.Count > 2); //learning options number

            response = sendBot("בלה בלה");   //learning topic options
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.NotAnOption))); 

            response = sendBot("לאומיות");   //learning topic options
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.letsLearn))); //class assert

            response = sendBot("תשובה 1");   //class options
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.notAnAnswer))); //class assert

            response = sendBot("sdsdds");   //evaluation wrong option
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.notNumber)));

            response = sendBot("100");   //evaluation wrong option
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.GeneralAck)));

            response = sendBot(" תשובה תשובה 2 2");   //class options
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.goodAnswer))); //class assert

            response = sendBot("100");   //evaluation wrong option
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.GeneralAck)));

            response = sendBot(" תשובה תשובה 3 3");   //class options
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.goodAnswer))); //class assert

            response = sendBot("100");   //evaluation wrong option
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.GeneralAck)));
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.endOfSession)));
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.goodSessionEnd)));
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.MainMenuText)));
        }

        private string[] DBbotPhrase(Pkey key)
        {
            return db.getBotPhrase(Pkey.letsLearn, new string[] { }, new string[] { });
        }

        private void assertNewUserDialog()
        {
            task = sendConversationContantUpdated("add");
            task.Wait();
            response = task.Result;
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.selfIntroduction))); //wellcome message
            response = sendBot("יוחאי");
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.NewUserGreeting))); //bot using user name


            options = getOptions(response[2]);  //gender options
            response = sendBot(options[1]);
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.GenderAck))); //gender assert

            options = getOptions(response[1]);
            response = sendBot(options[1]);   //class options
            Assert.IsTrue(Contains(response, DBbotPhrase(Pkey.GeneralAck))); //class assert
        }

  




        private bool Contains(List<string> response, string[] options)
        {
            string res = "";
            foreach (var r in response)
            {
                res += r;
            }
 
                foreach (var o in options)
                {
                    var pStr = o;
                    while(pStr.Contains("<") || pStr.Contains("<"))
                    {
                        pStr.Remove(pStr.IndexOf("<"), pStr.IndexOf(">") - pStr.IndexOf("<"));
                    }

                    foreach (var w in pStr.Split(' '))
                    {
                        if (!res.Contains(w)) return false;
                       
                    }
                       
                }
            
            return true;
        }

        private string[] getOptions(string response)
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

        private List<string> sendBot(string test)
        {
            var task = sendMessage(test);
            task.Wait();
            var response = task.Result;
            return response;
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

        private async Task<List<string>> sendConversationContantUpdated(string action)
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
            var i = client.Conversations.GetActivities(convID).Activities.Count;
            await client.Conversations.PostActivityAsync(convID, activity);
         
            var activities = client.Conversations.GetActivities(convID);
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



    }
}
    
