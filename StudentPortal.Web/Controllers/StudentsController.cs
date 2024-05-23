using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentsController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            Student student = new Student
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Subscribed = viewModel.Subscribed
            };
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            
            return View();

            // return RedirectToAction("Add", "Students"); Bunun yerine alttaki yöntem best practice
            // Sunucuya ek yük binmemesi için input temizleme, view kısmında JS ile yapılmalı
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
           List<Student> students = await _dbContext.Students.ToListAsync();
           students.Reverse();

           return View(students);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await _dbContext.Students.FindAsync(id);
            return View(student);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Student viewModal)
        {
            var student = await _dbContext.Students.FindAsync(viewModal.Id);
            
            if (student is not null)
            {
                student.Name = viewModal.Name;
                student.Email = viewModal.Email;
                student.Phone = viewModal.Phone;
                student.Subscribed = viewModal.Subscribed;

                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Students");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Student viewModal)
        {
            var student = await _dbContext.Students
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.Id == viewModal.Id);

            if (student is not null)
            {
                _dbContext.Students.Remove(viewModal);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Students");
        }
    }
}
