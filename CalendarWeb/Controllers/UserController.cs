using CalendarWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Reflection;

namespace CalendarWeb.Controllers
{
    public class UserController : Controller
    {
        public IActionResult AddUser()
        {
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> LoginUser(string UserName, string UserPassword)
        {
            List<UserList> userList = new List<UserList>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiUserList/getusers"))
                {
                    string? apiResponse = await response.Content.ReadAsStringAsync();
                    userList = JsonConvert.DeserializeObject<List<UserList>>(apiResponse);
                    foreach (var item in userList)
                    {
                        if (item.UserName == UserName && item.UserPassword == UserPassword)
                        {
                            var claims = new List<Claim>
                               {
                                   new Claim(ClaimTypes.Name, item.UserId.ToString()),
                                   new Claim("UserName", item.UserName),
                                   new Claim(ClaimTypes.Role, item.UserRole)

                                };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            // Sign in the user and issue the authentication cookie
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    // Add an error message to TempData
                    TempData["ErrorMessage"] = "Invalid username or password.";
                }
            }
            return View("Login");
        }

      
        public async Task<IActionResult> GetUserList()
        {

            List<UserList>? userList = new List<UserList>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiUserList/getusers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    userList = JsonConvert.DeserializeObject<List<UserList>>(apiResponse);
                }
            }
            return View(userList);
        }

      

        [HttpPost]
        public async Task<IActionResult> Register(UserList userList)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(userList), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://apitest.lunarit.com.np/api/apiuserlist/adduser", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                }
            }
            return RedirectToAction("Login", "User");
        }

        public async Task<IActionResult> Logout()
        {
            // Sign out the user and delete the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect the user to the desired page
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            List<UserList> userList = new List<UserList>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiUserList/getusers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    userList = JsonConvert.DeserializeObject<List<UserList>>(apiResponse);
                }
            }
            var list = userList?.Where(x => x.UserId == id).FirstOrDefault();
            /*return Json(list);*/
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserList userlist)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(userlist), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("http://apitest.lunarit.com.np/api/apiuserlist/updateUser", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    /*return Content(apiResponse);*/

                }
                return RedirectToAction("Index","Home");
            }
        }



        /* [HttpPost]
         public async Task<IActionResult> RegisterUser(UserList userList)
         {
             *//*return Json(userList);*//*

             using (var httpClient = new HttpClient())
             {
                 StringContent content = new StringContent(JsonConvert.SerializeObject(userList), Encoding.UTF8, "application/json");

                 using (var response = await httpClient.PostAsync("http://apitest.lunarit.com.np/api/apiuserlist/adduser", content))
                 {
                     string apiResponse = await response.Content.ReadAsStringAsync();
                     *//*return Json(apiResponse);*//*
                 }
                 return RedirectToAction("AddUser", "User");
             }*/
    }

       

        
    }


/*using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using WebCalender.Models;

namespace WebCalender.Controllers
{
    public class ApiUserListController : Controller
    {
       

        public IActionResult Register()
        {
            return PartialView();
        }

       

        //[HttpPost]
       

    }
}*/
