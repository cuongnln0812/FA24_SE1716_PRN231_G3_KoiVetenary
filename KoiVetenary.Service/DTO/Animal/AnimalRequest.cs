using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service.DTO.Animal
{
    public class AnimalRequest
    {
        public int AnimalId { get; set; }
        public int? OwnerId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public int? TypeId { get; set; }
        public string Origin { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Age { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public string Color { get; set; }
        public string DistinguishingMarks { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? LastCheckup { get; set; }
        public string ImageUrl { get; set; }
        public string Gender { get; set; }
    }
}
