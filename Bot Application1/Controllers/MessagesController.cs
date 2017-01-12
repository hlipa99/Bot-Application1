﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using Bot_Application1.IDialog;
using Bot_Application1.dataBase;
using System.Collections.Generic;
using Model.dataBase;

namespace Bot_Application1.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// 
        public async Task<HttpResponseMessage> Get()
        {
            var response = Request.CreateResponse(HttpStatusCode.Forbidden,"ok");
            return response;
        }



        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {

            if (activity.Type == ActivityTypes.Message)
            {

                try
                {
                    var DB = new Entities();
                    Users NewUsers = new Users();

                    NewUsers.Channel = activity.ChannelId;
                    NewUsers.UserID = activity.From.Id;
                    NewUsers.UserName = activity.From.Name;
                    NewUsers.created = DateTime.UtcNow;
                    NewUsers.Message = activity.Text.Truncate(500);

                    DB.Users.Add(NewUsers);
                    DB.SaveChanges();

                    //    DataBaseControler DC = new DataBaseControler();
                    //    DC.isUserExist(activity.From.Id);
                    //   DC.getUser(activity.From.Id);

                    //  string s = "dfdfed";

                }
                catch (Exception e)
                {

                }





                //create typing replay
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                var typingReplay = activity.CreateReply();
                typingReplay.Type = ActivityTypes.Typing;
                await connector.Conversations.ReplyToActivityAsync(typingReplay);

                await Conversation.SendAsync(activity, () => new MainDialog());
                //ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                //// calculate something for us to return
                //int length = (activity.Text ?? string.Empty).Length;


                //// return our reply to the user
                //Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
                //await connector.Conversations.ReplyToActivityAsync(reply);
                //

            }
            else if (activity.Type == ActivityTypes.ContactRelationUpdate)
            {
                if(activity.Action == "add")
                {
                                   await Conversation.SendAsync(activity, () => new MainDialog());
                }
            }
            else
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                Activity reply = HandleSystemMessage(activity);
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }



        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                return message.CreateReply("update");
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
                return message.CreateReply("You send PING");
                
                
            }

            return null;
        }
    }
}