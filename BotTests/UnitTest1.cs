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
        public void TestMethod1()
        {
            using (var client = new HttpClient())
            {
                DirectLineClient DLclient = new DirectLineClient();


                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://directline.botframework.com/v3/directline/tokens/generate"))
                {
                    //     var secret = new Requ
                    request.Headers.Add("Authorization", "Bearer IK1o_f1fBYc.cwA.LZs.ZrbtNGSTAYzcLqc6DPbq_0bBOV_zzyMSYn7KCaoVdpI");

                    using (HttpResponseMessage response = client.SendAsync(request, CancellationToken.None).Result)
                    {
                        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
                    }
                }
            }
        }
    }
}
    
