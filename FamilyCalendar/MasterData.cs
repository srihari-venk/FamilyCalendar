using System;

namespace FamilyCalendar
{
    class MasterData
    {
        public string Occassion { get; set; }
        public bool IsMalOccassion { get; set; }
        public string MalMonth { get; set; }
        public string MalStar1 { get; set; }
        public string MalStar2 { get; set; }
        public string MalStar3 { get; set; }
        public DateTime Date { get; set; }


        public MasterData(string _occassion, bool _isMalOccassion, string _MalMonth, string _MalStar)
        {
            Occassion = _occassion;
            IsMalOccassion = _isMalOccassion;
            MalMonth = _MalMonth;
            MalStar1 = _MalStar;
        }

        public MasterData()
        {

        }
    }


}
