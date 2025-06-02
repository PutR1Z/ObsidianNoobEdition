using Markdig;
using NotesApp;
using System;
using System.IO;
using System.Windows.Forms;

namespace NotesApp
{
    internal static class MainFormHelpers
    {

        [STAThread]
        static void Main3()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        [STAThread]
        private static void Main1()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}