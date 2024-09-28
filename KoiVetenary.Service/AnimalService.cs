using KoiVetenary.Common;
using KoiVetenary.Data.Models;
using KoiVetenary.Service.DTO.Animal;
using MomAndChildren.Business;
using MomAndChildren.Data;
using KoiVetenary.Business;
using KoiVetenary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service
{
    public interface IAnimalService
    {
        Task<IKoiVetenaryResult> GetAnimalsAsync();
        Task<IKoiVetenaryResult> GetAnimalByIdAsync(int? animalId);
        Task<IKoiVetenaryResult> CreateAnimal(Animal animal);
        Task<IKoiVetenaryResult> UpdateAnimal(Animal animal);
        Task<IKoiVetenaryResult> DeleteAnimal(int? animalId);
        Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm);
    }
    public class AnimalService : IAnimalService
    {

        private readonly UnitOfWork _unitOfWork;

        public AnimalService() {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IKoiVetenaryResult> CreateAnimal(Animal animal)
        {
            try
            {
                //check trung ten
                var animals = await _unitOfWork.AnimalRepository.GetAllAsync();
                foreach (var item in animals)
                {
                    if (animal.Name.Equals(item.Name))
                    {
                        return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Name is duplicated.");
                    }
                }

                int result = await _unitOfWork.AnimalRepository.CreateAsync(animal);
                if (result > 0)
                {
                    return new KoiVetenaryResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public Task<IKoiVetenaryResult> DeleteAnimal(int? animalId)
        {
            throw new NotImplementedException();    
        }

        public Task<IKoiVetenaryResult> GetAnimalByIdAsync(int? animalId)
        {
            throw new NotImplementedException();
        }

        public Task<IKoiVetenaryResult> GetAnimalsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task<IKoiVetenaryResult> UpdateAnimal(Animal animal)
        {
            throw new NotImplementedException();
        }
    }
}
