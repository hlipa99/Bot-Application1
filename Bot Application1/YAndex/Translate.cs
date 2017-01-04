using Bot_Application1.YAndex;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using Yandex.Translator;

namespace Bot_Application1.YAndex
{
    public class ControlerTranslate
    {

        

        public static string Translate(string str)
        {

            string strUrl = "https://translate.yandex.net/api/v1.5/tr.json/translate ?";
            strUrl += "key = trnsl.1.1.20161229T103726Z.33fde522ea363785.a069a5219ef7414abd5b7042ee5812951fc317a4";
            strUrl += " & text = " + str;
            strUrl += "&lang = en - he";

            WebClient wc = new WebClient();
           wc.Encoding = Encoding.UTF8;
            string strJson = wc.DownloadString(strUrl);

            return strJson;   
        }

    }

    
    
}