using Projektarbete_MVC1_JonasAlden.ViewModels.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projektarbete_MVC1_JonasAlden.ViewModels.Users
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SurName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EmployDate { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Experience { get; set; }
        [Required]
        public string Domain { get; set; }
        public List<ProjectListViewModel> Projects { get; set; }
        public bool Admin { get; set; }
        public bool Projectleader { get; set; }
    }
}