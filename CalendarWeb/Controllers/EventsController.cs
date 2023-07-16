using CalendarWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;

namespace CalendarWeb.Controllers
{
    [Authorize (Roles = "Admin")]
    public class EventsController : Controller
    {
        
        public async Task<IActionResult> Index()
        {

            List<CalendarEventCategory>? categories = new List<CalendarEventCategory>();
            HttpClient client = new HttpClient();
            using (var response = await client.GetAsync("http://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
            {
                string apiresponse = await response.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<CalendarEventCategory>>(apiresponse);
            }
            return View (categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CalendarEventCategory calendarEventCategory)
        {

/*            return Json(calendarEventCategory);*/

            CalendarEventCategory? recievedEventCategory = new CalendarEventCategory();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(calendarEventCategory), Encoding.UTF8, "application/json");
          
                using (var response = await httpClient.PostAsync("http://apitest.lunarit.com.np/api/apiEventCategory/addeventcategory", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    /*return Json(apiResponse);*/
                }
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            List<CalendarEventCategory> eventList = new List<CalendarEventCategory>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    eventList = JsonConvert.DeserializeObject<List<CalendarEventCategory>>(apiResponse);
                }
            }
            var list = eventList?.Where(x => x.EventId == id).FirstOrDefault();
            /*return Json(list);*/
            return View(list);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(CalendarEventCategory calendarEventCategory)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(calendarEventCategory), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("http://apitest.lunarit.com.np/api/apiEventCategory/updateeventcategory", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    /*return Content(apiResponse);*/

                }
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            /*return Json(id);*/
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("http://apitest.lunarit.com.np/api/apiEventCategory/deleteeventcategory/" + id.ToString()))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    /*return Json(apiResponse);*/
                }
            }
            return RedirectToAction("Index");

        }
    }
}


