using System;
using System.Collections.Generic;
using System.Text;

namespace gradebook
{
    public class Grade
    {
        public double points { get; set; }

        public double maxPoints { get; set; }

        public bool graded { get; set; }

        public Grade(double maxPoints)
        {
            points = 0.0;
            this.maxPoints = maxPoints;
            graded = false;
        }

        public Grade(Grade assignment)
        {
            points = assignment.points;
            maxPoints = assignment.maxPoints;
            graded = assignment.graded;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Grade ag = (Grade)obj;

            return this.points == ag.points && this.maxPoints == ag.maxPoints && this.graded == ag.graded;
        }

        public override int GetHashCode()
        {
            return points.GetHashCode() ^ maxPoints.GetHashCode() ^ graded.GetHashCode();
        }

        public static bool operator ==(Grade ag1, Grade ag2) => ag1.Equals(ag2);

        public static bool operator !=(Grade ag1, Grade ag2) => !ag1.Equals(ag2);

        // get the grade in percentage
        public double getGrade()
        {
            return points / maxPoints * 100.0;
        }
    }
}
