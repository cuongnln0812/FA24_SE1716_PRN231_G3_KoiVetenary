using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiVetenary.MVCWebApp.Models;
using KoiVetenary.Common;
using Newtonsoft.Json;
using KoiVetenary.Business;
using KoiVetenary.Service;
using KoiVetenary.Data.Models;

namespace KoiVetenary.MVCWebApp.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IOwnerService _ownerService;

        public AppointmentsController(IAppointmentService appointmentService, IOwnerService ownerService)
        {
            _appointmentService = appointmentService;
            _ownerService = ownerService;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var apiEndpoint = Const.API_Endpoint + "Appointments";
                    var response = await httpClient.GetAsync(apiEndpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Appointment>>(result.Data.ToString());
                            return View(data);
                        }
                    }
                    else
                    {
                        // Log the status code and response message for debugging
                        Console.WriteLine($"API call failed. Status code: {response.StatusCode}, Message: {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }

            return View(new List<Appointment>());
        }
    


        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Appointments/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<Appointment>(result.Data.ToString());

                            return View(data);
                        }
                    }
                }
            }

            return View(new Appointment());
            //var appointment = new Appointment();

            //using (var httpClient = new HttpClient())
            //{
            //    using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Appointments/" + id))
            //    {
            //        if (response.IsSuccessStatusCode)
            //        {
            //            var content = await response.Content.ReadAsStringAsync();
            //            var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);

            //            if (result != null && result.Data != null)
            //            {
            //                appointment = JsonConvert.DeserializeObject<Appointment>(result.Data.ToString());
            //                return View(appointment);
            //            }
            //        }
            //    }
            //}

            //return View(new Appointment());
        }

        //GET: Appointments/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["OwnerId"] = new SelectList(await this.GetOwners(), "OwnerId", "OwnerId");
            return View();
        }


        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,OwnerId,AppointmentDate,AppointmentTime,Status,Notes,TotalEstimatedDuration,TotalCost,CreatedBy,ModifiedBy,CreatedDate,UpdatedDate")] Appointment appointment)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.API_Endpoint + "Appointments/", appointment))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);
                            if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                            {
                                saveStatus = true;
                            }
                            else
                            {
                                saveStatus = false;
                            }
                        }
                    }
                }
            }
            if (saveStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["OwnerId"] = new SelectList(await this.GetOwners(), "OwnerId", "OwnerId");
                return View(appointment);
            }
        }

        //GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var appointment = new Appointment();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Appointments/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);

                        if (result != null && result.Data != null)
                        {
                            appointment = JsonConvert.DeserializeObject<Appointment>(result.Data.ToString());
                        }
                    }
                }
            }

            ViewData["OwnerId"] = new SelectList(await this.GetOwners(), "OwnerId", "OwnerId", appointment.OwnerId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,OwnerId,AppointmentDate,AppointmentTime,Status,Notes,TotalEstimatedDuration,TotalCost,CreatedBy,ModifiedBy,CreatedDate,UpdatedDate")] Appointment appointment)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsJsonAsync(Const.API_Endpoint + "Appointments/", appointment))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);
                            if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                            {
                                saveStatus = true;
                            }
                            else
                            {
                                saveStatus = false;
                            }
                        }
                    }
                }
            }
            if (saveStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["OwnerId"] = new SelectList(await this.GetOwners(), "OwnerId", "OwnerId");
                return View(appointment);
            }
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var appointment = new Appointment();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Appointments/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);

                        if (result != null && result.Data != null)
                        {
                            appointment = JsonConvert.DeserializeObject<Appointment>(result.Data.ToString());
                            return View(appointment);
                        }
                    }
                }
            }

            return View(new Appointment());
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleteStatus = false;

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync(Const.API_Endpoint + "Appointments/" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);
                            if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                            {
                                deleteStatus = true;
                            }
                            else
                            {
                                deleteStatus = false;
                            }
                        }
                    }
                }
            }
            if (deleteStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Delete));
            }
        }

        //private bool AppointmentExists(int id)
        //{
        //  return (_context.Appointments?.Any(e => e.AppointmentId == id)).GetValueOrDefault();
        //}

        private async Task<List<Owner>> GetOwners()
        {
            var listOwners = new List<Owner>();
            //
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Owners"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var owners = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (owners != null && owners.Data != null)
                        {
                            listOwners = JsonConvert.DeserializeObject<List<Owner>>(owners.Data.ToString());
                        }
                    }
                }
            }
            return listOwners;
        }
    }
}
