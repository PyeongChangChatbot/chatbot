namespace pyeongchangchatbot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;
    using System.Text;
    using System.Data.SqlClient;

    [Serializable]
    public class searchDateDialog : IDialog<string>
    {
        private int attempts = 5;

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("원하는 날짜를 입력하세요. 8일부터 25일까지 입력할 수 있습니다.\n다음 형식에 맞춰 입력하세요. e.g. 08 or 25");
            
            context.Wait(this.MessageReceivedAsync);
        }


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            string dates = message.Text.Trim();
            
            if ((message.Text != null) && (dates.Length == 2) && (dates[0]>=48 && dates[0]<=57) && (dates[1] >= 48 && dates[1] <= 57))
            {
                int exactDate = int.Parse(dates);

                if(exactDate >= 8 && exactDate <= 25)
                {
                    await context.PostAsync(exactDate + "일을 선택하였습니다. 잠시만 기다려주세요.");

                    //  데이터베이스에서 정보 끌어다놓기.
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
                            sb.Append("WHERE [Day] = " + exactDate);
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

                    context.Done(dates);
                }
                else
                {
                    --attempts;
                    if (attempts > 0)
                    {
                        await context.PostAsync("선택 가능한 날짜는 8일부터 25일까지입니다. 형식에 맞게 원하시는 날짜를 다시 입력해주세요. e.g. 08 or 25");

                        context.Wait(this.MessageReceivedAsync);
                    }
                    else
                    {
                        context.Fail(new TooManyAttemptsException("너무 많이 잘못된 메시지를 입력하였습니다."));
                    }
                }

            }
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("이해하지 못하였습니다. 형식에 맞게 원하시는 날짜를 다시 입력해주세요. e.g. 08 or 25");

                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("너무 많이 잘못된 메시지를 입력하였습니다."));
                }
            }
        }
    }

}