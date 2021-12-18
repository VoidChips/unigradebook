using NUnit.Framework;
using gradebook;
using System.Collections.Generic;

namespace GradebookTests
{
    class GradeTest
    {
        private List<Grade> grades;
        private int count;

        [SetUp]
        public void Setup()
        {
            grades = new List<Grade>();
            count = 100;

            // create assignmentGrades
            for (int i = 0; i < count; i++)
            {
                grades.Add(new Grade(1000.0));
                grades[i].points = i;
                grades[i].graded = true;
            }
        }

        // tests for correct initialization
        [Test]
        public void Test1()
        {
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(i, grades[i].points);
                Assert.AreEqual(true, grades[i].graded);
                Assert.AreEqual(1000.0, grades[i].maxPoints);
            }
        }

        // tests copy constructor
        [Test]
        public void Test2()
        {
            for (int i = 0; i < count; i++)
            {
                Grade ag = new Grade(grades[i]);
                Assert.AreEqual(i, ag.points);
                Assert.AreEqual(true, ag.graded);
                Assert.AreEqual(1000.0, ag.maxPoints);
            }
        }

        // tests equality for AssignmentGrade
        [Test]
        public void Test3()
        {
            for (int i = 0; i < count; i++)
            {
                Grade ag = new Grade(grades[i]);
                ag.points = grades[i].points;
                ag.maxPoints = grades[i].maxPoints;
                ag.graded = grades[i].graded;

                Assert.IsTrue(ag.Equals(grades[i]));
                Assert.IsTrue(ag == grades[i]);
                Assert.IsFalse(ag != grades[i]);

                ag.points += 10.0;
                Assert.IsFalse(ag.Equals(grades[i]));
                Assert.IsFalse(ag == grades[i]);
                Assert.IsTrue(ag != grades[i]);
            }
        }

        // tests GetHashCode()
        [Test]
        public void Test4()
        {
            for (int i = 0; i < count; i++)
            {
                Grade ag = new Grade(grades[i]);
                ag.points = grades[i].points;
                ag.maxPoints = grades[i].maxPoints;
                ag.graded = grades[i].graded;

                Assert.AreEqual(ag.GetHashCode(), grades[i].GetHashCode());

                ag.points += 10.0;
                Assert.AreNotEqual(ag.GetHashCode(), grades[i].GetHashCode());
            }
        }

        // tests the getGrade() method
        [Test]
        public void Test5()
        {
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(grades[i].points / 1000.0 * 100.0, grades[i].getGrade());
            }

            Grade ag = grades[0];
            ag.points = 100.0;
            ag.maxPoints = 100.0;

            Assert.AreEqual(100.0, ag.getGrade());
        }
    }
}
