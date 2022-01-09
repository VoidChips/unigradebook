using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using gradebook;

namespace unigradebook
{
    public partial class AddCourseForm : Form
    {
        private double weightSum;

        public AddCourseForm()
        {
            InitializeComponent();
            letterGradeType1Btn.Checked = true;
            gradeCutoff2.Enabled = false;
            weightSum = 100;
        }

        // update the displayed sum of all the assignment weights
        private void updateWeightSum()
        {
            double x = 0;
            double attendanceWeight = double.TryParse(attendanceTextbox.Text, out x) ? x : 0;
            double homeworkWeight = double.TryParse(homeworkTextbox.Text, out x) ? x : 0;
            double labWeight = double.TryParse(labTextbox.Text, out x) ? x : 0;
            double discussionWeight = double.TryParse(discussionTextbox.Text, out x) ? x : 0;
            double quizWeight = double.TryParse(quizTextbox.Text, out x) ? x : 0;
            double projectWeight = double.TryParse(projectTextbox.Text, out x) ? x : 0;
            double midtermWeight = double.TryParse(midtermTextbox.Text, out x) ? x : 0;
            double finalWeight = double.TryParse(finalTextbox.Text, out x) ? x : 0;

            weightSum =
                attendanceWeight +
                homeworkWeight +
                labWeight +
                discussionWeight +
                quizWeight +
                projectWeight +
                midtermWeight +
                finalWeight;
            sumLbl.Text = weightSum.ToString();
        }

        private void letterGradeType1Btn_CheckedChanged(object sender, EventArgs e)
        {
            gradeCutoff1.Enabled = true;
            gradeCutoff2.Enabled = false;
        }

        private void letterGradeType2Btn_CheckedChanged(object sender, EventArgs e)
        {
            gradeCutoff1.Enabled = false;
            gradeCutoff2.Enabled = true;
        }

        private void attendanceTextbox_TextChanged(object sender, EventArgs e)
        {
            updateWeightSum();
        }

        private void homeworkTextbox_TextChanged(object sender, EventArgs e)
        {
            updateWeightSum();
        }

        private void labTextbox_TextChanged(object sender, EventArgs e)
        {
            updateWeightSum();
        }

        private void discussionTextbox_TextChanged(object sender, EventArgs e)
        {
            updateWeightSum();
        }

        private void quizTextbox_TextChanged(object sender, EventArgs e)
        {
            updateWeightSum();
        }

        private void projectTextbox_TextChanged(object sender, EventArgs e)
        {
            updateWeightSum();
        }

        private void midtermTextbox_TextChanged(object sender, EventArgs e)
        {
            updateWeightSum();
        }

        private void finalTextbox_TextChanged(object sender, EventArgs e)
        {
            updateWeightSum();
        }

        // add the course if all values are valid
        private void doneBtn_Click(object sender, EventArgs e)
        {
            // check if all fields are filled
            if (nameTextbox.Text.Length == 0 ||
                titleTextbox.Text.Length == 0 ||
                sectionTextbox.Text.Length == 0 ||
                (gradeCutoff1.Enabled &&
                aTextbox1.Text.Length == 0 ||
                bTextbox1.Text.Length == 0 ||
                cTextbox1.Text.Length == 0 ||
                dTextbox1.Text.Length == 0) ||
                (gradeCutoff2.Enabled &&
                aPlusTextbox.Text.Length == 0 ||
                aTextbox2.Text.Length == 0 ||
                aMinusTextbox.Text.Length == 0 ||
                bPlusTextbox.Text.Length == 0 ||
                bTextbox2.Text.Length == 0 ||
                bMinusTextbox.Text.Length == 0 ||
                cPlusTextbox.Text.Length == 0 ||
                cTextbox2.Text.Length == 0 ||
                cMinusTextbox.Text.Length == 0 ||
                dPlusTextbox.Text.Length == 0 ||
                dTextbox2.Text.Length == 0 ||
                dMinusTextbox.Text.Length == 0) ||
                attendanceTextbox.Text.Length == 0 ||
                homeworkTextbox.Text.Length == 0 ||
                labTextbox.Text.Length == 0 ||
                discussionTextbox.Text.Length == 0 ||
                quizTextbox.Text.Length == 0 ||
                projectTextbox.Text.Length == 0 ||
                midtermTextbox.Text.Length == 0 ||
                finalTextbox.Text.Length == 0)
            {
                errorTextbox.Text = "Fill all fields in the form.";
                errorTextbox.Visible = true;
                return;
            }

            Course course = new Course(nameTextbox.Text, titleTextbox.Text, sectionTextbox.Text);
            double x = 0;

            double cutoffA1 = double.TryParse(aTextbox1.Text, out x) ? x : 0;
            double cutoffB1 = double.TryParse(bTextbox1.Text, out x) ? x : 0;
            double cutoffC1 = double.TryParse(cTextbox1.Text, out x) ? x : 0;
            double cutoffD1 = double.TryParse(dTextbox1.Text, out x) ? x : 0;

            double cutoffAp = double.TryParse(aPlusTextbox.Text, out x) ? x : 0;
            double cutoffA2 = double.TryParse(aTextbox2.Text, out x) ? x : 0;
            double cutoffAm = double.TryParse(aMinusTextbox.Text, out x) ? x : 0;
            double cutoffBp = double.TryParse(bPlusTextbox.Text, out x) ? x : 0;
            double cutoffB2 = double.TryParse(bTextbox2.Text, out x) ? x : 0;
            double cutoffBm = double.TryParse(bMinusTextbox.Text, out x) ? x : 0;
            double cutoffCp = double.TryParse(cPlusTextbox.Text, out x) ? x : 0;
            double cutoffC2 = double.TryParse(cTextbox2.Text, out x) ? x : 0;
            double cutoffCm = double.TryParse(cMinusTextbox.Text, out x) ? x : 0;
            double cutoffDp = double.TryParse(dPlusTextbox.Text, out x) ? x : 0;
            double cutoffD2 = double.TryParse(dTextbox2.Text, out x) ? x : 0;
            double cutoffDm = double.TryParse(dMinusTextbox.Text, out x) ? x : 0;

            if (gradeCutoff1.Enabled && !course.setGradeCutoff(cutoffA1, cutoffB1, cutoffC1, cutoffD1))
            {
                errorTextbox.Text = "Each preceding letter grades must have a cutoff higher than the next letter grade.\n";
                errorTextbox.Text += "Constraints: A < 100, D > 0";
                errorTextbox.Visible = true;
            } else if (
                gradeCutoff2.Enabled &&
                !course.setGradeCutoff(
                    cutoffAp,
                    cutoffA2,
                    cutoffAm,
                    cutoffBp,
                    cutoffB2,
                    cutoffBm,
                    cutoffCp,
                    cutoffC2,
                    cutoffCm,
                    cutoffDp,
                    cutoffD2,
                    cutoffDm))
            {
                errorTextbox.Text = "Each preceding letter grades must have a cutoff higher than the next letter grade.\n";
                errorTextbox.Text += "Constraints: A+ < 100, D- > 0";
                errorTextbox.Visible = true;
            } else if (weightSum != 100)
            {
                errorTextbox.Text = "The sum of assignment weights must be 100.\n";
                errorTextbox.Visible = true;
            } else
            {
                if (gradeCutoff1.Enabled)
                {
                    course.setGradeCutoff(cutoffA1, cutoffB1, cutoffC1, cutoffD1);
                }
                else
                {
                    course.useLetterGradePlusMinus(true);
                    course.setGradeCutoff(
                        cutoffAp,
                        cutoffA2,
                        cutoffAm,
                        cutoffBp,
                        cutoffB2,
                        cutoffBm,
                        cutoffCp,
                        cutoffC2,
                        cutoffCm,
                        cutoffDp,
                        cutoffD2,
                        cutoffDm);
                }

                double attendanceWeight = double.TryParse(attendanceTextbox.Text, out x) ? x : 0;
                double homeworkWeight = double.TryParse(homeworkTextbox.Text, out x) ? x : 0;
                double labWeight = double.TryParse(labTextbox.Text, out x) ? x : 0;
                double discussionWeight = double.TryParse(discussionTextbox.Text, out x) ? x : 0;
                double quizWeight = double.TryParse(quizTextbox.Text, out x) ? x : 0;
                double projectWeight = double.TryParse(projectTextbox.Text, out x) ? x : 0;
                double midtermWeight = double.TryParse(midtermTextbox.Text, out x) ? x : 0;
                double finalWeight = double.TryParse(finalTextbox.Text, out x) ? x : 0;
                course.setAssignmentTypeWeights(
                    attendanceWeight,
                    homeworkWeight,
                    labWeight,
                    discussionWeight,
                    quizWeight,
                    projectWeight,
                    midtermWeight,
                    finalWeight);

                Program.courses.Add(course);
                ListView courseList = (ListView)Owner.Controls.Find("courseList", true)[0];
                ListViewItem item = new ListViewItem(course.name, 0);
                item.SubItems.Add(course.title);
                item.SubItems.Add(course.section);
                courseList.Items.Add(item);
                Close();
            }
        }
    }
}
