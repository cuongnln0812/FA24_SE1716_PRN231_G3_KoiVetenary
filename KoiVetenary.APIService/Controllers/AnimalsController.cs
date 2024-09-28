
using KoiVetenary.Service;
using KoiVetenary.Business;
using KoiVetenary.Data.Models;
using Microsoft.AspNetCore.Mvc;
using KoiVetenary.Service.DTO.Animal;

namespace KoiVetenary.APIService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public AnimalsController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        // GET: Animals
        [HttpGet]
        public async Task<IKoiVetenaryResult> GetAnimalsAsync()
        {
            return await _animalService.GetAnimalsAsync();
            
            //var fA24_SE1716_PRN231_G3_KoiVetenaryContext = _service.Animals.Include(a => a.Owner).Include(a => a.Type);
            //return View(await fA24_SE1716_PRN231_G3_KoiVetenaryContext.ToListAsync());
        }

        // GET: Animals/Details/5
        [HttpGet("{id}")]
        public async Task<IKoiVetenaryResult> GetAnimalByIdAsync([FromRoute] int id)
        {
             return await _animalService.GetAnimalByIdAsync(id);
        }

        // POST: Animals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IKoiVetenaryResult> Create([FromBody] Animal animal)
        {
            return await _animalService.CreateAnimal(animal);
        }

        // PUT: Animals/Edit/5
        [HttpPut]
        public async Task<IKoiVetenaryResult> UpdateAnimalAsync([FromBody] Animal animal)
        {
            return await _animalService.UpdateAnimal(animal);
        }

        // DELETE: Animals/Delete/5
        [HttpDelete("{id}")]
        public async Task<IKoiVetenaryResult> DeleteAnimal([FromRoute] int id)
        {
            return await _animalService.DeleteAnimal(id);
        }


        //private bool AnimalExists(int id)
        //{
        //  return (_service.Animals?.Any(e => e.AnimalId == id)).GetValueOrDefault();
        //}
    }
}
