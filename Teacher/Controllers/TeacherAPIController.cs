using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using Teacher.Models;
using System;

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

        //static List<ATeacher> ?Teachers;


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
        public List<ATeacher> ListTeachers(string SearchKey = null)
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
            //bool teacherFound = false;

            //for(int i = 0; i < Teachers.Count; i++)
            //{
            //    //if (t.TeacherId != id)
            //    if (Teachers[i].TeacherId != id)
            //    {
            //       Console.WriteLine("Teacher does not exist");

            //        //Console.WriteLine(t.TeacherId);
            //        //Console.WriteLine(t);
            //        teacherFound = false;

            //    }
            //    else
            //    {
            //        Console.WriteLine("Teacher does exist");
            //        teacherFound = true;
            //        break;
            //    }

            //}


            //if (teacherFound)
            //{
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

            //}
            //    else
            //    {
            //        //Teacher does not exist logic 
            //        return null;

            //    }
        }




        /// <summary>
        /// Adds a teacher to the database
        /// </summary>
        /// <param name="TeacherData">Teacher Object</param>
        /// <example>
        /// POST: api/Teacher/AddTeacher
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///	    "TeacherFirstName":"John",
        ///	    "TeacherLastName":"Doe",
        ///	    "EmployeeNumber":"000",
        ///	    "HireDate":"2007",
        ///	    "Salary":"50"
        /// } -> 11
        /// </example>
        /// <returns>
        /// The inserted Teacher Id from the database if successful. 0 if Unsuccessful
        /// </returns>
        [HttpPost(template: "AddTeacher")]
        public int AddTeacher([FromBody] ATeacher TeacherData)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@teacherfname, @teacherlname, @employeenumber, @hiredate, @salary)";
                Command.Parameters.AddWithValue("@teacherfname", TeacherData.TeacherFirstName);
                Command.Parameters.AddWithValue("@teacherlname", TeacherData.TeacherLastName);
                Command.Parameters.AddWithValue("@employeenumber", TeacherData.EmployeeNumber);
                Command.Parameters.AddWithValue("@hiredate", TeacherData.HireDate);
                Command.Parameters.AddWithValue("@salary", TeacherData.Salary);

                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);
            }
            // if failure
            return 0;
        }



        /// <summary>
        /// Deletes a teacher from the database
        /// </summary>
        /// <param name="TeacherId">Primary key of the teacher to delete</param>
        /// <example>
        /// DELETE: api/Teacher/DeleteTeacher -> 1
        /// </example>
        /// <returns>
        /// Numbers of rows affected by delete operation
        /// </returns>
        [HttpDelete(template:"DeleteTeacher/{TeacherId}")]
        public int DeleteTeacher(int TeacherId)
        {
            //foreach(ATeacher t in Teachers)
            //{
            //    if (t.TeacherId == TeacherId)
            //    {
                    using (MySqlConnection Connection = _context.AccessDatabase())
                    {
                        Connection.Open();
                        MySqlCommand Command = Connection.CreateCommand();

                        Command.CommandText = "delete from teachers where teacherid=@id";
                        Command.Parameters.AddWithValue("@id", TeacherId);
                        return Command.ExecuteNonQuery();
                    } 
                    
                //} 
            //}
            // if failure
            return 0;
        }
    }
}

