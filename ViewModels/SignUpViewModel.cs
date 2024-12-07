using System.ComponentModel.DataAnnotations;

namespace CompanyMangementSystem_PL.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage ="Full Name Is Required")]
        public string FullName { get; set; }

        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress(ErrorMessage ="InValid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage ="Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password) , ErrorMessage ="Confirm Password Doesn't Match Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage ="Required To Agree")]
        public bool IsAgree { get; set; }
    }
}
