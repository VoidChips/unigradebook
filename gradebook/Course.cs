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

        // copy constructor
        // deep copy
        public Course(Course c)
        {
            id = c.id;
            name = c.name;
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
                id != c.id ||
                name != c.name ||
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
