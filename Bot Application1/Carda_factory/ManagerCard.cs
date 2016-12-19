using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bot_Application1.Cardatt_achment
{
    public class ManagerCard
    {

        public Attachment GetMainMenuChoice(String titles,string subTitle,String text,String urlImage,String[] buttonMessage )
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            var heroCard = new HeroCard
            {
                Title =titles,
                Subtitle = subTitle,
                Text = text,
                //Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction>
                {

                       new CardAction(ActionTypes.PostBack, "שיבת ציון", value: "start"),
                       new CardAction(ActionTypes.PostBack, "שואה", value: "yes"),
                       new CardAction(ActionTypes.PostBack, "עליה שניה", value: "Aliya2")

                     
                   
                }
            };


                   

            return heroCard.ToAttachment();
        }


        //private List<CardAction> GetListCardAction(Dictionary<string, string> dataMap )
        //{
        //    List<CardAction> listcars = new List<CardAction>();
        //    foreach (var item in myDictionary)
        //    {
        //        CardAction ca = new CardAction(ActionTypes.PostBack,dataMap.G , value: "start");

        //    }

        //}
        public Attachment GetCardAction(String[] titles )
        {
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton1 = new CardAction()
            {
                Value = "https://<OAuthSignInURL>",
                Type = "signin",
                Title = "Connect"
            };
            CardAction plButton2 = new CardAction()
            {
                Value = "https://<OAuthSignInURL>",
                Type = "signin",
                Title = "Connect"
            };
            cardButtons.Add(plButton1);
            cardButtons.Add(plButton2);
            SigninCard plCard = new SigninCard("title:", cardButtons);
            Attachment plAttachment = plCard.ToAttachment();
            return plAttachment;


        }

        public Attachment GetMenuFromQuestion(String[] titles )
        {
            var heroCard = new HeroCard
            {
                Title = "הבוט שלנו",
                Subtitle = "Your bots — wherever your users are talking",
                Text = "Build and connect intelligent bots to interact with your users naturally wherever they are, from text/sms to Skype, Slack, Office 365 mail and other popular services.",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Get Started", value: "start"),
                new CardAction(ActionTypes.PostBack, "Yes We can", value: "yes")}
            };

            return heroCard.ToAttachment();
        }
    

    }
}