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

        public Assignment(Assignment a)
        {
            name = a.name;
            type = a.type;
            maxPoints = a.maxPoints;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Assignment a = (Assignment)obj;
            return this.name == a.name && this.type == a.type && this.maxPoints == a.maxPoints;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode() ^ type.GetHashCode() ^ maxPoints.GetHashCode();
        }

        public static bool operator ==(Assignment a1, Assignment a2) => a1.Equals(a2);

        public static bool operator !=(Assignment a1, Assignment a2) => !a1.Equals(a2);
    }
}
