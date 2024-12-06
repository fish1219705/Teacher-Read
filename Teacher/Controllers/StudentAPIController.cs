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
        /// This method will return a list of students
        /// </summary>
        /// <example>
        /// GET： api/Teacher/ListStudents -> [{"studentId":1,"studentFirstName":"Sarah","studentLastName":"Valdez","studentNumber":"N1678","enrolDate":"2018-06-18T00:00:00"},{"studentId":2,"studentFirstName":"Jennifer","studentLastName":"Faulkner","studentNumber":"N1679","enrolDate":"2018-08-02T00:00:00"}....]
        /// </example>
        /// <returns>
        /// A list of students objects
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
        /// Returns a student in the database by their ID
        /// </summary>
        /// <example>
        /// GET: api/Student/FindStudent/2 -> {"studentId":2,"studentFirstName":"Jennifer","studentLastName":"Faulkner","studentNumber":"N1679","enrolDate":"2018-08-02T00:00:00"}
        /// </example>
        /// <returns>
        /// A matching student object by its ID. Empty object if student not found
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


        [HttpPost(template:"AddStudent")]
        public int AddStudent([FromBody] Student StudentData)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "insert into students (studentfname, studentlname, studentnumber, enroldate) values (@studentfname, @studentlname, @studentnumber, @enroldate)";
                Command.Parameters.AddWithValue("@studentfname", StudentData.StudentFirstName);
                Command.Parameters.AddWithValue("@studentlname", StudentData.StudentLastName);
                Command.Parameters.AddWithValue("@studentnumber", StudentData.StudentNumber);
                Command.Parameters.AddWithValue("@enroldate", StudentData.EnrolDate);

                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);
            }

            return 0;
        }

        [HttpDelete(template: "DeleteStudent/{StudentId}")]
        public int DeleteStudent(int StudentId)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "delete from students where studentid=@id";
                Command.Parameters.AddWithValue("@id", StudentId);
                return Command.ExecuteNonQuery();
            }
            return 0;
        }

        [HttpPut(template:"UpdateStudent/{StudentId}")]
        public Student UpdateStudent (int StudentId, [FromBody] Student StudentData)
        {
            using MySqlConnection Connection = _context.AccessDatabase();
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "update students set studentfname=@studentfname, studentlname=@studentlname, studentnumber=@studentnumber, enroldate=@enroldate where studentid=@id";
                Command.Parameters.AddWithValue("@studentfname", StudentData.StudentFirstName);
                Command.Parameters.AddWithValue("@studentlname", StudentData.StudentLastName);
                Command.Parameters.AddWithValue("@studentnumber", StudentData.StudentNumber);
                Command.Parameters.AddWithValue("@enroldate", StudentData.EnrolDate);

                Command.Parameters.AddWithValue("@id", StudentId);

                Command.ExecuteNonQuery();

            }
            return FindStudent(StudentId);
            
        }
    }
}
