using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
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
            Form addCourseForm = new AddCourseForm();
            addCourseForm.Show();
            //Program.courses.Add(new Course(100, "CS101"));
            //Debug.WriteLine(Program.courses[0].name);
        }
    }
}
