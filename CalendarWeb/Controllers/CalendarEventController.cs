using CalendarWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace CalendarWeb.Controllers
{
    public class CalendarEventController: Controller
    {
        public async Task <IActionResult> Create (string param)
        {

            List<CalendarEventCategory>? categories = new List<CalendarEventCategory>();
           HttpClient client = new HttpClient();
            using (var response = await client.GetAsync("http://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
            {
                string apiresponse = await response.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<CalendarEventCategory>>(apiresponse);
            }

            var elist = new SelectList(categories, nameof(CalendarEventCategory.EventName), nameof(CalendarEventCategory.EventName));
            ViewBag.eventList = elist;
            CalendarEventDate calendarEventDate = new CalendarEventDate
            {
                NepaliDate = param
            };
            
            return PartialView(calendarEventDate);
        }

        //update
        [HttpGet]
        public async Task<IActionResult> Update(string param)
        {
            DateTime date = DateTime.ParseExact(param, "yyyy-M-d", null);
            int month = date.Month;

            List<CalendarEventCategory>? eventCategory = new List<CalendarEventCategory>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    eventCategory = JsonConvert.DeserializeObject<List<CalendarEventCategory>>(apiResponse);
                }
            }
            var elist = new SelectList(eventCategory, nameof(CalendarEventCategory.EventName), nameof(CalendarEventCategory.EventName));
            ViewBag.eventList = elist;


            List<CalendarEventDate> events = new List<CalendarEventDate>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventDate/GetEventDayList/2080/" + month))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    events = JsonConvert.DeserializeObject<List<CalendarEventDate>>(apiResponse);
                }
            }
            var list = events?.Where(x => x.NepaliDate == param).FirstOrDefault();
            return PartialView(list);
        }


        /*[HttpPost]
        public async Task<IActionResult> Update(CalendarEventDate calendarEventDate)
        {
            *//*return Json(calendarEventDate);*//*

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(calendarEventDate), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://apitest.lunarit.com.np/api/apiEventDate/AddCalendarEventDate", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    *//*return Json (apiResponse);*//*
                }
                return RedirectToAction("Index", "Home");
            }
        }*/
        public IActionResult Index ()
        {
           
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Add(CalendarEventDate calendarEventDate)
        {
            /*return Json(calendarEventDate);*/

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(calendarEventDate), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://apitest.lunarit.com.np/api/apiEventDate/AddCalendarEventDate", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    /*return Json (apiResponse);*/
                }
                return RedirectToAction("Index", "Home");
            }
        }

        

        [HttpGet]
        public async Task<IActionResult> ReadEvents(int? month = 1)
        {

            List<DayList>? monthList = new List<DayList>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apidaylist/getdaylist/2080/" + month))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    monthList = JsonConvert.DeserializeObject<List<DayList>>(apiResponse);
                }
                ViewBag.MonthList = monthList;
            }

            List<CalendarEventDate> events = new List<CalendarEventDate>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventDate/GetEventDayList/2080/" + month ))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    events = JsonConvert.DeserializeObject<List<CalendarEventDate>>(apiResponse);
                }
            }
            return View(events);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            /*return Json(id);*/
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"http://apitest.lunarit.com.np/api/apiEventDate/DeleteCalendarEventDate/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    /*return Json(apiResponse);*/
                }
            }
            return RedirectToAction("Index", "Home" );
        }
    }
}
