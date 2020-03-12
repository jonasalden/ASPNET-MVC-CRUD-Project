using System;
using System.ComponentModel.DataAnnotations;

namespace Projektarbete_MVC1_JonasAlden.ViewModels.Projects
{
    public class ProjectListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }
        [Required(ErrorMessage = "Please enter a date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
        public string ProjectLeader { get; set; }
        public int Employees { get; set; }
    }
}