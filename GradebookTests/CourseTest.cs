using NUnit.Framework;
using gradebook;
using System.Collections.Generic;
using System;
using System.IO;

namespace GradebookTests
{
    class CourseTest
    {
        private static Random rand;
        private List<string> names;
        private List<Student> students;
        private List<Student> randomStudents;
        private List<Assignment> assignments;
        private int students_num;

        // returns a random name
        private string generateName()
        {
            return names[rand.Next(students_num)];
        }

        [SetUp]
        public void Setup()
        {
            rand = new Random();
            names = new List<string>();
            students = new List<Student>();
            randomStudents = new List<Student>();
            assignments = new List<Assignment>();

            // get names from the csv file
            string[] lines = File.ReadAllLines("random_names_fossbytes.csv");
            foreach (string line in lines)
            {
                names.Add(line);
            }
            students_num = lines.Length;

            // add students
            for (int i = 0; i < students_num; i++)
            {
                students.Add(new Student(i, names[i]));

                // add random students
                // since the names are random, students can have same names
                randomStudents.Add(new Student(i + 100000, generateName()));
            }

            // add assignments
            for (int i = 0; i < 5; i++)
            {
                assignments.Add(new Assignment("Assignment " + i.ToString(), Assignment.Type.Project, i + 10));
            }
        }

        // tests for correct initialization
        [Test]
        public void Test1()
        {
            Course course = new Course("CS101", "Intro to Programming", "001");
            Assert.AreEqual("CS101", course.name);
            Assert.AreEqual("Intro to Programming", course.title);
            Assert.AreEqual("001", course.section);

            // tests for correct assignment weights by type
            Assert.AreEqual(8, course.assignmentTypeWeights.Count);
            Assert.AreEqual(5, course.assignmentTypeWeights["attendance"]);
            Assert.AreEqual(35, course.assignmentTypeWeights["homework"]);
            Assert.AreEqual(0, course.assignmentTypeWeights["lab"]);
            Assert.AreEqual(0, course.assignmentTypeWeights["discussion"]);
            Assert.AreEqual(10, course.assignmentTypeWeights["quiz"]);
            Assert.AreEqual(0, course.assignmentTypeWeights["project"]);
            Assert.AreEqual(30, course.assignmentTypeWeights["midterm"]);
            Assert.AreEqual(20, course.assignmentTypeWeights["final"]);
        }

        // tests copy constructor
        [Test]
        public void Test2()
        {
            Course course1 = new Course("CS101", "Intro to Programming", "001");
            foreach (Student student in students)
            {
                course1.addStudent(student);
            }
            foreach(Assignment assignment in assignments)
            {
                course1.addAssignment(assignment);
            }

            Course course2 = new Course(course1);
            Assert.AreEqual(course1.name, course2.name);
            Assert.AreEqual(course1.title, course2.title);
            Assert.AreEqual(course1.section, course2.section);
            Assert.AreEqual(course1.students, course2.students);
            Assert.AreEqual(course1.assignments, course2.assignments);

            Course course3 = new Course("CS102", "Intro to Programming 2", "003");
            foreach (Student student in randomStudents)
            {
                course1.addStudent(student);
            }
            foreach (Assignment assignment in assignments)
            {
                course1.addAssignment(assignment);
            }

            Course course4 = new Course(course3);
            Assert.AreEqual(course3.name, course4.name);
            Assert.AreEqual(course3.title, course4.title);
            Assert.AreEqual(course3.section, course4.section);
            Assert.AreEqual(course3.students, course4.students);
            Assert.AreEqual(course3.assignments, course4.assignments);
        }

        // tests equality
        [Test]
        public void Test3()
        {
            Course course1 = new Course("CS101", "Intro to Programming", "001");
            foreach (Student student in students)
            {
                course1.addStudent(student);
            }
            foreach (Assignment assignment in assignments)
            {
                course1.addAssignment(assignment);
            }

            Course course2 = new Course(course1);

            Assert.IsTrue(course1.Equals(course2));
            Assert.IsTrue(course1 == course2);
            Assert.IsFalse(course1 != course2);

            course2.addStudent(new Student(100, "new student"));
            Assert.IsFalse(course1.Equals(course2));
            Assert.IsFalse(course1 == course2);
            Assert.IsTrue(course1 != course2);
        }

        // tests GetHashCode()
        [Test]
        public void Test4()
        {
            Course course1 = new Course("CS101", "Intro to Programming", "001");
            foreach (Student student in students)
            {
                course1.addStudent(student);
            }
            foreach (Assignment assignment in assignments)
            {
                course1.addAssignment(assignment);
            }

            Course course2 = new Course(course1);

            Assert.AreEqual(course1.GetHashCode(), course2.GetHashCode());

            course2.addStudent(new Student(100, "new student"));
            Assert.AreNotEqual(course1.GetHashCode(), course2.GetHashCode());
        }

        // tests addStudent()
        [Test]
        public void Test5()
        {
            Course course = new Course("CS101", "Intro to Programming", "001");
            int oldSize = course.students.Count;
            foreach (Student student in students)
            {
                Assert.IsFalse(course.students.ContainsKey(student));
                course.addStudent(student);
                Assert.IsTrue(course.students.ContainsKey(student));
                Assert.IsTrue(course.students[student].Count == 0);
                Assert.AreEqual(++oldSize, course.students.Count);
                Assert.AreEqual(0, course.students[student].Count);
            }
        }

        // tests removeStudent()
        [Test]
        public void Test6()
        {
            Course course = new Course("CS101", "Intro to Programming", "001");
            foreach (Student student in students)
            {
                course.addStudent(student);
            }

            int oldSize = course.students.Count;
            foreach (Student student in students)
            {
                Assert.IsTrue(course.students.ContainsKey(student));
                Assert.IsTrue(course.students[student].Count == 0);
                Assert.AreEqual(0, course.students[student].Count);
                course.removeStudent(student);
                Assert.IsFalse(course.students.ContainsKey(student));

                if (oldSize != 0)
                {
                    Assert.AreEqual(--oldSize, course.students.Count);
                }
            }

            Assert.AreEqual(0, course.students.Count);
            course.removeStudent(new Student(840238, "student"));  // attempt to remove nonexistent student
            Assert.AreEqual(0, course.students.Count);
        }

        // tests addAssignment()
        [Test]
        public void Test7()
        {
            Course course = new Course("CS101", "Intro to Programming", "001");
            foreach (Student student in students)
            {
                course.addStudent(student);
            }

            int oldSize = course.assignments.Count;
            foreach (Assignment assignment in assignments)
            {
                Assert.IsFalse(course.assignments.Contains(assignment));
                foreach (Student student in course.students.Keys)
                {
                    Assert.IsFalse(course.students[student].ContainsKey(assignment));
                    Assert.AreEqual(oldSize, course.students[student].Count);
                }

                course.addAssignment(assignment);
                Assert.IsTrue(course.assignments.Contains(assignment));
                Assert.AreEqual(++oldSize, course.assignments.Count);
                foreach (Student student in course.students.Keys)
                {
                    Assert.IsTrue(course.students[student].ContainsKey(assignment));
                    Assert.AreEqual(oldSize, course.students[student].Count);
                    Grade grade = course.students[student][assignment];
                    Assert.AreEqual(0, grade.points);
                    Assert.AreEqual(assignment.maxPoints, grade.maxPoints);
                    Assert.AreEqual(false, grade.graded);
                }
            }
        }

        // tests removeAssignment()
        [Test]
        public void Test8()
        {
            Course course = new Course("CS101", "Intro to Programming", "001");
            foreach (Student student in students)
            {
                course.addStudent(student);
            }
            foreach (Assignment assignment in assignments)
            {
                course.addAssignment(assignment);
            }

            int oldSize = course.assignments.Count;
            foreach (Assignment assignment in assignments)
            {
                Assert.IsTrue(course.assignments.Contains(assignment));
                foreach (Student student in course.students.Keys)
                {
                    Assert.IsTrue(course.students[student].ContainsKey(assignment));
                    Assert.AreEqual(oldSize, course.students[student].Count);
                    Grade grade = course.students[student][assignment];
                    Assert.AreEqual(0, grade.points);
                    Assert.AreEqual(assignment.maxPoints, grade.maxPoints);
                    Assert.AreEqual(false, grade.graded);
                }

                course.removeAssignment(assignment);
                Assert.IsFalse(course.assignments.Contains(assignment));
                Assert.AreEqual(--oldSize, course.assignments.Count);
                foreach (Student student in course.students.Keys)
                {
                    Assert.IsFalse(course.students[student].ContainsKey(assignment));
                    Assert.AreEqual(oldSize, course.students[student].Count);
                }
            }
        }

        // tests getAssignmentGrade()
        [Test]
        public void Test9()
        {
            Course course = new Course("CS101", "Intro to Programming", "001");
            foreach (Student student in students)
            {
                course.addStudent(student);
            }
            foreach (Assignment assignment in assignments)
            {
                course.addAssignment(assignment);
            }

            foreach (Student student in course.students.Keys)
            {
                foreach ((Assignment assignment, Grade grade) in course.students[student])
                {
                    Assert.AreEqual(0, grade.points);
                    Assert.AreEqual(assignment.maxPoints, grade.maxPoints);
                    Assert.AreEqual(false, grade.graded);
                    Assert.AreEqual(0, grade.getGrade());
                    Assert.AreEqual(course.students[student][assignment], grade);

                    grade.points = 10;
                    grade.graded = true;
                    Assert.AreEqual(10 / assignment.maxPoints * 100, course.getAssignmentGrade(student, assignment).getGrade());
                    Assert.AreEqual(grade.points / grade.maxPoints * 100, course.getAssignmentGrade(student, assignment).getGrade());
                    Assert.AreEqual(course.students[student][assignment], grade);
                }
            }
        }

        // tests gradeAssignment()
        [Test]
        public void Test10()
        {
            Course course = new Course("CS101", "Intro to Programming", "001");
            foreach (Student student in students)
            {
                course.addStudent(student);
            }
            foreach (Assignment assignment in assignments)
            {
                course.addAssignment(assignment);
            }

            foreach(Student student in course.students.Keys)
            {
                foreach ((Assignment assignment, Grade grade) in course.students[student])
                {
                    Assert.AreEqual(0, grade.points);
                    Assert.AreEqual(assignment.maxPoints, grade.maxPoints);
                    Assert.AreEqual(false, grade.graded);
                    Assert.AreEqual(0, grade.getGrade());
                    Assert.AreEqual(course.students[student][assignment], grade);

                    course.gradeAssignment(student, assignment, 10);
                    Assert.AreEqual(10 / assignment.maxPoints * 100, course.students[student][assignment].getGrade());
                    Assert.AreEqual(grade.points / grade.maxPoints * 100, course.students[student][assignment].getGrade());
                    Assert.AreEqual(course.students[student][assignment], grade);
                    Assert.AreEqual(true, grade.graded);
                }
            }
        }

        // tests setAssignmentWeights()
        [Test]
        public void Test11()
        {
            Course course = new Course("CS101", "Intro to Programming", "002");
            // tests for correct assignment weights by type from initialization
            Assert.AreEqual(8, course.assignmentTypeWeights.Count);
            Assert.AreEqual(5, course.assignmentTypeWeights["attendance"]);
            Assert.AreEqual(35, course.assignmentTypeWeights["homework"]);
            Assert.AreEqual(0, course.assignmentTypeWeights["lab"]);
            Assert.AreEqual(0, course.assignmentTypeWeights["discussion"]);
            Assert.AreEqual(10, course.assignmentTypeWeights["quiz"]);
            Assert.AreEqual(0, course.assignmentTypeWeights["project"]);
            Assert.AreEqual(30, course.assignmentTypeWeights["midterm"]);
            Assert.AreEqual(20, course.assignmentTypeWeights["final"]);

            Assert.AreEqual(true, course.setAssignmentTypeWeights(0, 0, 0, 0, 5, 50, 20, 25));
            // tests for correct assignment weights by type from calling setAssignmentWeights()
            Assert.AreEqual(8, course.assignmentTypeWeights.Count);
            Assert.AreEqual(0.0, course.assignmentTypeWeights["attendance"]);
            Assert.AreEqual(0.0, course.assignmentTypeWeights["homework"]);
            Assert.AreEqual(0.0, course.assignmentTypeWeights["lab"]);
            Assert.AreEqual(0.0, course.assignmentTypeWeights["discussion"]);
            Assert.AreEqual(5.0, course.assignmentTypeWeights["quiz"]);
            Assert.AreEqual(50.0, course.assignmentTypeWeights["project"]);
            Assert.AreEqual(20.0, course.assignmentTypeWeights["midterm"]);
            Assert.AreEqual(25.0, course.assignmentTypeWeights["final"]);

            for (int i = 0; i < 100; i++)
            {
                Course newCourse = new Course("Course " + i.ToString(), "Course " + i.ToString() + " title", i.ToString());
                // tests for correct assignment weights by type from initialization
                Assert.AreEqual(8, newCourse.assignmentTypeWeights.Count);
                Assert.AreEqual(5.0, newCourse.assignmentTypeWeights["attendance"]);
                Assert.AreEqual(35.0, newCourse.assignmentTypeWeights["homework"]);
                Assert.AreEqual(0.0, newCourse.assignmentTypeWeights["lab"]);
                Assert.AreEqual(0.0, newCourse.assignmentTypeWeights["discussion"]);
                Assert.AreEqual(10.0, newCourse.assignmentTypeWeights["quiz"]);
                Assert.AreEqual(0.0, newCourse.assignmentTypeWeights["project"]);
                Assert.AreEqual(30.0, newCourse.assignmentTypeWeights["midterm"]);
                Assert.AreEqual(20.0, newCourse.assignmentTypeWeights["final"]);

                // create a list that adds up to 100
                List<double> sumHundredList = new List<double>();
                double remainder = 100;
                for (int j = 0; j < 8; j++)
                {
                    double x = rand.Next(101);

                    if (j < 7 && remainder - x >= 0)
                    {
                        // add random number from 0.0 to 100.0
                        sumHundredList.Add(x);
                        remainder -= x;
                    } else if (j == 7 && remainder > 0)
                    {
                        // add the remainder for the last element
                        sumHundredList.Add(remainder);
                    } else
                    {
                        // add 0 if the sum from the previous elements is already 100
                        sumHundredList.Add(0);
                    }
                }

                Assert.AreEqual(true, course.setAssignmentTypeWeights(sumHundredList[0],
                                                                      sumHundredList[1],
                                                                      sumHundredList[2],
                                                                      sumHundredList[3],
                                                                      sumHundredList[4],
                                                                      sumHundredList[5],
                                                                      sumHundredList[6],
                                                                      sumHundredList[7]));
                // tests for correct assignment weights by type from calling setAssignmentWeights()
                Assert.AreEqual(8, course.assignmentTypeWeights.Count);
                Assert.AreEqual(sumHundredList[0], course.assignmentTypeWeights["attendance"]);
                Assert.AreEqual(sumHundredList[1], course.assignmentTypeWeights["homework"]);
                Assert.AreEqual(sumHundredList[2], course.assignmentTypeWeights["lab"]);
                Assert.AreEqual(sumHundredList[3], course.assignmentTypeWeights["discussion"]);
                Assert.AreEqual(sumHundredList[4], course.assignmentTypeWeights["quiz"]);
                Assert.AreEqual(sumHundredList[5], course.assignmentTypeWeights["project"]);
                Assert.AreEqual(sumHundredList[6], course.assignmentTypeWeights["midterm"]);
                Assert.AreEqual(sumHundredList[7], course.assignmentTypeWeights["final"]);
            }
        }

        // tests getUnweightedStudentGrade()
        [Test]
        public void Test12()
        {
            Course course = new Course("CS101", "Intro to Programming", "001");
            double maxCoursePts = 0;
            foreach (Student student in students)
            {
                course.addStudent(student);
            }
            foreach (Assignment assignment in assignments)
            {
                course.addAssignment(assignment);
                maxCoursePts += assignment.maxPoints;
            }

            foreach (Student student in course.students.Keys)
            {
                // tests the grade before any assignments are graded
                // testing for the grade out of zero assignments
                Grade ungradedStudentGrade = course.getUnweightedStudentGrade(student, true);
                Assert.AreEqual(0, ungradedStudentGrade.getGrade());
                Assert.AreEqual(true, ungradedStudentGrade.graded);

                // testing for the grade out of all ungraded assignments
                ungradedStudentGrade = course.getUnweightedStudentGrade(student, false);
                Assert.AreEqual(0, ungradedStudentGrade.getGrade());
                Assert.AreEqual(true, ungradedStudentGrade.graded);

                double currPtsSum = 0;
                double currMaxPtsSum = 0;
                foreach ((Assignment assignment, Grade grade) in course.students[student])
                {
                    double points = grade.points;
                    double maxPoints = grade.maxPoints;
                    Assert.AreEqual(0, points);
                    Assert.AreEqual(assignment.maxPoints, maxPoints);
                    Assert.AreEqual(false, grade.graded);
                    Assert.AreEqual(0, grade.getGrade());
                    Assert.AreEqual(course.students[student][assignment], grade);

                    course.gradeAssignment(student, assignment, 10);
                    Assert.AreEqual(10 / assignment.maxPoints * 100, grade.getGrade());
                    Assert.AreEqual(grade.points / grade.maxPoints * 100, grade.getGrade());
                    Assert.AreEqual(course.students[student][assignment], grade);
                    Assert.AreEqual(true, grade.graded);

                    // testing the grade out of graded assignments
                    Grade studentGrade = course.getUnweightedStudentGrade(student, true);
                    currPtsSum += grade.points;
                    currMaxPtsSum += grade.maxPoints;
                    Assert.AreEqual(currPtsSum / currMaxPtsSum * 100, studentGrade.getGrade());
                    Assert.AreEqual(true, studentGrade.graded);

                    // testing the grade out of all assignments
                    studentGrade = course.getUnweightedStudentGrade(student, false);
                    Assert.AreEqual(currPtsSum / maxCoursePts * 100, studentGrade.getGrade());
                    Assert.AreEqual(true, studentGrade.graded);
                }
            }

            course = new Course("PSY101", "Introduction to Psychology", "001");
            Student student1 = new Student(432456, "student 1");
            Assignment homework = new Assignment("homework 1", Assignment.Type.Homework, 100.0);
            Assignment bonusAssignment = new Assignment("extra credit", Assignment.Type.Bonus, 2);
            course.addStudent(student1);
            course.addAssignment(homework);
            course.addAssignment(bonusAssignment);

            // grade before grading anything
            Assert.AreEqual(0, course.getUnweightedStudentGrade(student1, false).getGrade());
            Assert.AreEqual(0, course.getUnweightedStudentGrade(student1, true).getGrade());

            course.gradeAssignment(student1, homework, 80.5);
            // grade after grading homework
            Assert.AreEqual(80.5, course.getUnweightedStudentGrade(student1, false).getGrade());
            Assert.AreEqual(80.5, course.getUnweightedStudentGrade(student1, true).getGrade());

            // grade bonus assignment
            course.gradeAssignment(student1, bonusAssignment, 2);
            // grade after grading bonus assignment
            Assert.AreEqual(82.5, course.getUnweightedStudentGrade(student1, false).getGrade());
            Assert.AreEqual(82.5, course.getUnweightedStudentGrade(student1, false).getGrade());
        }

        // tests getWeightedStudentGrade()
        [Test]
        public void Test13()
        {
            Course course = new Course("CS101", "Intro to Programming", "001");
            foreach (Student student in students)
            {
                course.addStudent(student);
            }

            // add assignments
            Assignment assignment1 = new Assignment("homework 1", Assignment.Type.Homework, 100);
            Assignment assignment2 = new Assignment("homework 2", Assignment.Type.Homework, 100);
            Assignment assignment3 = new Assignment("quiz 1", Assignment.Type.Quiz, 10);
            Assignment assignment4 = new Assignment("final", Assignment.Type.Final, 100);
            Assignment assignment5 = new Assignment("extra credit", Assignment.Type.Bonus, 3);
            course.addAssignment(assignment1);
            course.addAssignment(assignment2);
            course.addAssignment(assignment3);
            course.addAssignment(assignment4);
            course.addAssignment(assignment5);

            foreach(Student student in students)
            {
                // default weights
                // tests the grade before any assignments are graded
                // testing for the grade out of zero assignments
                Grade grade = course.getWeightedStudentGrade(student, true);
                Assert.AreEqual(0, grade.getGrade());
                Assert.AreEqual(true, grade.graded);

                // testing for the grade out of all ungraded assignments
                grade = course.getWeightedStudentGrade(student, false);
                Assert.AreEqual(0.0, grade.getGrade());
                Assert.AreEqual(true, grade.graded);

                // grade assignments
                course.gradeAssignment(student, assignment1, 100);
                course.gradeAssignment(student, assignment2, 50);
                course.gradeAssignment(student, assignment4, 76.5);

                // testing the grade out of graded assignments
                grade = course.getWeightedStudentGrade(student, true);
                double homeworkGrade1 = course.getAssignmentGrade(student, assignment1).getGrade();
                double homeworkGrade2 = course.getAssignmentGrade(student, assignment2).getGrade();
                double homeworkGrade = (homeworkGrade1 + homeworkGrade2) / 2 * course.assignmentTypeWeights["homework"] / 100;
                double finalGrade = course.getAssignmentGrade(student, assignment4).getGrade() * course.assignmentTypeWeights["final"] / 100;
                Assert.AreEqual(26.25, homeworkGrade);
                Assert.AreEqual(15.3, finalGrade);
                // (26.25 + 15.3) / (35 + 20) * 100 = 75.5454...
                double finalCourseGrade = 
                    (homeworkGrade + finalGrade) /
                    (course.assignmentTypeWeights["homework"] + course.assignmentTypeWeights["final"]) *
                    100;
                Assert.AreEqual(finalCourseGrade, grade.getGrade());
                Assert.AreEqual(true, grade.graded);

                // testing the grade out of all assignments
                grade = course.getWeightedStudentGrade(student, false);
                double quizGrade = course.students[student][assignment3].getGrade() * course.assignmentTypeWeights["quiz"] / 100;
                // 26.25 + 0 + 15.3 = 41.55
                finalCourseGrade = homeworkGrade + quizGrade + finalGrade;
                Assert.AreEqual(finalCourseGrade, grade.getGrade());
                Assert.AreEqual(true, grade.graded);

                // grade the extra credit assignment
                course.gradeAssignment(student, assignment5, 1);
                double extraCreditPoints = course.getAssignmentGrade(student, assignment5).points;
                grade = course.getWeightedStudentGrade(student, true);
                finalCourseGrade =
                    (homeworkGrade + finalGrade) /
                    (course.assignmentTypeWeights["homework"] + course.assignmentTypeWeights["final"]) *
                    100 +
                    extraCreditPoints;
                Assert.AreEqual(finalCourseGrade, grade.getGrade());

                grade = course.getWeightedStudentGrade(student, false);
                finalCourseGrade = homeworkGrade + quizGrade + finalGrade + extraCreditPoints;
                Assert.AreEqual(finalCourseGrade, grade.getGrade());

                // change the weights
                Dictionary<string, double> originalWeights = new Dictionary<string, double>(course.assignmentTypeWeights);
                course.setAssignmentTypeWeights(0, 0, 0, 0, 0, 0, 0, 100);
                // testing the grade out of graded assignments
                grade = course.getWeightedStudentGrade(student, true);
                // 76.5 + 1 = 77.5
                finalGrade = course.getAssignmentGrade(student, assignment4).getGrade() + extraCreditPoints;
                Assert.AreEqual(finalGrade, grade.getGrade());
                // testing the grade out of all assignments
                grade = course.getWeightedStudentGrade(student, false);
                Assert.AreEqual(finalGrade, grade.getGrade());

                // reset the weights
                course.setAssignmentTypeWeights(
                    originalWeights["attendance"],
                    originalWeights["homework"],
                    originalWeights["lab"],
                    originalWeights["discussion"],
                    originalWeights["quiz"],
                    originalWeights["project"],
                    originalWeights["midterm"],
                    originalWeights["final"]);
            }
        }
    }
}
