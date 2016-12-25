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

        public static Attachment GetMainMenuChoice(string titles,string subTitle,string text,string urlImage, Dictionary<string, string> data)
        {


            HeroCard heroCard = new HeroCard
            {
                Title = titles,
                Subtitle = subTitle,
                Text = text,
                Images = new List<CardImage> { new CardImage("C:\\Users\\use\\Documents\\Visual Studio 2015\\Projects\\Bot Application1\\Bot Application1\\images\\MainBot.png") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Get Started", value: "https://docs.botframework.com/en-us/") }

        };
            

                   

            return heroCard.ToAttachment();
        }


        private List<CardAction> GetListCardAction(Dictionary<string, string> dataMap)
        {
            List<CardAction> listcars = new List<CardAction>();
            foreach (var item in dataMap)
            {
                CardAction ca = new CardAction(ActionTypes.PostBack, item.Key, item.Value);
                listcars.Add(ca);

            }
            return listcars;

        }
        public static Attachment GetCardAction( )
        {
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton1 = new CardAction()
            {
                Value = "hsA",
                Type = "imBack",
                Title = "היסטוריה א"
            };
            CardAction plButton2 = new CardAction()
            {
                Value = "hsB",
                Type = "imBack",
                Title = "היסטוריה ב"
            };
            cardButtons.Add(plButton1);
            cardButtons.Add(plButton2);
            SigninCard plCard = new SigninCard("בבקשה בחר תחום:", cardButtons);
            Attachment plAttachment = plCard.ToAttachment();
            return plAttachment;


        }

        public static Attachment GetCardAction(string title,string[] options)
        {
            List<CardAction> cardButtons = new List<CardAction>();
            int idx = 0;
            foreach(var o in options)
            {
                CardAction plButton = new CardAction()
                {
                    Value = idx.ToString(),
                    Type = "imBack",
                    Title = o
                };
                cardButtons.Add(plButton);
            }
          
    
            SigninCard plCard = new SigninCard(title, cardButtons);
            Attachment plAttachment = plCard.ToAttachment();
            return plAttachment;
        }


        public static Attachment GetCardSubCategoryAction()
        {
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton1 = new CardAction()
            {
                Value = "zi",
                Type = "imBack",
                Title = "ציונות"
            };
            CardAction plButton2 = new CardAction()
            {
                Value = "bit2",
                Type = "imBack",
                Title = "בית שני"
            };
            cardButtons.Add(plButton1);
            cardButtons.Add(plButton2);
            SigninCard plCard = new SigninCard("בבקשה בחר תחום:", cardButtons);
            Attachment plAttachment = plCard.ToAttachment();
            return plAttachment;


        }
        public static Attachment getFacebookButtons()
        {
            List<CardAction> buttons = new List<CardAction>();
            CardAction button1 = new CardAction()
            {
                Value = "https://en.wikipedia.org/wiki/Pig_Latin",
                Type = "openUrl",
                Title = "WikiPedia Page"
            };
            buttons.Add(button1);


           

            HeroCard plCard = new HeroCard()
            {
                Title = "I'm a hero card",
                Subtitle = "Pig Latin Wikipedia Page",
              //  Images = ,
                Buttons = buttons
            };


            return plCard.ToAttachment();



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