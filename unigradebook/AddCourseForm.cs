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
            errorLbl.Visible = false;
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

        private void doneBtn_Click(object sender, EventArgs e)
        {
            // add the course if all values are valid
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
                errorLbl.Text = "Fill all textboxes in the form.";
                errorLbl.Visible = true;
            }
            else
            {
                Course course = new Course(nameTextbox.Text, titleTextbox.Text, sectionTextbox.Text);

                if (gradeCutoff1.Enabled)
                {
                    double cutoffA = Convert.ToDouble(aTextbox1.Text);
                    double cutoffB = Convert.ToDouble(bTextbox1.Text);
                    double cutoffC = Convert.ToDouble(cTextbox1.Text);
                    double cutoffD = Convert.ToDouble(dTextbox1.Text);
                    course.setGradeCutoff(cutoffA, cutoffB, cutoffC, cutoffD);
                }
                else
                {
                    double cutoffAp = Convert.ToDouble(aPlusTextbox.Text);
                    double cutoffA = Convert.ToDouble(aTextbox2.Text);
                    double cutoffAm = Convert.ToDouble(aMinusTextbox.Text);
                    double cutoffBp = Convert.ToDouble(bPlusTextbox.Text);
                    double cutoffB = Convert.ToDouble(bTextbox2.Text);
                    double cutoffBm = Convert.ToDouble(bMinusTextbox.Text);
                    double cutoffCp = Convert.ToDouble(cPlusTextbox.Text);
                    double cutoffC = Convert.ToDouble(cTextbox2.Text);
                    double cutoffCm = Convert.ToDouble(cMinusTextbox.Text);
                    double cutoffDp = Convert.ToDouble(dPlusTextbox.Text);
                    double cutoffD = Convert.ToDouble(dTextbox2.Text);
                    double cutoffDm = Convert.ToDouble(dMinusTextbox.Text);
                    course.setGradeCutoff(
                        cutoffAp,
                        cutoffA,
                        cutoffAm,
                        cutoffBp,
                        cutoffB,
                        cutoffBm,
                        cutoffCp,
                        cutoffC,
                        cutoffCm,
                        cutoffDp,
                        cutoffD,
                        cutoffDm);
                }

                double x = 0;
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
                Close();
            }
        }
    }
}
