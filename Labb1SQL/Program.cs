using Microsoft.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Labb1SQL
{
    internal class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Welcome to SchoolDB application!");
                Console.WriteLine("Choose the following:");
                Console.WriteLine("1. View all students");
                Console.WriteLine("2. View all students from specific class");
                Console.WriteLine("3. Add new staff");
                Console.WriteLine("4. View staff");
                Console.WriteLine("5. View grades set from the past month");
                Console.WriteLine("6. Average grade per course");
                Console.WriteLine("7. Add new student");
                Console.WriteLine("8. Exit application");

                Console.WriteLine("Choose your option: ");
                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        ViewStudents();
                        break;
                    case 2:
                        ViewStudentsInClass();
                        break;
                    case 3:
                        AddStaff();
                        break;
                    case 4:
                        ViewStaff();
                        break;
                    case 5:
                        ViewGradesLastMonth();
                        break;
                    case 6:
                        ViewAverageGrades();
                        break;
                    case 7:
                        AddNewStudent();
                        break;
                    case 8:
                        Environment.Exit(0);
                        break;
                    default
                        : Console.WriteLine("Invalid choice. Try again!");
                        break;
                }

                Console.WriteLine("\n Press Enter to return to main menu.");
                Console.ReadLine();
                Console.Clear();
            }

            static void ViewStudents()
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Labb1SQL;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    Console.Clear();
                    Console.WriteLine("Choose order:");
                    Console.WriteLine("1. Sort by ascending first name");
                    Console.WriteLine("2. Sort by descending first name");
                    Console.WriteLine("3. Sort by ascending last name");
                    Console.WriteLine("4. Sort by descending last name");

                    Console.WriteLine("Enter choice");
                    int sortChoice = int.Parse (Console.ReadLine());

                    string sortBy = "";
                    string sortOrder = "";

                    switch (sortChoice)
                    {
                        case 1:
                            sortBy = "FirstName";
                            sortOrder = "ASC";
                            break;
                        case 2:
                            sortBy = "FirstName";
                            sortOrder = "DESC";
                            break;
                        case 3:
                            sortBy = "LastName";
                            sortOrder = "ASC";
                            break;
                        case 4:
                            sortBy = "LastName";
                            sortOrder = "DESC";
                            break;
                        default:
                            Console.WriteLine("Wrong choice. Try again");
                            break;
                    }

                    string studentQuery = $"SELECT * FROM Students ORDER by {sortBy} {sortOrder}";

                    using (SqlCommand command = new SqlCommand(studentQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader()){

                            Console.Clear();
                            Console.WriteLine("List of all students:\n");
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["StudentID"]}, {reader["FirstName"]}, {reader["LastName"]}");
                            }
                        }
                    }
                }
            }

            static void ViewStudentsInClass()
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Labb1SQL;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    Console.Clear();
                    Console.WriteLine("List of all classes:");
                    string classQuery = "SELECT * FROM Classes";

                    using (SqlCommand classCommand = new SqlCommand(classQuery, connection))
                    {
                        using (SqlDataReader classReader = classCommand.ExecuteReader())
                        {
                            while (classReader.Read())
                            {
                                Console.WriteLine($"{classReader["ClassID"]}, {classReader["ClassName"]}");
                            }
                        }
                    }

                    Console.Write("Enter ClassID to see students in that class: ");
                    string classID = Console.ReadLine();

                    string studentQuery = "SELECT FirstName, LastName, CLasses.ClassName FROM Students " +
                                          "JOIN Classes ON Classes.ClassID = Students.ClassID " +
                                          "WHERE Classes.ClassID = @ClassID";
                    
                    using (SqlCommand studentCommand = new SqlCommand(studentQuery, connection))
                    {
                        studentCommand.Parameters.AddWithValue("@ClassID", classID);

                        using (SqlDataReader studentReader = studentCommand.ExecuteReader())
                        {
                            Console.WriteLine($"List of student from class {classID}");
                            while (studentReader.Read())
                            {
                                Console.WriteLine($"{studentReader["FirstName"]}, {studentReader["LastName"]}");
                            }
                        }
                    }
                }
            }

            static void AddStaff()
            {
                Console.Clear();
                Console.WriteLine("Enter staff details:");
                Console.Write("Enter last name: ");
                string staffLastName = Console.ReadLine();

                Console.Write("Enter role (Teacher, Janitor, Principal etc.): ");
                string staffEmployment = Console.ReadLine();

                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Labb1SQL;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO SchoolStaff (LastName, Employemnt) VALUES (@LastName, @Employment)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@LastName", staffLastName);
                        command.Parameters.AddWithValue("@Employment", staffEmployment);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                Console.WriteLine("New staff member added!");
                            }

                            else
                            {
                                Console.WriteLine("Failed to add staff member, try again!");
                            }
                        }
                        catch (Exception ex) {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }

            static void ViewStaff()
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Labb1SQL;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    Console.Clear();
                    Console.WriteLine("1. View staff.");
                    Console.WriteLine("2. View staff by role.");

                    Console.Write("Enter choice: ");
                    int choice = int.Parse(Console.ReadLine());

                    if (choice == 1)
                    {
                        ViewAllStaff(connection);
                    }
                    else if (choice == 2)
                    {
                        Console.WriteLine("Teacher");
                        Console.WriteLine("Administrator");
                        Console.WriteLine("Principal");
                        Console.WriteLine("Janitor");
                        Console.Write("Enter role or leave blank to view all staff: ");
                        string selectedEmployment = Console.ReadLine();

                        ViewStaffByRole(connection, selectedEmployment);
                    }
                    else
                    {
                        Console.WriteLine("Wrong choice");
                    }
                }

                static void ViewAllStaff(SqlConnection connection)
                {
                    string query = "SELECT * FROM SchoolStaff";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("List of all staff: ");
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["StaffID"]}, {reader["LastName"]}, {reader["Employment"]}");
                            }
                        }
                    }
                }

                static void ViewStaffByRole(SqlConnection connection, string selectedEmployment)
                {
                    Console.Clear();

                    string query = "SELECT FROM SchoolStaff WHERE Employment = @Employment";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Employment", selectedEmployment);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine($"List of staff with role {selectedEmployment}");
                            while (reader.Read())
                            {
                                Console.WriteLine();
                                Console.WriteLine($"{reader["StaffID"]}, {reader["LastName"]}, {reader["Employment"]}");
                            }
                        }
                    }
                }
            }

            static void ViewGradesLastMonth()
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Labb1SQL;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    DateTime lastMonth = DateTime.Now.AddMonths(-1);

                    string query = $"SELECT Students.FirstName + Students.LastName AS StudentName, Courses.CourseName, Grades.Grade " +
                                   $"FROM Grades " +
                                   $"INNER JOIN Students ON Grades.StudentID = Students.StudentID " +
                                   $"INNER JOIN Courses ON Grades.CourseID = Courses.CourseID" +
                                   $"WHERE Grades.GradeDate >= @LastMonth";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LastMonth", lastMonth);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.Clear();
                            Console.WriteLine($"List of grades set in the last month");
                            Console.WriteLine("Student name, Course name, Grade");
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["StudentName"]}, {reader["CourseName"]}, {reader["Grade"]}");
                            }
                        }
                    }
                }
            }

            static void ViewAverageGrades()
            {
                Console.Clear();
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Labb1SQL;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT " +
                                   "Courses.CourseID, " +
                                   "Courses.CourseName, " +
                                   "AVG(CAST(Grades.Grade AS FLOAT)) AS AverageGrade, " +
                                   "MAX(Grades.Grade) AS HighestGrade, " +
                                   "MIN(Grades.Grade) AS LowestGrade, " +
                                   "FROM Courses " +
                                   "LEFT JOIN Grades ON Courses.CourseID = Grades.CourseID " +
                                   "GROUP BY Courses.CourseID, Courses.CourseName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("CourseID, CourseName, AverageGrade, HighestGrade, LowestGrade");
                            while (reader.Read())
                            {
                                Console.WriteLine();
                                Console.WriteLine($"{reader["CourseID"]}, {reader["CourseName"]}, {reader["AverageGrade"]}, {reader["HighestGrade"]}, {reader["LowestGrade"]}");
                            }
                        }
                    }
                }
            }

            static void AddNewStudent()
            {
                Console.Clear();
                Console.WriteLine("Enter student details: ");

                Console.Write("First name: ");
                string studentFirstName = Console.ReadLine();

                Console.Write("Last name: ");
                string studentLastName = Console.ReadLine();

                Console.Write("Age: ");
                int studentAge = int.Parse(Console.ReadLine());

                Console.Write("Class ID: ");
                int classID = int.Parse(Console.ReadLine());

                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Labb1SQL;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Students (FirstName, LastName, Age, ClassID) " +
                                         "VALUES (@FirstName, @LastName, @Age, @ClassID";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", studentFirstName);
                        command.Parameters.AddWithValue("@LastName", studentLastName);
                        command.Parameters.AddWithValue("@Age", studentAge);
                        command.Parameters.AddWithValue("@ClassID", classID);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                Console.WriteLine("Student added!");
                            }
                            else
                            {
                                Console.WriteLine("Failed to add new student, try again!");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }

            }
        }
    }
}