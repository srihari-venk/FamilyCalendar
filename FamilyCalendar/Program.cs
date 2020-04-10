using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FamilyCalendar
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<MasterData> lstmasterData = LoadInitialData();
                List<MasterData> calendar = MapDates(lstmasterData);
                WriteToFile(calendar);
                Console.ReadLine();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message, exc.StackTrace);
            }
        }

        private static void WriteToFile(List<MasterData> calendar)
        {
            if (File.Exists(@".\MalCalendar.csv"))
                File.Delete(@".\MalCalendar.csv");


            using (var file = File.CreateText(@".\MalCalendar.csv"))
            {
                file.WriteLine("Date,Star,Star,Masam");
                foreach (var row in calendar)
                {
                    file.WriteLine("{0},{1},{2},{3},", row.Date.ToString("dd-MMM-yyyy"), row.MalStar1, row.MalStar2, row.MalMonth);
                }
            }
        }

        private static List<MasterData> MapDates(List<MasterData> lstmasterData)
        {
            DateTime currentDate = new DateTime(DateTime.Now.Year, 1, 1);
            int numberOfDays = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
            int currentDayCount = 1;

            List<MasterData> calendar = new List<MasterData>();
            //while(currentDate.Year == DateTime.Now.Year)
            MasterData day = null;
            while(currentDayCount <= 365)
            {
                day = new MasterData();
                day.Date = currentDate;
                var responseMessage = CalendarHelper.GetDatesForYear(DateTime.Now.Year, currentDate.Month, currentDate.Day).Result;
                (string month, string star1, string star2, string star3) malDate = ExtractDates(currentDate, responseMessage);
                day.MalMonth = malDate.month;
                day.MalStar1 = malDate.star1;
                day.MalStar2 = malDate.star2;
                day.MalStar3 = malDate.star3;
                ++currentDayCount;
                currentDate = currentDate.AddDays(1);
                calendar.Add(day);
                Console.WriteLine(String.Format(
                    "Date: {0}, Star1:{1}, Star2: {2}, Month:{3}",
                    day.Date.ToString("dd-MMM-yyyy"),
                    day.MalStar1,
                    day.MalStar2,
                    day.MalMonth));
            }

            return calendar;
        }

        private static (string month, string star1, string star2, string star3) ExtractDates(DateTime currentDate, string responseMessage)
        {
            string month = string.Empty;
            string star1 = string.Empty;
            string star2 = string.Empty;
            string star3 = string.Empty;
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseMessage);
            //string Date = document.DocumentNode.SelectSingleNode("//span[@class='cday-date']").InnerHtml;
            //string Month = document.DocumentNode.SelectSingleNode("//span[@class='cday-month']").InnerHtml;
            //string Star = ((HtmlNode)document.DocumentNode.SelectSingleNode("//table[contains(@class='calendar-details table-sm t-small')]/tbody/tr[2]/td/div")).ToString().Trim();
            var tablenodes = document.DocumentNode.SelectNodes("//table[contains(@class,'calendar-details')]");
            month = tablenodes[0].SelectSingleNode("//tr[1]/td[2]/span").InnerText.Trim().Split(" ")[0];
            var starNodes = tablenodes[1].SelectNodes("//tr[2]/td/div");
            string inputText = string.Empty;
            if (starNodes.Count > 1)
            {
                inputText = starNodes[0].InnerText.Trim();
                star1 = string.Join(" ", inputText.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
                //star1 = starNodes[0].InnerText.Trim();
                star2 = starNodes[1].InnerText.Trim();
            }
            else
                star1 = starNodes[0].InnerText.Trim();

           
            var malDate = (month, star1, star2, star3);
            return malDate;
        }

        private static List<MasterData> LoadInitialData() => new List<MasterData> {
                new MasterData("Sourav's Birthday", true, "Dhanu", "Makam"),
                new MasterData("Chuppu Birthday", true, "Dhanu", "Pooram"),
                new MasterData("V' chalam's Birthday", true, "Makaram", "Revathi")
                };

        public static class Calendar { 
            public static DateTime GregorianDate { get; set; }
            public static MasterData masterData { get; set; }

        }



    }
}
