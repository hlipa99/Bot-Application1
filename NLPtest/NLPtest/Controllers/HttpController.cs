using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.Controllers
{
    class HttpController
    {
        public string sendToHebrewMorphAnalizer(string text)
        {
            try
            {
                WebRequest request = WebRequest.Create("http://localhost:4567/parse");
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
    }
}
