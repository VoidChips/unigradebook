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

    }
}
