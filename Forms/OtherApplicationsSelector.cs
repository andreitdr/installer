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
    public partial class OtherApplicationsSelector : Form
    {
        public OtherApplicationsSelector()
        {
            InitializeComponent();

            Load += (async (sender, args) =>
                        {
                        test:
                            try
                            {
                                List<string> items = await Online.ServerCom.ReadTextFromFile("https://sethdiscordbot.000webhostapp.com/Storage/Installers/OtherAppsList");
                                label1.Visible = false;
                                items.Sort();
                                foreach (var VARIABLE in items) checkedListBox1.Items.Add(VARIABLE);
                                checkedListBox1.CheckOnClick = true;
                                checkedListBox1.ItemCheck += (sender2, e2) =>
                                {
                                    if (e2.NewValue == CheckState.Checked)
                                        Globals.SelectedApplications.Add(items[e2.Index].Replace("\n", "").Replace("\r", ""));
                                    else if (e2.NewValue == CheckState.Unchecked) Globals.SelectedApplications.Remove(items[e2.Index].Replace("\n", "").Replace("\r", ""));
                                };
                            }
                            catch
                            {
                                goto test;
                            }
                        });
        }

        public void HighlightSearchedItem(string item)
        {
            checkedListBox1.SetSelected(checkedListBox1.Items.IndexOf(item), true);
        }
    }
}
