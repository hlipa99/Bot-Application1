using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiAiSDK;
using ApiAiSDK.Model;


namespace NLPtest.Controllers
{
    class OuterAPIController
    {
        private ApiAi apiAi;

        public OuterAPIController()
        {
            var config = new AIConfiguration("b9069a86b4f1499ab8560e9f6ee5b54a", SupportedLanguage.English);
            apiAi = new ApiAi(config);

        }


        public string getIntentApiAi(string str,string context)
        {
          
            try
            {
                var response = apiAi.TextRequest(str);
                var intent = response.Result.Metadata.IntentName;
                return intent;
            }
            catch(Exception ex)
            {
                 return null;

            }
        }



        public string sendToHebrewMorphAnalizer(string text)
        {
            try
            {
                WebRequest request = WebRequest.Create("http://6a554add.ngrok.io/parse");
                string responseFromServer = "";
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                request.ContentType = "application/json;charset=utf-8";

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                using (WebResponse response = request.GetResponse())
                {

                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(dataStream))
                        {

                            responseFromServer = reader.ReadToEnd();
                        }
                    }
                }

                return responseFromServer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    
    public string correctSpelling(string text)
    {
        try
        {
            WebRequest request = WebRequest.Create("http://xspell.ga/?token=57d1b5fd8f45189c136d0b99c628d4e1&check=" + text);
   //             WebRequest request = WebRequest.Create("http://xspell.ga/?token=c9acedeff1e873a46bef7a6c38e5d82d&check=\"" + text + "\"");

                string responseFromServer = "";
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            using (WebResponse response = request.GetResponse())
            {

                using (Stream dataStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(dataStream))
                    {

                        responseFromServer = reader.ReadToEnd();
                    }
                }
            }

            if(responseFromServer != null && !responseFromServer.Contains("Invalid") && !(responseFromServer.Length > text.Length + 50))
                {
                    return new string(responseFromServer.Where(x=> ((x >= 0x0590) && (x <= 0x05FF)) || char.IsPunctuation(x) || char.IsWhiteSpace(x)).ToArray());
                }else
                {
                    return null;
                }
          
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
}
