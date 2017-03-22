
using Bot_Application1.YAndex;

using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;


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
                //Logger.log("ControlerTranslate", MethodBase.GetCurrentMethod().Name, e.ToString());
            }

            return tr.text[0];
             
        }

        public static string TranslateToEng(string str)
        {
            Translate tr = new Translate();
            try
            {
                string strUrl = "https://translate.yandex.net/api/v1.5/tr.json/translate?"; ;
                strUrl += "key=trnsl.1.1.20161229T103726Z.33fde522ea363785.a069a5219ef7414abd5b7042ee5812951fc317a4";
                strUrl += "&text=";
                strUrl += str;
                strUrl += "&lang=he-en";

                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                string strJson = wc.DownloadString(strUrl);
                tr = Newtonsoft.Json.JsonConvert.DeserializeObject<Translate>(strJson);
                return tr.text[0];
            }
            catch (Exception e)
            {
                //Logger.log("ControlerTranslate", MethodBase.GetCurrentMethod().Name, e.ToString());
            }

            return tr.text[0];
        }
    }

    public class Translate
    {
        public int code { get; set; }
        public string lang { get; set; }
        public List<string> text { get; set; }
    }


}

