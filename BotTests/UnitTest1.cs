using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading;
using System.Net;
using Microsoft.Bot.Connector.DirectLine;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async void TestMethod1()
        {
            var secret = "IK1o_f1fBYc.cwA.LZs.ZrbtNGSTAYzcLqc6DPbq_0bBOV_zzyMSYn7KCaoVdpI";
            var uri = new Uri("https://directline.botframework.com");
            var creds = new DirectLineClientCredentials(secret);

            DirectLineClient client = new DirectLineClient(uri, creds);
            Conversations convs = new Conversations(client);


            var conv = convs.StartConversation();

            var set = await convs.GetActivitiesAsync(conv.ConversationId);
            var a = set;
            
            //IMessageActivity message = new Message(conversationId: conv.ConversationId, text: "do that thing");
            //Console.WriteLine(message.Text);
            //convs.PostMessage(conv.ConversationId, message);

            //set = convs.GetMessages(conv.ConversationId, waterMark);
            //PrintResponse(set);
            //waterMark = set.Watermark;










            //using (var client = new HttpClient())
            //{
          

            //    {

            //        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://directline.botframework.com/v3/directline/tokens/generate"))
            //    {
            //        //     var secret = new Requ
            //        request.Headers.Add("Authorization", "Bearer IK1o_f1fBYc.cwA.LZs.ZrbtNGSTAYzcLqc6DPbq_0bBOV_zzyMSYn7KCaoVdpI");

            //        using (HttpResponseMessage response = client.SendAsync(request, CancellationToken.None).Result)
            //        {
            //            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            //        }
            //    }
            //}
        }
    }
}
    
