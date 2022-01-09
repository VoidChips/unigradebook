using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Diagnostics;
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

            // create table if it doesn't exist
            string cs = "Data Source=./gradebook.db";      
            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);
            cmd.CommandText =
                "CREATE TABLE IF NOT EXISTS courses (" +
                "name TEXT NOT NULL, title TEXT NOT NULL, section TEXT NOT NULL);";
            cmd.ExecuteNonQuery();
            con.Close();

            home = new Home();
            Application.Run(home);
        }
    }
}
