using NUnit.Framework;
using gradebook;
using System.Collections.Generic;
using System;
using System.IO;

namespace GradebookTests
{
    public class StudentTest
    {
        private static Random rand;
        private List<string> names;
        private List<Student> students;
        private List<Student> randomStudents;
        private int students_num;

        // returns a random name
        private string generateName()
        {
            return names[rand.Next(students_num)];
        }

        [SetUp]
        public void Setup()
        {
            names = new List<string>();
            students = new List<Student>();
            randomStudents = new List<Student>();
            rand = new Random();

            // get names from the csv file
            string[] lines = File.ReadAllLines("random_names_fossbytes.csv");
            foreach (string line in lines)
            {
                names.Add(line);
            }
            students_num = lines.Length;

            // create a list of students
            // create a list of random students
            for (int i = 0; i < students_num; i++)
            {
                students.Add(new Student(i, names[i]));

                // since the names are random, students can have same names
                randomStudents.Add(new Student(i + 100000, generateName()));
            }
        }

        // tests if the Student object are initialized correctly
        [Test]
        public void Test1()
        {
            for (int i = 0; i < students_num; i++)
            {
                Assert.AreEqual(i, students[i].id);
                Assert.AreEqual(names[i], students[i].name);
            }
        }

        // tests copy constructor
        [Test]
        public void Test2()
        {
            for (int i = 0; i < students_num; i++)
            {
                Student s = new Student(students[i]);
                Assert.AreEqual(i, s.id);
                Assert.AreEqual(names[i], s.name);
            }
        }

        // test for equality with Student
        [Test]
        public void Test3()
        {
            for (int i = 0; i < students_num; i++)
            {
                Student student = new Student(i, names[i]);
                Assert.IsTrue(student.Equals(students[i]));
                Assert.IsTrue(student == students[i]);
                Assert.IsFalse(student == new Student(500, "Lorem Ipsum"));
            }
        }
    }
}