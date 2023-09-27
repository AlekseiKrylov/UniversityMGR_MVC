using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityMGR_MVC.Models;
using UniversityMGR_MVC.Services.Interfaces;

namespace UniversityMGR_MVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly ICRUDService<Group> _groupCRUDService;
        private readonly ICRUDService<Student> _studentCRUDService;
        private readonly IStudentService _studentService;

        public StudentController(ICRUDService<Student> studentCRUDService, IStudentService studentService, ICRUDService<Group> groupCRUDService)
        {
            _studentCRUDService = studentCRUDService;
            _studentService = studentService;
            _groupCRUDService = groupCRUDService;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _studentCRUDService.GetAllAsync();
            return View(students);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentService.GetDetailsAsync((int)id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["GroupId"] = await ListOfGroupsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                ViewData["GroupId"] = await ListOfGroupsAsync();
                return View(student);
            }

            try
            {
                await _studentCRUDService.CreateAsync(student);
                return RedirectToAction(nameof(Index));

            }
            catch (ArgumentNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewData["GroupId"] = await ListOfGroupsAsync();
                return View(student);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentCRUDService.GetByIdAsync((int)id);

            if (student == null)
                return NotFound();

            ViewData["GroupId"] = await ListOfGroupsAsync();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Student student)
        {
            if (!ModelState.IsValid)
            {
                ViewData["GroupId"] = await ListOfGroupsAsync();
                return View(student);
            }

            try
            {
                await _studentCRUDService.UpdateAsync(student);
                return RedirectToAction(nameof(Details), new { id = student.Id });
            }
            catch (ArgumentNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewData["GroupId"] = await ListOfGroupsAsync();
                return View(student);
            }
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentCRUDService.GetByIdAsync((int)id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _studentCRUDService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var student = await _studentCRUDService.GetByIdAsync(id);
                return View(student);
            }
        }

        public async Task<ActionResult> Expel(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentService.GetDetailsAsync((int)id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Expel")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExpelConfirmed(int id)
        {
            try
            {
                int groupId = await _studentService.ExpelAsync(id);
                return RedirectToAction(nameof(GroupController.Details), "Group", new { id = groupId });
            }
            catch (ArgumentNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var student = await _studentService.GetDetailsAsync(id);
                return View(student);
            }
        }

        private async Task<SelectList> ListOfGroupsAsync()
        {
            return new SelectList(await _groupCRUDService.GetAllAsync(), "Id", "Name");
        }
    }
}
