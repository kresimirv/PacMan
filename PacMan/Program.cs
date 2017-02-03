using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PacMan
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (frmMain f = new frmMain())
            {
                f.Show();
                f.GameLoop();
            }
            //Application.Run(new frmMain());

        }
    }
}
