﻿namespace pyeongchangchatbot.Dialogs
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

        public async Task StartAsync(IDialogContext context)
        {
           
            await context.PostAsync("What is your name?");

            //context.Wait(this.MessageReceivedAsync);
        }





    }
}