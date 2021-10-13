using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitiveTypes.Generators
{
    class DateGen : IPrimitiveGen
    {
        public Type CurType => typeof(DateTime);

        public object Create()
        {
            var random = new Random();
            int hour = random.Next(0, 24);
            int minute = random.Next(0, 60);
            int second = random.Next(0, 60);
            int milisecond = random.Next(0, 1000);
            int year = random.Next(DateTime.MinValue.Year, DateTime.MaxValue.Year);
            int month = random.Next(1, 13);
            int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);

            return new DateTime(year, month, day, hour, minute, second, milisecond);
        }
    }
}
