﻿using Teacher.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;

namespace Teacher.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly TeacherAPIController _api;
        
        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }
        //GET : TeacherPage/List/SearchKey={SearchKey}
        public IActionResult List(string SearchKey)
        {
            List<ATeacher> Teachers = _api.ListTeachers(SearchKey);
            return View(Teachers);
        }

        //GET : TeacherPage/Show/{id}
        public IActionResult Show(int id)
        {
            ATeacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }



        // GET : TeacherPage/New
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }


        // POST: TeacherPage/Create
        [HttpPost]
        public IActionResult Create(ATeacher NewTeacher)
        {
            int TeacherId = _api.AddTeacher(NewTeacher);

            return RedirectToAction("Show", new { id = TeacherId });
                
        }


        // GET : TeacherPage/DeleteConfirm/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            ATeacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }

        // POST: TeacherPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int TeacherId = _api.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        // GET: TeacherPage/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ATeacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }

        // POST: TeacherPage/Update/{id}
        [HttpPost]
        public IActionResult Update(int id, string TeacherFirstName, string TeacherLastName, string EmployeeNumber, DateTime HireDate, Decimal Salary)
        {
            ATeacher UpdatedTeacher = new ATeacher();
            UpdatedTeacher.TeacherFirstName = TeacherFirstName; ;
            UpdatedTeacher.TeacherLastName = TeacherLastName;
            UpdatedTeacher.EmployeeNumber = EmployeeNumber;
            UpdatedTeacher.HireDate = HireDate;
            UpdatedTeacher.Salary = Salary;

            _api.UpdateTeacher(id, UpdatedTeacher);

            return RedirectToAction("Show", new { id = id });

        }
    }
}
