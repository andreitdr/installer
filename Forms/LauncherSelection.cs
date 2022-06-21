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
    public partial class LauncherSelection : Form
    {
        public LauncherSelection()
        {
            InitializeComponent();
            Load += (async (sender, args) =>
                        {
                        test:
                            try
                            {
                                List<string> items = await Online.ServerCom.ReadTextFromFile("https://sethdiscordbot.000webhostapp.com/Storage/Installers/LauncherList");
                                label1.Visible = false;
                                items.Sort();
                                foreach (var VARIABLE in items) checkedListBox1.Items.Add(VARIABLE);
                                checkedListBox1.CheckOnClick = true;
                                checkedListBox1.ItemCheck += (sender2, e2) =>
                                {
                                    if (e2.NewValue == CheckState.Checked)
                                        Globals.SelectedLaunchers.Add(items[e2.Index].Replace("\n", "").Replace("\r", ""));
                                    else if (e2.NewValue == CheckState.Unchecked) Globals.SelectedLaunchers.Remove(items[e2.Index].Replace("\n", "").Replace("\r", ""));
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
