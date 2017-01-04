using Bot_Application1.log;
using Bot_Application1.YAndex;

using System;
using System.Net;
using System.Reflection;
using System.Text;

using Bot_Application1.Json;

namespace Bot_Application1.YAndex
{
    public class ControlerTranslate
    {

        

        public static string Translate(string str)
        {
            Translate tr = new Translate();
            try
            {
                string strUrl = "https://translate.yandex.net/api/v1.5/tr.json/translate?"; ;
                strUrl += "key=trnsl.1.1.20161229T103726Z.33fde522ea363785.a069a5219ef7414abd5b7042ee5812951fc317a4";
                strUrl += "&text=";
                strUrl += str; 
                strUrl += "&lang=en-he";

                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                string strJson = wc.DownloadString(strUrl);
                tr  = Newtonsoft.Json.JsonConvert.DeserializeObject<Translate>(strJson);
                return tr.text[0];
            }catch(Exception e)
            {
                Logger.log("ControlerTranslate", MethodBase.GetCurrentMethod().Name, e.ToString());
            }

            return tr.text[0];
             
        }

    }

    
    
}