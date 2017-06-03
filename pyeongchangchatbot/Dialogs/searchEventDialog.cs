namespace pyeongchangchatbot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;
    using System.Text;
    using System.Data.SqlClient;

    [Serializable]
    public class searchEventDialog : IDialog<string>
    {
        private const string event1 = "Opening Ceremony";
        private const string event2 = "Alpine Skiing";
        private const string event3 = "Bobsleigh";
        private const string event4 = "Biathlon";
        private const string event5 = "Cross-Country Skiing";
        private const string event6 = "Curling";
        private const string event7 = "Freestyle Skiing";
        private const string event8 = "Figure Skating";
        private const string event9 = "Ice Hockey";
        private const string event10 = "Luge";
        private const string event11 = "Nordic Combined";
        private const string event12 = "Snowboard";
        private const string event13 = "Ski Jumping";
        private const string event14 = "Skeleton";
        private const string event15 = "Speed Skating";
        private const string event16 = "Short Track Speed Skating";
        private const string event17 = "Closing Ceremony";

       

        public async Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(
                context,
                this.AfterChoiceSelected,
                new[] { event1, event2, event3, event4, event5, event6, event7, event8, event9, event10,
                    event11, event12, event13, event14, event15, event16, event17},
                "원하시는 경기를 선택해주세요",
                "원하시는 경기를 선택해주세요",
                attempts: 3);
        }
        
        private async Task AfterChoiceSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selection = await result;

                await context.PostAsync(selection + " 경기를 선택하였습니다. 잠시만 기다려주세요.");

                try
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = "pyeongchang.database.windows.net";
                    builder.UserID = "hjs0579";
                    builder.Password = "(wnstjs)3578";
                    builder.InitialCatalog = "PyeongChangDatabase";

                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        Console.WriteLine("\nQuery data example:");
                        Console.WriteLine("=========================================\n");

                        connection.Open();
                        StringBuilder sb = new StringBuilder();
                        sb.Append("SELECT * ");
                        sb.Append("FROM [dbo].[SportsInformation] ");
                        sb.Append("WHERE [Sports] = '\"" + selection + "\"'");
                        String sql = sb.ToString();

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    await context.PostAsync(
                                        "Sports: " + reader.GetString(0) +
                                        "\nDescription: " + reader.GetString(1) +
                                        "\nDate: " + reader.GetString(3) + "." + reader.GetString(4) + "." + reader.GetString(5) +
                                        "\nDay of Week: " + reader.GetString(6) +
                                        "\nStart Time: " + reader.GetString(7) + ":" + reader.GetString(8) +
                                        "\nEnd Time: " + reader.GetString(9) + ":" + reader.GetString(10) +
                                        "\nVenue: " + reader.GetString(11));
                                }
                            }
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.Source);
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(e.ToString());
                }

                switch (selection)
                {
                    case event1:
                        // 데이터베이스 집어넣기

                        break;
                    case event2:
                        // 데이터베이스 집어넣기

                        break;
                    case event3:
                        // 데이터베이스 집어넣기

                        break;
                    case event4:
                        // 데이터베이스 집어넣기

                        break;
                    case event5:
                        // 데이터베이스 집어넣기

                        break;
                    case event6:
                        // 데이터베이스 집어넣기

                        break;
                    case event7:
                        // 데이터베이스 집어넣기

                        break;
                    case event8:
                        // 데이터베이스 집어넣기

                        break;
                    case event9:
                        // 데이터베이스 집어넣기

                        break;
                    case event10:
                        // 데이터베이스 집어넣기

                        break;
                    case event11:
                        // 데이터베이스 집어넣기

                        break;
                    case event12:
                        // 데이터베이스 집어넣기

                        break;
                    case event13:
                        // 데이터베이스 집어넣기

                        break;
                    case event14:
                        // 데이터베이스 집어넣기

                        break;
                    case event15:
                        // 데이터베이스 집어넣기

                        break;
                    case event16:
                        // 데이터베이스 집어넣기

                        break;
                    case event17:
                        // 데이터베이스 집어넣기

                        break;
                }

                context.Done(selection);
                
            }
            catch (TooManyAttemptsException)
            {
                await this.StartAsync(context);
            }
        }
    }
}