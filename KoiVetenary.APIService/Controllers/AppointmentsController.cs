using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiVetenary.Data.Models;
using KoiVetenary.Service;
using KoiVetenary.Common;
using Newtonsoft.Json;
using KoiVetenary.Business;

namespace KoiVetenary.APIService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService context)
        {
            _appointmentService = context;
        }

        // GET: Appointments
        [HttpGet]
        public async Task<IKoiVetenaryResult> GetAppoinentAsync()
        {
            return await _appointmentService.GetAppointmentsAsync();

            //var fA24_SE1716_PRN231_G3_KoiVetenaryContext = _appointmentService.Appointments.Include(a => a.Owner);
            //return View(await fA24_SE1716_PRN231_G3_KoiVetenaryContext.ToListAsync());
        }

        //GET: Appointments/Details/5
        [HttpGet("{id}")]
        public async Task<IKoiVetenaryResult> Details( int? id)
        {
            return await _appointmentService.GetAppointmentByIdAsync(id);
        }

        //    // GET: Appointments/Create
        //    public IActionResult Create()
        //    {
        //        ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "OwnerId");
        //        return View();
        //    }

        //    // POST: Appointments/Create
        //    // To protect from overposting attacks, enable the specific properties you want to bind to.
        //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Create([Bind("AppointmentId,OwnerId,AppointmentDate,AppointmentTime,Status,Notes,TotalEstimatedDuration,TotalCost,CreatedBy,ModifiedBy,CreatedDate,UpdatedDate")] Appointment appointment)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _context.Add(appointment);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "OwnerId", appointment.OwnerId);
        //        return View(appointment);
        //    }

        //    // GET: Appointments/Edit/5
        //    public async Task<IActionResult> Edit(int? id)
        //    {
        //        if (id == null || _context.Appointments == null)
        //        {
        //            return NotFound();
        //        }

        //        var appointment = await _context.Appointments.FindAsync(id);
        //        if (appointment == null)
        //        {
        //            return NotFound();
        //        }
        //        ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "OwnerId", appointment.OwnerId);
        //        return View(appointment);
        //    }

        //    // POST: Appointments/Edit/5
        //    // To protect from overposting attacks, enable the specific properties you want to bind to.
        //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,OwnerId,AppointmentDate,AppointmentTime,Status,Notes,TotalEstimatedDuration,TotalCost,CreatedBy,ModifiedBy,CreatedDate,UpdatedDate")] Appointment appointment)
        //    {
        //        if (id != appointment.AppointmentId)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                _context.Update(appointment);
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!AppointmentExists(appointment.AppointmentId))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "OwnerId", appointment.OwnerId);
        //        return View(appointment);
        //    }

        //    // GET: Appointments/Delete/5
        //    public async Task<IActionResult> Delete(int? id)
        //    {
        //        if (id == null || _context.Appointments == null)
        //        {
        //            return NotFound();
        //        }

        //        var appointment = await _context.Appointments
        //            .Include(a => a.Owner)
        //            .FirstOrDefaultAsync(m => m.AppointmentId == id);
        //        if (appointment == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(appointment);
        //    }

        //    // POST: Appointments/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> DeleteConfirmed(int id)
        //    {
        //        if (_context.Appointments == null)
        //        {
        //            return Problem("Entity set 'FA24_SE1716_PRN231_G3_KoiVetenaryContext.Appointments'  is null.");
        //        }
        //        var appointment = await _context.Appointments.FindAsync(id);
        //        if (appointment != null)
        //        {
        //            _context.Appointments.Remove(appointment);
        //        }

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    private bool AppointmentExists(int id)
        //    {
        //      return (_context.Appointments?.Any(e => e.AppointmentId == id)).GetValueOrDefault();
        //    }
    }
}
