using System;
using System.Collections.Generic;
using System.Text;

namespace gradebook
{
    public class Course
    {
        public int id { get; set; }

        public string name { get; set; }

        public string section { get; set; }

        public Dictionary<Student, Dictionary<Assignment, Grade>> students { get; set; }

        public List<Assignment> assignments { get; set; }

        public Course(int id, string name)
        {
            this.id = id;
            this.name = name;
            this.students = new Dictionary<Student, Dictionary<Assignment, Grade>>();
            this.assignments = new List<Assignment>();
        }


        public void addStudent(Student s)
        {
            students.Add(s, new Dictionary<Assignment, Grade>());
        }

        public void removeStudent(Student s)
        {
            students.Remove(s);
        }

        public void addAssignment(Assignment a)
        {
            assignments.Add(a);

            // add the assignment for all students
            foreach(Student student in students.Keys)
            {
                students[student].Add(a, new Grade(a.maxPoints));
            }
        }

        public void removeAssigment(Assignment a)
        {
            assignments.Remove(a);

            // remove the assignment for all students
            foreach(Student student in students.Keys)
            {
                students[student].Remove(a);
            }
        }

        // get the grade for an assignment for a specific student
        public Grade getAssignmentGrade(Student s, Assignment a)
        {
            return students[s][a];
        }

        // set the grade for an assignment for a specific student
        public void setAssignmentGrade(Student s, Assignment a, double points)
        {
            students[s][a].points = points;
            students[s][a].graded = true;
        }
    }
}
