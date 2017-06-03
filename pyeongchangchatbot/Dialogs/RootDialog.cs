namespace pyeongchangchatbot.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

#pragma warning disable 1998

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const string searchDate = "날짜에 따른 경기일정 조회";
        private const string searchTime = "시간에 따른 경기일정 조회";
        private const string searchEvent = "경기에 따른 경기일정 조회";
        private const string eventAlarm = "경기 알람 받아보기 ";
        private const string news = "최신소식 받아보기";
        private const string socialMedia = "평창올림픽 공식소셜미디어 찾기";
        private const string tour = "평창 관광명소 추천받기";


        
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }
        
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            await this.SendWelcomeMessageAsync(context);
        }
        
        
        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            PromptDialog.Choice(
               context,
               this.AfterChoiceSelected,
               new[] { searchDate, searchTime, searchEvent, eventAlarm, news, socialMedia, tour },
               "원하는 항목을 골라주세요. 대답까지는 최대 5초가 걸립니다.",
               "원하는 항목을 골라주세요. 대답까지는 최대 5초가 걸립니다..",
               attempts: 3);
        }
        

        private async Task AfterChoiceSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var selection = await result;

                switch (selection)
                {
                    case searchDate:
                        context.Call(new searchDateDialog(), this.playingAfter);
                        break;

                    case searchTime:
                        context.Call(new searchTimeDialog(), this.playingAfter);
                        break;

                    case searchEvent:
                        context.Call(new searchEventDialog(), this.playingAfter);
                        break;

                    case eventAlarm:
                        context.Call(new eventAlarmDialog(), this.playingAfter);
                        break;

                    case news:
                        context.Call(new newsDialog(), this.playingAfter);
                        break;

                    case socialMedia:
                        context.Call(new socialMediaDialog(), this.playingAfter);
                        break;

                    case tour:
                        context.Call(new tourDialog(), this.playingAfter);
                        break;

                }
            }
            catch (TooManyAttemptsException)
            {
                await this.StartAsync(context);
            }
        }


        private async Task playingAfter(IDialogContext context, IAwaitable<string> result)
        {  
            await this.SendWelcomeMessageAsync(context);
        }
    }
}