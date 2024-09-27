using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoiVetenary.Data.Models;
using KoiVetenary.Service;
using KoiVetenary.Business;

namespace KoiVetenary.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _service;

        public ServicesController(IServiceService service)
        {
            _service = service;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<IKoiVetenaryResult> GetServices()
        {
          return await _service.GetServicesAsync();
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<IKoiVetenaryResult> GetService(int id)
        {
          return await _service.GetServiceByIdAsync(id);
        }

        // PUT: api/Services/5
        [HttpPut("{id}")]
        public async Task<IKoiVetenaryResult> PutService(int id, Data.Models.Service service)
        {
            return await _service.UpdateService(service);
        }

        // POST: api/Services
        [HttpPost]
        public async Task<IKoiVetenaryResult> PostService(Data.Models.Service service)
        {
          return await _service.CreateService(service);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IKoiVetenaryResult> DeleteService(int id)
        {
            return await _service.DeleteService(id);
        }    
    }
}
