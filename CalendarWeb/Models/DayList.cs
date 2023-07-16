using CalendarWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalendarWeb.Models
{
    public  class DayList
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public DateTime EnglishDate { get; set; }

        public String NeplaiDateFormatted { get; set; } = null!;
    }
}



