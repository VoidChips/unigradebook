using System;
using System.Collections.Generic;

namespace gradebook
{
    public class Gradebook
    {
        public List<Course> courses { get; set; }

        public Gradebook()
        {
            this.courses = new List<Course>();
        }

    }
}
