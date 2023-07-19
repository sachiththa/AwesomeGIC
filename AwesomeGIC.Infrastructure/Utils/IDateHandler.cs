using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC.Infrastructure.Utils
{
    public interface IDateHandler
    {
        bool isDateFormatValid(string dateInputa);
        DateTime ConvertDateTime(string dateInput, string format);
    }
}
