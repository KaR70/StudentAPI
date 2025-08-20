using StudentAPIDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAPIBusinessLayer
{
    public class Student
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public StudentDTO SDTO
        {
            get { return (new StudentDTO(ID, Name, Age, Grade)); }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }

        public Student(StudentDTO SDTO, enMode cMode = enMode.AddNew)
        {
            ID = SDTO.Id;
            Name = SDTO.Name;
            Age = SDTO.Age;
            Grade = SDTO.Grade;

            Mode = cMode;
        }

        public static List<StudentDTO> GetAllStudents()
        {
            return StudentData.GetAllStudents();
        }

        public static List<StudentDTO> GetPassedStudents()
        {
            return StudentData.GetPassedStudents();
        }

        public static double GetAVGGrade()
        {
            return StudentData.GetAVGGrade();
        }

        public static Student Find(int ID)
        {
            StudentDTO SDTO = StudentData.GetStudentById(ID);

            if (SDTO != null)
            {
                return new Student(SDTO, enMode.Update);
            }
            else
            {
                return null;
            }
        }

        public static bool DeleteStudent(int id)
        {
            return StudentData.DeleteStudent(id);
        }

        private bool _AddNewStudent()
        {
            ID = StudentData.AddStudent(SDTO);
            return ID != -1;
        }

        private bool _UpdateStudent()
        {
            return StudentData.UpdateStudent(SDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewStudent())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateStudent();
            }

            return false;
        }
    }
}
