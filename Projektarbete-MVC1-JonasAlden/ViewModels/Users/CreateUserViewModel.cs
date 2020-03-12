using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Projektarbete_MVC1_JonasAlden.ViewModels.Users
{
    public class CreateUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Firstname is required")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }
        [Required]
        public string Experience { get; set; }
        [Required]
        public DateTime EmployedDate { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Address can be no larger than 50 characters")]
        public string Address { get; set; }
        [StringLength(30, ErrorMessage = "Domain can be no larger than 50 characters")]
        [Required]
        public string Domain { get; set; }
        public IEnumerable<SelectListItem> Rolelist { get; set; }
        public string SelectedRole { get; set; }
    }
}