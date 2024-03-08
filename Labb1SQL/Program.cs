using Labb1SQL.Data;
using Labb1SQL.Models;
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
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SkolaDB;Integrated Security=True";
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
                        using SqlDataReader reader = command.ExecuteReader(){

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


        }
    }
}