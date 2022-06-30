using Microsoft.AspNetCore.Mvc;
using MyProject.DAL;
using MyProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _sql;

        public HomeController(AppDbContext sql)
        {
            _sql = sql;
        }
        
        public IActionResult Index()
        {
            List<Person> persons = _sql.Persons.ToList();
            return View(persons);
        }

    }
}
