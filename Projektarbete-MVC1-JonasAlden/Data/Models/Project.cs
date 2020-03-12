using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projektarbete_MVC1_JonasAlden.Data.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
        public string ProjectleaderId { get; set; }
        public string ProjectleaderName { get; set; }
    }
}