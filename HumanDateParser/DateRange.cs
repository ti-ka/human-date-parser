using System;
using System.Collections.Generic;

namespace HumanDateParser
{
    public class DateRange
    {
        private int _pointer = -1;
        public IList<DateTime> Dates { get; set; }

        public DateRange()
        {
            Dates = new List<DateTime>();
        }

        public void AddDate(DateTime date)
        {
            Dates.Add(date);
            _pointer++;
        }

        public DateTime CurrentDate
        {
            get => Dates[_pointer];
            set =>  Dates[_pointer] = value;
        }
  
    }
}