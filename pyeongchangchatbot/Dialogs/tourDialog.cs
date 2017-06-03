namespace pyeongchangchatbot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class tourDialog : IDialog<string>
    {
        public string[] a = new string[3] { "상원사", "http://korean.visitkorea.or.kr/kor/bz15/where/where_main_search.jsp?cid=125771", "강원도 평창군 진부면 오대산로 1215-89 (진부면)" };
        public string[] b = new string[3] { "월정사", "http://korean.visitkorea.or.kr/kor/bz15/where/where_main_search.jsp?cid=125772", "강원도 평창군 진부면 오대산로 374-8" };
        public string[] c = new string[3] { "평창 백룡동굴", "http://korean.visitkorea.or.kr/kor/bz15/where/where_main_search.jsp?cid=1333464", "강원도 평창군 미탄면 문희길 63 (미탄면)" };
        public string[] d = new string[3] { "정강원", "http://korean.visitkorea.or.kr/kor/bz15/where/where_main_search.jsp?cid=1081744", "강원도 평창군 용평면 금당계곡로 2010-13 (용평면)" };
        public string[] e = new string[3] { "이효석 생가터", "https://korean.visitkorea.or.kr/kor/bz15/where/where_main_search.jsp?cid=129415", "강원도 평창군 봉평면 이효석길 33-11 (봉평면)" };
        public string[] f = new string[3] { "평창무이예술관", "http://korean.visitkorea.or.kr/kor/bz15/where/where_main_search.jsp?cid=130319", "강원도 평창군 봉평면 사리평길 233" };
        public string[] g = new string[3] { "한국자생식물원", "https://korean.visitkorea.or.kr/kor/bz15/where/where_tour.jsp?cid=127901&gotoPage=774&listType=cdesc", "강원도 평창군 대관령면 비안길 159-4" };
        public string[] h = new string[3] { "상원사", "http://korean.visitkorea.or.kr/kor/bz15/where/where_main_search.jsp?cid=125771", "강원도 평창군 진부면 오대산로 1215-89 (진부면)" };
        public string z = "https://korean.visitkorea.or.kr/kor/bz15/travel_plus/tp_find.jsp";

        public int limit = 3;

        public async Task StartAsync(IDialogContext context)
        {
            Random r = new Random();

            int num = r.Next(1, 8);

            if (num == 1)
            {
                await context.PostAsync(a[0]);
                await context.PostAsync(a[1]);
                await context.PostAsync(a[2]);
            }
            else if (num == 2)
            {
                await context.PostAsync(b[0]);
                await context.PostAsync(b[1]);
                await context.PostAsync(b[2]);
            }
            else if (num == 3)
            {
                await context.PostAsync(c[0]);
                await context.PostAsync(c[1]);
                await context.PostAsync(c[2]);
            }
            else if (num == 4)
            {
                await context.PostAsync(d[0]);
                await context.PostAsync(d[1]);
                await context.PostAsync(d[2]);
            }
            else if (num == 5)
            {
                await context.PostAsync(e[0]);
                await context.PostAsync(e[1]);
                await context.PostAsync(e[2]);
            }
            else if (num == 6)
            {
                await context.PostAsync(f[0]);
                await context.PostAsync(f[1]);
                await context.PostAsync(f[2]);
            }
            else if (num == 7)
            {
                await context.PostAsync(g[0]);
                await context.PostAsync(g[1]);
                await context.PostAsync(g[2]);
            }
            else
            {
                await context.PostAsync(h[0]);
                await context.PostAsync(h[1]);
                await context.PostAsync(h[2]);
            }

            limit--;

            if (limit > 0)
            {
                PromptDialog.Choice(
               context,
               this.AfterChoiceSelected,
               new[] { "다시추천", "처음으로" },
               "다른 추천을 원하시다면 '다시추천' 버튼을, 아니시라면 '처음으로' 버튼을 눌러주세요.",
               "다른 추천을 원하시다면 '다시추천' 버튼을, 아니시라면 '처음으로' 버튼을 눌러주세요.",
               attempts: 3);
            }
            else
            {
                await context.PostAsync("더 많은 추천은 다음 사이트에서 찾아볼 수 있습니다.");
                await context.PostAsync(z);
                context.Done(z);

            }
        }

        private async Task AfterChoiceSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selection = await result;

                switch (selection)
                {
                    case "다시추천" :
                        await this.StartAsync(context);
                        break;
                    case "처음으로" :
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