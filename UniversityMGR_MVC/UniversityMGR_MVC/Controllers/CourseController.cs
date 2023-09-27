using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityMGR_MVC.Models;
using UniversityMGR_MVC.Services.Interfaces;

namespace UniversityMGR_MVC.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICRUDService<Course> _courseCRUDService;
        private readonly ICourseService _courseService;

        public CourseController(ICRUDService<Course> courseCRUDService, ICourseService courseService)
        {
            _courseCRUDService = courseCRUDService;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseCRUDService.GetAllAsync();
            return View(courses);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _courseService.GetDetailsAsync((int)id);
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
            if (!ModelState.IsValid)
                return View(course);

            try
            {
                await _courseCRUDService.CreateAsync(course);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(course);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _courseCRUDService.GetByIdAsync((int)id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Course course)
        {
            if (!ModelState.IsValid)
                return View(course);

            try
            {
                await _courseCRUDService.UpdateAsync(course);
                return RedirectToAction(nameof(Details), new { id = course.Id });
            }
            catch (ArgumentNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(course);
            }
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _courseCRUDService.GetByIdAsync((int)id);

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
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var course = await _courseCRUDService.GetByIdAsync(id);
                return View(course);
            }
        }
    }
}
