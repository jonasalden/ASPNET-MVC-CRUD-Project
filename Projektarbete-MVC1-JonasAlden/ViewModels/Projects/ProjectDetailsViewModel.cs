using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projektarbete_MVC1_JonasAlden.ViewModels.Projects
{
    public class ProjectDetailsViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Platform { get; set; }
        [Required(ErrorMessage = "Please enter a date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        [Required(ErrorMessage = "Please enter a date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public string ProjectleaderId { get; set; }
        public string Projectleader { get; set; }
        public List<ProjectUserListViewModel> Users { get; set; }
    }
}