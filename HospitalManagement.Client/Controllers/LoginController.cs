using HospitalManagement.Client.DTOs;
using HospitalManagement.Client.DTOs.UserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HospitalManagement.Client.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _client;

        private readonly string urlBase = "https://localhost:7191/api/Authenticate/";

        public LoginController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(urlBase);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public IActionResult Index()
        {
            return View("../Login/Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password")] UserLoginDTO login)
        {

            if (login.Email == null || login.Password == null)
            {
                ViewBag.LoginError = "Email and password can not be empty";
                return View("../Login/Login");
            }
            var jsonContent = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage responseMessage = await _client.PostAsync(urlBase + "login", jsonContent);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContext.Session.SetString("Token", response.Data.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewBag.LoginError = response.Message.ToString();
                    return View("../Login/Login");
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.PaymentRequired)
                {
                    ViewBag.LoginError = response.Message.ToString();
                    return View("../Login/Login");
                }
                return View("../Login/Login");
            }
            catch (Exception ex)
            {
                ViewBag.LoginError = "Have some Error with server";
                return View("../Login/Login");
            }
        }
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDTO user)
        {
            if (user.Email == null || user.Password == null || user.FullName == null)
            {
                ViewBag.LoginError = "All input must be filled";
                return View("../Login/Register");
            }
            var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(urlBase + "register", jsonContent);
            string stringData = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContext.Session.SetString("Token", stringData.ToString());
                return View("../Login/Login");
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ViewBag.Notifications = stringData.ToString();
                return View("../Login/Register");
            }
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                ViewBag.Notifications = stringData.ToString();
                return View("../Login/Register");
            }

            return View("../Login/Register");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }


    }
}
