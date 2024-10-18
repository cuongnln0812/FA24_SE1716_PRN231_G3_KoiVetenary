using KoiVetenary.Data.Base;
using KoiVetenary.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Data.Repositories
{
    public class AppointmentDetailRepository : GenericRepository<AppointmentDetail>
    {
        public AppointmentDetailRepository() { }

        //update serviceId in AppointmentDetail
        public async Task<bool> UpdateServiceId(int appointmentId, int serviceId)
        {
            try
            {
                var appointmentDetail = await _context.AppointmentDetails.FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);
                if (appointmentDetail != null)
                {
                    appointmentDetail.ServiceId = serviceId;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}
