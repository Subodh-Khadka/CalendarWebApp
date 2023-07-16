using CalendarWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using System.Globalization;


namespace CalendarWeb.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        public IActionResult AddUser()
        {
            return View();
        }

        public async Task<IActionResult> Index(int month = 1)
        {
            List<CalendarEventCategory> events_ = new List<CalendarEventCategory>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    events_ = JsonConvert.DeserializeObject<List<CalendarEventCategory>>(apiResponse);
                }
            }


            List<CalendarEventDate> events = new List<CalendarEventDate>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventDate/GetEventDayList/2080/" + month))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    events = JsonConvert.DeserializeObject<List<CalendarEventDate>>(apiResponse);
                }
            }

            List<DayList>? CalendarList = new List<DayList>();
            using (var httpClient = new HttpClient())
            {
                /* string apiUrl = $"http://apitest.lunarit.com.np/api/apidaylist/getdaylist/2080/{month}";*/
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apidaylist/getdaylist/2080/" + month))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    CalendarList = JsonConvert.DeserializeObject<List<DayList>>(apiResponse);
                }
            }

            //for endlishDate
            var selectedMonth = month;
            string? selectedEnglishMonth = null;
            string? selectedEnglishMonth2 = null;
            string? selectedEnglishMonth3 = null;

            if (selectedMonth != null)
            {
                DateTimeFormatInfo dateFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
                int adjustedMonth = (int)selectedMonth + 3;
                if (adjustedMonth <= 11)
                {
                    selectedEnglishMonth = dateFormatInfo.GetMonthName(adjustedMonth);
                    selectedEnglishMonth2 = dateFormatInfo.GetMonthName(adjustedMonth + 1);
                }
                else if (adjustedMonth == 12)
                {
                    selectedEnglishMonth = dateFormatInfo.GetMonthName(adjustedMonth);
                    selectedEnglishMonth2 = dateFormatInfo.GetMonthName(adjustedMonth - 11);
                }

                else
                {
                    selectedEnglishMonth = dateFormatInfo.GetMonthName(adjustedMonth - 12);
                    selectedEnglishMonth2 = dateFormatInfo.GetMonthName(adjustedMonth - 11);
                }
            }
            else
            {
                selectedEnglishMonth3 = "unkown Month";

            }

            ViewBag.Events = events;
            ViewBag.Events_ = events_;
            ViewBag.selectedEnglishMonth = selectedEnglishMonth + "-" + selectedEnglishMonth2;
            return View(CalendarList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}



