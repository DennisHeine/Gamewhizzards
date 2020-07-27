using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameWhizzards_Launcher
{
    public partial class Form1 : Form
    {
        public String DLURL = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                DLURL = File.ReadAllText(".\\version.info");
            } catch (Exception ex) { DLURL = ""; }

            String NewURL = ((new WebClient()).DownloadString("https://gamewhizzards.ml/currentversion"));
            if (NewURL != DLURL)
            {

                label2.Text = "Rdy for Update";
                setButton(button1, true);                
            }
            else
            {
                label2.Text = "Ready to play.";
                button1.Enabled = true;
            }
        }



        public void SetProgressStyle(Control progressBar, ProgressBarStyle style)
        {
            if(progressBar.InvokeRequired)
            {
                progressBar.Invoke(new ProgressStyleDelegate(SetProgressStyle), new Object[] { progressBar, style });
            }
            else
            { 
                ((ProgressBar)progressBar).Style = style;
            }
        }

        void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            SetProgressStyle(progressBar1,ProgressBarStyle.Marquee);
            setLabel(label2,"Extracting");

            try
            {

                System.IO.Directory.Delete(".\\Client", true);
            }
            catch (Exception ex) { }
            try
            { 
                if (!Directory.Exists(".\\Client"))
                    Directory.CreateDirectory(".\\Client");


                ZipFile.ExtractToDirectory(Path.GetTempPath() + "\\gw_update.zip", ".\\Client\\");
                setLabel(label2, "Finished.");
                setButton(button1, true);
                SetProgressStyle(progressBar1, ProgressBarStyle.Continuous);
                setProgress(progressBar1, 0);
                String NewURL = ((new WebClient()).DownloadString("https://gamewhizzards.ml/currentversion"));
                File.WriteAllText(".\\version.info", NewURL);
               
            }catch(Exception ex)
            {
                MessageBox.Show("Error extracting Data");
                Application.Exit();
            }

        }
        WebClient wcDownload = null;
        public delegate void ControlStringConsumer(Control control, int progress);
        public delegate void SetLableDelegate(Control control, String text);
        public delegate void ButtonDelegte(Control control, bool text);
        public delegate void ProgressStyleDelegate(Control control, ProgressBarStyle text);
        

        private void DoUpdate()
        {
                       
            wcDownload = new WebClient();
            wcDownload.DownloadFileCompleted += DownloadFileCompleted;
            wcDownload.DownloadProgressChanged += ProgressChanged;
            try { 
                wcDownload.DownloadFileAsync(new Uri("https://www.dropbox.com/s/ztzktbej2b4djjb/GameWhizzards%20devbuild.zip?dl=1"), Path.GetTempPath() + "\\gw_update.zip");
            }catch(Exception ex)
            {
                MessageBox.Show("Error downloading update file.");
                Application.Exit();
            }
        }

        public void setProgress(Control progessBar, int percent)
        {            
            if(progessBar.InvokeRequired)
                progessBar.Invoke(new ControlStringConsumer(setProgress), new Object[]{ progessBar, percent });
            else
            {
                ((ProgressBar)progessBar).Value = percent;
            }
        }

        public void setLabel(Control label, String text)
        {
            if(label.InvokeRequired)
            {
                label.Invoke(new SetLableDelegate(setLabel), new object[] { label, text });
            }
            else
            {
                ((Label)label).Text = text;
            }
        }

        public void setButton(Control button, bool Enabled)
        {
            if (button.InvokeRequired)
                button.Invoke(new ButtonDelegte(setButton), new Object[] { button, Enabled });
            else
            {
                ((Button)button).Enabled = Enabled;
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            setProgress(progressBar1, e.ProgressPercentage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DLURL = File.ReadAllText(".\\version.info");
            }
            catch (Exception ex) { DLURL = ""; }
            String NewURL = ((new WebClient()).DownloadString("https://gamewhizzards.ml/currentversion"));

            if (DLURL != NewURL)
            { 

            System.Threading.Thread updateThread = new System.Threading.Thread(DoUpdate);
                updateThread.Start();
                setButton(button1, false);
                setLabel(label2, "Updating...");
            }
            else
            {

                Process[] p = Process.GetProcessesByName("GameWhizzards.exe");
                if (p.Length > 0)
                    Application.Exit();
                else
                {
                    try
                    {
                        Process.Start(".\\Client\\GameWhizzards.exe");
                    } catch (Exception ex)
                    {
                        MessageBox.Show("Can not start client.");
                    }
                    Application.Exit();
                }
            }
        }
    }
}
