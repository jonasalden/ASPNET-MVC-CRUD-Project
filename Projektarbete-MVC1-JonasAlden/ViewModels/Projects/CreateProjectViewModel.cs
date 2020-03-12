using System;
using System.ComponentModel.DataAnnotations;

namespace Projektarbete_MVC1_JonasAlden.ViewModels.Projects
{
    public class CreateProjectViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Platform { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SurName { get; set; }
    }
}