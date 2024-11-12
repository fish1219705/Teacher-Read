using Teacher.Models;
using Microsoft.AspNetCore.Mvc;

namespace Teacher.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly TeacherAPIController _api;
        
        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }
        public IActionResult List(string SearchKey)
        {
            List<ATeacher> Teachers = _api.ListTeachers(SearchKey);
            return View(Teachers);
        }
        
        public IActionResult Show(int id)
        {
            ATeacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);

        }
    }
}
