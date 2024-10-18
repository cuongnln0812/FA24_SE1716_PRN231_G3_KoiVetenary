﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiVetenary.Data.Models;
using KoiVetenary.Service;
using KoiVetenary.Business;
using KoiVetenary.Common;
using Newtonsoft.Json;
using KoiVetenary.Service.DTO.Animal;

namespace KoiVetenary.MVCWebApp.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly IAnimalService _animal;


        public AnimalsController(IAnimalService animal)
        {
            _animal = animal;
        }

        // GET: Animals
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Animals"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var animals = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (animals != null && animals.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Animal>>(animals.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }
            return View(new List<Data.Models.Service>());
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Animals/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var animal = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (animal != null && animal.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<Animal>(animal.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }
            return View(new Data.Models.Animal());
        }

        // GET: Animals/Create
        public async Task<IActionResult> Create()
        {
            ViewData["OwnerId"] = new SelectList(await GetOwners(), "OwnerId", "FirstName");
            ViewData["TypeId"] = new SelectList(await GetAnimalTypes(), "TypeId", "TypeName");
            return View(new Animal());
        }

        //// POST: Animals/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnimalId,OwnerId,Name,Species,TypeId,Origin,DateOfBirth,Age,Weight,Length,Color,DistinguishingMarks,ImageUrl,Gender")] AnimalRequest animalRequest)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.API_Endpoint + "Animals", animalRequest))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);
                            if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                                saveStatus = true;
                            else
                                saveStatus = false;
                        }
                    }
                }
            }
            if (saveStatus)
                return RedirectToAction(nameof(Index));
            else
            {
                var animal = new Animal
                {
                    OwnerId = animalRequest.OwnerId,
                    TypeId = animalRequest.TypeId
                };

                ViewData["OwnerId"] = new SelectList(await GetOwners(), "OwnerId", "FirstName", animal.OwnerId);
                ViewData["TypeId"] = new SelectList(await GetAnimalTypes(), "TypeId", "TypeName", animal.TypeId);
                return View(animal);
            }
        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var animal = new Animal();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Animals/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var animalResult = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (animalResult != null && animalResult.Data != null)
                        {
                            animal = JsonConvert.DeserializeObject<Animal>(animalResult.Data.ToString());
                        }
                    }
                }
            }
            ViewData["OwnerId"] = new SelectList(await GetOwners(), "OwnerId", "FirstName");
            ViewData["TypeId"] = new SelectList(await GetAnimalTypes(), "TypeId", "TypeName");
            return View(animal);
        }

        //// POST: Animals/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("AnimalId,OwnerId,Name,Species,TypeId,Origin,DateOfBirth,Age,Weight,Length,Color,DistinguishingMarks,ImageUrl,Gender")] AnimalRequest animalRequest)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsJsonAsync(Const.API_Endpoint + "Animals/", animalRequest))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);
                            if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                                saveStatus = true;
                            else
                                saveStatus = false;
                        }
                    }
                }
            }
            if (saveStatus)
                return RedirectToAction(nameof(Index));
            else
            {
                var animal = new Animal
                {
                    OwnerId = animalRequest.OwnerId,
                    TypeId = animalRequest.TypeId
                };

                ViewData["OwnerId"] = new SelectList(await GetOwners(), "OwnerId", "FirstName", animal.OwnerId);
                ViewData["TypeId"] = new SelectList(await GetAnimalTypes(), "TypeId", "TypeName", animal.TypeId);
                return View(animal);
            }
        }

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var animal = new Animal();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Animals/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var animalResult = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (animalResult != null && animalResult.Data != null)
                        {
                            animal = JsonConvert.DeserializeObject<Animal>(animalResult.Data.ToString());
                            return View(animal);
                        }
                    }
                }
            }
            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleteStatus = false;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(Const.API_Endpoint + "Animals/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return RedirectToAction(nameof(Delete), new { id = id });
        }


        private static async Task<List<Owner>> GetOwners()
        {
            var listOwner = new List<Owner>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Owners"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var settings = new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        };
                        var owners = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse, settings);

                        if (owners != null && owners.Data != null)
                        {
                            listOwner = JsonConvert.DeserializeObject<List<Owner>>(owners.Data.ToString());
                        }
                    }
                }
            }
            return listOwner;
        }

        private static async Task<List<AnimalType>> GetAnimalTypes()
        {
            var listAnimalType = new List<AnimalType>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "AnimalTypes"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var settings = new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        };
                        var animalTypes = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse, settings);
                    
                        if (animalTypes != null && animalTypes.Data != null)
                        {
                            listAnimalType = JsonConvert.DeserializeObject<List<AnimalType>>(animalTypes.Data.ToString());
                        }
                    }
                }
            }
            return listAnimalType;
        }
    }
}
