using System.Net.Http.Json;

namespace StudentApiClient
{
    internal class Program
    {
        static readonly HttpClient httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            httpClient.BaseAddress = new Uri("https://localhost:7247/api/Students/");

            await GetAllStudents();

            await GetPassedStudent();

            await GetAVGGrade();

            await GetStudentByID(5);
            await GetStudentByID(4);
            await GetStudentByID(0);

            Student newStudent = new Student() { Name = "mazen abdullah", Age = 20, Grade = 85};
            await AddStudent(newStudent);

            await GetAllStudents();

            await DeleteStudent(4);
            await GetAllStudents();

            await UpdateStudent(2, new Student { Name = "Salma", Age = 22, Grade = 90 });
            await GetAllStudents();
        }

        static async Task GetAllStudents()
        {
            try
            {
                Console.WriteLine("\n______________________");
                Console.WriteLine("\nFetching all students...\n");
                var response = await httpClient.GetAsync("All");

                if (response.IsSuccessStatusCode)
                {
                    var students = await response.Content.ReadFromJsonAsync<List<Student>>();
                    if (students != null && students.Count > 0)
                    {
                        foreach (var student in students)
                        {
                            Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                        }
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("no students found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error occurred: {ex.Message}");
            }
        }

        static async Task GetPassedStudent()
        {
            try
            {
                Console.WriteLine("\n______________________");
                Console.WriteLine("\nFetching Passed students...\n");
                var response = await httpClient.GetAsync("Passed");

                if (response.IsSuccessStatusCode)
                {
                    var students = await response.Content.ReadFromJsonAsync<List<Student>>();
                    if (students != null)
                    {
                        foreach (var student in students)
                        {
                            Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                        }
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("no Passed students found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error occurred: {ex.Message}");
            }
        }

        static async Task GetAVGGrade()
        {
            try
            {
                Console.WriteLine("\n______________________");
                Console.WriteLine("\nGetting avg Grade...\n");
                var response =await httpClient.GetAsync("Avg");

                if (response.IsSuccessStatusCode)
                {
                    double averageGrade = await response.Content.ReadFromJsonAsync<double>();
                    Console.WriteLine($"Average Grade is: {averageGrade}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("no students found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error occurred: {ex.Message}");
            }
        }

        static async Task GetStudentByID(int id)
        {
            try
            {
                Console.WriteLine("\n______________________");
                Console.WriteLine($"\nGetting Student of id {id}...\n");

                var response = await httpClient.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    var student = await response.Content.ReadFromJsonAsync<Student>();
                    if (student != null)
                    {
                        Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request: not accepted ID {id}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"not found: student with ID {id} not found.");
                }
            } 
            catch (Exception ex)
            {

                Console.WriteLine($"An Error occurred: {ex.Message}");
            }
        }

        static async Task AddStudent(Student newStudent)
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine("\nAdding a new student...\n");

                var response = await httpClient.PostAsJsonAsync("", newStudent);

                if (response.IsSuccessStatusCode)
                {
                    var addedStudent = await response.Content.ReadFromJsonAsync<Student>();
                    Console.WriteLine($"Added Student - ID: {addedStudent.Id}, Name: {addedStudent.Name}, Age: {addedStudent.Age}, Grade: {addedStudent.Grade}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine("Bad Request: Invalid student data.");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task DeleteStudent(int id)
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine($"\nDeleting student with ID {id}...\n");
                var response = await httpClient.DeleteAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Student with ID {id} has been deleted.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request: Not accepted ID {id}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Not Found: Student with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task UpdateStudent(int id, Student updatedStudent)
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine($"\nUpdating student with ID {id}...\n");
                var response = await httpClient.PutAsJsonAsync($"{id}", updatedStudent);

                if (response.IsSuccessStatusCode)
                {
                    var student = await response.Content.ReadFromJsonAsync<Student>();
                    Console.WriteLine($"Updated Student: ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine("Failed to update student: Invalid data.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Student with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }
    }
}
