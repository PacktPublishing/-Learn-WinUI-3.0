using System;
using System.Windows.Forms;

namespace XamlIslandsSample.WinFormsCore
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //using (new UwpApp.App())
            //{
            //    UwpApp.App app = new UwpApp.App();
            //    app.InitializeComponent();
            //    app.Run();
            //}
        }
    }
}