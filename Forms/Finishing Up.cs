using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Installer.Forms
{
    public partial class Finishing_Up : Form
    {
        public Finishing_Up()
        {
            InitializeComponent();

            Load += async (sender, e) =>
            {
                while (true)
                {
                    await ReloadItems();
                    await Task.Delay(1000);
                }
            };
        }

        private async Task ReloadItems()
        {
            groupBox1.Focus();
            treeView1.Nodes.Clear();

            treeView1.Nodes.Add("Browsers");
            foreach (var VARIABLE in Globals.SelectedBrowsers) treeView1.Nodes[0].Nodes.Add(VARIABLE);

            treeView1.Nodes.Add("Launchers");
            foreach (var VARIABLE in Globals.SelectedLaunchers) treeView1.Nodes[1].Nodes.Add(VARIABLE);

            treeView1.Nodes.Add("Apps");
            foreach (var VARIABLE in Globals.SelectedApplications) treeView1.Nodes[2].Nodes.Add(VARIABLE);

            treeView1.ExpandAll();
            treeView1.Focus();
        }
    }
}
