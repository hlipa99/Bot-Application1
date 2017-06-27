using Microsoft.Bot.Connector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace BotTests
{
    [TestClass]
     public class adHokMessage
    {
        public async void sendMessage(string toId, string toName, string fromId, string fromName)
        {
            try
            {
                // Use the data stored previously to create the required objects.
                var cred = new MicrosoftAppCredentials("604abc6d-5c77-45b7-b4d9-b1677e8d74c4", "TcR1SbxXb6TvDkfzecPZmp9");
                var userAccount = new ChannelAccount(toId, toName);
                var botAccount = new ChannelAccount(fromId, fromName);
                var connector = new ConnectorClient(new Uri("http://botapplication120170122015429.azurewebsites.net"), cred);
                IMessageActivity message = Activity.CreateMessageActivity();
                var conversationIdTask = connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount);
                var conversationId = conversationIdTask.Id;
                // Set the address-related properties in the message and send the message.
                message.From = botAccount;
                message.Recipient = userAccount;
                message.Conversation = new ConversationAccount(id: conversationId.ToString());
                message.Text = "Hello, this is a notification";
                message.Locale = "en-Us";
                connector.Conversations.SendToConversation((Activity)message);
            }
            catch (Exception ex)
            {


            }
        }

        [TestMethod]
        public void sendMessageToAllUsers()
        {
            sendMessage("1594749223884940", "User", "n4k9eek6dbg2bs9fi", "Bot");
        }

    }
}
