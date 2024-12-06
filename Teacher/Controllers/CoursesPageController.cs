using Teacher.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;

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

        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Course NewCourse)
        {
            int CourseId = _api.AddCourse(NewCourse);

            return RedirectToAction("Show", new {id = CourseId});
        }

        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            int CourseId = _api.DeleteCourse(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }

        [HttpPost]
        public IActionResult Update(int id, string CourseCode, int TeacherId, DateTime StartDate, DateTime FinishDate, string CourseName)
        {
            Course UpdatedCourse = new Course();
            UpdatedCourse.CourseCode = CourseCode;
            UpdatedCourse.TeacherId = TeacherId;
            UpdatedCourse.StartDate = StartDate;
            UpdatedCourse.FinishDate = FinishDate;
            UpdatedCourse.CourseName = CourseName;

            _api.UpdateCourse(id, UpdatedCourse);

            return RedirectToAction("Show", new {id = id});
        }
    }
}
