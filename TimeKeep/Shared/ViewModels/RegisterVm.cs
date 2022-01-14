using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeep.Shared.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 8)]
        public string Password { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 8)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        public DateTime HireDate { get; set; } = DateTime.Today;
        [Required]
        [Range(0, 260)]
        public decimal VacationDaysAccruedPerMonth { get; set; } = 1.25M;
        [Required]
        [Range(0, 2080)]
        public decimal SickHoursAccruedPerMonth { get; set; } = 5.33M;
        [Required]
        [Range(0, 260)]
        public int PersonalDaysPerYear { get; set; } = 3;
        [Range(0, 5)]
        public decimal VacationDaysCarriedOver { get; set; }
    }
}
