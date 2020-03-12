using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projektarbete_MVC1_JonasAlden.Data.Models;

namespace Projektarbete_MVC1_JonasAlden.ViewModels.Projects
{
    public class AddUserProjectViewModel
    {
        public int ProjectId { get; set; }
        public List<User> Users { get; set; }
    }
}