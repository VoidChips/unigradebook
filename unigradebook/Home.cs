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
            courseList.FullRowSelect = true;
            courseList.Columns.Add("Name", -2, HorizontalAlignment.Left);
            courseList.Columns.Add("Title", -2, HorizontalAlignment.Left);
            courseList.Columns.Add("Section", -2, HorizontalAlignment.Left);
        }

        private void addCourseBtn_Click(object sender, EventArgs e)
        {
            Form addCourseForm = new AddCourseForm();
            addCourseForm.Show(this);
        }
    }
}
