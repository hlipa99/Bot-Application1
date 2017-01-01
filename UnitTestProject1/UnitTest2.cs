//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.Bot.Connector;
//using Microsoft.Bot.Builder.Dialogs;
//using Bot_Application1.IDialog;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;

//namespace UnitTestProject1
//{
//    [TestClass]
//    public class UnitTest2
//    {
//        [TestMethod]
//        public async Task TestMethod1()
//        {
//       //         < add key = "MicrosoftAppId" value = "604abc6d-5c77-45b7-b4d9-b1677e8d74c4" />
   
//       //< add key = "MicrosoftAppPassword" value = "TcR1SbxXb6TvDkfzecPZmp9" />

//                  var client = new HttpClient();
//            client.BaseAddress = new Uri("https://directline.botframework.com/api/conversations/");
//            client.DefaultRequestHeaders.Accept.Clear();
//            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", "TcR1SbxXb6TvDkfzecPZmp9");

//            var response =  await client.GetAsync("/api/tokens/");
//           // (HttpResponseMessage)
         
//                Assert.IsTrue(response.IsSuccessStatusCode) ;


//            var conversation = new Conversation();
//            response = await client.PostAsJsonAsync("/api/conversations/", conversation);
//            if (response.IsSuccessStatusCode)
//            {

//                Conversation ConversationInfo = response.Content.ReadAsAsync(typeof(Conversation)).Result as Conversation; string conversationUrl = ConversationInfo.conversationId + "/messages/"; Message msg = new Message() { text = message }; response = await client.PostAsJsonAsync(conversationUrl, msg);
//                if (response.IsSuccessStatusCode)
//                {
//                    response = await client.GetAsync(conversationUrl);
//                    if (response.IsSuccessStatusCode) {
//                    //    MessageSet BotMessage = response.Content.ReadAsAsync(typeof(MessageSet)).Result as MessageSet;
//                    //    ViewBag.Messages = BotMessage; IsReplyReceived = true;
//                    }
//                }
//            }
//        }

//        private async Task<bool> PostMessage(string message)
//        {
//            bool IsReplyReceived = false;

//            var client = new HttpClient();
//            client.BaseAddress = new Uri("https://directline.botframework.com/api/conversations/");
//            client.DefaultRequestHeaders.Accept.Clear();
//            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", "TcR1SbxXb6TvDkfzecPZmp9");
//            var response = await client.GetAsync("/api/tokens/");
//            if (response.IsSuccessStatusCode)
//            {
//                var conversation = new Conversation();
//                response = await client.PostAsJsonAsync("/api/conversations/", conversation);
//                if (response.IsSuccessStatusCode)
//                {
//                    Conversation ConversationInfo = response.Content.ReadAsAsync(typeof(Conversation)).Result as Conversation;
//                    string conversationUrl = ConversationInfo.conversationId + "/messages/";
//                    Message msg = new Message() { text = message };
//                    response = await client.PostAsJsonAsync(conversationUrl, msg);
//                    if (response.IsSuccessStatusCode)
//                    {
//                        response = await client.GetAsync(conversationUrl);
//                        if (response.IsSuccessStatusCode)
//                        {
//                            MessageSet BotMessage = response.Content.ReadAsAsync(typeof(MessageSet)).Result as MessageSet;
//                            ViewBag.Messages = BotMessage;
//                            IsReplyReceived = true;
//                        }
//                    }
//                }

//            }
//            return IsReplyReceived;
//        }

//        public class Conversation
//        {
//            public string conversationId { get; set; }
//            public string token { get; set; }
//            public string eTag { get; set; }
//        }

//        public class MessageSet
//        {
//            public Message[] messages { get; set; }
//            public string watermark { get; set; }
//            public string eTag { get; set; }
//        }

//        public class Message
//        {
//            public string id { get; set; }
//            public string conversationId { get; set; }
//            public DateTime created { get; set; }
//            public string from { get; set; }
//            public string text { get; set; }
//            public string channelData { get; set; }
//            public string[] images { get; set; }
//            public Attachment[] attachments { get; set; }
//            public string eTag { get; set; }
//        }

//        public class Attachment
//        {
//            public string url { get; set; }
//            public string contentType { get; set; }
//        }


//        private async Task<bool> PostMessage(string message)
//    {
//        bool IsReplyReceived = false;

//        client = new HttpClient();
//        client.BaseAddress = new Uri("https://directline.botframework.com/api/conversations/");
//        client.DefaultRequestHeaders.Accept.Clear();
//        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", "[Your Secret Code Here]");
//        response = await client.GetAsync("/api/tokens/");
//        if (response.IsSuccessStatusCode)
//        {
//            var conversation = new Conversation();
//            response = await client.PostAsJsonAsync("/api/conversations/", conversation);
//            if (response.IsSuccessStatusCode)
//            {
//                Conversation ConversationInfo = response.Content.ReadAsAsync(typeof(Conversation)).Result as Conversation;
//                string conversationUrl = ConversationInfo.conversationId+"/messages/";
//                Message msg = new Message() { text = message };
//                response = await client.PostAsJsonAsync(conversationUrl,msg);
//                if (response.IsSuccessStatusCode)
//                {
//                    response = await client.GetAsync(conversationUrl);
//                    if (response.IsSuccessStatusCode)
//                    {
//                        MessageSet BotMessage = response.Content.ReadAsAsync(typeof(MessageSet)).Result as MessageSet;
//                        ViewBag.Messages = BotMessage;
//                        IsReplyReceived = true;
//                    }
//                }
//            }

//        }
//        return IsReplyReceived;
//    }

//    }
//}
