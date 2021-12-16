using NUnit.Framework;
using gradebook;
using System.Collections.Generic;

namespace GradebookTests
{
    class AssignmentGradeTest
    {
        private List<AssignmentGrade> assignmentGrades;
        private int count;

        [SetUp]
        public void Setup()
        {
            assignmentGrades = new List<AssignmentGrade>();
            count = 100;

            // create assignmentGrades
            for (int i = 0; i < count; i++)
            {
                assignmentGrades.Add(new AssignmentGrade(new Assignment("Assignment " + i.ToString(), Assignment.Type.Homework, 1000.0)));
                assignmentGrades[i].points = i;
                assignmentGrades[i].graded = true;
            }
        }

        // tests for correct initialization
        [Test]
        public void Test1()
        {
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(i, assignmentGrades[i].points);
                Assert.AreEqual(true, assignmentGrades[i].graded);
                Assert.AreEqual(1000.0, assignmentGrades[i].maxPoints);
            }
        }

        // tests copy constructor
        [Test]
        public void Test2()
        {
            for (int i = 0; i < count; i++)
            {
                AssignmentGrade ag = new AssignmentGrade(assignmentGrades[i]);
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
                AssignmentGrade ag = new AssignmentGrade(assignmentGrades[i]);
                ag.points = assignmentGrades[i].points;
                ag.maxPoints = assignmentGrades[i].maxPoints;
                ag.graded = assignmentGrades[i].graded;

                Assert.IsTrue(ag.Equals(assignmentGrades[i]));
                Assert.IsTrue(ag == assignmentGrades[i]);
                Assert.IsFalse(ag != assignmentGrades[i]);

                ag.points += 10.0;
                Assert.IsFalse(ag.Equals(assignmentGrades[i]));
                Assert.IsFalse(ag == assignmentGrades[i]);
                Assert.IsTrue(ag != assignmentGrades[i]);
            }
        }

        // tests the getGrade() method
        [Test]
        public void Test4()
        {
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(assignmentGrades[i].points / 1000.0 * 100.0, assignmentGrades[i].getGrade());
            }

            AssignmentGrade ag = assignmentGrades[0];
            ag.points = 100.0;
            ag.maxPoints = 100.0;

            Assert.AreEqual(100.0, ag.getGrade());
        }
    }
}
