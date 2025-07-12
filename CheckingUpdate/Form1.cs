using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckingUpdate
{
    public partial class Form1 : Form
    {
        public Form1 ()
        {
            InitializeComponent();
        }
        private async Task checkUpdate ()
        {
            string currentVersion = label1.Text;//version no.
            string versionUrl = "https://raw.githubusercontent.com/vhertuds26/CheckingUpdate/refs/heads/master/version.txt";
            string downloadUrl = "https://github.com/user-attachments/files/21195892/CheckingUpdateInstaller.zip";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string latestVersion = await client.GetStringAsync(versionUrl);
                    latestVersion = latestVersion.Trim();

                    if (Version.TryParse(latestVersion, out Version latest) &&
                        Version.TryParse(currentVersion, out Version current))
                    {
                        if( latest > current)
                        {
                            DialogResult result = MessageBox.Show($"A new version ({latestVersion})is available. Do you want to Update?",
                                "Update Available",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);
                            if (result == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                                {
                                    FileName = downloadUrl,
                                    UseShellExecute = true
                                });
                                Application.Exit();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to parse version information.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Could not check for updates:\n" + ex.Message); 
            }
        }
        private async void button1_Click ( object sender, EventArgs e )
        {
            await checkUpdate(); 
        }

    }
}
