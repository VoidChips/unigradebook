using System;
using System.Collections.Generic;
using System.Text;

namespace gradebook
{
    public class Course
    {
        public string name { get; set; }  // e.g. CS101

        public string title { get; set; }  // e.g. Intro to Programming

        public string section { get; set; }  // e.g. 001

        public Dictionary<Student, Dictionary<Assignment, Grade>> students { get; }

        public List<Assignment> assignments { get; }

        public Course(string name, string title, string section)
        {
            this.name = name;
            this.title = title;
            this.section = section;
            this.students = new Dictionary<Student, Dictionary<Assignment, Grade>>();
            this.assignments = new List<Assignment>();
        }

        // copy constructor
        // deep copy
        public Course(Course c)
        {
            name = c.name;
            title = c.title;
            section = c.section;

            students = new Dictionary<Student, Dictionary<Assignment, Grade>>();
            foreach((Student student, Dictionary<Assignment, Grade> ag_dict) in c.students)
            {
                Student s = new Student(student);
                Dictionary<Assignment, Grade> ag_dict_copy = new Dictionary<Assignment, Grade>();
                
                foreach((Assignment assignment, Grade grade) in ag_dict)
                {
                    Assignment a = new Assignment(assignment);
                    Grade g = new Grade(grade);
                    ag_dict_copy.Add(a, g);
                }

                students.Add(s, ag_dict_copy);
            }

            assignments = new List<Assignment>();
            foreach(Assignment assignment in c.assignments)
            {
                Assignment a = new Assignment(assignment);
                assignments.Add(a);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Course c = (Course)obj;
            if (students.Count != c.students.Count || 
                assignments.Count != c.assignments.Count ||
                name != c.name ||
                title != c.title ||
                section != c.section)
            {
                return false;
            }

            // checks for equality for this.students and c.students
            foreach((Student s, Dictionary<Assignment, Grade> ag_dict1) in students)
            {
                if (!c.students.ContainsKey(s))
                {
                    return false;
                }

                Dictionary<Assignment, Grade> ag_dict2 = c.students[s];
                if (ag_dict1.Count != ag_dict2.Count)
                {
                    return false;
                }
                
                foreach((Assignment a1, Grade g1) in ag_dict1)
                {
                    if (!ag_dict1.ContainsKey(a1) || g1 != ag_dict2[a1])
                    {
                        return false;
                    }
                }
            }

            // checks for equality for this.assignments and c.assignments
            for (int i = 0; i < assignments.Count; i++)
            {
                if (assignments[i] != c.assignments[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int studentsHashCode = 0;
            int assignmentsHashCode = 0;

            foreach((Student student, Dictionary<Assignment, Grade> ag_dict) in students)
            {
                studentsHashCode ^= student.GetHashCode();
                foreach((Assignment assignment, Grade grade) in ag_dict)
                {
                    studentsHashCode ^= assignment.GetHashCode() ^ grade.GetHashCode();
                }
            }

            foreach(Assignment assignment in assignments)
            {
                assignmentsHashCode ^= assignment.GetHashCode();
            }

            return name.GetHashCode() ^ title.GetHashCode() ^ section.GetHashCode() ^ studentsHashCode ^ assignmentsHashCode;
        }

        public static bool operator ==(Course c1, Course c2) => c1.Equals(c2);

        public static bool operator !=(Course c1, Course c2) => !c1.Equals(c2);

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
            // check for unique assignment
            if (!assignments.Contains(a))
            {
                assignments.Add(a);
            }

            // add the assignment for all students
            foreach((Student student, Dictionary<Assignment, Grade> ag_dict) in students)
            {
                // check for unique assignment
                if (!ag_dict.ContainsKey(a))
                {
                    students[student].Add(a, new Grade(a.maxPoints));
                }
            }
        }

        public void removeAssignment(Assignment a)
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

        // set the points for an assignment for a specific student
        public void gradeAssignment(Student s, Assignment a, double points)
        {
            students[s][a].points = points;
            students[s][a].graded = true;
        }
    }
}
