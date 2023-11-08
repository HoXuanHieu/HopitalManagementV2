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

namespace HospitalManagement.Client.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;
        private readonly string urlBase = "https://localhost:7191/api/Doctors";


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
            catch(Exception ex)
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
                HttpResponseMessage responseMessage = await _client.GetAsync(urlBase + "/"+ id.ToString());
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

        // GET: Doctors/Create
        public IActionResult Create()
        {
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Address");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Description,HospitalId")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Address", doctor.HospitalId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", doctor.UserId);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Address", doctor.HospitalId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", doctor.UserId);
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
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Address", doctor.HospitalId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", doctor.UserId);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Hospital)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Doctors == null)
            {
                return Problem("Entity set 'DatabaseContext.Doctors'  is null.");
            }
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
          return (_context.Doctors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
