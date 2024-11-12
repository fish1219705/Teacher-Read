using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using Teacher.Models;
using System;
using System.Collections.Generic;
using Mysqlx.Datatypes;

namespace Teacher.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase

    {
        private readonly SchoolDbContext _context;
        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// This method will return a list of teachers
        /// </summary>
        /// <example>
        /// GET： api/Teacher/ListTeachers -> [{"teacherId":1,"teacherFirstName":"Alexander","teacherLastName":"Bennett","employeeNumber":"T378","hireDate":"2016-08-05T00:00:00","salary":55.30},{"teacherId":2,"teacherFirstName":"Caitlin","teacherLastName":"Cummings","employeeNumber":"T381","hireDate":"2014-06-10T00:00:00","salary":62.77}.....]
        /// </example>
        /// <returns>
        /// A list of teacher objects
        /// </returns>

        [HttpGet]
        [Route(template: "ListTeachers")]
        public List<ATeacher> ListTeachers(string SearchKey=null)
        {
            List<ATeacher> Teachers = new List<ATeacher>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();

                string query = "select * from teachers";

                if (SearchKey != null)
                {
                    query += " where hiredate like @key";
                    Command.Parameters.AddWithValue("@key", $"%{SearchKey}%");
                }
                Command.CommandText = query;
                Command.Prepare();

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {

                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string Number = ResultSet["employeenumber"].ToString();
                        DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);

                        ATeacher CurrentTeacher = new ATeacher()
                        {
                            TeacherId = Id,
                            TeacherFirstName = FirstName,
                            TeacherLastName = LastName,
                            EmployeeNumber = Number,
                            HireDate = TeacherHireDate,
                            Salary = TeacherSalary
                        };

                        Teachers.Add(CurrentTeacher);

                    }
                }
            }
            return Teachers;
        }

        /// <summary>
        /// Returns a teacher in the database by their ID
        /// </summary>
        /// <example>
        /// GET: api/Teacher/FindTeacher/3 -> {"teacherId":3,"teacherFirstName":"Linda","teacherLastName":"Chan","employeeNumber":"T382","hireDate":"2015-08-22T00:00:00","salary":60.22}
        /// </example>
        /// <returns>
        /// A matching teacher object by its ID. Empty object if teacher not found
        /// </returns>
        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public ATeacher FindTeacher(int id)
        {

            ATeacher SelectedTeacher = new ATeacher();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "select teachers.*,courses.coursename from teachers left join courses on (teachers.teacherid=courses.teacherid) where teachers.teacherid=@id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
  
                            int Id = Convert.ToInt32(ResultSet["teacherid"]);
                            string FirstName = ResultSet["teacherfname"].ToString();
                            string LastName = ResultSet["teacherlname"].ToString();
                            string Number = ResultSet["employeenumber"].ToString();
                            DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                            decimal TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);
                            string courseName = ResultSet["coursename"].ToString();

                            SelectedTeacher.TeacherId = Id;
                            SelectedTeacher.TeacherFirstName = FirstName;
                            SelectedTeacher.TeacherLastName = LastName;
                            SelectedTeacher.EmployeeNumber = Number;
                            SelectedTeacher.HireDate = TeacherHireDate;
                            SelectedTeacher.Salary = TeacherSalary;
                            SelectedTeacher.TeacherCourse = courseName;
                        }
                    }
                }
            return SelectedTeacher;
        }
    }
}
