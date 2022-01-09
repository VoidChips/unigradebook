using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using gradebook;

namespace unigradebook
{
    static class Program
    {
        public static List<Course> courses { get; set; }
        public static Form home { get; set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>

        [STAThread]
        static void Main()
        {
            courses = new List<Course>();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            home = new Home();
            Application.Run(home);
        }
    }
}
