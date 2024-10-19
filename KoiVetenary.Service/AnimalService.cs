using KoiVetenary.Common;
using KoiVetenary.Data.Models;
using KoiVetenary.Service.DTO.Animal;
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
        Task<IKoiVetenaryResult> CreateAnimal(AnimalRequest animalRequest);
        Task<IKoiVetenaryResult> UpdateAnimal(AnimalRequest animalRequest);
        Task<IKoiVetenaryResult> DeleteAnimal(int? animalId);
        Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm);
    }
    public class AnimalService : IAnimalService
    {
        private readonly UnitOfWork _unitOfWork;

        public AnimalService() {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IKoiVetenaryResult> CreateAnimal(AnimalRequest animalRequest)
        {
            try
            {
                var animals = await _unitOfWork.AnimalRepository.GetAllAsync();
                foreach (var item in animals)
                {
                    if (animalRequest.Name.Equals(item.Name))
                    {
                        return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Name is duplicated.");
                    }
                }

                var animalType = await _unitOfWork.AnimalTypeRepository.GetByIdAsync((int)animalRequest.TypeId);
                if (animalType == null)
                {
                    return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Animal Type is not found!");
                }

                var owner = await _unitOfWork.OwnerRepository.GetByIdAsync((int)animalRequest.OwnerId);
                if (owner == null)
                {
                    return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Owner is not found!");
                }

                var newAnimal = new Animal
                {
                    Name = animalRequest.Name,
                    OwnerId = animalRequest.OwnerId,
                    Species = animalRequest.Species,
                    TypeId = animalRequest.TypeId,
                    Origin = animalRequest.Origin,
                    DateOfBirth = animalRequest.DateOfBirth,
                    Age = animalRequest.Age,
                    Weight = animalRequest.Weight,
                    Length = animalRequest.Length,
                    Color = animalRequest.Color,
                    DistinguishingMarks = animalRequest.DistinguishingMarks,
                    DateAdded = DateTime.Now,
                    LastCheckup = DateTime.Now,
                    ImageUrl = animalRequest.ImageUrl,
                    Gender = animalRequest.Gender,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now
                };

                int result = await _unitOfWork.AnimalRepository.CreateAsync(newAnimal);
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

        public async Task<IKoiVetenaryResult> DeleteAnimal(int? animalId)
        {
            try
            {
                var removedItem = await _unitOfWork.AnimalRepository.GetByIdAsync((int)animalId);
                _unitOfWork.AnimalRepository.PrepareRemove(removedItem);
                var result = await _unitOfWork.AnimalRepository.SaveAsync();
                if (result > 0)
                {
                    return new KoiVetenaryResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IKoiVetenaryResult> GetAnimalByIdAsync(int? animalId)
        {
            try
            {
                var result = await _unitOfWork.AnimalRepository.GetByIdAsync((int) animalId);

                if (result != null)
                {
                    return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IKoiVetenaryResult> GetAnimalsAsync()
        {
            try
            {
                var result = await _unitOfWork.AnimalRepository.GetAllAsync();

                if (result != null)
                {
                    return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm)
        {
            throw new NotImplementedException();
        }

        public async Task<IKoiVetenaryResult> UpdateAnimal(AnimalRequest animalRequest)
        {
            try
            {
                var existingAnimal = await _unitOfWork.AnimalRepository.GetByIdAsync(animalRequest.AnimalId);
                if (existingAnimal == null)
                {
                    return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Animal not found.");
                }

                var animals = await _unitOfWork.AnimalRepository.GetAllAsync();
                foreach (var item in animals)
                {
                    if (animalRequest.Name.Equals(item.Name) && item.AnimalId != animalRequest.AnimalId)
                    {
                        return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Name is duplicated.");
                    }
                }

                var animalType = await _unitOfWork.AnimalTypeRepository.GetByIdAsync((int)animalRequest.TypeId);
                if (animalType == null)
                {
                    return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Animal Type is not found!");
                }

                var owner = await _unitOfWork.OwnerRepository.GetByIdAsync((int)animalRequest.OwnerId);
                if (owner == null)
                {
                    return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Owner is not found!");
                }

                existingAnimal.Name = animalRequest.Name;
                existingAnimal.Species = animalRequest.Species;
                existingAnimal.Origin = animalRequest.Origin;
                existingAnimal.DateOfBirth = animalRequest.DateOfBirth;
                existingAnimal.Age = animalRequest.Age;
                existingAnimal.Weight = animalRequest.Weight;
                existingAnimal.Length = animalRequest.Length;
                existingAnimal.Color = animalRequest.Color;
                existingAnimal.DistinguishingMarks = animalRequest.DistinguishingMarks;
                existingAnimal.ImageUrl = animalRequest.ImageUrl;
                existingAnimal.Gender = animalRequest.Gender;
                existingAnimal.LastCheckup = DateTime.Now;
                existingAnimal.ModifiedBy = "Admin";
                existingAnimal.UpdatedDate = DateTime.Now;
                existingAnimal.Type = animalType;
                existingAnimal.Owner = owner;

                int result = await _unitOfWork.AnimalRepository.UpdateAsync(existingAnimal);
                if (result > 0)
                {
                    return new KoiVetenaryResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
