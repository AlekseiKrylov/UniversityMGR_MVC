using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task9.Models;
using Task9.Services.Interfaces;

namespace Task9.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICRUDService<Course> _courseCRUDService;

        public CourseController(ICRUDService<Course> courseCRUDService)
        {
            _courseCRUDService = courseCRUDService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseCRUDService.GetAllEntitiesAsync();
            return View(courses);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _courseCRUDService.GetEntityByIdAsync((int)id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                await _courseCRUDService.CreateAsync(course);
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _courseCRUDService.GetEntityByIdAsync((int)id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                await _courseCRUDService.UpdateAsync(course);
                return RedirectToAction(nameof(Details), new { id = course.Id });
            }

            return View(course);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _courseCRUDService.GetEntityByIdAsync((int)id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _courseCRUDService.DeleteAsync(id);
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var course = await _courseCRUDService.GetEntityByIdAsync((int)id);
                return View(course);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
