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
using System.Data.SQLite;
using gradebook;

namespace unigradebook
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();

            // setup course list
            courseList.FullRowSelect = true;
            courseList.Columns.Add("Name", -2, HorizontalAlignment.Left);
            courseList.Columns.Add("Title", -2, HorizontalAlignment.Left);
            courseList.Columns.Add("Section", -2, HorizontalAlignment.Left);

            // get courses from the database
            string cs = "Data Source=./gradebook.db";
            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "SELECT * FROM courses;";
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.WriteLine($"{rdr.GetString(0)}, {rdr.GetString(1)}, {rdr.GetString(2)}");
                Course course = new Course(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2));
                Program.courses.Add(course);
                ListViewItem item = new ListViewItem(course.name, 0);
                item.SubItems.Add(course.title);
                item.SubItems.Add(course.section);
                courseList.Items.Add(item);
            }
            con.Close();

            courseList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void addCourseBtn_Click(object sender, EventArgs e)
        {
            Form addCourseForm = new AddCourseForm();
            addCourseForm.Show(this);
        }
    }
}
