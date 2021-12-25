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
            Assert.AreEqual(5, course.assignmentTypeWeights["attendance"], 0.000001);
            Assert.AreEqual(35, course.assignmentTypeWeights["homework"], 0.000001);
            Assert.AreEqual(0, course.assignmentTypeWeights["lab"], 0.000001);
            Assert.AreEqual(0, course.assignmentTypeWeights["discussion"], 0.000001);
            Assert.AreEqual(10, course.assignmentTypeWeights["quiz"], 0.000001);
            Assert.AreEqual(0, course.assignmentTypeWeights["project"], 0.000001);
            Assert.AreEqual(30, course.assignmentTypeWeights["midterm"], 0.000001);
            Assert.AreEqual(20, course.assignmentTypeWeights["final"], 0.000001);
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
                    Assert.AreEqual(0, grade.points, 0.000001);
                    Assert.AreEqual(assignment.maxPoints, grade.maxPoints, 0.000001);
                    Assert.AreEqual(false, grade.graded);
                }
            }

            // add a new student after adding assignments to the course
            Student newStudent = new Student(12930, generateName());
            course.addStudent(newStudent);
            Assert.AreEqual(assignments.Count, course.students[newStudent].Count);
            foreach(Assignment assignment in assignments)
            {
                Assert.IsTrue(course.students[newStudent].ContainsKey(assignment));
                Grade grade = course.students[newStudent][assignment];
                Assert.AreEqual(0, grade.points, 0.000001);
                Assert.AreEqual(assignment.maxPoints, grade.maxPoints, 0.000001);
                Assert.AreEqual(false, grade.graded);
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
                    Assert.AreEqual(0, grade.points, 0.000001);
                    Assert.AreEqual(assignment.maxPoints, grade.maxPoints, 0.000001);
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
                    Assert.AreEqual(0, grade.points, 0.000001);
                    Assert.AreEqual(assignment.maxPoints, grade.maxPoints, 0.000001);
                    Assert.AreEqual(false, grade.graded);
                    Assert.AreEqual(0, grade.getGrade(), 0.000001);
                    Assert.AreEqual(course.students[student][assignment], grade);

                    grade.points = 10;
                    grade.graded = true;
                    Assert.AreEqual(10 / assignment.maxPoints * 100, course.getAssignmentGrade(student, assignment).getGrade(), 0.000001);
                    Assert.AreEqual(grade.points / grade.maxPoints * 100, course.getAssignmentGrade(student, assignment).getGrade(), 0.000001);
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
                    Assert.AreEqual(0, grade.points, 0.000001);
                    Assert.AreEqual(assignment.maxPoints, grade.maxPoints, 0.000001);
                    Assert.AreEqual(false, grade.graded);
                    Assert.AreEqual(0, grade.getGrade(), 0.000001);
                    Assert.AreEqual(course.students[student][assignment], grade);

                    course.gradeAssignment(student, assignment, 10);
                    Assert.AreEqual(10 / assignment.maxPoints * 100, course.students[student][assignment].getGrade(), 0.000001);
                    Assert.AreEqual(grade.points / grade.maxPoints * 100, course.students[student][assignment].getGrade(), 0.000001);
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
            Assert.AreEqual(5, course.assignmentTypeWeights["attendance"], 0.000001);
            Assert.AreEqual(35, course.assignmentTypeWeights["homework"], 0.000001);
            Assert.AreEqual(0, course.assignmentTypeWeights["lab"], 0.000001);
            Assert.AreEqual(0, course.assignmentTypeWeights["discussion"], 0.000001);
            Assert.AreEqual(10, course.assignmentTypeWeights["quiz"], 0.000001);
            Assert.AreEqual(0, course.assignmentTypeWeights["project"], 0.000001);
            Assert.AreEqual(30, course.assignmentTypeWeights["midterm"], 0.000001);
            Assert.AreEqual(20, course.assignmentTypeWeights["final"], 0.000001);

            Assert.AreEqual(true, course.setAssignmentTypeWeights(0, 0, 0, 0, 5, 50, 20, 25));
            // tests for correct assignment weights by type from calling setAssignmentWeights()
            Assert.AreEqual(8, course.assignmentTypeWeights.Count);
            Assert.AreEqual(0.0, course.assignmentTypeWeights["attendance"], 0.000001);
            Assert.AreEqual(0.0, course.assignmentTypeWeights["homework"], 0.000001);
            Assert.AreEqual(0.0, course.assignmentTypeWeights["lab"], 0.000001);
            Assert.AreEqual(0.0, course.assignmentTypeWeights["discussion"], 0.000001);
            Assert.AreEqual(5.0, course.assignmentTypeWeights["quiz"], 0.000001);
            Assert.AreEqual(50.0, course.assignmentTypeWeights["project"], 0.000001);
            Assert.AreEqual(20.0, course.assignmentTypeWeights["midterm"], 0.000001);
            Assert.AreEqual(25.0, course.assignmentTypeWeights["final"], 0.000001);

            for (int i = 0; i < 100; i++)
            {
                Course newCourse = new Course("Course " + i.ToString(), "Course " + i.ToString() + " title", i.ToString());
                // tests for correct assignment weights by type from initialization
                Assert.AreEqual(8, newCourse.assignmentTypeWeights.Count);
                Assert.AreEqual(5.0, newCourse.assignmentTypeWeights["attendance"], 0.000001);
                Assert.AreEqual(35.0, newCourse.assignmentTypeWeights["homework"], 0.000001);
                Assert.AreEqual(0.0, newCourse.assignmentTypeWeights["lab"], 0.000001);
                Assert.AreEqual(0.0, newCourse.assignmentTypeWeights["discussion"], 0.000001);
                Assert.AreEqual(10.0, newCourse.assignmentTypeWeights["quiz"], 0.000001);
                Assert.AreEqual(0.0, newCourse.assignmentTypeWeights["project"], 0.000001);
                Assert.AreEqual(30.0, newCourse.assignmentTypeWeights["midterm"], 0.000001);
                Assert.AreEqual(20.0, newCourse.assignmentTypeWeights["final"], 0.000001);

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
                Assert.AreEqual(sumHundredList[0], course.assignmentTypeWeights["attendance"], 0.000001);
                Assert.AreEqual(sumHundredList[1], course.assignmentTypeWeights["homework"], 0.000001);
                Assert.AreEqual(sumHundredList[2], course.assignmentTypeWeights["lab"], 0.000001);
                Assert.AreEqual(sumHundredList[3], course.assignmentTypeWeights["discussion"], 0.000001);
                Assert.AreEqual(sumHundredList[4], course.assignmentTypeWeights["quiz"], 0.000001);
                Assert.AreEqual(sumHundredList[5], course.assignmentTypeWeights["project"], 0.000001);
                Assert.AreEqual(sumHundredList[6], course.assignmentTypeWeights["midterm"], 0.000001);
                Assert.AreEqual(sumHundredList[7], course.assignmentTypeWeights["final"], 0.000001);
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
                Assert.AreEqual(new Grade(0), course.getUnweightedStudentGrade(student, true));
                Assert.AreEqual(new Grade(0), course.getUnweightedStudentGrade(student, false));
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
                Assert.AreEqual(0, ungradedStudentGrade.getGrade(), 0.000001);
                Assert.AreEqual(true, ungradedStudentGrade.graded);

                // testing for the grade out of all ungraded assignments
                ungradedStudentGrade = course.getUnweightedStudentGrade(student, false);
                Assert.AreEqual(0, ungradedStudentGrade.getGrade(), 0.000001);
                Assert.AreEqual(true, ungradedStudentGrade.graded);

                double currPtsSum = 0;
                double currMaxPtsSum = 0;
                foreach ((Assignment assignment, Grade grade) in course.students[student])
                {
                    double points = grade.points;
                    double maxPoints = grade.maxPoints;
                    Assert.AreEqual(0, points, 0.000001);
                    Assert.AreEqual(assignment.maxPoints, maxPoints, 0.000001);
                    Assert.AreEqual(false, grade.graded);
                    Assert.AreEqual(0, grade.getGrade(), 0.000001);
                    Assert.AreEqual(course.students[student][assignment], grade);

                    course.gradeAssignment(student, assignment, 10);
                    Assert.AreEqual(10 / assignment.maxPoints * 100, grade.getGrade(), 0.000001);
                    Assert.AreEqual(grade.points / grade.maxPoints * 100, grade.getGrade(), 0.000001);
                    Assert.AreEqual(course.students[student][assignment], grade);
                    Assert.AreEqual(true, grade.graded);

                    // testing the grade out of graded assignments
                    Grade studentGrade = course.getUnweightedStudentGrade(student, true);
                    currPtsSum += grade.points;
                    currMaxPtsSum += grade.maxPoints;
                    Assert.AreEqual(currPtsSum / currMaxPtsSum * 100, studentGrade.getGrade(), 0.000001);
                    Assert.AreEqual(true, studentGrade.graded);

                    // testing the grade out of all assignments
                    studentGrade = course.getUnweightedStudentGrade(student, false);
                    Assert.AreEqual(currPtsSum / maxCoursePts * 100, studentGrade.getGrade(), 0.000001);
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
            Assert.AreEqual(0, course.getUnweightedStudentGrade(student1, false).getGrade(), 0.000001);
            Assert.AreEqual(0, course.getUnweightedStudentGrade(student1, true).getGrade(), 0.000001);

            course.gradeAssignment(student1, homework, 80.5);
            // grade after grading homework
            Assert.AreEqual(80.5, course.getUnweightedStudentGrade(student1, false).getGrade(), 0.000001);
            Assert.AreEqual(80.5, course.getUnweightedStudentGrade(student1, true).getGrade(), 0.000001);

            // grade bonus assignment
            course.gradeAssignment(student1, bonusAssignment, 2);
            // grade after grading bonus assignment
            Assert.AreEqual(82.5, course.getUnweightedStudentGrade(student1, false).getGrade(), 0.000001);
            Assert.AreEqual(82.5, course.getUnweightedStudentGrade(student1, false).getGrade(), 0.000001);
        }

        // tests getWeightedStudentGrade()
        [Test]
        public void Test13()
        {
            Course course = new Course("CS101", "Intro to Programming", "001");
            foreach (Student student in students)
            {
                course.addStudent(student);
                Assert.AreEqual(new Grade(0), course.getWeightedStudentGrade(student, true));
                Assert.AreEqual(new Grade(0), course.getWeightedStudentGrade(student, false));
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
                Assert.AreEqual(0, grade.getGrade(), 0.000001);
                Assert.AreEqual(true, grade.graded);

                // testing for the grade out of all ungraded assignments
                grade = course.getWeightedStudentGrade(student, false);
                Assert.AreEqual(0.0, grade.getGrade(), 0.000001);
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
                Assert.AreEqual(26.25, homeworkGrade, 0.000001);
                Assert.AreEqual(15.3, finalGrade, 0.000001);
                // (26.25 + 15.3) / (35 + 20) * 100 = 75.5454...
                double finalCourseGrade = 
                    (homeworkGrade + finalGrade) /
                    (course.assignmentTypeWeights["homework"] + course.assignmentTypeWeights["final"]) *
                    100;
                Assert.AreEqual(finalCourseGrade, grade.getGrade(), 0.000001);
                Assert.AreEqual(true, grade.graded);

                // testing the grade out of all assignments
                grade = course.getWeightedStudentGrade(student, false);
                double quizGrade = course.students[student][assignment3].getGrade() * course.assignmentTypeWeights["quiz"] / 100;
                // 26.25 + 0 + 15.3 = 41.55
                finalCourseGrade = homeworkGrade + quizGrade + finalGrade;
                Assert.AreEqual(finalCourseGrade, grade.getGrade(), 0.000001);
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
                Assert.AreEqual(finalCourseGrade, grade.getGrade(), 0.000001);

                grade = course.getWeightedStudentGrade(student, false);
                finalCourseGrade = homeworkGrade + quizGrade + finalGrade + extraCreditPoints;
                Assert.AreEqual(finalCourseGrade, grade.getGrade(), 0.000001);

                // change the weights
                Dictionary<string, double> originalWeights = new Dictionary<string, double>(course.assignmentTypeWeights);
                course.setAssignmentTypeWeights(0, 0, 0, 0, 0, 0, 0, 100);
                // testing the grade out of graded assignments
                grade = course.getWeightedStudentGrade(student, true);
                // 76.5 + 1 = 77.5
                finalGrade = course.getAssignmentGrade(student, assignment4).getGrade() + extraCreditPoints;
                Assert.AreEqual(finalGrade, grade.getGrade(), 0.000001);
                // testing the grade out of all assignments
                grade = course.getWeightedStudentGrade(student, false);
                Assert.AreEqual(finalGrade, grade.getGrade(), 0.000001);

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

        // tests getAssignmentMean()
        [Test]
        public void Test14()
        {
            Course course = new Course("CS101", "Intro to Programming", "001");

            // add students
            Student student1 = new Student(1, "student 1");
            Student student2 = new Student(2, "student 2");
            Student student3 = new Student(3, "student 3");
            Student student4 = new Student(4, "student 4");
            Student student5 = new Student(5, "student 5");
            course.addStudent(student1);
            course.addStudent(student2);
            course.addStudent(student3);
            course.addStudent(student4);
            course.addStudent(student5);

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

            Assert.AreEqual(0, course.getAssignmentMean(assignment1).points, 0.000001);
            Assert.AreEqual(true, course.getAssignmentMean(assignment1).graded);
            Assert.AreEqual(0, course.getAssignmentMean(assignment2).points, 0.000001);
            Assert.AreEqual(true, course.getAssignmentMean(assignment2).graded);
            Assert.AreEqual(0, course.getAssignmentMean(assignment3).points, 0.000001);
            Assert.AreEqual(true, course.getAssignmentMean(assignment3).graded);
            Assert.AreEqual(0, course.getAssignmentMean(assignment4).points, 0.000001);
            Assert.AreEqual(true, course.getAssignmentMean(assignment4).graded);
            Assert.AreEqual(0, course.getAssignmentMean(assignment5).points, 0.000001);
            Assert.AreEqual(true, course.getAssignmentMean(assignment5).graded);

            // grade assignment 1
            course.gradeAssignment(student1, assignment1, 50.5);
            course.gradeAssignment(student2, assignment1, 70);
            course.gradeAssignment(student3, assignment1, 83);
            course.gradeAssignment(student4, assignment1, 89.8);
            course.gradeAssignment(student5, assignment1, 95);

            // grade assignment 2
            course.gradeAssignment(student1, assignment2, 100);
            course.gradeAssignment(student2, assignment2, 82);
            course.gradeAssignment(student3, assignment2, 40);
            course.gradeAssignment(student4, assignment2, 56);
            course.gradeAssignment(student5, assignment2, 86.3);

            // grade assignment 3
            course.gradeAssignment(student1, assignment3, 10);
            course.gradeAssignment(student2, assignment3, 9);
            course.gradeAssignment(student3, assignment3, 8);
            course.gradeAssignment(student4, assignment3, 7.5);
            course.gradeAssignment(student5, assignment3, 7.8);

            // grade assignment 4
            course.gradeAssignment(student1, assignment4, 83);
            course.gradeAssignment(student2, assignment4, 85.6);
            course.gradeAssignment(student3, assignment4, 79.9);
            course.gradeAssignment(student4, assignment4, 81);
            course.gradeAssignment(student5, assignment4, 90.8);

            // grade assignment 5
            course.gradeAssignment(student1, assignment5, 2);
            course.gradeAssignment(student2, assignment5, 3);
            course.gradeAssignment(student3, assignment5, 2.2);
            course.gradeAssignment(student4, assignment5, 1);
            course.gradeAssignment(student5, assignment5, 0);

            // test class average for assignments
            Grade assignment1Avg = course.getAssignmentMean(assignment1);
            Grade assignment2Avg = course.getAssignmentMean(assignment2);
            Grade assignment3Avg = course.getAssignmentMean(assignment3);
            Grade assignment4Avg = course.getAssignmentMean(assignment4);
            Grade assignment5Avg = course.getAssignmentMean(assignment5);
            Assert.AreEqual(77.66, assignment1Avg.getGrade(), 0.000001);
            Assert.AreEqual(72.86, assignment2Avg.getGrade(), 0.000001);
            Assert.AreEqual(84.6, assignment3Avg.getGrade(), 0.000001);
            Assert.AreEqual(84.06, assignment4Avg.getGrade(), 0.000001);
            Assert.AreEqual(1.64 / 3 * 100, assignment5Avg.getGrade(), 0.000001);
        }

        // tests getClassMedian()
        [Test]
        public void Test15()
        {
            Course course = new Course("CS101", "Intro to Programming", "001");

            // add students
            Student student1 = new Student(1, "student 1");
            Student student2 = new Student(2, "student 2");
            Student student3 = new Student(3, "student 3");
            Student student4 = new Student(4, "student 4");
            Student student5 = new Student(5, "student 5");
            course.addStudent(student1);
            course.addStudent(student2);
            course.addStudent(student3);
            course.addStudent(student4);
            course.addStudent(student5);

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

            Assert.AreEqual(0, course.getAssignmentMedian(assignment1).points, 0.000001);
            Assert.AreEqual(true, course.getAssignmentMedian(assignment1).graded);
            Assert.AreEqual(0, course.getAssignmentMedian(assignment2).points, 0.000001);
            Assert.AreEqual(true, course.getAssignmentMedian(assignment2).graded);
            Assert.AreEqual(0, course.getAssignmentMedian(assignment3).points, 0.000001);
            Assert.AreEqual(true, course.getAssignmentMedian(assignment3).graded);
            Assert.AreEqual(0, course.getAssignmentMedian(assignment4).points, 0.000001);
            Assert.AreEqual(true, course.getAssignmentMedian(assignment4).graded);
            Assert.AreEqual(0, course.getAssignmentMedian(assignment5).points, 0.000001);
            Assert.AreEqual(true, course.getAssignmentMedian(assignment5).graded);

            // grade assignment 1
            course.gradeAssignment(student1, assignment1, 50.5);
            course.gradeAssignment(student2, assignment1, 70);
            course.gradeAssignment(student3, assignment1, 83);
            course.gradeAssignment(student4, assignment1, 89.8);
            course.gradeAssignment(student5, assignment1, 95);

            // grade assignment 2
            course.gradeAssignment(student1, assignment2, 100);
            course.gradeAssignment(student2, assignment2, 82);
            course.gradeAssignment(student3, assignment2, 40);
            course.gradeAssignment(student4, assignment2, 56);
            course.gradeAssignment(student5, assignment2, 86.3);

            // grade assignment 3
            course.gradeAssignment(student1, assignment3, 10);
            course.gradeAssignment(student2, assignment3, 9);
            course.gradeAssignment(student3, assignment3, 8);
            course.gradeAssignment(student4, assignment3, 7.5);
            course.gradeAssignment(student5, assignment3, 7.8);

            // grade assignment 4
            course.gradeAssignment(student1, assignment4, 83);
            course.gradeAssignment(student2, assignment4, 85.6);
            course.gradeAssignment(student3, assignment4, 79.9);
            course.gradeAssignment(student4, assignment4, 81);
            course.gradeAssignment(student5, assignment4, 90.8);

            // grade assignment 5
            course.gradeAssignment(student1, assignment5, 2);
            course.gradeAssignment(student2, assignment5, 3);
            course.gradeAssignment(student3, assignment5, 2.2);
            course.gradeAssignment(student4, assignment5, 1);
            course.gradeAssignment(student5, assignment5, 0);

            // tests the median with odd number of students
            Grade assignment1Median = course.getAssignmentMedian(assignment1);
            Grade assignment2Median = course.getAssignmentMedian(assignment2);
            Grade assignment3Median = course.getAssignmentMedian(assignment3);
            Grade assignment4Median = course.getAssignmentMedian(assignment4);
            Grade assignment5Median = course.getAssignmentMedian(assignment5);
            Assert.AreEqual(83, assignment1Median.getGrade(), 0.000001);
            Assert.AreEqual(82, assignment2Median.getGrade(), 0.000001);
            Assert.AreEqual(8, assignment3Median.points, 0.000001);
            Assert.AreEqual(80, assignment3Median.getGrade(), 0.000001);
            Assert.AreEqual(83, assignment4Median.getGrade(), 0.000001);
            Assert.AreEqual(2, assignment5Median.points, 0.000001);
            Assert.AreEqual(2.0 / 3.0 * 100, assignment5Median.getGrade(), 0.000001);

            // add a new student to have even number of students
            Student newStudent = new Student(43294, generateName());
            course.addStudent(newStudent);
            // grade the assignments for the new student
            course.gradeAssignment(newStudent, assignment1, 80);
            course.gradeAssignment(newStudent, assignment2, 33);
            course.gradeAssignment(newStudent, assignment3, 9);
            course.gradeAssignment(newStudent, assignment4, 83.7);
            course.gradeAssignment(newStudent, assignment5, 3);
            // tests the median with even students
            assignment1Median = course.getAssignmentMedian(assignment1);
            assignment2Median = course.getAssignmentMedian(assignment2);
            assignment3Median = course.getAssignmentMedian(assignment3);
            assignment4Median = course.getAssignmentMedian(assignment4);
            assignment5Median = course.getAssignmentMedian(assignment5);
            Assert.AreEqual(81.5, assignment1Median.getGrade(), 0.000001);
            Assert.AreEqual(69, assignment2Median.getGrade(), 0.000001);
            Assert.AreEqual(8.5, assignment3Median.points, 0.000001);
            Assert.AreEqual(85, assignment3Median.getGrade(), 0.000001);
            Assert.AreEqual(83.35, assignment4Median.getGrade(), 0.000001);
            Assert.AreEqual(2.1, assignment5Median.points, 0.000001);
            Assert.AreEqual(70, assignment5Median.getGrade(), 0.000001);
        }

        // tests getUnweightedClassMean()
        [Test]
        public void Test16()
        {
            Course course = new Course("CS101", "Intro to Programming", "001");

            Assert.AreEqual(new Grade(0), course.getUnweightedClassMean(true));
            Assert.AreEqual(new Grade(0), course.getUnweightedClassMean(false));

            // add students
            Student student1 = new Student(1, "student 1");
            Student student2 = new Student(2, "student 2");
            Student student3 = new Student(3, "student 3");
            Student student4 = new Student(4, "student 4");
            Student student5 = new Student(5, "student 5");
            course.addStudent(student1);
            course.addStudent(student2);
            course.addStudent(student3);
            course.addStudent(student4);
            course.addStudent(student5);

            // add assignments
            Assignment assignment1 = new Assignment("homework 1", Assignment.Type.Homework, 100);
            Assignment assignment2 = new Assignment("homework 2", Assignment.Type.Homework, 100);
            Assignment assignment3 = new Assignment("quiz 1", Assignment.Type.Quiz, 10);
            Assignment assignment4 = new Assignment("final", Assignment.Type.Final, 100);
            Assignment assignment5 = new Assignment("extra credit", Assignment.Type.Bonus, 3);
            Assignment assignment6 = new Assignment("quiz 2", Assignment.Type.Quiz, 10);
            course.addAssignment(assignment1);
            course.addAssignment(assignment2);
            course.addAssignment(assignment3);
            course.addAssignment(assignment4);
            course.addAssignment(assignment5);
            course.addAssignment(assignment6);

            // grade assignment 1
            course.gradeAssignment(student1, assignment1, 50.5);
            course.gradeAssignment(student2, assignment1, 70);
            course.gradeAssignment(student3, assignment1, 83);
            course.gradeAssignment(student4, assignment1, 89.8);
            course.gradeAssignment(student5, assignment1, 95);

            // grade assignment 2
            course.gradeAssignment(student1, assignment2, 100);
            course.gradeAssignment(student2, assignment2, 82);
            course.gradeAssignment(student3, assignment2, 40);
            course.gradeAssignment(student4, assignment2, 56);
            course.gradeAssignment(student5, assignment2, 86.3);

            // grade assignment 3
            course.gradeAssignment(student1, assignment3, 10);
            course.gradeAssignment(student2, assignment3, 9);
            course.gradeAssignment(student3, assignment3, 8);
            course.gradeAssignment(student4, assignment3, 7.5);
            course.gradeAssignment(student5, assignment3, 7.8);

            // grade assignment 4
            course.gradeAssignment(student1, assignment4, 83);
            course.gradeAssignment(student2, assignment4, 85.6);
            course.gradeAssignment(student3, assignment4, 79.9);
            course.gradeAssignment(student4, assignment4, 81);
            course.gradeAssignment(student5, assignment4, 90.8);

            // grade assignment 5
            course.gradeAssignment(student1, assignment5, 2);
            course.gradeAssignment(student2, assignment5, 3);
            course.gradeAssignment(student3, assignment5, 2.2);
            course.gradeAssignment(student4, assignment5, 1);
            course.gradeAssignment(student5, assignment5, 0);

            // assignment 6 not graded

            // test the unweighted student grades
            Assert.AreEqual(243.5 / 310 * 100 + 2, course.getUnweightedStudentGrade(student1, true).getGrade(), 0.000001);
            Assert.AreEqual(243.5 / 320 * 100 + 2, course.getUnweightedStudentGrade(student1, false).getGrade(), 0.000001);
            Assert.AreEqual(246.6 / 310 * 100 + 3, course.getUnweightedStudentGrade(student2, true).getGrade(), 0.000001);
            Assert.AreEqual(246.6 / 320 * 100 + 3, course.getUnweightedStudentGrade(student2, false).getGrade(), 0.000001);
            Assert.AreEqual(210.9 / 310 * 100 + 2.2, course.getUnweightedStudentGrade(student3, true).getGrade(), 0.000001);
            Assert.AreEqual(210.9 / 320 * 100 + 2.2, course.getUnweightedStudentGrade(student3, false).getGrade(), 0.000001);
            Assert.AreEqual(234.3 / 310 * 100 + 1, course.getUnweightedStudentGrade(student4, true).getGrade(), 0.000001);
            Assert.AreEqual(234.3 / 320 * 100 + 1, course.getUnweightedStudentGrade(student4, false).getGrade(), 0.000001);
            Assert.AreEqual(279.9 / 310 * 100, course.getUnweightedStudentGrade(student5, true).getGrade(), 0.000001);
            Assert.AreEqual(279.9 / 320 * 100, course.getUnweightedStudentGrade(student5, false).getGrade(), 0.000001);

            // test the mean
            Assert.AreEqual(
                (243.5 / 310 * 100 + 2 +
                246.6 / 310 * 100 + 3 +
                210.9 / 310 * 100 + 2.2 +
                234.3 / 310 * 100 + 1 +
                279.9 / 310 * 100) /
                5,
                course.getUnweightedClassMean(true).getGrade(), 0.000001);
            Assert.AreEqual(
                (243.5 / 320 * 100 + 2 +
                246.6 / 320 * 100 + 3 +
                210.9 / 320 * 100 + 2.2 +
                234.3 / 320 * 100 + 1 +
                279.9 / 320 * 100) /
                5,
                course.getUnweightedClassMean(false).getGrade(), 0.000001);
        }
    }
}
