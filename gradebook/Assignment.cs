using System;
using System.Collections.Generic;
using System.Text;

namespace gradebook
{
    public class Assignment
    {
        public enum Type
        {
            Attendance,
            Homework,
            Quiz,
            Test,
            Project
        }

        public string name { get; set; }

        public Type type { get; set; }

        public double maxPoints { get; set; }

        public Assignment(string name, Type type, double maxPoints)
        {
            this.name = name;
            this.type = type;
            this.maxPoints = maxPoints;
        }

    }
}
