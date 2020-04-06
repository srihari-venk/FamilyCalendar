using log4net;
using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCalendar
{
    public static class CalendarHelper
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static string URL = "https://calendar.kollavarsham.org/api/";
        private static string URL = "https://www.prokerala.com/general/calendar/date.php?theme=unity";
        private static string PATH_CALENDAR_FULL_YEAR = URL + "&calendar=malayalam&la=&sb=1&loc=1275339";
        private static string RES_IMPORT_EXECUTION = URL + "rest/raven/1.0/import/execution/junit";
        private static string RES_IMPORT_EXECUTION_JSON = URL + "rest/raven/1.0/import/execution";

        private static HttpClientHandler handler = new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            Credentials = CredentialCache.DefaultNetworkCredentials
        };
        private static HttpClient client = new HttpClient(handler);

        public async static Task<string> GetDatesForYear(int year, int month, int date)
        {

            try
            {
                string responseText = string.Empty;
                StringBuilder requestParam = null;
                Hashtable issueMap = new Hashtable();
                string issueSummary = string.Empty;

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                               

                requestParam = new StringBuilder(PATH_CALENDAR_FULL_YEAR + String.Format("&year={0}&month={1}&day={2}",year,month,date));
                //requestParam.AppendFormat(@"{0}?lang=en", year);

                HttpResponseMessage response = client.GetAsync(Convert.ToString(requestParam)).Result;
                if (response.IsSuccessStatusCode)
                {
                    responseText = await response.Content.ReadAsStringAsync();
                    //issueSummary = Convert.ToString(((dynamic)JsonConvert.DeserializeObject(responseText)));
                    Logger.Info(String.Format("Get dates for year call ended with Status Code {0}", response.StatusCode));

                }
                else
                {
                    Logger.Error(String.Format("Get dates for year call ended with Status Code {0} and message {1}", response.StatusCode, response.ReasonPhrase));
                }

                return responseText;

            }
            catch (Exception exc)
            {
                Logger.Error("Call failure", exc);
                throw;
            }
        }
    }
}
