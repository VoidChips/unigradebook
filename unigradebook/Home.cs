﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using gradebook;

namespace unigradebook
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }


        private void addCourseBtn_Click(object sender, EventArgs e)
        {
            //Program.gradebook.courses.Add(new Course(100, "CS101"));
        }
    }
}
