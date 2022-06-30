using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Level { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
