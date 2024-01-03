using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace SandStrider_ExternalTool
{
    public partial class Form1 : Form
    {
        string path;
        public Form1()
        {
            InitializeComponent();
            path = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files | *.png"; // file types, that will be allowed to upload
            dialog.Multiselect = false; // allow/deny user to upload more than one file at a time
            if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
            {
                path = dialog.FileName;
                using (Stream st = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    pictureBox1.Image = Image.FromStream(st);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.InitialDirectory = "../../../../";
                sfd.FileName = "customStarterItem";
                sfd.Filter = "SSI files (*.ssi) | *.ssi";
                sfd.FilterIndex = 0;
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        sw.WriteLine(path + "\n" + stat1.Text + "\n" + stat2.Text + "\n" + stat3.Text + "\n" + stat4.Text
                            + "\n" + stat5.Text + "\n" + stat6.Text + "\n" + stat7.Text + "\n" + stat8.Text + "\n" + stat9.Text
                            + "\n" + stat10.Text + "\n" + stat11.Text + "\n" + stat12.Text + "\n" + stat13.Text + "\n" + stat14.Text);
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            path = "";
        }
    }
}