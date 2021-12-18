using NUnit.Framework;
using gradebook;
using System.Collections.Generic;
using System;
using System.IO;

namespace GradebookTests
{
    public class StudentTest
    {
        private List<string> names;
        private List<Student> students;
        private int students_num;

        [SetUp]
        public void Setup()
        {
            names = new List<string>();
            students = new List<Student>();

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
            }
        }

        // tests if the Student object are initialized correctly
        [Test]
        public void Test1()
        {
            for (int i = 0; i < students.Count; i++)
            {
                Assert.AreEqual(i, students[i].id);
                Assert.AreEqual(names[i], students[i].name);
            }
        }

        // tests copy constructor
        [Test]
        public void Test2()
        {
            for (int i = 0; i < students.Count; i++)
            {
                Student s = new Student(students[i]);
                Assert.AreEqual(i, s.id);
                Assert.AreEqual(names[i], s.name);
            }
        }

        // tests for equality with Student
        [Test]
        public void Test3()
        {
            for (int i = 0; i < students.Count; i++)
            {
                Student student = new Student(i, names[i]);
                Assert.IsTrue(student.Equals(students[i]));
                Assert.IsTrue(student == students[i]);
                Assert.IsFalse(student != students[i]);
                Assert.IsFalse(student == new Student(500, "Lorem Ipsum"));
                Assert.IsTrue(student != new Student(500, "Lorem Ipsum"));
            }
        }

        // tests GetHashCode()
        [Test]
        public void Test4()
        {
            for (int i = 0; i < students.Count; i++)
            {
                Student student = new Student(i, names[i]);
                Assert.AreEqual(student.GetHashCode(), students[i].GetHashCode());
                Assert.AreNotEqual(student.GetHashCode(), new Student(500, "Lorem Ipsum").GetHashCode());
            }
        }
    }
}