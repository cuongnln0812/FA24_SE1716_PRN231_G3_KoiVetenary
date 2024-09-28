using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiVetenary.Data.Models;
using KoiVetenary.Service;
using KoiVetenary.Common;
using KoiVetenary.Business;
using Newtonsoft.Json;
using System.Text;

namespace KoiVetenary.MVCWebApp.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService _service;
        private readonly ICategoryService _category;

        public ServicesController(IServiceService service, ICategoryService category)
        {
            _service = service;
            _category = category;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Services"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var services = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (services != null && services.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Data.Models.Service>>(services.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }
            return View(new List<Data.Models.Service>());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Services/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var service = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (service != null && service.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<Data.Models.Service>(service.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }
            return NotFound();
        }

        //GET: Services/Create
        public async Task<IActionResult> Create()
        {
            var listCate = new List<Category>();
            //
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Category"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var categories = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (categories != null && categories.Data != null)
                        {
                            listCate = JsonConvert.DeserializeObject<List<Category>>(categories.Data.ToString());
                            ViewData["CategoryId"] = new SelectList(listCate, "CategoryId", "Name");
                            return View();
                        }
                    }
                }
            }
            return NotFound();           
        }

        //POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,Description,Duration,BasePrice,CategoryId,IsActive,RequiredEquipment,SpecialInstructions,ServiceImg,CreatedBy,ModifiedBy,CreatedDate,UpdatedDate")] Data.Models.Service service)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Prepare service to be created (e.g., setting CreatedBy, CreatedDate, etc.)
                    service.CreatedDate = DateTime.UtcNow;
                    service.CreatedBy = User.Identity?.Name;  // Assuming you have identity set up for current user

                    using var httpClient = new HttpClient();
                    var content = new StringContent(JsonConvert.SerializeObject(service), Encoding.UTF8, "application/json");

                    // Call the API to create the service
                    var response = await httpClient.PostAsync($"{Const.API_Endpoint}Services", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Redirect to Index or other relevant page after successful creation
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Handle the error from API response
                        ModelState.AddModelError(string.Empty, "Failed to create service.");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception and return a 500 error or relevant message
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            // If we get here, something went wrong - repopulate the categories and return the view
            var listCate = new List<Category>();

            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{Const.API_Endpoint}Categories");

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                    if (categories?.Data != null)
                    {
                        listCate = JsonConvert.DeserializeObject<List<Category>>(categories.Data.ToString());
                    }
                }

                ViewData["CategoryId"] = new SelectList(listCate, "CategoryId", "CategoryName");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while loading categories: {ex.Message}");
            }

            return View(service);
        }

        //// GET: Services/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _service.Services == null)
        //    {
        //        return NotFound();
        //    }

        //    var service = await _service.Services.FindAsync(id);
        //    if (service == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CategoryId"] = new SelectList(_service.Categories, "CategoryId", "CategoryId", service.CategoryId);
        //    return View(service);
        //}

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceName,Description,Duration,BasePrice,CategoryId,IsActive,RequiredEquipment,SpecialInstructions,ServiceImg,CreatedBy,ModifiedBy,CreatedDate,UpdatedDate")] Service service)
        //{
        //    if (id != service.ServiceId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _service.Update(service);
        //            await _service.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ServiceExists(service.ServiceId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_service.Categories, "CategoryId", "CategoryId", service.CategoryId);
        //    return View(service);
        //}

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("ID cannot be null.");
            }
            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Details/" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            var service = JsonConvert.DeserializeObject<IKoiVetenaryResult>(apiResponse);

                            if (service != null && service.Data != null)
                            {
                                var data = JsonConvert.DeserializeObject<Data.Models.Service>(service.Data.ToString());

                                if (data != null)
                                    return View(data);
                                else
                                    return NotFound("Service data could not be deserialized.");
                            }
                            else
                                return NotFound("Service or service data is null.");
                        }
                        else
                            return NotFound("Failed to retrieve data from the API.");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }


        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.DeleteAsync(Const.API_Endpoint + "Services/" + id);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index"); 
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to delete the service.");
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Internal server error: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }

        
    }
}
