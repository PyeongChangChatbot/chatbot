namespace pyeongchangchatbot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class socialMediaDialog : IDialog<string>
    {
        string a = "test";

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("평창 올림픽 공식소셜미디어는 다음과 같습니다. 차례대로 Facebook, Twitter, Instagram, Youtube, Flickr 입니다.");
            await context.PostAsync("https://www.facebook.com/PyeongChang2018");
            await context.PostAsync("https://twitter.com/PyeongChang2018");
            await context.PostAsync("https://www.instagram.com/pyeongchang2018/");
            await context.PostAsync("https://www.youtube.com/user/PyeongChang2018");
            await context.PostAsync("https://www.flickr.com/photos/pyeongchang2018_kr");

            context.Done(a);
        }
    }
}