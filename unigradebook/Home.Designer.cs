
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
            this.SuspendLayout();
            // 
            // courses_lbl
            // 
            this.courses_lbl.AutoSize = true;
            this.courses_lbl.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.courses_lbl.Location = new System.Drawing.Point(359, 30);
            this.courses_lbl.Name = "courses_lbl";
            this.courses_lbl.Size = new System.Drawing.Size(116, 32);
            this.courses_lbl.TabIndex = 0;
            this.courses_lbl.Text = "Courses";
            // 
            // addCourseBtn
            // 
            this.addCourseBtn.Location = new System.Drawing.Point(496, 30);
            this.addCourseBtn.Name = "addCourseBtn";
            this.addCourseBtn.Size = new System.Drawing.Size(106, 29);
            this.addCourseBtn.TabIndex = 1;
            this.addCourseBtn.Text = "New Course";
            this.addCourseBtn.UseVisualStyleBackColor = true;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.addCourseBtn);
            this.Controls.Add(this.courses_lbl);
            this.Name = "Home";
            this.Text = "Unigradebook";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label courses_lbl;
        private System.Windows.Forms.Button addCourseBtn;
    }
}

