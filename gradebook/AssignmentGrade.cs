using System;
using System.Collections.Generic;
using System.Text;

namespace gradebook
{
    public class AssignmentGrade
    {
        public double points { get; set; }

        public double maxPoints { get; set; }

        public bool graded { get; set; }

        public AssignmentGrade(Assignment assignment)
        {
            points = 0.0;
            maxPoints = assignment.maxPoints;
            graded = false;
        }

        public AssignmentGrade(AssignmentGrade assignment)
        {
            points = assignment.points;
            maxPoints = assignment.maxPoints;
            graded = assignment.graded;
        }

        // get the grade in percentage
        public double getGrade()
        {
            return points / maxPoints * 100.0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            AssignmentGrade ag = (AssignmentGrade)obj;

            return this.points == ag.points && this.maxPoints == ag.maxPoints && this.graded == ag.graded;
        }

        public static bool operator ==(AssignmentGrade ag1, AssignmentGrade ag2) => ag1.Equals(ag2);

        public static bool operator !=(AssignmentGrade ag1, AssignmentGrade ag2) => !ag1.Equals(ag2);
    }
}
