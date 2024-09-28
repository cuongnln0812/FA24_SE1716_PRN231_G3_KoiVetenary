using KoiVetenary.Business;
using KoiVetenary.Common;
using KoiVetenary.Data;
using KoiVetenary.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service
{
    public interface IAppointmentService
    {
        Task<IKoiVetenaryResult> GetAppointmentsAsync();
        Task<IKoiVetenaryResult> GetAppointmentByIdAsync(int? id);
        Task<IKoiVetenaryResult> CreateAppointment(Appointment appointment);
        Task<IKoiVetenaryResult> UpdateAppointment(Appointment appointment);
        Task<IKoiVetenaryResult> DeleteAppointment(int? id);
        Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm);
    }
    public class AppointmentService : IAppointmentService
    {

        private readonly UnitOfWork _unitOfWork;

        public AppointmentService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IKoiVetenaryResult> CreateAppointment(Appointment appointment)
        {
            try
            {

                int result = await _unitOfWork.AppointmentRepository.CreateAsync(appointment);
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

        public Task<IKoiVetenaryResult> DeleteAppointment(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<IKoiVetenaryResult> GetAppointmentByIdAsync(int? id)
        {
            try
            {
                
                    var result = await _unitOfWork.AppointmentRepository.GetByIdAsync((int)id);

                if (result != null)
                {
                    return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG);
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

        public async Task<IKoiVetenaryResult> GetAppointmentsAsync()
        {
            try
            {

                var result = await _unitOfWork.AppointmentRepository.GetAllAsync();

                if (result != null)
                {
                    return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG);
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

        public Task<IKoiVetenaryResult> UpdateAppointment(Appointment appointmenta)
        {
            throw new NotImplementedException();
        }
    }
}
