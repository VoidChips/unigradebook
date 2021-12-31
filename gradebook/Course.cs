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

        public bool isPlusMinusLetterGrade { get; private set; }  // flag for whether the letter grade uses pluses and minuses

        // describes how much each assignment type is worth in percentages out of the total grade
        public Dictionary<string, double> assignmentTypeWeights { get; }

        public Dictionary<string, double> gradeCutoff { get; }  // cutoff for letter grades

        // mapping of students and their grades on assignments
        public Dictionary<Student, Dictionary<Assignment, Grade>> students { get; }

        public List<Assignment> assignments { get; }  // all assignments currently used in the course

        public Course(string name, string title, string section)
        {
            this.name = name;
            this.title = title;
            this.section = section;
            this.isPlusMinusLetterGrade = false;

            // default percentages
            assignmentTypeWeights = new Dictionary<string, double>();
            assignmentTypeWeights.Add("attendance", 5);
            assignmentTypeWeights.Add("homework", 35);
            assignmentTypeWeights.Add("lab", 0);
            assignmentTypeWeights.Add("discussion", 0);
            assignmentTypeWeights.Add("quiz", 10);
            assignmentTypeWeights.Add("project", 0);
            assignmentTypeWeights.Add("midterm", 30);
            assignmentTypeWeights.Add("final", 20);

            // default cutoff
            gradeCutoff = new Dictionary<string, double>();
            gradeCutoff.Add("A-", -1);
            gradeCutoff.Add("A", 90);
            gradeCutoff.Add("A+", -1);
            gradeCutoff.Add("B-", -1);
            gradeCutoff.Add("B", 80);
            gradeCutoff.Add("B+", -1);
            gradeCutoff.Add("C-", -1);
            gradeCutoff.Add("C", 70);
            gradeCutoff.Add("C+", -1);
            gradeCutoff.Add("D-", -1);
            gradeCutoff.Add("D", 60);
            gradeCutoff.Add("D+", -1);

            students = new Dictionary<Student, Dictionary<Assignment, Grade>>();
            assignments = new List<Assignment>();
        }

        // copy constructor
        // deep copy
        public Course(Course c)
        {
            name = c.name;
            title = c.title;
            section = c.section;
            isPlusMinusLetterGrade = c.isPlusMinusLetterGrade;

            // set percentages
            assignmentTypeWeights = new Dictionary<string, double>();
            assignmentTypeWeights.Add("attendance", c.assignmentTypeWeights["attendance"]);
            assignmentTypeWeights.Add("homework", c.assignmentTypeWeights["homework"]);
            assignmentTypeWeights.Add("lab", c.assignmentTypeWeights["lab"]);
            assignmentTypeWeights.Add("discussion", c.assignmentTypeWeights["discussion"]);
            assignmentTypeWeights.Add("quiz", c.assignmentTypeWeights["quiz"]);
            assignmentTypeWeights.Add("project", c.assignmentTypeWeights["project"]);
            assignmentTypeWeights.Add("midterm", c.assignmentTypeWeights["midterm"]);
            assignmentTypeWeights.Add("final", c.assignmentTypeWeights["final"]);

            // set cutoff
            gradeCutoff = new Dictionary<string, double>();
            gradeCutoff.Add("A-", c.gradeCutoff["A-"]);
            gradeCutoff.Add("A", c.gradeCutoff["A"]);
            gradeCutoff.Add("A+", c.gradeCutoff["A+"]);
            gradeCutoff.Add("B-", c.gradeCutoff["B-"]);
            gradeCutoff.Add("B", c.gradeCutoff["B"]);
            gradeCutoff.Add("B+", c.gradeCutoff["B+"]);
            gradeCutoff.Add("C-", c.gradeCutoff["C-"]);
            gradeCutoff.Add("C", c.gradeCutoff["C"]);
            gradeCutoff.Add("C+", c.gradeCutoff["C+"]);
            gradeCutoff.Add("D-", c.gradeCutoff["D-"]);
            gradeCutoff.Add("D", c.gradeCutoff["D"]);
            gradeCutoff.Add("D+", c.gradeCutoff["D+"]);

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
                assignmentTypeWeights.Count != c.assignmentTypeWeights.Count ||
                gradeCutoff.Count != c.gradeCutoff.Count ||
                name != c.name ||
                title != c.title ||
                section != c.section ||
                isPlusMinusLetterGrade != c.isPlusMinusLetterGrade)
            {
                return false;
            }

            // checks for equality for this.assignmentTypeWeights and c.assignmentTypeWeights
            foreach((String type, double weight) in assignmentTypeWeights)
            {
                if (!c.assignmentTypeWeights.ContainsKey(type) ||
                    c.assignmentTypeWeights[type] != weight)
                {
                    return false;
                }
            }

            // checks for equality for this.gradeCutoff and c.gradeCutoff
            foreach((String letterGrade, double cutoff) in gradeCutoff)
            {
                if (!c.gradeCutoff.ContainsKey(letterGrade) ||
                    c.gradeCutoff[letterGrade] != cutoff)
                {
                    return false;
                }
            }

            // checks for equality for this.students and c.students
            foreach ((Student s, Dictionary<Assignment, Grade> ag_dict1) in students)
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
            int assignmentTypeWeightsHashCode = 0;
            int gradeCutoffHashCode = 0;
            int studentsHashCode = 0;
            int assignmentsHashCode = 0;

            foreach((String type, double weight) in assignmentTypeWeights)
            {
                assignmentTypeWeightsHashCode ^= type.GetHashCode() ^ weight.GetHashCode();
            }

            foreach((String letterGrade, double cutoff) in gradeCutoff)
            {
                gradeCutoffHashCode ^= letterGrade.GetHashCode() ^ cutoff.GetHashCode();
            }

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

            return 
                name.GetHashCode() ^
                title.GetHashCode() ^ 
                section.GetHashCode() ^
                isPlusMinusLetterGrade.GetHashCode() ^
                assignmentTypeWeightsHashCode ^
                gradeCutoffHashCode ^
                studentsHashCode ^ 
                assignmentsHashCode;
        }

        public static bool operator ==(Course c1, Course c2) => c1.Equals(c2);

        public static bool operator !=(Course c1, Course c2) => !c1.Equals(c2);

        public void addStudent(Student s)
        {
            students.Add(s, new Dictionary<Assignment, Grade>());
            // if there are existing assignments when the student is added,
            // assign the assignments to that student
            if (assignments.Count > 0)
            {
                foreach(Assignment a in assignments)
                {
                    students[s].Add(a, new Grade(a.maxPoints));
                }
            }
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
                                             double lab,
                                             double discussion,
                                             double quiz,
                                             double project,
                                             double midterm,
                                             double final)
        {
            if (attendance + homework + lab + discussion + quiz + project + midterm + final == 100.0)
            {
                assignmentTypeWeights["attendance"] = attendance;
                assignmentTypeWeights["homework"] = homework;
                assignmentTypeWeights["lab"] = lab;
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
        // if all assignments are graded, soFar doesn't affect the results
        // e.g. If the class has 10% of grade worth of assignments out and the student gets 100% in all of them,
        // soFar == true will result in a grade of 100%, while soFar == false will result
        // in a grade of 10%.
        // note: Extra credit assignment points add to the percentage,
        // so a score of 1 out 3 points is still +1% to the final student grade.
        // Treat the maxPoints of extra credit as the maximum possible percentage added to the grade,
        // and points as the actual percentage added to the grade.
        public Grade getUnweightedStudentGrade(Student s, bool soFar)
        {
            Grade studentGrade = new Grade(0);
            Grade bonusGrade = new Grade(0);

            if (assignments.Count == 0)
            {
                return studentGrade;
            }

            foreach ((Assignment assignment, Grade grade) in students[s])
            {
                if ((soFar && grade.graded) || !soFar)
                {
                    if (assignment.type != Assignment.Type.Bonus)
                    {
                        studentGrade.points += grade.points;
                        studentGrade.maxPoints += grade.maxPoints;
                    } else
                    {
                        bonusGrade.points += grade.points;
                        bonusGrade.maxPoints += grade.maxPoints;
                    }
                }
            }

            // add bonusGrade.points percent to the grade
            if (studentGrade.points != 0)
            {
                double gradeAfterBonus = studentGrade.getGrade() + bonusGrade.points;
                double ratio = gradeAfterBonus / studentGrade.getGrade();
                studentGrade.points *= ratio;
            } else
            {
                studentGrade.points = bonusGrade.points;
            }

            studentGrade.graded = true;
            return studentGrade;
        }

        // get the weighted grade of student
        // if soFar is true, get the grade of graded assignments only
        // if all assignments are graded, soFar doesn't affect the results
        // e.g. If the class has 10% of grade worth of assignments out and the student gets 100% in all of them,
        // soFar == true will result in a grade of 100%, while soFar == false will result
        // in a grade of 10%.
        // note: Extra credit assignment points add to the percentage,
        // so a score of 1 out 3 points is still +1% to the final student grade.
        // Treat the maxPoints of extra credit as the maximum possible percentage added to the grade,
        // and points as the actual percentage added to the grade.
        public Grade getWeightedStudentGrade(Student s, bool soFar)
        {
            Grade studentGrade = new Grade(100);
            Grade attendanceGrade = new Grade(0);
            Grade homeworkGrade = new Grade(0);
            Grade labGrade = new Grade(0);
            Grade discussionGrade = new Grade(0);
            Grade quizGrade = new Grade(0);
            Grade projectGrade = new Grade(0);
            Grade midtermGrade = new Grade(0);
            Grade finalGrade = new Grade(0);
            Grade bonusGrade = new Grade(1);
            Dictionary<string, (int, int)> assignmentCountByType = new Dictionary<string, (int, int)>();
            assignmentCountByType.Add("attendance", (0, 0));
            assignmentCountByType.Add("homework", (0, 0));
            assignmentCountByType.Add("lab", (0, 0));
            assignmentCountByType.Add("discussion", (0, 0));
            assignmentCountByType.Add("quiz", (0, 0));
            assignmentCountByType.Add("project", (0, 0));
            assignmentCountByType.Add("midterm", (0, 0));
            assignmentCountByType.Add("final", (0, 0));

            if (assignments.Count == 0)
            {
                return new Grade(0);
            }

            foreach ((Assignment assignment, Grade grade) in students[s])
            {
                switch(assignment.type)
                {
                    case Assignment.Type.Attendance:
                        {
                            int gradedCount = assignmentCountByType["attendance"].Item1;
                            int assignmentCount = assignmentCountByType["attendance"].Item2 + 1;
                            if ((soFar && grade.graded) || !soFar)
                            {
                                attendanceGrade.points += grade.points;
                                attendanceGrade.maxPoints += grade.maxPoints;
                            }
                            if (grade.graded)
                            {
                                gradedCount++;
                            }
                            assignmentCountByType["attendance"] = (gradedCount, assignmentCount);
                            break;
                        }
                    case Assignment.Type.Homework:
                        {
                            int gradedCount = assignmentCountByType["homework"].Item1;
                            int assignmentCount = assignmentCountByType["homework"].Item2 + 1;
                            if ((soFar && grade.graded || !soFar))
                            {
                                homeworkGrade.points += grade.points;
                                homeworkGrade.maxPoints += grade.maxPoints;
                            }
                            if (grade.graded)
                            {
                                gradedCount++;
                            }
                            assignmentCountByType["homework"] = (gradedCount, assignmentCount);
                            break;
                        }
                    case Assignment.Type.Lab:
                        {
                            int gradedCount = assignmentCountByType["lab"].Item1;
                            int assignmentCount = assignmentCountByType["lab"].Item2 + 1;
                            if ((soFar && grade.graded || !soFar))
                            {
                                labGrade.points += grade.points;
                                labGrade.maxPoints += grade.points;
                            }
                            if (grade.graded)
                            {
                                gradedCount++;
                            }
                            assignmentCountByType["lab"] = (gradedCount, assignmentCount);
                            break;
                        }
                    case Assignment.Type.Discussion:
                        {
                            int gradedCount = assignmentCountByType["discussion"].Item1;
                            int assignmentCount = assignmentCountByType["discussion"].Item2 + 1;
                            if ((soFar && grade.graded || !soFar))
                            {
                                discussionGrade.points += grade.points;
                                discussionGrade.maxPoints += grade.points;
                            }
                            if (grade.graded)
                            {
                                gradedCount++;
                            }
                            assignmentCountByType["discussion"] = (gradedCount, assignmentCount);
                            break;
                        }
                    case Assignment.Type.Quiz:
                        {
                            int gradedCount = assignmentCountByType["quiz"].Item1;
                            int assignmentCount = assignmentCountByType["quiz"].Item2 + 1;
                            if ((soFar && grade.graded || !soFar))
                            {
                                quizGrade.points += grade.points;
                                quizGrade.maxPoints += grade.maxPoints;
                            }
                            if (grade.graded)
                            {
                                gradedCount++;
                            }
                            assignmentCountByType["quiz"] = (gradedCount, assignmentCount);
                            break;
                        }
                    case Assignment.Type.Project:
                        {
                            int gradedCount = assignmentCountByType["project"].Item1;
                            int assignmentCount = assignmentCountByType["project"].Item2 + 1;
                            if ((soFar && grade.graded || !soFar))
                            {
                                projectGrade.points += grade.points;
                                projectGrade.maxPoints += grade.points;
                            }
                            if (grade.graded)
                            {
                                gradedCount++;
                            }
                            assignmentCountByType["project"] = (gradedCount, assignmentCount);
                            break;
                        }
                    case Assignment.Type.Midterm:
                        {
                            int gradedCount = assignmentCountByType["midterm"].Item1;
                            int assignmentCount = assignmentCountByType["midterm"].Item2 + 1;
                            if ((soFar && grade.graded || !soFar))
                            {
                                midtermGrade.points += grade.points;
                                midtermGrade.maxPoints += grade.maxPoints;
                            }
                            if (grade.graded)
                            {
                                gradedCount++;
                            }
                            assignmentCountByType["midterm"] = (gradedCount, assignmentCount);
                            break;
                        }
                    case Assignment.Type.Final:
                        {
                            int gradedCount = assignmentCountByType["final"].Item1;
                            int assignmentCount = assignmentCountByType["final"].Item2 + 1;
                            if ((soFar && grade.graded || !soFar))
                            {
                                finalGrade.points += grade.points;
                                finalGrade.maxPoints += grade.maxPoints;
                            }
                            if (grade.graded)
                            {
                                gradedCount++;
                            }
                            assignmentCountByType["final"] = (gradedCount, assignmentCount);
                            break;
                        }
                    case Assignment.Type.Bonus:
                        bonusGrade.points += grade.points;
                        break;
                    default:
                        break;
                }
            }

            if (soFar)
            {
                if (assignmentCountByType["attendance"].Item1 != 0)
                {
                    attendanceGrade.points = attendanceGrade.getGrade() * assignmentTypeWeights["attendance"] / 100;
                    attendanceGrade.maxPoints = assignmentTypeWeights["attendance"];
                } else
                {
                    attendanceGrade.points = 0;
                    attendanceGrade.maxPoints = 0;
                }
                if (assignmentCountByType["homework"].Item1 != 0)
                {
                    homeworkGrade.points = homeworkGrade.getGrade() * assignmentTypeWeights["homework"] / 100;
                    homeworkGrade.maxPoints = assignmentTypeWeights["homework"];
                } else
                {
                    homeworkGrade.points = 0;
                    homeworkGrade.maxPoints = 0;
                }
                if (assignmentCountByType["lab"].Item1 != 0)
                {
                    labGrade.points = labGrade.getGrade() * assignmentTypeWeights["lab"] / 100;
                    labGrade.maxPoints = assignmentTypeWeights["lab"];
                } else
                {
                    labGrade.points = 0;
                    labGrade.maxPoints = 0;
                }
                if (assignmentCountByType["discussion"].Item1 != 0)
                {
                    discussionGrade.points = discussionGrade.getGrade() * assignmentTypeWeights["discussion"] / 100;
                    discussionGrade.maxPoints = assignmentTypeWeights["discussion"];
                } else
                {
                    discussionGrade.points = 0;
                    discussionGrade.maxPoints = 0;
                }
                if (assignmentCountByType["quiz"].Item1 != 0)
                {
                    quizGrade.points = quizGrade.getGrade() * assignmentTypeWeights["quiz"] / 100;
                    quizGrade.maxPoints = assignmentTypeWeights["quiz"];
                } else
                {
                    quizGrade.points = 0;
                    quizGrade.maxPoints = 0;
                }
                if (assignmentCountByType["project"].Item1 != 0)
                {
                    projectGrade.points = projectGrade.getGrade() * assignmentTypeWeights["project"] / 100;
                    projectGrade.maxPoints = assignmentTypeWeights["project"];
                } else
                {
                    projectGrade.points = 0;
                    projectGrade.maxPoints = 0;
                }
                if (assignmentCountByType["midterm"].Item1 != 0)
                {
                    midtermGrade.points = midtermGrade.getGrade() * assignmentTypeWeights["midterm"] / 100;
                    midtermGrade.maxPoints = assignmentTypeWeights["midterm"];
                } else
                {
                    midtermGrade.points = 0;
                    midtermGrade.maxPoints = 0;
                }
                if (assignmentCountByType["final"].Item1 != 0)
                {
                    finalGrade.points = finalGrade.getGrade() * assignmentTypeWeights["final"] / 100;
                    finalGrade.maxPoints = assignmentTypeWeights["final"];
                } else
                {
                    finalGrade.points = 0;
                    finalGrade.maxPoints = 0;
                }

                studentGrade.points =
                    attendanceGrade.points +
                    homeworkGrade.points +
                    labGrade.points +
                    discussionGrade.points +
                    quizGrade.points +
                    projectGrade.points +
                    midtermGrade.points +
                    finalGrade.points;

                studentGrade.maxPoints =
                    attendanceGrade.maxPoints +
                    homeworkGrade.maxPoints +
                    labGrade.maxPoints +
                    discussionGrade.maxPoints +
                    quizGrade.maxPoints +
                    projectGrade.maxPoints +
                    midtermGrade.maxPoints +
                    finalGrade.maxPoints;

                // add bonusGrade.points percent to the grade
                double gradeAfterBonus = studentGrade.getGrade() + bonusGrade.points;
                double ratio = gradeAfterBonus / studentGrade.getGrade();
                studentGrade.points *= ratio;
            }
            else
            {
                studentGrade.points += attendanceGrade.getGrade() * assignmentTypeWeights["attendance"] / 100;
                studentGrade.points += homeworkGrade.getGrade() * assignmentTypeWeights["homework"] / 100;
                studentGrade.points += labGrade.getGrade() * assignmentTypeWeights["lab"] / 100;
                studentGrade.points += discussionGrade.getGrade() * assignmentTypeWeights["discussion"] / 100;
                studentGrade.points += quizGrade.getGrade() * assignmentTypeWeights["quiz"] / 100;
                studentGrade.points += projectGrade.getGrade() * assignmentTypeWeights["project"] / 100;
                studentGrade.points += midtermGrade.getGrade() * assignmentTypeWeights["midterm"] / 100;
                studentGrade.points += finalGrade.getGrade() * assignmentTypeWeights["final"] / 100;
                studentGrade.points += bonusGrade.points;
            }

            studentGrade.graded = true;
            return studentGrade;
        }

        // get the class average of the grade of an assignment
        public Grade getAssignmentMean(Assignment a)
        {
            if (students.Count == 0)
            {
                return new Grade(0);
            }

            double sum = 0;
            foreach(Student student in students.Keys)
            {
                sum += getAssignmentGrade(student, a).points;
            }

            Grade avg = new Grade(a.maxPoints);
            avg.points = sum / students.Count;
            avg.graded = true;

            return avg;
        }

        // get the class median of the grade of an assignment
        public Grade getAssignmentMedian(Assignment a)
        {
            if (students.Count == 0)
            {
                return new Grade(0);
            }

            List<Grade> grades = new List<Grade>();
            foreach(Student student in students.Keys)
            {
                grades.Add(getAssignmentGrade(student, a));
            }

            if (students.Count == 1)
            {
                return grades[0];
            }

            // if the numbers of grades are even, return the average of two middle grades
            // if odd, return the middle grade
            grades.Sort();
            Grade median = new Grade(0);
            if (students.Count % 2 == 0)
            {
                median = new Grade(a.maxPoints);
                Grade mid1 = grades[students.Count / 2 - 1];
                Grade mid2 = grades[students.Count / 2];
                median.points = (mid1.points + mid2.points) / 2;
                median.graded = true;
                return median;
            }

            median = grades[students.Count / 2];
            median.graded = true;
            return grades[students.Count / 2];
        }

        // get the unweighted mean grade of the course
        // if soFar is true, exclude the ungraded assignments
        public Grade getUnweightedClassMean(bool soFar)
        {
            if (students.Count == 0)
            {
                return new Grade(0);
            }

            double sum = 0;
            double maxPoints = 0;
            
            foreach(Student student in students.Keys)
            {
                Grade studentGrade = getUnweightedStudentGrade(student, soFar);
                sum += studentGrade.points;
                maxPoints = studentGrade.maxPoints;
            }

            Grade mean = new Grade(maxPoints);
            mean.points = sum / students.Count;
            return mean;
        }

        // get the weighted mean grade of the course
        // if soFar is true, exclude the ungraded assignments
        public Grade getWeightedClassMean(bool soFar)
        {
            if (students.Count == 0)
            {
                return new Grade(0);
            }

            double sum = 0;
            double maxPoints = 0;

            foreach (Student student in students.Keys)
            {
                Grade studentGrade = getWeightedStudentGrade(student, soFar);
                sum += studentGrade.points;
                maxPoints = studentGrade.maxPoints;
            }

            Grade mean = new Grade(maxPoints);
            mean.points = sum / students.Count;
            mean.graded = true;
            return mean;
        }

        // get the median unweighted grade of the class
        // if soFar is true, exclude the ungraded assignments
        public Grade getUnweightedClassMedian(bool soFar)
        {
            if (students.Count == 0)
            {
                return new Grade(0);
            }

            List<Grade> grades = new List<Grade>();
            foreach (Student student in students.Keys)
            {
                grades.Add(getUnweightedStudentGrade(student, soFar));
            }

            if (students.Count == 1)
            {
                return grades[0];
            }

            // if the numbers of grades are even, return the average of two middle grades
            // if odd, return the middle grade
            grades.Sort();
            if (students.Count % 2 == 0)
            {
                Grade median = new Grade(0);
                median = new Grade(grades[0].maxPoints);
                Grade mid1 = grades[students.Count / 2 - 1];
                Grade mid2 = grades[students.Count / 2];
                median.points = (mid1.points + mid2.points) / 2;
                median.graded = true;
                return median;
            }

            return grades[students.Count / 2];
        }

        // get the median weighted grade of the class
        // if soFar is true, exclude the ungraded assignments
        public Grade getWeightedClassMedian(bool soFar)
        {
            if (students.Count == 0)
            {
                return new Grade(0);
            }

            List<Grade> grades = new List<Grade>();
            foreach (Student student in students.Keys)
            {
                grades.Add(getWeightedStudentGrade(student, soFar));
            }

            if (students.Count == 1)
            {
                return grades[0];
            }

            // if the numbers of grades are even, return the average of two middle grades
            // if odd, return the middle grade
            grades.Sort();
            if (students.Count % 2 == 0)
            {
                Grade median = new Grade(0);
                median = new Grade(grades[0].maxPoints);
                Grade mid1 = grades[students.Count / 2 - 1];
                Grade mid2 = grades[students.Count / 2];
                median.points = (mid1.points + mid2.points) / 2;
                median.graded = true;
                return median;
            }

            return grades[students.Count / 2];
        }

        // set the grade cutoff that is used for finding the letter grade
        // e.g. 90.0 for A means that the student's grade must be at least 90.0 to get an A.
        // The cutoff for a letter grade must be a greater than the lower letter grades,
        // greater than 0, and less than 100
        // e.g. A - 90, B - 92 will not work and return false
        public bool setGradeCutoff(double cutoffA, double cutoffB, double cutoffC, double cutoffD)
        {
            if (!isPlusMinusLetterGrade && 
                cutoffA > cutoffB && cutoffB > cutoffC && cutoffC > cutoffD 
                && cutoffD > 0 && cutoffA < 100)
            {
                gradeCutoff["A"] = cutoffA;
                gradeCutoff["B"] = cutoffB;
                gradeCutoff["C"] = cutoffC;
                gradeCutoff["D"] = cutoffD;
                return true;
            }
            return false;
        }

        // set the grade cutoff if using plus and minus system
        public bool setGradeCutoff(
            double cutoffAp, 
            double cutoffA, 
            double cutoffAm, 
            double cutoffBp, 
            double cutoffB, 
            double cutoffBm, 
            double cutoffCp, 
            double cutoffC, 
            double cutoffCm, 
            double cutoffDp,
            double cutoffD,
            double cutoffDm)
        {
            if (isPlusMinusLetterGrade &&
                cutoffAp > cutoffA && 
                cutoffA > cutoffAm && 
                cutoffAm > cutoffBp && 
                cutoffBp > cutoffB && 
                cutoffB > cutoffBm && 
                cutoffBm > cutoffCp && 
                cutoffCp > cutoffC && 
                cutoffC > cutoffCm && 
                cutoffCm > cutoffDp && 
                cutoffDp > cutoffD && 
                cutoffD > cutoffDm && 
                cutoffDm > 0 && 
                cutoffAp < 100)
            {
                gradeCutoff["A+"] = cutoffAp;
                gradeCutoff["A"] = cutoffA;
                gradeCutoff["A-"] = cutoffAm;
                gradeCutoff["B+"] = cutoffBp;
                gradeCutoff["B"] = cutoffB;
                gradeCutoff["B-"] = cutoffBm;
                gradeCutoff["C+"] = cutoffCp;
                gradeCutoff["C"] = cutoffC;
                gradeCutoff["C-"] = cutoffCm;
                gradeCutoff["D+"] = cutoffDp;
                gradeCutoff["D"] = cutoffD;
                gradeCutoff["D-"] = cutoffDm;
                return true;
            }
            return false;
        }

        // set whether the course uses plus minus for the letter grade
        public void useLetterGradePlusMinus(bool isPlusMinus)
        {
            // set to default values
            if (isPlusMinus)
            {
                isPlusMinusLetterGrade = true;
                gradeCutoff["A+"] = 97;
                gradeCutoff["A"] = 93;
                gradeCutoff["A-"] = 90;
                gradeCutoff["B+"] = 87;
                gradeCutoff["B"] = 83;
                gradeCutoff["B-"] = 80;
                gradeCutoff["C+"] = 77;
                gradeCutoff["C"] = 73;
                gradeCutoff["C-"] = 70;
                gradeCutoff["D+"] = 67;
                gradeCutoff["D"] = 63;
                gradeCutoff["D-"] = 60;
            } else
            {
                isPlusMinusLetterGrade = false;
                gradeCutoff["A+"] = -1;
                gradeCutoff["A"] = 90;
                gradeCutoff["A-"] = -1;
                gradeCutoff["B+"] = -1;
                gradeCutoff["B"] = 80;
                gradeCutoff["B-"] = -1;
                gradeCutoff["C+"] = -1;
                gradeCutoff["C"] = 70;
                gradeCutoff["C-"] = -1;
                gradeCutoff["D+"] = -1;
                gradeCutoff["D"] = 60;
                gradeCutoff["D-"] = -1;
            }
        }

        // get the letter grade of the student based on the weighted grade
        // if soFar is true, exclude the ungraded assignments
        // if there are no assignments graded, return "N/A"
        public string getLetterGrade(Student s, bool soFar)
        {
            double grade = getWeightedStudentGrade(s, soFar).getGrade();
            List<string> letterGrades = new List<string>()
            { "A", "B", "C", "D"};

            if (assignments.Count == 0)
            {
                return "N/A";
            }

            bool noAssignmentGraded = true;
            foreach(Grade assignmentGrade in students[s].Values)
            {
                if (assignmentGrade.graded)
                {
                    noAssignmentGraded = false;
                    break;
                }
            }

            if (noAssignmentGraded)
            {
                return "N/A";
            }

            if (isPlusMinusLetterGrade)
            {
                letterGrades = new List<string>()
                { "A+", "A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D+", "D", "D-" };
            }
            
            foreach(string letterGrade in letterGrades)
            {
                if (grade >= gradeCutoff[letterGrade])
                {
                    return letterGrade;
                }
            }
            return "F";
        }
    }
}
