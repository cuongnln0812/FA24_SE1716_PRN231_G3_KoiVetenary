using KoiVetenary.Business;
using KoiVetenary.Common;
using KoiVetenary.Data;
using KoiVetenary.Data.Models;


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

        public async Task<IKoiVetenaryResult> DeleteAppointment(int? id)
        {
            try
            {
                var removedItem = await _unitOfWork.AppointmentRepository.GetByIdAsync((int)id);
                _unitOfWork.AppointmentRepository.PrepareRemove(removedItem);
                var result = await _unitOfWork.AppointmentRepository.SaveAsync();
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

        public async Task<IKoiVetenaryResult> GetAppointmentByIdAsync(int? id)
        {
            try
            {
                
                var result = await _unitOfWork.AppointmentRepository.GetByIdAsync((int)id);

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

        public async Task<IKoiVetenaryResult> GetAppointmentsAsync()
        {
            try
            {

                var result = await _unitOfWork.AppointmentRepository.GetAllAsync();

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

        public async Task<IKoiVetenaryResult> UpdateAppointment(Appointment appointment)
        {
            try
            {
                var existed = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointment.AppointmentId);
                if (existed == null)
                {
                    return new KoiVetenaryResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            
                existed.AppointmentDate = appointment.AppointmentDate;
                existed.AppointmentTime = appointment.AppointmentTime;
                existed.Status = appointment.Status;
                existed.Notes = appointment.Notes;
                existed.TotalEstimatedDuration = appointment.TotalEstimatedDuration;
                existed.TotalCost = appointment.TotalCost;
                existed.ModifiedBy = "admin";
                existed.UpdatedDate = DateTime.Now;
                existed.OwnerId = appointment.OwnerId;

                _unitOfWork.AppointmentRepository.PrepareUpdate(existed);

                // Save the changes asynchronously
                var result = await _unitOfWork.AppointmentRepository.SaveAsync();

                // Check the result and return appropriate response
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
                // Handle any exceptions and return an error result
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
    }
}
