namespace pyeongchangchatbot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;
    using TLSharp;
    using TLSharp.Core;

    [Serializable]
    public class eventAlarmDialog : IDialog<string>
    {
        Activity activity = new Activity();

        public async Task StartAsync(IDialogContext context)
        {
            string userName = activity.From.Name;

            await context.PostAsync("Your name is: " + userName);
        }





    }
}