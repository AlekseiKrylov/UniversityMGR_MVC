using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task9.Models;
using Task9.Services.Interfaces;
using Group = Task9.Models.Group;

namespace Task9.Controllers
{
    public class GroupController : Controller
    {
        private readonly ICRUDService<Course> _courseCRUDService;
        private readonly ICRUDService<Group> _groupCRUDService;
        private readonly IGroupService _groupService;

        public GroupController(ICRUDService<Group> groupCRUDService, IGroupService groupService, ICRUDService<Course> courseCRUDService)
        {
            _groupCRUDService = groupCRUDService;
            _groupService = groupService;
            _courseCRUDService = courseCRUDService;
        }

        public async Task<IActionResult> Index()
        {
            var groups = await _groupCRUDService.GetAllAsync();
            return View(groups);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var group = await _groupService.GetDetailsAsync((int)id);

            if (group == null)
                return NotFound();

            return View(group);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CourseId"] = await ListOfCoursesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Group group)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CourseId"] = await ListOfCoursesAsync();
                return View(group);
            }

            try
            {
                await _groupCRUDService.CreateAsync(group);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewData["CourseId"] = await ListOfCoursesAsync();
                return View(group);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var group = await _groupCRUDService.GetByIdAsync((int)id);

            if (group == null)
                return NotFound();

            ViewData["CourseId"] = await ListOfCoursesAsync();
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Group group)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CourseId"] = await ListOfCoursesAsync();
                return View(group);
            }

            try
            {
                await _groupCRUDService.UpdateAsync(group);
                return RedirectToAction(nameof(Details), new { id = group.Id });
            }
            catch (ArgumentNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewData["CourseId"] = await ListOfCoursesAsync();
                return View(group);
            }
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var group = await _groupCRUDService.GetByIdAsync((int)id);

            if (group == null)
                return NotFound();

            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _groupCRUDService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var group = await _groupCRUDService.GetByIdAsync(id);
                return View(group);
            }
        }

        public async Task<ActionResult> ExpelAll(int? id)
        {
            if (id == null)
                return NotFound();

            var group = await _groupService.GetDetailsAsync((int)id);

            if (group == null)
                return NotFound();

            return View(group);
        }

        [HttpPost, ActionName("ExpelAll")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExpelAllConfirmed(int id)
        {
            await _groupService.ExpelAllStudentsAsync(id);
            return RedirectToAction(nameof(Details), new { id });
        }

        private async Task<SelectList> ListOfCoursesAsync()
        {
            return new SelectList(await _courseCRUDService.GetAllAsync(), "Id", "Name");
        }
    }
}
