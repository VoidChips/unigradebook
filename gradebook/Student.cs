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

        // Student objects with the same student ids are equal
        public override bool Equals(object obj)
        {
            Student student = (Student)obj;
            return this.id == student.id;
        }

        public static bool operator ==(Student s1, Student s2) => s1.Equals(s2);

        public static bool operator !=(Student s1, Student s2) => !s1.Equals(s2);
    }
}
