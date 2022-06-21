using Installer.Forms;

namespace Installer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Load += (sender, e) => FormLoad();
        }

        private void FormLoad()
        {
            this.Text       = "Installer";
            button2.Enabled = false;
            List<Form> formList = new List<Form>() { new BrowserSelection(), new LauncherSelection(), new OtherApplicationsSelector(), new Finishing_Up() };
            for (int i = 0; i < formList.Count; i++)
            {
                formList[i].TopLevel        = false;
                formList[i].AutoScroll      = true;
                formList[i].Dock            = DockStyle.Fill;
                formList[i].FormBorderStyle = FormBorderStyle.None;
            }

            panel1.Controls.Add(formList[0]);
            formList[0].Show();
            int id = 1;

            button1.Click += (sender, e) =>
            {
                if (id >= formList.Count)
                {
                    DownloadForm form = new DownloadForm();
                    form.Show();
                    this.Hide();

                    return;
                }

                formList[id - 1].Hide();
                panel1.Controls.Remove(formList[id - 1]);
                panel1.Controls.Add(formList[id]);
                formList[id].Show();
                id++;
                button2.Enabled = true;
            };

            button2.Click += (sender, e) =>
            {
                if (id == 1) return;
                id--;
                formList[id].Hide();
                panel1.Controls.Remove(formList[id]);
                panel1.Controls.Add(formList[id - 1]);

                formList[id - 1].Show();

                if (id == 1) button2.Enabled = false;
            };
        }
    }
}