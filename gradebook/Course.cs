﻿using System;
using System.Collections.Generic;
using System.Text;

namespace gradebook
{
    public class Course
    {
        public string name { get; set; }  // e.g. CS101

        public string title { get; set; }  // e.g. Intro to Programming

        public string section { get; set; }  // e.g. 001

        // describes how much each assignment type is worth in percentages out of the total grade
        public Dictionary<string, double> assignmentTypeWeights { get; }

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

            assignmentTypeWeights = new Dictionary<string, double>();
            // default percentages
            assignmentTypeWeights.Add("attendance", 5.0);
            assignmentTypeWeights.Add("homework", 35.0);
            assignmentTypeWeights.Add("discussion", 0.0);
            assignmentTypeWeights.Add("quiz", 10.0);
            assignmentTypeWeights.Add("project", 0.0);
            assignmentTypeWeights.Add("midterm", 30.0);
            assignmentTypeWeights.Add("final", 20.0);

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

        // set how much each assignment types are worth out of the total grade in percentages
        // the values must add up to 100%, return false if not
        public bool setAssignmentTypeWeights(double attendance,
                                             double homework,
                                             double discussion,
                                             double quiz,
                                             double project,
                                             double midterm,
                                             double final)
        {
            if (attendance + homework + discussion + quiz + project + midterm + final == 100.0)
            {
                assignmentTypeWeights["attendance"] = attendance;
                assignmentTypeWeights["homework"] = homework;
                assignmentTypeWeights["discussion"] = discussion;
                assignmentTypeWeights["quiz"] = quiz;
                assignmentTypeWeights["project"] = project;
                assignmentTypeWeights["midterm"] = midterm;
                assignmentTypeWeights["final"] = final;
                return true;
            }

            return false;
        }

        // get the unweighted grade of student
        // if soFar is true, get the grade of graded assignments only
        // e.g. If the class has 10% of assignments out and the student gets 100% in all of them,
        // soFar == true will result in a grade of 100%, while soFar == false will result
        // in a grade of 10%.
        public Grade getUnWeightedStudentGrade(Student s, bool soFar)
        {
            Grade studentGrade = new Grade(0.0);

            foreach ((Assignment assignment, Grade grade) in students[s])
            {
                if ((soFar && grade.graded) || !soFar)
                {
                    studentGrade.points += grade.points;
                    if (assignment.type != Assignment.Type.Bonus)
                    {
                        studentGrade.maxPoints += grade.maxPoints;
                    }
                }
            }

            studentGrade.graded = true;
            return studentGrade;
        }

        // get the weighted grade of student
        // if soFar is true, get the grade of graded assignments only
        // e.g. If the class has 10% of assignments out and the student gets 100% in all of them,
        // soFar == true will result in a grade of 100%, while soFar == false will result
        // in a grade of 10%.
        public Grade getWeightedStudentGrade(Student s, bool soFar)
        {
            Grade studentGrade = new Grade(100.0);
            List<Grade> attendanceGrades = new List<Grade>();
            List<Grade> homeworkGrades = new List<Grade>();
            List<Grade> discussionGrades = new List<Grade>();
            List<Grade> quizGrades = new List<Grade>();
            List<Grade> projectGrades = new List<Grade>();
            List<Grade> midtermGrades = new List<Grade>();
            List<Grade> finalGrades = new List<Grade>();
            Grade attendanceGrade = new Grade(0.0);
            Grade homeworkGrade = new Grade(0.0);
            Grade discussionGrade = new Grade(0.0);
            Grade quizGrade = new Grade(0.0);
            Grade projectGrade = new Grade(0.0);
            Grade midtermGrade = new Grade(0.0);
            Grade finalGrade = new Grade(0.0);

            foreach ((Assignment assignment, Grade grade) in students[s])
            {
                if ((soFar && grade.graded) || !soFar)
                {
                    switch(assignment.type)
                    {
                        case Assignment.Type.Attendance:
                            attendanceGrades.Add(grade);
                            break;
                        case Assignment.Type.Homework:
                            homeworkGrades.Add(grade);
                            break;
                        case Assignment.Type.Discussion:
                            discussionGrades.Add(grade);
                            break;
                        case Assignment.Type.Quiz:
                            quizGrades.Add(grade);
                            break;
                        case Assignment.Type.Project:
                            projectGrades.Add(grade);
                            break;
                        case Assignment.Type.Midterm:
                            midtermGrades.Add(grade);
                            break;
                        case Assignment.Type.Final:
                            finalGrades.Add(grade);
                            break;
                        case Assignment.Type.Bonus:
                            studentGrade.points += grade.points;
                            break;
                        default:
                            break;
                    }
                }
            }

            
            foreach(Grade grade in attendanceGrades)
            {
                attendanceGrade.points += grade.points;
                attendanceGrade.maxPoints += grade.maxPoints;
            }
            foreach(Grade grade in homeworkGrades)
            {
                homeworkGrade.points += grade.points;
                homeworkGrade.maxPoints += grade.maxPoints;
            }
            foreach(Grade grade in discussionGrades)
            {
                discussionGrade.points += grade.points;
                discussionGrade.maxPoints += grade.maxPoints;
            }
            foreach(Grade grade in quizGrades)
            {
                quizGrade.points += grade.points;
                quizGrade.maxPoints += grade.maxPoints;
            }
            foreach(Grade grade in projectGrades)
            {
                projectGrade.points += grade.points;
                projectGrade.maxPoints += grade.maxPoints;
            }
            foreach(Grade grade in midtermGrades)
            {
                midtermGrade.points += grade.points;
                midtermGrade.maxPoints += grade.maxPoints;
            }
            foreach(Grade grade in finalGrades)
            {
                finalGrade.points += grade.points;
                finalGrade.maxPoints += grade.maxPoints;
            }

            studentGrade.points += attendanceGrade.getGrade() * assignmentTypeWeights["attendance"];
            studentGrade.points += homeworkGrade.getGrade() * assignmentTypeWeights["homework"];
            studentGrade.points += discussionGrade.getGrade() * assignmentTypeWeights["discussion"];
            studentGrade.points += quizGrade.getGrade() * assignmentTypeWeights["quiz"];
            studentGrade.points += projectGrade.getGrade() * assignmentTypeWeights["project"];
            studentGrade.points += midtermGrade.getGrade() * assignmentTypeWeights["midterm"];
            studentGrade.points += finalGrade.getGrade() * assignmentTypeWeights["final"];

            studentGrade.graded = true;
            return studentGrade;
        }
    }
}
