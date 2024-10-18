using KoiVetenary.Business;
using KoiVetenary.Data.Models;
using KoiVetenary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiVetenary.Common;

namespace KoiVetenary.Service
{
    public interface IAppointmentDetailService
    {
        Task<IKoiVetenaryResult> GetAppointmentDetailsAsync();

        Task<IKoiVetenaryResult> UpdateDetailAppointmentServiceID(int appointmentId, int serviceId);
        Task<IKoiVetenaryResult> CreateAppointmentDetailAsync(AppointmentDetail appointment);
    }
    public class AppointmentDetailService : IAppointmentDetailService
    {

        private readonly UnitOfWork _unitOfWork;

        public AppointmentDetailService()
        {
            _unitOfWork ??= new UnitOfWork();
        } 

        public async Task<IKoiVetenaryResult> GetAppointmentDetailsAsync()
        {
            try
            {
                var result = await _unitOfWork.AppointmentDetailRepository.GetAllAsync();

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
    }
}
