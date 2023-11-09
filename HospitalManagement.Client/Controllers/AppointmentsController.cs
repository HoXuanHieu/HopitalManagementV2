using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HospitalManagement.Client.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using HospitalManagement.Client.DTOs;
using HospitalManagement.Client.DTOs.AppointmentDTOs;
using HospitalManagement.Client.DTOs.UserDTOs;
using HospitalManagement.Client.DTOs.DoctorDTOs;
using NuGet.Protocol.Plugins;
using System.Text;

namespace HospitalManagement.Client.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;
        private readonly string urlBase = "https://localhost:7191/api/Appointment";


        public AppointmentsController(ILogger<HomeController> logger, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(urlBase);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessor.HttpContext.Session.GetString("Token"));
        }

        //https://localhost:7191/api/Appointment?page=1&sortBy=Date
        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage responseMessage = await _client.GetAsync(urlBase + "?page=1&sortBy=Date");
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });
                

                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var dataResponse = JsonConvert.DeserializeObject<PaginationDTO<Appointment>>(response.Data.ToString(), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                    });
                    if (!dataResponse.Items.Any()) ViewBag.ErrorMessage = "No appointments found";
                    return View(dataResponse.Items);
                }
                if(responseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
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
     

        // GET: Appointments/Create
        public async Task<IActionResult> CreateAsync()
        {
            var users = await GetUsers();
            var doctors = await GetDoctors();
            if (!users.Any() || !doctors.Any())
            {
                return NotFound();
            }
            var doctorSeleteDTO = new List<DoctorSeleteDTO>();
            foreach (var item in doctors)
            {
                doctorSeleteDTO.Add(new DoctorSeleteDTO { Id = item.Id, Description = item.Description, FullName = item.User.FullName });
            }
            ViewData["UserId"] = new SelectList(users, "Id", "FullName");
            ViewData["DoctorId"] = new SelectList(doctorSeleteDTO, "Id", "FullName");

            return View();
        }
        private async Task<List<User>> GetUsers()
        {
            try
            {
                string UserApiRequest = "https://localhost:7191/api/Users?sortColumn=Id";
                HttpResponseMessage responseMessage = await _client.GetAsync(UserApiRequest);
                string responseContent = await responseMessage.Content.ReadAsStringAsync();
                var responseUser = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var users = JsonConvert.DeserializeObject<PaginationDTO<User>>(responseUser.Data.ToString(), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                    });                   
                    return users.Items.Where(x => x.RoleId == 3).ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<List<Doctor>> GetDoctors()
        {
            try
            {
                string DocterApiRequest = "https://localhost:7191/api/Doctors?page=0&pageSize=2147483647&sortColumn=Id";
                var responseMessage = await _client.GetAsync(DocterApiRequest);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var responseDoctor = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var doctors = JsonConvert.DeserializeObject<PaginationDTO<Doctor>>(responseDoctor.Data.ToString(), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                    });
                    return doctors.Items;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Symptoms,UserId,DoctorId")] AppointmentCreateDTO appointment)
        {
            if (appointment == null)
            {
                return View("Error");
            }
            var jsonContent = new StringContent(JsonConvert.SerializeObject(appointment), Encoding.UTF8, "application/json");
            var role = HttpContext.Session.GetInt32("role");
            if (role == 1)
            {
                var responseMessage = await _client.PostAsync("https://localhost:7191/api/Appointment/admin-create", jsonContent);
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
                    ViewBag.ErrorMessage = response.Message;
                    return View("Error");
                }
            }
            if (role == 2|| role == 3)
            {              
                var responseMessage = await _client.PostAsync(urlBase+ "/patient-create", jsonContent);
                var response = JsonConvert.DeserializeObject<APIResponse>(responseMessage.ToString(), new JsonSerializerSettings
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
                    ViewBag.ErrorMessage = response.Message;
                    return View("Error");
                }
            }  
            var users = await GetUsers();
            var doctors = await GetDoctors();
            if (!users.Any() || !doctors.Any())
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(users, "Id", "Id", appointment.DoctorId);
            ViewData["UserId"] = new SelectList(doctors, "Id", "Email", appointment.UserId);
            return View(appointment);
        }
       
    }
}
