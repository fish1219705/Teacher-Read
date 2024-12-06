using Teacher.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;

namespace Teacher.Controllers
{
    public class StudentPageController : Controller
    {
        private readonly StudentAPIController _api;

        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }
        public IActionResult List()
        {
            List<Student> Students = _api.ListStudents();
            return View(Students);
        }

        public IActionResult Show(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);

        }
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student NewStudent)
        {
            int StudentId = _api.AddStudent(NewStudent);

            return RedirectToAction("Show", new { id = StudentId });
        }

        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            int StudentId = _api.DeleteStudent(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }

        [HttpPost]
        public IActionResult Update(int id, string StudentFirstName,  string StudentLastName, string StudentNumber, DateTime EnrolDate)
        {
            Student UpdatedStudent = new Student();
            UpdatedStudent.StudentFirstName = StudentFirstName;
            UpdatedStudent.StudentLastName = StudentLastName;
            UpdatedStudent.StudentNumber = StudentNumber;
            UpdatedStudent.EnrolDate = EnrolDate;

            _api.UpdateStudent(id, UpdatedStudent);

            return RedirectToAction("Show", new { id = id });
        }
    }
}