using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApi.DataSimulation;
using StudentApi.Model;
using StudentAPIDataAccessLayer;

namespace StudentApi.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Students")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            #region Getting Data From Memory
            //if (StudentDataSimulation.StudentList.Count == 0)
            //{
            //    return NotFound("no students found");
            //}
            //return Ok(StudentDataSimulation.StudentList); 
            #endregion
            List<StudentDTO> StudentsList = StudentAPIBusinessLayer.Student.GetAllStudents();
            if (StudentsList.Count == 0)
            {
                return NotFound("No students Found");
            }
            return Ok(StudentsList);
        }

        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<Student>> GetPassedStudents()
        {
            #region Getting Data From Memory
            //var passedStudents = StudentDataSimulation.StudentList.Where(student => student.Grade > 50).ToList();
            ////passedStudents.Clear();

            //if (passedStudents.Count() == 0)
            //{
            //    return NotFound("no student found");
            //}

            //return Ok(passedStudents); 
            #endregion
            List<StudentDTO> StudentsList = StudentAPIBusinessLayer.Student.GetPassedStudents();
            if (StudentsList.Count == 0)
            {
                return NotFound("No students Found");
            }
            return Ok(StudentsList);
        }

        [HttpGet("Avg",Name = "GetAVGGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<double> GetAVGGrade()
        {
            #region Getting Data From Memory
            ////StudentDataSimulation.StudentList.Clear();

            //if (StudentDataSimulation.StudentList.Count == 0)
            //{
            //    return NotFound("No students found");
            //}

            //double avg = StudentDataSimulation.StudentList.Average(Student => Student.Grade);
            //return Ok(avg); 
            #endregion
            double AVGGrade = StudentAPIBusinessLayer.Student.GetAVGGrade();
            return Ok(AVGGrade);
        }

        [HttpGet("{id}", Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<Student> GetStudentByID(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not Accepted ID {id}");
            }
            #region Getting Data From Memory
            //var student = StudentDataSimulation.StudentList.FirstOrDefault(s => s.Id == id);
            //if (student == null)
            //{
            //    return NotFound($"Student with ID {id} not Found.");
            //}

            //return Ok(student); 
            #endregion

            StudentAPIBusinessLayer.Student student = StudentAPIBusinessLayer.Student.Find(id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not Found.");
            }

            StudentDTO SDTO = student.SDTO;
            return Ok(SDTO);
        }

        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<Student> AddStudent(StudentDTO newStudentDTO)
        {
            if (newStudentDTO == null || string.IsNullOrEmpty(newStudentDTO.Name) || newStudentDTO.Age < 0 || newStudentDTO.Grade < 0)
            {
                return BadRequest("Invalid student data");
            }

            #region Getting Data from memory
            //newStudent.Id = StudentDataSimulation.StudentList.Count > 0 ? StudentDataSimulation.StudentList.Max(s => s.Id) + 1 : 1;
            //StudentDataSimulation.StudentList.Add(newStudent); 
            #endregion
            StudentAPIBusinessLayer.Student student = new StudentAPIBusinessLayer.Student(new StudentDTO(newStudentDTO.Id, newStudentDTO.Name, newStudentDTO.Age, newStudentDTO.Grade), StudentAPIBusinessLayer.Student.enMode.AddNew);
            student.Save();
            newStudentDTO.Id = student.ID;

            return CreatedAtRoute("GetStudentById", new {id = newStudentDTO.Id}, newStudentDTO);
        }

        [HttpDelete("{id}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteStudent(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            #region Deleting from memory
            //var student = StudentDataSimulation.StudentList.FirstOrDefault(s => s.Id == id);
            //StudentDataSimulation.StudentList.Remove(student); 
            #endregion 
            if (StudentAPIBusinessLayer.Student.DeleteStudent(id))
            {
                return Ok($"student with id {id} has been deleted");
            }
            else
            {
                return NotFound($"student with ID {id} not found");
            }
        }

        [HttpPut("{id}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Student> UpdateStudent(int id, StudentDTO updatedStudent)
        {
            if (id < 1 || UpdateStudent == null || string.IsNullOrEmpty(updatedStudent.Name) || updatedStudent.Age < 0 || updatedStudent.Grade < 0)
            {
                return BadRequest("Invalid student data");
            }

            #region getting setting data in memory
            //var student = StudentDataSimulation.StudentList.FirstOrDefault(s => s.Id == id); 
            #endregion 

            StudentAPIBusinessLayer.Student student = StudentAPIBusinessLayer.Student.Find(id);
            if (student == null)
            {
                return NotFound($"Student with ID: {id} not found");
            }

            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Grade = updatedStudent.Grade;
            
            if (student.Save())
            {
                return Ok(student.SDTO);
            }
            else
            {
                return StatusCode(500, new { message = "Error updating student" });
            }
        }
    }
}
