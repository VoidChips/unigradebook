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
                assignments.Add(new Assignment("Assignment " + i.ToString(), Assignment.Type.Project, i + 10.0));
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
                    Assert.AreEqual(0.0, grade.points);
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
                    Assert.AreEqual(0.0, grade.points);
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
                foreach(Student student in course.students.Keys)
                {
                    Grade grade = course.getAssignmentGrade(student, assignment);
                    Assert.AreEqual(0.0, grade.getGrade());
                    double points = course.students[student][assignment].points;
                    double maxPoints = course.students[student][assignment].maxPoints;
                    Assert.AreEqual(points / maxPoints * 100.0, grade.getGrade());
                    Assert.AreEqual(course.students[student][assignment], grade);
                }

                foreach (Student student in course.students.Keys)
                {
                    course.gradeAssignment(student, assignment, 10.0);
                    Grade grade = course.getAssignmentGrade(student, assignment);
                    Assert.AreEqual(10.0 / assignment.maxPoints * 100.0, grade.getGrade());
                    double points = course.students[student][assignment].points;
                    double maxPoints = course.students[student][assignment].maxPoints;
                    Assert.AreEqual(points / maxPoints * 100.0, grade.getGrade());
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
                foreach (Student student in course.students.Keys)
                {
                    Grade grade = course.students[student][assignment];
                    Assert.AreEqual(0.0, grade.getGrade());
                    double points = course.students[student][assignment].points;
                    double maxPoints = course.students[student][assignment].maxPoints;
                    Assert.AreEqual(points / maxPoints * 100.0, grade.getGrade());
                    Assert.AreEqual(course.students[student][assignment], grade);
                }

                foreach (Student student in course.students.Keys)
                {
                    course.gradeAssignment(student, assignment, 10.0);
                    Grade grade = course.students[student][assignment];
                    Assert.AreEqual(10.0 / assignment.maxPoints * 100.0, grade.getGrade());
                    double points = course.students[student][assignment].points;
                    double maxPoints = course.students[student][assignment].maxPoints;
                    Assert.AreEqual(points / maxPoints * 100.0, grade.getGrade());
                }
            }
        }
    }
}
