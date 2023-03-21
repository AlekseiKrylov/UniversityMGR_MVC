using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task9.Models;
using Task9.Services.Interfaces;

namespace Task9.Controllers
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
            var students = await _studentCRUDService.GetAllEntitiesAsync();
            return View(students);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentCRUDService.GetEntityByIdAsync((int)id);
            
            if (student == null)
                return NotFound();

            return View(student);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["GroupId"] = new SelectList(await _groupCRUDService.GetAllEntitiesAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentCRUDService.CreateAsync(student);
                return RedirectToAction(nameof(Index));
            }

            ViewData["GroupId"] = new SelectList(await _groupCRUDService.GetAllEntitiesAsync(), "Id", "Name");
            return View(student);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            
            var student = await _studentCRUDService.GetEntityByIdAsync((int)id);
            
            if (student == null)
                return NotFound();
            
            ViewData["GroupId"] = new SelectList(await _groupCRUDService.GetAllEntitiesAsync(), "Id", "Name");
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentCRUDService.UpdateAsync(student);
                return RedirectToAction(nameof(Details), new { id = student.Id });
            }
            ViewData["GroupId"] = new SelectList(await _groupCRUDService.GetAllEntitiesAsync(), "Id", "Name");
            return View(student);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentCRUDService.GetEntityByIdAsync((int)id);

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
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var student = await _studentCRUDService.GetEntityByIdAsync(id);
                return View(student);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Expel(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentCRUDService.GetEntityByIdAsync((int)id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Expel")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExpelConfirmed(int id)
        {
            int groupId = await _studentService.ExpelAsync(id);
            return RedirectToAction(nameof(GroupController.Details), "Group", new { id = groupId });
        }
    }
}
