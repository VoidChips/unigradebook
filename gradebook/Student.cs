using System;
using System.Collections.Generic;
using System.Text;

namespace gradebook
{
    public class Student
    {
        public int id { get; }

        public string name { get; }

        public Student(int id, String name)
        {
            this.id = id;
            this.name = name;
        }

        public Student(Student s)
        {
            id = s.id;
            name = s.name;
        }

        // Student objects with the same student ids are equal
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Student student = (Student)obj;
            return this.id == student.id;
        }

        public override int GetHashCode()
        {
            return id ^ name.GetHashCode();
        }

        public static bool operator ==(Student s1, Student s2) => s1.Equals(s2);

        public static bool operator !=(Student s1, Student s2) => !s1.Equals(s2);
    }
}
