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
        /// This method will return a list of courses
        /// </summary>
        /// <example>
        /// GET： api/Course/ListCourses -> [{"courseId":1,"courseCode":"http5101","teacherId":1,"startDate":"2018-09-04T00:00:00","finishDate":"2018-12-14T00:00:00","courseName":"Web Application Development"},{"courseId":2,"courseCode":"http5102","teacherId":2,"startDate":"2018-09-04T00:00:00","finishDate":"2018-12-14T00:00:00","courseName":"Project Management"}.....]
        /// <returns>
        /// A list of course objects
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
                            CourseName = Name

                        };

                        Courses.Add(CurrentCourse);

                    }
                }
            }
            return Courses;
        }

        /// <summary>
        /// Returns a course in the database by their ID
        /// </summary>
        /// <example>
        /// GET: api/Course/FindCourse/1 -> {"courseId":1,"courseCode":"http5101","teacherId":1,"startDate":"2018-09-04T00:00:00","finishDate":"2018-12-14T00:00:00","courseName":"Web Application Development"}]
        /// <returns>
        /// A matching course object by its ID. Empty object if course not found
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
