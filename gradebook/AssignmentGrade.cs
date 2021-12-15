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
            this.points = 0.0;
            this.maxPoints = assignment.maxPoints;
            this.graded = false;
        }


        // get the grade in percentage
        public double getGrade()
        {
            return points / maxPoints * 100;
        }
    }
}
