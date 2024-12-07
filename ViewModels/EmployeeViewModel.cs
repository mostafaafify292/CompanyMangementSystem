using System.ComponentModel.DataAnnotations;
using Company_DAL.Models;

namespace CompanyMangementSystem_PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is Required")]
        [MaxLength(100 , ErrorMessage = "Max Length is 100")]
        [MinLength(4 , ErrorMessage ="Min Length is 4")]
        public string Name { get; set; }
        [Range(21,60)]
        public int? Age { get; set; }
        [Required(ErrorMessage ="Address is Required")]
        public string Address { get; set; }
        public decimal Salery { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }
        public Gender Gender { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public IFormFile Image { get; set; }
        public string? ImageName { get; set; }
    }
}
