using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using Teacher.Models;
using System;

namespace Teacher.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentAPIController : ControllerBase

    {
        private readonly SchoolDbContext _context;
        public StudentAPIController(SchoolDbContext context)
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
        [Route(template: "ListStudents")]
        public List<Student> ListStudents()
        {
            List<Student> Students = new List<Student>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();

                string query = "select * from students";

                Command.CommandText = query;

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {

                        int Id = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string Number = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);
                        

                        Student CurrentStudent = new Student()
                        {
                            StudentId = Id,
                            StudentFirstName = FirstName,
                            StudentLastName = LastName,
                            StudentNumber = Number,
                            EnrolDate = EnrolDate,
                            
                        };

                        Students.Add(CurrentStudent);

                    }
                }
            }
            return Students;
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
        [Route(template: "FindStudent/{id}")]
        public Student FindStudent(int id)
        {

            Student SelectedStudent = new Student();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "select * from students where studentid=@id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {

                        int Id = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string Number = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                        SelectedStudent.StudentId = Id;
                        SelectedStudent.StudentFirstName = FirstName;
                        SelectedStudent.StudentLastName = LastName;
                        SelectedStudent.StudentNumber = Number;
                        SelectedStudent.EnrolDate = EnrolDate;
                    }
                }
            }
            return SelectedStudent;
        }
    }
}
