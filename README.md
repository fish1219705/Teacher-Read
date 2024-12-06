# ASP.NET Core School 

- Program.cs
  - Configuration of the application
- Models/SchoolDbContext.cs
  - A class which connects to MySQL database


- Controllers/TeacherAPIController.cs
  - A WebAPI Controller which allows to access information about teachers
- Controllers/TeacherPageController.cs
  -  An MVC Controller which allows to route to dynamic pages
- Models/Teacher.cs
  - A Model which allows to represent information about a teacher
- /Views/TeacherPage/List.cshtml
  - A View which uses server rendering to display a list of teachers from the MySQL Database
- /Views/TeacherPage/Show.cshtml
  - A View which uses server rendering to display a teacher from the MySQL Database
- /Views/TeacherPage/New.cshtml
  - A View which uses server rendering to display a page that allows for a user to enter a new teacher
- /Views/TeacherPage/DeleteConfirm.cshtml
  - A View which uses server rendering to display a “Confirm Delete” page for a teacher from the MySQL Database
- /Views/TeacherPage/Edit.cshtml
  - A View which uses server rendering to display a page that allows for a user to Update a teacher
    
 
- Controllers/StudentAPIController.cs
  - A WebAPI Controller which allows to access information about students
- Controllers/StudentPageController.cs
  -  An MVC Controller which allows to route to dynamic pages
- Models/Student.cs
  - A Model which allows to represent information about a student
- /Views/StudentPage/List.cshtml
  - A View which uses server rendering to display a list of students from the MySQL Database
- /Views/StudentPage/Show.cshtml
  - A View which uses server rendering to display a student from the MySQL Database
- /Views/StudentPage/New.cshtml
  - A View which uses server rendering to display a page that allows for a user to enter a new student
- /Views/StudentPage/DeleteConfirm.cshtml
  - A View which uses server rendering to display a “Confirm Delete” page for a student from the MySQL Database
- /Views/StudentPage/Edit.cshtml
  - A View which uses server rendering to display a page that allows for a user to Update a student


- Controllers/CourseAPIController.cs
  - A WebAPI Controller which allows to access information about courses
- Controllers/CoursePageController.cs
  -  An MVC Controller which allows to route to dynamic pages
- Models/Course.cs
  - A Model which allows to represent information about a course
- /Views/CoursePage/List.cshtml
  - A View which uses server rendering to display a list of courses from the MySQL Database
- /Views/CoursePage/Show.cshtml
  - A View which uses server rendering to display a course from the MySQL Database
- /Views/CoursePage/New.cshtml
  - A View which uses server rendering to display a page that allows for a user to enter a new course
- /Views/CoursePage/DeleteConfirm.cshtml
  - A View which uses server rendering to display a “Confirm Delete” page for a course from the MySQL Database
- /Views/CoursePage/Edit.cshtml
  - A View which uses server rendering to display a page that allows for a user to Update a course
