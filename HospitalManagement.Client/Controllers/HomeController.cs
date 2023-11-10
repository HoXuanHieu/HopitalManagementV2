using HospitalManagement.Client.DTOs;
using HospitalManagement.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace HospitalManagement.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;

        private readonly string urlBase = "https://localhost:7191/api/Users/";
        //https://localhost:7191/api/Users/profile

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger; 
            _client = new HttpClient();
            _client.BaseAddress = new Uri(urlBase);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IActionResult> Index()
        {
            var result = await GetUsernfoAsync();
            return result ? View() : View("Error");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        private async Task<bool> GetUsernfoAsync()
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseMessage = await _client.GetAsync(urlBase + "profile");
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });               
                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }
                if(responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var user = JsonConvert.DeserializeObject<User>(response.Data.ToString(), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                    }); 
                    HttpContext.Session.SetInt32("Id", user.Id);
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetString("FulName", user.FullName);
                    HttpContext.Session.SetInt32("role", user.RoleId);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Profile()
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string UserApiRequest = "https://localhost:7191/api/Users/profile";
                HttpResponseMessage responseMessage = await _client.GetAsync(UserApiRequest);
                string responseContent = await responseMessage.Content.ReadAsStringAsync();
                var responseUser = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var user = JsonConvert.DeserializeObject<User>(responseUser.Data.ToString(), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                    });
                    return View(user);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception )
            {
                return View("Error");
            }
        }   
    }
}