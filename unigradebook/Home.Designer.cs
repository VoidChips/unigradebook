
namespace unigradebook
{
    partial class Home
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.courses_lbl = new System.Windows.Forms.Label();
            this.addCourseBtn = new System.Windows.Forms.Button();
            this.courseList = new System.Windows.Forms.ListView();
            this.topPanel = new System.Windows.Forms.Panel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.topPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // courses_lbl
            // 
            this.courses_lbl.AutoSize = true;
            this.courses_lbl.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.courses_lbl.Location = new System.Drawing.Point(40, 35);
            this.courses_lbl.Margin = new System.Windows.Forms.Padding(0);
            this.courses_lbl.Name = "courses_lbl";
            this.courses_lbl.Size = new System.Drawing.Size(99, 26);
            this.courses_lbl.TabIndex = 0;
            this.courses_lbl.Text = "Courses";
            // 
            // addCourseBtn
            // 
            this.addCourseBtn.AutoSize = true;
            this.addCourseBtn.BackColor = System.Drawing.SystemColors.Control;
            this.addCourseBtn.Location = new System.Drawing.Point(151, 35);
            this.addCourseBtn.Name = "addCourseBtn";
            this.addCourseBtn.Size = new System.Drawing.Size(106, 30);
            this.addCourseBtn.TabIndex = 1;
            this.addCourseBtn.Text = "New Course";
            this.addCourseBtn.UseVisualStyleBackColor = false;
            this.addCourseBtn.Click += new System.EventHandler(this.addCourseBtn_Click);
            // 
            // courseList
            // 
            this.courseList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.courseList.HideSelection = false;
            this.courseList.Location = new System.Drawing.Point(40, 20);
            this.courseList.MultiSelect = false;
            this.courseList.Name = "courseList";
            this.courseList.Size = new System.Drawing.Size(720, 412);
            this.courseList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.courseList.TabIndex = 2;
            this.courseList.UseCompatibleStateImageBehavior = false;
            this.courseList.View = System.Windows.Forms.View.Details;
            // 
            // topPanel
            // 
            this.topPanel.AutoSize = true;
            this.topPanel.BackColor = System.Drawing.SystemColors.Control;
            this.topPanel.Controls.Add(this.courses_lbl);
            this.topPanel.Controls.Add(this.addCourseBtn);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.MinimumSize = new System.Drawing.Size(0, 70);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(800, 70);
            this.topPanel.TabIndex = 3;
            // 
            // bottomPanel
            // 
            this.bottomPanel.AutoSize = true;
            this.bottomPanel.Controls.Add(this.courseList);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomPanel.Location = new System.Drawing.Point(0, 70);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(40, 20, 40, 40);
            this.bottomPanel.Size = new System.Drawing.Size(800, 472);
            this.bottomPanel.TabIndex = 4;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 542);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.MinimumSize = new System.Drawing.Size(700, 450);
            this.Name = "Home";
            this.Text = "Unigradebook";
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label courses_lbl;
        private System.Windows.Forms.Button addCourseBtn;
        private System.Windows.Forms.ListView courseList;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel bottomPanel;
    }
}

