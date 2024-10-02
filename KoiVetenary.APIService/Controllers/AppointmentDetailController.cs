using KoiVetenary.Business;
using KoiVetenary.Service;
using Microsoft.AspNetCore.Mvc;

namespace KoiVetenary.APIService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentDetailController : ControllerBase
    {
        private readonly IAppointmentDetailService _appointmentDetailService;

        public AppointmentDetailController(IAppointmentDetailService context)
        {
            _appointmentDetailService = context;
        }

        // GET: Appointments
        [HttpGet]
        public async Task<IKoiVetenaryResult> GetAppoinentAsync()
        {
            return await _appointmentDetailService.GetAppointmentDetailsAsync();

            //var fA24_SE1716_PRN231_G3_KoiVetenaryContext = _appointmentService.Appointments.Include(a => a.Owner);
            //return View(await fA24_SE1716_PRN231_G3_KoiVetenaryContext.ToListAsync());
        }
    }
}
