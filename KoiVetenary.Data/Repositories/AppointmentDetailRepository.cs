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

        public async Task<int> CreateAsync(AppointmentDetail detail){
            _context.Add(detail);
            await _context.SaveChangesAsync();
            return detail.AppointmentDetailId;
        }
    }
}
