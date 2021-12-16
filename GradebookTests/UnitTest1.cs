using NUnit.Framework;
using gradebook;
using System.Collections.Generic;
using System;
using System.IO;

namespace GradebookTests
{
    public class Tests
    {
        private List<string> names;
        private List<Student> students;
        private int students_num;

        // returns a random name
        private string generateName()
        {
            var rand = new Random();
            return names[rand.Next(students_num)];
        }

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

            // create a list of students
            for (int i = 0; i < students_num; i++)
            {
                // since the names are random, students can have same names
                students.Add(new Student(i + 100000, generateName()));
            }
        }

        // test if the Student object are initialized correctly
        [Test]
        public void StudentTest1()
        {
            for (int i = 0; i < students_num; i++)
            {
                Student student = new Student(i, names[i]);
                Assert.AreEqual(i, student.id);
                Assert.AreEqual(names[i], student.name);
            }
        }

        // test for equality with Student
        [Test]
        public void StudentTest2()
        {
            for (int i = 0; i < students_num; i++)
            {
                Student student = new Student(i, names[i]);
                Assert.IsTrue(student.Equals(new Student(i, names[i])));
                Assert.IsTrue(student == new Student(i, names[i]));
                Assert.IsFalse(student == new Student(500, "Lorem Ipsum"));
            }
        }
    }
}