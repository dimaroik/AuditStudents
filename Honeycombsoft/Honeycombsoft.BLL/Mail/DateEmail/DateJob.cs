using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycombsoft.BLL.Mail.DateEmail
{
    public class DateJob
    {
        public static double CreateDateForSendEmail(DateTime dateStudy , string limit){

            int time = 0;

            switch (limit)
            {
                case "day":
                    time = 30;
                break;

                case "week":
                    time = 7;
                break;

                case "month":
                    time = 30;
                break;

                default:
                    time = 1000;
                break;
            }

            if((dateStudy - DateTime.Now).TotalDays >= time){
                return (dateStudy - DateTime.Now).TotalDays - time;
            }
            else
            {
                return 0;
            }
              
        }

    }
}
