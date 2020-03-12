using System.Collections.Generic;

namespace Projektarbete_MVC1_JonasAlden.ViewModels.Projects
{
    public class ProjectsViewModel
    {
        public List<ProjectListViewModel> Projects { get; set; }
        public ProjectsViewModel()
        {
            Projects = new List<ProjectListViewModel>();
        }
    }
}