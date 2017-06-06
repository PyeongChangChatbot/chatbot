namespace pyeongchangchatbot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;
    using System.Collections;

    [Serializable]
    public class newsDialog : IDialog<string>
    {

        SHDocVw.InternetExplorer IE = new SHDocVw.InternetExplorer();

        ArrayList titles = new ArrayList();
        ArrayList descriptions = new ArrayList();
        ArrayList dates = new ArrayList();
        ArrayList links = new ArrayList();

        Random rnd = new Random();
        int index;

        dynamic url = "https://www.pyeongchang2018.com/en/bbs/press/image/list";

        public async Task Crawling(IDialogContext context)
        {
            titles.Clear();
            descriptions.Clear();
            dates.Clear();
            links.Clear();

            IE.Navigate2(ref url);
            while (IE.Busy == true || IE.ReadyState != SHDocVw.tagREADYSTATE.READYSTATE_COMPLETE)
            {
                System.Threading.Thread.Sleep(100);
            }

            mshtml.HTMLDocument doc = IE.Document;
            mshtml.IHTMLElementCollection elemDiv = null, elemA = null;
            elemDiv = doc.getElementsByTagName("div") as mshtml.IHTMLElementCollection;
            elemA = doc.getElementsByTagName("a") as mshtml.IHTMLElementCollection;

            foreach (mshtml.IHTMLElement elem in elemDiv)
            {
                //photo-tit(title), photo-desc(description), photo-date(date)
                if (elem.className == "photo-tit")
                {
                    titles.Add(elem.innerText);
                }
                else if (elem.className == "photo-desc")
                {
                    descriptions.Add(elem.innerText);
                }
                else if (elem.className == "photo-date")
                {
                    dates.Add(elem.innerText);
                }
            }
            foreach (mshtml.IHTMLElement elem in elemA)
            {
                //get start and end positions
                int iStartPos = elem.outerHTML.IndexOf("onclick=\"") + ("onclick=\"").Length;
                int iEndPos = elem.outerHTML.IndexOf("\">", iStartPos);
                //get our substring

                if (iEndPos > iStartPos)
                {
                    String s = elem.outerHTML.Substring(iStartPos, iEndPos - iStartPos);
                    String[] onclickStrings = s.Split('\'');
                    if (onclickStrings[0] == "$.funView(")
                    {
                        String link = "https://www.pyeongchang2018.com/en/bbs/press/image/view?menuId=255&bbsId=28&searchOpt=&searchTxt=&pageNo=1&sortSeCd=3&cnId=" + onclickStrings[1] + "&rows=" + onclickStrings[3] + "&langSeCd=en";
                        links.Add(link);
                    }
                }
            }

            
        }



        public int limit = 3;
        public async Task StartAsync(IDialogContext context)
        {
            await this.Crawling(context);

            index = rnd.Next(0, 15);

            await context.PostAsync(" - title : " + titles[index]);
            await context.PostAsync(" - description : " + descriptions[index]);
            await context.PostAsync(" - date : " + dates[index]);
            await context.PostAsync(" - link : " + links[index]);


            if (limit > 0)
            {
                PromptDialog.Choice(
               context,
               this.AfterChoiceSelected,
               new[] { "다른뉴스", "처음으로" },
               "다른 뉴스를 원하시다면 '다른뉴스' 버튼을, 아니시라면 '처음으로' 버튼을 눌러주세요.",
               "다른 뉴스를 원하시다면 '다른뉴스' 버튼을, 아니시라면 '처음으로' 버튼을 눌러주세요.",
               attempts: 3);
            }
            else
            {
                await context.PostAsync("더 많은 추천은 다음 사이트에서 찾아볼 수 있습니다.");
                await context.PostAsync(url);
                context.Done(url);

            }

        }

        private async Task AfterChoiceSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selection = await result;

                switch (selection)
                {
                    case "다른뉴스":
                        await this.StartAsync(context);
                        break;
                    case "처음으로":
                        context.Done(selection);
                        break;
                }
            }
            catch (TooManyAttemptsException)
            {
                await this.StartAsync(context);
            }
        }

    }
    

}