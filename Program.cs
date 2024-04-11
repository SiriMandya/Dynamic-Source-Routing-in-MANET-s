using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TS_AOMDV
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static MainForm _mainFormObj = null;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _mainFormObj = new MainForm();
            Application.Run(_mainFormObj);
        }
    }
}
