using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyProject.DAL;
using MyProject.Extentions;
using MyProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProject.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Moderator")]
    public class PersonController : Controller
    {
        private readonly AppDbContext _sql;
        private readonly IWebHostEnvironment _env;

        public PersonController(AppDbContext sql,IWebHostEnvironment env)
        {
            _sql = sql;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Person> persons = _sql.Persons.ToList();
            return View(persons);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Person person)
        {
            if (!ModelState.IsValid) return View();
            if (person == null) return NotFound();
            if (person.ImageFile!=null)
            {
                if (!person.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile","Sekilin formati duzgun deyil!!");
                    return View();
                }
                if (!person.ImageFile.IsSizeOk(5))
                {
                    ModelState.AddModelError("ImageFile", "Sekil 5 mb-dan boyuk ola bilmez!!");
                    return View();
                }
                person.Image = person.ImageFile.SaveImage(_env.WebRootPath,"assets/images");
            }
            await _sql.Persons.AddAsync(person);
            await _sql.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            Person person = _sql.Persons.Find(id);
            return View(person);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,Person person)
        {
            if (!ModelState.IsValid) return View();
            if (person == null) return NotFound();
            Person personExist= _sql.Persons.Find(id);
            if (personExist == null) return NotFound();
            if (person.ImageFile != null)
            {
                if (!person.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Sekilin formati duzgun deyil!!");
                    return View();
                }
                if (!person.ImageFile.IsSizeOk(5))
                {
                    ModelState.AddModelError("ImageFile", "Sekil 5 mb-dan boyuk ola bilmez!!");
                    return View();
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath,"assets/images",personExist.Image);
                personExist.Image = person.ImageFile.SaveImage(_env.WebRootPath,"assets/images");
            }
            personExist.Fullname = person.Fullname;
            personExist.Level = person.Level;
            await _sql.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            Person person = await _sql.Persons.FindAsync(id);
            if (person == null) return NotFound();
             _sql.Persons.Remove(person);
            await _sql.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
