using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using pyeongchangchatbot.Dialogs;
using System;

namespace pyeongchangchatbot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            
            var userAccount = new ChannelAccount(name: "Larry", id: "@UV357341");
            var botAccount = new ChannelAccount("1234567890", "pyeongchangchatbot");
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            var conversationId = await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount);

            Activity reply = activity.CreateReply($"You sent {activity.Text} which was {activity.Text.Length} characters");
            await connector.Conversations.ReplyToActivityAsync(reply);


            IMessageActivity message = Activity.CreateMessageActivity();
            message.From = botAccount;
            message.Recipient = userAccount;
            message.Conversation = new ConversationAccount(id: conversationId.Id);
            message.Text = "Hello, Larry!";
            message.Locale = "en-Us";
            await connector.Conversations.SendToConversationAsync((Activity)message);


            if (activity.Type == ActivityTypes.Message)
            {
                /* Creates a dialog stack for the new conversation, adds RootDialog to the stack, and forwards all 
                 *  messages to the dialog stack. */
                await Conversation.SendAsync(activity, () => new RootDialog());
            }
            else
            {
                this.HandleSystemMessage(activity);
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
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}