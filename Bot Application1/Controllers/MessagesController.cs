using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using Bot_Application1.IDialog;

using System.Collections.Generic;
using Model.dataBase;
using System.IO;
using Model;
using Autofac;
using Microsoft.Bot.Builder.Dialogs.Internals;

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


        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
   
            try
            {
                if (activity.Conversation.IsGroup == null || !activity.Conversation.IsGroup.GetValueOrDefault())
                {
                  
                    if (activity.Type == ActivityTypes.Message && activity.Text.Length > 0)
                    {
                        try
                        {

                            //          var text = activity.Text;
                            //         Logger.addLog("User: " + text);


                            var dialog = new MainDialog();

                            var builder = new ContainerBuilder();
                            builder
                                .Register(c => new CachingBotDataStore(c.ResolveKeyed<IBotDataStore<BotData>>(typeof(ConnectorStore)), CachingBotDataStoreConsistencyPolicy.LastWriteWins))
                                .As<IBotDataStore<BotData>>()
                                .AsSelf()
                                .InstancePerLifetimeScope();

                            builder.Update(Conversation.Container);
                            await Conversation.SendAsync(activity, () => new MainDialog());

                            
                            //create typing replay
                            //var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                            //var typingReplay = activity.CreateReply();
                            //typingReplay.Type = ActivityTypes.Typing;
                            //await connector.Conversations.ReplyToActivityAsync(typingReplay);

                        }catch (Exception ex)
                        {
                            Logger.addErrorLog("messageControler", ex.Message + Environment.NewLine + ex.StackTrace + ex.InnerException);
                        }
                        //

                    }
                    else if (activity.Type == ActivityTypes.ContactRelationUpdate && !activity.Conversation.IsGroup.GetValueOrDefault())
                    {
                        if (activity.Action == "add")
                        {
                            await Conversation.SendAsync(activity, () => new MainDialog());
                        }
                    }
            
                    else
                    {
                        //ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                        //Activity reply = HandleSystemMessage(activity);
                        //await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                }
                var response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch(Exception ex)
            {
                //  var response = Request.CreateResponse(ex.ToString());
                //   return response;
                var response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
      
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