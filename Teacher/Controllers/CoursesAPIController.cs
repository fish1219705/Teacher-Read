using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using Teacher.Models;
using System;

namespace Teacher.Controllers
{
    [Route("api/Course")]
    [ApiController]
    public class CourseAPIController : ControllerBase

    {
        private readonly SchoolDbContext _context;
        public CourseAPIController(SchoolDbContext context)
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
        [Route(template: "ListCourses")]
        public List<Course> ListCourses()
        {
            List<Course> Courses = new List<Course>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();

                string query = "select * from courses";

                Command.CommandText = query;

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {

                        int Id = Convert.ToInt32(ResultSet["courseid"]);
                        string Code = ResultSet["coursecode"].ToString();
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        
                        DateTime SDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string Name = ResultSet["coursename"].ToString();


                        Course CurrentCourse = new Course()
                        {
                            CourseId = Id,
                            CourseCode = Code,
                            TeacherId = TeacherId,
                            StartDate = SDate,
                            FinishDate = FDate,

                        };

                        Courses.Add(CurrentCourse);

                    }
                }
            }
            return Courses;
        }

        /// <summary>
        /// Returns a teacher in the database by their ID
        /// </summary>
        /// <example>
        /// GET: api/Teacher/FindTeacher/3 -> {"teacherId":3,"teacherFirstName":"Linda","teacherLastName":"Chan","employeeNumber":"T382","hireDate":"2015-08-22T00:00:00","salary":60.22}
        /// </example>
        /// <returns>
        /// A matching author object by its ID. Empty object if Author not found
        /// </returns>
        [HttpGet]
        [Route(template: "FindCourse/{id}")]
        public Course FindCourse(int id)
        {

            Course SelectedCourse = new Course();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "select * from courses where courseid=@id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {

                        int Id = Convert.ToInt32(ResultSet["courseid"]);
                        string Code = ResultSet["coursecode"].ToString();
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);

                        DateTime SDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string Name = ResultSet["coursename"].ToString();

                        SelectedCourse.CourseId = Id;
                        SelectedCourse.CourseCode = Code;
                        SelectedCourse.TeacherId = TeacherId;
                        SelectedCourse.StartDate = SDate;
                        SelectedCourse.FinishDate = FDate;
                        SelectedCourse.CourseName = Name;
                    }
                }
            }
            return SelectedCourse;
        }
    }
}
