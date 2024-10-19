using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service.DTO.VNPAY
{
    public class VnPayRequestModel
    {
        public int AppointmentId { get; set; }
        public string OwnerId { get; set; }
        public string Notes { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int? TotalEstimatedDuration { get; set; }
    }
}
