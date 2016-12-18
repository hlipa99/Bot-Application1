using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace fxp
{
    class Controler
    {

        private string urlMain = "https://www.fxp.co.il/forumdisplay.php?f=550";
        

        public void start()
        {

            startHttp();
          //  var webClient = new WebClient();
          //  var pageSourceCode = webClient.DownloadString(urlMain);
          //  byte[] bytes = Encoding.Default.GetBytes(pageSourceCode);
         //   String myString = Encoding.UTF8.GetString(bytes);
         //   startHtml(myString);

        }

        private void startHtml(String st)
        {


            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(st);
            IEnumerable<HtmlAgilityPack.HtmlNode> forms;


            HtmlAgilityPack.HtmlNode countPage = htmlDoc.GetElementbyId("yui-gen4");

            //     List<HtmlAgilityPack.HtmlNode> h = forms.ToList();

            //       forms = htmlDoc.DocumentNode.Descendants("h1");
            //      List<HtmlAgilityPack.HtmlNode> h1 = forms.ToList();

            //            forms = htmlDoc.DocumentNode.Descendants("h2");
            //          List<HtmlAgilityPack.HtmlNode> h2 = forms.ToList();

            //          forms = htmlDoc.DocumentNode.Descendants("h3");
            //         List<HtmlAgilityPack.HtmlNode> h3 = forms.ToList();

      //      forms = htmlDoc.DocumentNode.Descendants("p");
      //      List<HtmlAgilityPack.HtmlNode> p = forms.ToList();

            //            forms = htmlDoc.DocumentNode.Descendants("p1");
            //          List<HtmlAgilityPack.HtmlNode> p1 = forms.ToList();

        }


        public void startHttp()
        {
            HttpWebRequest webReq = (HttpWebRequest)HttpWebRequest.Create(urlMain);
            try
            {
                webReq.CookieContainer = new CookieContainer();
                webReq.Method = "GET";
                using (WebResponse response = webReq.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        String res = reader.ReadToEnd();
                   //     byte[] bytes = Encoding.Default.GetBytes(res);
                   //     String myString = Encoding.UTF8.GetString(bytes);
                        startHtml(res);


                    }
                }
            }
            catch (Exception ex)
            {

    
            }
        }
    }
}
