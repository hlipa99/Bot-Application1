using System;
using System.Net;
using System.Reflection;
using System.Text;

using Bot_Application1.Json;
using Bot_Application1.log;
using System.Collections.Specialized;
using Speedy.Linq;

namespace Bot_Application1.YAndex
{
    public class ControlerGrammar
    {

        public static string  start(string str)
        {

            try
            {
                // string strUrl = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=he&dt=t&q="; ;
                string strUrl = "https://translate.google.co.il/?hl=iw#iw/en/";
                //strUrl += "key=trnsl.1.1.20161229T103726Z.33fde522ea363785.a069a5219ef7414abd5b7042ee5812951fc317a4";
                //strUrl += "&text=";
                strUrl += str;
               // strUrl += "&lang=en-he";



                WebClient wc = new WebClient();
            //    var encoding = System.Text.Encoding.UTF8;
            //    wc.Encoding = encoding; 
           //     var strJson = wc.DownloadString(strUrl);

                var pageSourceCode = wc.DownloadString(strUrl);
                byte[] bytes = Encoding.Default.GetBytes(pageSourceCode);
                String myString = Encoding.UTF8.GetString(bytes);

                //    byte[] bytes = Encoding.Default.GetBytes(strJson);
                //   String myString = Encoding.UTF8.GetString(strJson);
                //tr = Newtonsoft.Json.JsonConvert.DeserializeObject<Translate>(strJson);
                return "dsd";
            }
            catch (Exception e)
            {
                Logger.log("ControlerGrammar", MethodBase.GetCurrentMethod().Name, e.ToString());
            }
            return "Dfdf";
        }


     


    }
}