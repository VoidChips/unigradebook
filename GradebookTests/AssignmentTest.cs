using NUnit.Framework;
using gradebook;
using System.Collections.Generic;

namespace GradebookTests
{
    public class AssignmentTest
    {
        private List<Assignment> assignments;
        private int count;

        [SetUp]
        public void Setup()
        {
            assignments = new List<Assignment>();
            count = 100;

            // create assignments
            for (int i = 0; i < count; i++)
            {
                Assignment.Type type = Assignment.Type.Attendance;
                double maxPoints = 10.0;
                if (i <= 10)
                {
                    type = Assignment.Type.Attendance;
                }
                else if (i <= 25)
                {
                    type = Assignment.Type.Homework;
                    maxPoints = 100.0;
                }
                else if (i <= 50)
                {
                    type = Assignment.Type.Project;
                    maxPoints = 200.0;
                }
                else if (i <= 75)
                {
                    type = Assignment.Type.Quiz;
                    maxPoints = 500.0;
                }
                else
                {
                    type = Assignment.Type.Test;
                    maxPoints = 1000.0;
                }

                assignments.Add(new Assignment("Assignment " + i.ToString(), type, maxPoints));
            }
        }

        // tests if Assignments are initialized correctly
        [Test]
        public void Test1()
        {
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual("Assignment " + i, assignments[i].name);

                Assignment.Type type = Assignment.Type.Attendance;
                double maxPoints = 10.0;
                if (i <= 10)
                {
                    type = Assignment.Type.Attendance;
                }
                else if (i <= 25)
                {
                    type = Assignment.Type.Homework;
                    maxPoints = 100.0;
                }
                else if (i <= 50)
                {
                    type = Assignment.Type.Project;
                    maxPoints = 200.0;
                }
                else if (i <= 75)
                {
                    type = Assignment.Type.Quiz;
                    maxPoints = 500.0;
                }
                else
                {
                    type = Assignment.Type.Test;
                    maxPoints = 1000.0;
                }
                Assert.AreEqual(type, assignments[i].type);
                Assert.AreEqual(maxPoints, assignments[i].maxPoints);
            }
        }

        [Test]
        // tests copy constructor
        public void Test2()
        {
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual("Assignment " + i, assignments[i].name);

                Assignment.Type type = Assignment.Type.Attendance;
                double maxPoints = 10.0;
                if (i <= 10)
                {
                    type = Assignment.Type.Attendance;
                }
                else if (i <= 25)
                {
                    type = Assignment.Type.Homework;
                    maxPoints = 100.0;
                }
                else if (i <= 50)
                {
                    type = Assignment.Type.Project;
                    maxPoints = 200.0;
                }
                else if (i <= 75)
                {
                    type = Assignment.Type.Quiz;
                    maxPoints = 500.0;
                }
                else
                {
                    type = Assignment.Type.Test;
                    maxPoints = 1000.0;
                }

                Assignment a = new Assignment(assignments[i]);
                Assert.AreEqual(type, a.type);
                Assert.AreEqual(maxPoints, a.maxPoints);
            }
        }

        // tests equality for Assignment
        [Test]
        public void Test3()
        {
            for (int i = 0; i < count; i++)
            {
                Assignment.Type type = Assignment.Type.Attendance;
                double maxPoints = 10.0;
                if (i <= 10)
                {
                    type = Assignment.Type.Attendance;
                }
                else if (i <= 25)
                {
                    type = Assignment.Type.Homework;
                    maxPoints = 100.0;
                }
                else if (i <= 50)
                {
                    type = Assignment.Type.Project;
                    maxPoints = 200.0;
                }
                else if (i <= 75)
                {
                    type = Assignment.Type.Quiz;
                    maxPoints = 500.0;
                }
                else
                {
                    type = Assignment.Type.Test;
                    maxPoints = 1000.0;
                }

                Assignment a = new Assignment("Assignment " + i.ToString(), type, maxPoints);
                Assert.IsTrue(a.Equals(assignments[i]));
                Assert.IsTrue(a == assignments[i]);
                Assert.IsFalse(a != assignments[i]);

                a.name = "New Name";
                Assert.IsFalse(a.Equals(assignments[i]));
                Assert.IsFalse(a == assignments[i]);
                Assert.IsTrue(a != assignments[i]);
            }
        }
    }
}
