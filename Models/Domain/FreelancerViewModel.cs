using System.ComponentModel.DataAnnotations;

namespace Freelancers.Models.Domain
{
    public class FreelancerViewModel
    {
        public int ID { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Mail { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Skillsets { get; set; }
        [Required]
        public string? Hobby { get; set; }

    }
}
