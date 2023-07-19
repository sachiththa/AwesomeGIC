using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC.Infrastructure.Utils
{
    public class DateHandler : IDateHandler
    {
        public bool isDateFormatValid(string dateInput) {
            if (DateTime.TryParseExact(dateInput, "yyyyMMdd",
                              new CultureInfo("en-US"),
                              DateTimeStyles.None,
                              out DateTime formattedDate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DateTime ConvertDateTime(string dateInput, string format) {
            DateTime convertedDate = DateTime.ParseExact(dateInput, "yyyyMMdd", new CultureInfo("en-US"));
            return convertedDate;
        }
    }
}
