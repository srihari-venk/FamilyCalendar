using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FamilyCalendar
{
    class Program
    {
        static void Main(string[] args)
        {
            List<MasterData> lstmasterData = LoadInitialData();
            MapDates(lstmasterData);
            Console.ReadLine();
        }

        private static void MapDates(List<MasterData> lstmasterData)
        {
            DateTime currentDate = new DateTime(DateTime.Now.Year, 1, 1);
            int numberOfDays = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
            int currentDayCount = 1;
            //while(currentDate.Year == DateTime.Now.Year)
            while(currentDayCount <= 5)
            {
                var responseMessage = CalendarHelper.GetDatesForYear(DateTime.Now.Year,currentDate.Month,currentDate.Day).Result;
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(responseMessage);
                string Date = document.DocumentNode.SelectSingleNode("//span[@class='cday-date']").InnerHtml;
                string Month = document.DocumentNode.SelectSingleNode("//span[@class='cday-month']").InnerHtml;
                string Star = ((HtmlNode)document.DocumentNode.SelectSingleNode("//table[contains(@class='table calendar-details table-sm t-small')]/tbody/tr[2]/td/div")).ToString().Trim();
                //string Star = Convert.ToString(((HtmlNode)document.DocumentNode.Descendants("table")
                //    .Where(d => d.GetAttributeValue("class", "")
                //    .Contains("calendar-details")))).Trim();
                Console.WriteLine(String.Format("Date {0}-{1}, Star {2}", Date, Month, Star));
                ++currentDayCount;
                currentDate.AddDays(1);
            }



        }

        private static List<MasterData> LoadInitialData() => new List<MasterData> {
                new MasterData("Sourav's Birthday", true, "Dhanu", "Makam"),
                new MasterData("Chuppu Birthday", true, "Dhanu", "Pooram"),
                new MasterData("V' chalam's Birthday", true, "Makaram", "Revathi")
                };



    }
}
