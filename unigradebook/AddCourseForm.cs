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
        public AddCourseForm()
        {
            InitializeComponent();
            letterGradeType1Btn.Checked = true;
            gradeCutoff2.Enabled = false;
            errorLbl.Visible = false;
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
            } else
            {
                Course course = new Course(nameTextbox.Text, titleTextbox.Text, sectionTextbox.Text);
                
                if (gradeCutoff1.Enabled)
                {
                    double cutoffA = Convert.ToDouble(aTextbox1.Text);
                    double cutoffB = Convert.ToDouble(bTextbox1.Text);
                    double cutoffC = Convert.ToDouble(cTextbox1.Text);
                    double cutoffD = Convert.ToDouble(dTextbox1.Text);
                    course.setGradeCutoff(cutoffA, cutoffB, cutoffC, cutoffD);
                } else
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

                double attendanceWeight = Convert.ToDouble(attendanceTextbox.Text);
                double homeworkWeight = Convert.ToDouble(homeworkTextbox.Text);
                double labWeight = Convert.ToDouble(labTextbox.Text);
                double discussionWeight = Convert.ToDouble(discussionTextbox.Text);
                double quizWeight = Convert.ToDouble(quizTextbox.Text);
                double projectWeight = Convert.ToDouble(projectTextbox.Text);
                double midtermWeight = Convert.ToDouble(midtermTextbox.Text);
                double finalWeight = Convert.ToDouble(finalTextbox.Text);
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
