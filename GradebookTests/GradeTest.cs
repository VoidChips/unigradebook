using gradebook;
using NUnit.Framework;
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
                Assert.AreEqual(i, grades[i].points, 0.000001);
                Assert.AreEqual(1000.0, grades[i].maxPoints, 0.000001);
                Assert.AreEqual(true, grades[i].graded);
            }
        }

        // tests copy constructor
        [Test]
        public void Test2()
        {
            for (int i = 0; i < count; i++)
            {
                Grade g = new Grade(grades[i]);
                Assert.AreEqual(i, g.points, 0.000001);
                Assert.AreEqual(1000.0, g.maxPoints, 0.000001);
                Assert.AreEqual(true, g.graded);
            }
        }

        // tests equality for AssignmentGrade
        [Test]
        public void Test3()
        {
            for (int i = 0; i < count; i++)
            {
                Grade g = new Grade(grades[i]);
                g.points = grades[i].points;
                g.maxPoints = grades[i].maxPoints;
                g.graded = grades[i].graded;

                Assert.IsTrue(g.Equals(grades[i]));
                Assert.IsTrue(g == grades[i]);
                Assert.IsFalse(g != grades[i]);

                g.points += 10.0;
                Assert.IsFalse(g.Equals(grades[i]));
                Assert.IsFalse(g == grades[i]);
                Assert.IsTrue(g != grades[i]);
            }
        }

        // tests GetHashCode()
        [Test]
        public void Test4()
        {
            for (int i = 0; i < count; i++)
            {
                Grade g = new Grade(grades[i]);
                g.points = grades[i].points;
                g.maxPoints = grades[i].maxPoints;
                g.graded = grades[i].graded;

                Assert.AreEqual(g.GetHashCode(), grades[i].GetHashCode());

                g.points += 10.0;
                Assert.AreNotEqual(g.GetHashCode(), grades[i].GetHashCode());
            }
        }

        // tests CompareTo()
        [Test]
        public void Test5()
        {
            Grade grade1 = new Grade(1000);
            Assert.AreEqual(0, grades[0].CompareTo(grade1));
            Assert.AreEqual(0, grade1.CompareTo(grades[0]));

            for (int i = 1; i < count; i++)
            {
                Grade grade2 = new Grade(1000);
                Assert.AreEqual(1, grades[i].CompareTo(grade2));
                Assert.AreEqual(1, grades[i].CompareTo(grades[i - 1]));
                if (i != count - 1)
                {
                    Assert.AreEqual(-1, grades[i].CompareTo(grades[i + 1]));
                }
            }
        }

        // tests the getGrade() method
        [Test]
        public void Test6()
        {
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(grades[i].points / 1000.0 * 100.0, grades[i].getGrade(), 0.000001);
            }

            Grade g = grades[0];
            g.points = 100.0;
            g.maxPoints = 100.0;
            Assert.AreEqual(100.0, g.getGrade(), 0.000001);

            g.maxPoints = 0.0;
            Assert.AreEqual(0.0, g.getGrade(), 0.000001);

            g.points = 8.5;
            g.maxPoints = 10;
            Assert.AreEqual(85, g.getGrade(), 0.000001);
        }
    }
}
