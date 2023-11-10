using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalManagement.Client;
using HospitalManagement.Client.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using HospitalManagement.Client.DTOs;
using HospitalManagement.Client.DTOs.UserDTOs;
using NuGet.Protocol.Plugins;
using System.Text;

namespace HospitalManagement.Client.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;
        private readonly string urlBase = "https://localhost:7191/api/Users";


        public UsersController(ILogger<HomeController> logger, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(urlBase);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessor.HttpContext.Session.GetString("Token"));
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage responseMessage = await _client.GetAsync(urlBase + "?sortColumn=Id");
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });


                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var dataResponse = JsonConvert.DeserializeObject<PaginationDTO<User>>(response.Data.ToString(), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                    });
                    if (!dataResponse.Items.Any()) ViewBag.ErrorMessage = "No appointments found";
                    var role = HttpContext.Session.GetInt32("Role");
                    if (role == 2)
                    {
                        return View(dataResponse.Items.Where(x => x.RoleId == 1));
                    }
                    return View(dataResponse.Items);
                }
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return View("Forbidden");
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Error");
            }
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                HttpResponseMessage responseMessage = await _client.GetAsync(urlBase + "/" + id);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var dataResponse = JsonConvert.DeserializeObject<User>(response.Data.ToString(), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                    });
                    
                    return View(dataResponse);
                }
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return View("Forbidden");
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Error");
            }
        }
        public IActionResult CreateAsync()
        {
            return View();
        }


        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //https://localhost:7191/api/Users
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,FullName,PhoneNumber,Birthday,Gender,Address,RoleName")] UserCreateDTO user)
        {
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await _client.PostAsync(urlBase, jsonContent);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });
                if (response.StatusCode == 404 && response.Message == "Roles doesn't exist") {
                    ViewBag.CreateUserError = response.Message; 
                    return View();
                }
                if(response.StatusCode == 400 && response.Message == "Invalid email address")
                {
                    ViewBag.CreateUserError = response.Message;
                    return View();
                }
                if(response.StatusCode == 400 && response.Message == "Invalid PhoneNumber")
                {
                    ViewBag.CreateUserError = response.Message;
                    return View();
                }
                if (response.StatusCode == 200) return RedirectToAction(nameof(Index));              
            }
            catch (Exception)
            {
                return View("Error");
            }
            return View();
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                HttpResponseMessage responseMessage = await _client.GetAsync(urlBase + "/" + id);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var dataResponse = JsonConvert.DeserializeObject<User>(response.Data.ToString(), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                    });

                    return View(dataResponse);
                }
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return View("Forbidden");
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Error");
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //https://localhost:7191/api/Users/1
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage responseMessage = await _client.DeleteAsync(urlBase + "/" + id);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToAction(nameof(Index));
                }
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return View("Forbidden");
                }
                else
                {
                    return View("Error");
                }
            }            
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Error");
            }
        }
    }
}
