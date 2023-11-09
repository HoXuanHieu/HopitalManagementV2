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
using HospitalManagement.Client.DTOs.DoctorDTOs;
using HospitalManagement.Client.DTOs.HospitalDTOs;
using HospitalManagement.Client.DTOs.UserDTOs;
using System.Net.Http.Json;
using System.Text;

namespace HospitalManagement.Client.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;
        private readonly string urlBase = "https://localhost:7191/api/Doctors";
        private readonly string urlUserBase = "https://localhost:7191/api/Users";
        private readonly string urlHospitalBase = "https://localhost:7191/api/Hospitals";

        public DoctorsController(ILogger<HomeController> logger, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(urlBase);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessor.HttpContext.Session.GetString("Token"));
        }
        // GET: Doctors
        //https://localhost:7191/api/Doctors?page=0&pageSize=2147483647&sortColumn=Id
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage responseMessage = await _client.GetAsync(urlBase + "?page=0&pageSize=2147483647&sortColumn=Id");
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });


                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var dataResponse = JsonConvert.DeserializeObject<PaginationDTO<DoctorDTO>>(response.Data.ToString(), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                    });
                    if (!dataResponse.Items.Any()) ViewBag.ErrorMessage = "No appointments found";
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
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: Doctors/Details/5
        //https://localhost:7191/api/Doctors/1
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                HttpResponseMessage responseMessage = await _client.GetAsync(urlBase + "/" + id.ToString());
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });


                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var dataResponse = JsonConvert.DeserializeObject<DoctorDTO>(response.Data.ToString(), new JsonSerializerSettings
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
            catch
            (Exception ex)
            {
                return View("Error");
            }
        }

        private async Task<DoctorDTO> GetDoctor(int id)
        {
            HttpResponseMessage responseMessage = await _client.GetAsync(urlBase + "/" + id.ToString());
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<APIResponse>(responseContent.ToString(), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = JsonConvert.DeserializeObject<DoctorDTO>(response.Data.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });
                return dataResponse;
            }
            return null;
        }

        private async Task<List<HospitalDTO>> GetHospitals()
        {
            var hospitalResponse = await _client.GetAsync(urlHospitalBase + "?page=0&pageSize=2147483647&sortColumn=Id");
            var hospitalResponseContent = await hospitalResponse.Content.ReadAsStringAsync();
            if (hospitalResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = JsonConvert.DeserializeObject<PaginationDTO<HospitalDTO>>(hospitalResponseContent.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });
                return dataResponse.Items;
            }
            return null;
        }

        private async Task<List<UserDTO>> GetUsers()
        {
            var userResponse = await _client.GetAsync(urlUserBase + "?page=0&pageSize=2147483647&sortColumn=Id");
            var userResponseContent = await userResponse.Content.ReadAsStringAsync();
            var userResponseData = JsonConvert.DeserializeObject<APIResponse>(userResponseContent.ToString(), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });
            if (userResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = JsonConvert.DeserializeObject<PaginationDTO<UserDTO>>(userResponseData.Data.ToString(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                });
                return dataResponse.Items;
            }
            return null;
        }

        // GET: Doctors/Create
        public async Task<IActionResult> Create()
        {
            var hospitals = await GetHospitals();
            ViewData["HospitalId"] = new SelectList(hospitals, "Id", "Name");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] DoctorCreateDTO doctor)
        {
            if (ModelState.IsValid)
            {
                doctor.User.RoleName = "Doctor";
                var jsonContent = new StringContent(JsonConvert.SerializeObject(doctor), Encoding.UTF8, "application/json");
                var responseMessage = await _client.PostAsync(urlBase, jsonContent);
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
                    ViewBag.ErrorMessage = "Error";
                    return View("Error");
                }
            }

            var hospitals = await GetHospitals();
            ViewData["HospitalId"] = new SelectList(hospitals, "Id", "Name", doctor.HospitalId);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await GetDoctor(id);
            var hospitals = await GetHospitals();
            ViewData["HospitalId"] = new SelectList(hospitals, "Id", "Name", doctor.HospitalId);
            var users = await GetUsers();
            ViewData["UserId"] = new SelectList(users, "Id", "FullName", doctor.UserId);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Description,HospitalId")] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(doctor), Encoding.UTF8, "application/json");
                var responseMessage = await _client.PutAsync(urlBase, jsonContent);
                var response = JsonConvert.DeserializeObject<APIResponse>(responseMessage.Content.ToString(), new JsonSerializerSettings
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

            var hospitals = await GetHospitals();
            ViewData["HospitalId"] = new SelectList(hospitals, "Id", "Name", doctor.HospitalId);
            var users = await GetUsers();
            ViewData["UserId"] = new SelectList(users, "Id", "FullName", doctor.UserId);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await GetDoctor(id);
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var responseMessage = await _client.DeleteAsync(urlBase + "/" + id.ToString());
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
                ViewBag.ErrorMessage = "Error";
                return View("Error");
            }
        }
    }
}
