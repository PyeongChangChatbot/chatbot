namespace pyeongchangchatbot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;
    using System.Text;
    using System.Data.SqlClient;

    [Serializable]
    public class searchTimeDialog : IDialog<string>
    {
        private int attempts = 5;
        public int real_date = 0;
        public int real_time1 = 0;
        public int real_time2 = 0;


        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("먼저 원하는 날짜를 입력하세요. 8일부터 25일까지 입력할 수 있습니다. 다음 형식에 맞춰 입력하세요. e.g. 08 or 25");

            context.Wait(this.MessageReceivedAsync);
        }


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            string dates = message.Text.Trim();
            
            if ((message.Text != null) && (dates.Length == 2) && (dates[0] >= 48 && dates[0] <= 57) && (dates[1] >= 48 && dates[1] <= 57))
            {
                int exactDate = int.Parse(dates);

                if (exactDate >= 8 && exactDate <= 25)
                {
                    real_date = exactDate;
                    await context.PostAsync(real_date + "일을 선택하였습니다. 이번에는 원하는 시간 범위를 입력해주세요. 시 단위로만 선택 가능하며 선택 가능한 시간은 00시부터 24시까지입니다. 형식에 맞게 입력해주세요. e.g. 01-23");

                    context.Wait(this.MessageReceivedAsync2);
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

        private async Task MessageReceivedAsync2(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            string times = message.Text.Trim();
            
            if ((message.Text != null) && (times.Length == 5) && (times[0] >= 48 && times[0] <= 50) && (times[1] >= 48 && times[1] <= 57) && (times[3] >= 48 && times[3] <= 50) && (times[4] >= 48 && times[4] <= 57) && times[2] == '-')
            {

                int exactDate1 = 10*(times[0]-48) + (times[1]-48);
                int exactDate2 = 10*(times[3]-48) + (times[4]-48);

                if ((exactDate1 >= 0 && exactDate1 <= 24) && (exactDate2 >= 0 && exactDate2 <= 24))
                {
                    if(exactDate1 > exactDate2)
                    {
                        int i = exactDate1;
                        exactDate1 = exactDate2;
                        exactDate2 = i;
                    }

                    real_time1 = exactDate1;
                    real_time2 = exactDate2;

                    await context.PostAsync(real_time1 + "시부터 " + real_time2 + "시까지를 선택하였습니다. 잠시만 기다려주세요.");

                    // 데이터베이스 입력
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
                            sb.Append("WHERE [Day] = " + real_date + "and [StartTime_H] >= " + real_time1 + "and [EndTime_H] < " + real_time2);
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

                    context.Done(times);

                }
                else
                {
                    --attempts;
                    if (attempts > 0)
                    {
                        await context.PostAsync("선택 가능한 시간은 00시부터 24시까지입니다. 원하는 시간 범위를 다시 입력해주세요. 시 단위로만 선택 가능하며 선택 가능한 시간은 00시부터 24시까지입니다. 형식에 맞게 입력해주세요. e.g. 01-23");

                        context.Wait(this.MessageReceivedAsync2);
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
                    await context.PostAsync("이해하지 못하였습니다. 원하는 시간 범위를 다시 입력해주세요. 시 단위로만 선택 가능하며 선택 가능한 시간은 00시부터 24시까지입니다. 형식에 맞게 입력해주세요. e.g. 01-23");

                    context.Wait(this.MessageReceivedAsync2);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("너무 많이 잘못된 메시지를 입력하였습니다."));
                }
            }



        }




    }
}