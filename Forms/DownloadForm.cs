using Installer.Online;

namespace Installer.Forms
{
    public partial class DownloadForm : Form
    {
        public DownloadForm()
        {
            InitializeComponent();

            Load += async (sender, e) => await FormLoad();
        }

        private async Task FormLoad()
        {
            Directory.CreateDirectory("./Downloads");
            progressBar1.Visible = true;
            if (Globals.SelectedBrowsers.Count > 0) await DownloadBrowsers();
            if (Globals.SelectedLaunchers.Count > 0) await DownloadLaunchers();
            if (Globals.SelectedApplications.Count > 0) await DownloadApps();

            richTextBox1.Text = "Everything is downloaded.\nClosing app in 2 seconds ...";
            await Task.Delay(2000);

            Application.Exit();
        }

        private async Task DownloadBrowsers()
        {
            List<string>     items              = await ServerCom.ReadTextFromFile("https://sethdiscordbot.000webhostapp.com/Storage/Installers/DownloadLinks/Browsers");
            foreach (var VARIABLE in items)
            {
                if (VARIABLE.Length < 2) continue;
                string name = VARIABLE.Split(',')[0];
                string url  = VARIABLE.Split(',')[1];

                if (Globals.SelectedBrowsers.Contains(name))
                {
                    richTextBox1.Text = $"Downloading {name} ...";
                    await ServerCom.DownloadFileAsync(url, $"./Downloads/{name}.exe", progressBar1);
                }
            }
        }

        private async Task DownloadLaunchers()
        {
            List<string>     items    = await ServerCom.ReadTextFromFile("https://sethdiscordbot.000webhostapp.com/Storage/Installers/DownloadLinks/Launchers");
            foreach (var VARIABLE in items)
            {
                if (VARIABLE.Length <= 2) continue;
                string name = VARIABLE.Split(',')[0];
                string url  = VARIABLE.Split(',')[1];

                if (Globals.SelectedLaunchers.Contains(name))
                {
                    richTextBox1.Text = $"Downloading {name} ...";
                    await ServerCom.DownloadFileAsync(url, $"./Downloads/{name}.exe", progressBar1);
                }
            }
        }

        private async Task DownloadApps()
        {
            List<string>     items    = await ServerCom.ReadTextFromFile("https://sethdiscordbot.000webhostapp.com/Storage/Installers/DownloadLinks/OtherApps");
            foreach (var VARIABLE in items)
            {
                if (VARIABLE.Length <= 2) continue;
                string name = VARIABLE.Split(',')[0];
                string url  = VARIABLE.Split(',')[1];

                if (Globals.SelectedApplications.Contains(name))
                {
                    richTextBox1.Text = $"Downloading {name} ...";
                    await ServerCom.DownloadFileAsync(url, $"./Downloads/{name}.exe", progressBar1);
                }
            }
        }
    }
}
