using Teacher.Models;
using Microsoft.AspNetCore.Mvc;

namespace Teacher.Controllers
{
    public class CoursePageController : Controller
    {
        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }
        public IActionResult List()
        {
            List<Course> Courses= _api.ListCourses();
            return View(Courses);
        }

        public IActionResult Show(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);

        }
    }
}
