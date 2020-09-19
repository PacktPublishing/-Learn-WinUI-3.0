using System.Windows.Forms;

namespace XamlIslandsSample.WinFormsCore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            SuspendLayout();
            ClientSize = new System.Drawing.Size(1200, 768);
            Text = "Host Form";
            ResumeLayout(true);

            var myHostControl = new Microsoft.Toolkit.Forms.UI.XamlHost.WindowsXamlHost();

            var entryForm = Microsoft.Toolkit.Win32.UI.XamlHost.UWPTypeFactory.CreateXamlContentByType(
                "XamlIslandsSample.UwpApp.EntryForm") as UwpApp.EntryForm;

            myHostControl.Name = "myUwpAppHostControl";
            myHostControl.Child = entryForm;
            myHostControl.Location = new System.Drawing.Point(0, 0);
            myHostControl.Size = Size;

            Controls.Add(myHostControl);
        }
    }
}