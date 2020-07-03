using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Helpers
{
    public  class CalPeople
    {
        public static string SubStringHouse(string str)
        {
            var strlen = str.Length;
            var xx = 4 - strlen;
            var textx = "";
            var res = str;
            for (var i = 0; i < xx; i++)
            {
                textx += "0";
            }
            res = textx + res;
            return res;
        }






        public static string getAge(DateTime? Dob)
        {  //คำนวณอายุ
            try
            {


                DateTime UpdatedTime = (DateTime)Dob;
                DateTime Now = DateTime.Now;
                int Years = new DateTime(DateTime.Now.Subtract(UpdatedTime).Ticks).Year - 1;
                DateTime PastYearDate = UpdatedTime.AddYears(Years);
                int Months = 0;
                for (int i = 1; i <= 12; i++)
                {
                    if (PastYearDate.AddMonths(i) == Now)
                    {
                        Months = i;
                        break;
                    }
                    else if (PastYearDate.AddMonths(i) >= Now)
                    {
                        Months = i - 1;
                        break;
                    }
                }
                int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
                int Hours = Now.Subtract(PastYearDate).Hours;
                int Minutes = Now.Subtract(PastYearDate).Minutes;
                int Seconds = Now.Subtract(PastYearDate).Seconds;
                return String.Format("อายุ: {0} ปี {1} เดือน {2} วัน",
                Years, Months, Days);
            }
            catch
            {
                return "ไม่ทราบอายุ";
            }
            }

    
    }
}
