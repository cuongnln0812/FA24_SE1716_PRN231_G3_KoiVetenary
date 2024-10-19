using KoiVetenary.Data;
using KoiVetenary.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service
{
    public interface ICheckoutService
    {
        public Task<Appointment> Checkout(int appointmentId);
        public Task<Appointment> CreatePayment(int appointmentId, Payment transaction);
    }
    public class CheckoutService : ICheckoutService
    {
        private readonly UnitOfWork _unitOfWork;
        public CheckoutService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public async Task<Appointment> Checkout(int appointmentId)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);
            appointment.Status = "To Pay";
            await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
            return appointment;
        }

        public async Task<Appointment> CreatePayment(int appointmentId, Payment transaction)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);
            appointment.Payments = (ICollection<Payment>)transaction;
            appointment.Status = "Paid";
            await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
            return appointment;
        }
    }
}
