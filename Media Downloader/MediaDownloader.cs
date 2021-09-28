using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Instadown;
using VideoLibrary;
using MediaToolkit;
using System.Diagnostics;

namespace Media_Downloader
{
    public partial class MediaDownloader : Form
    {
        MediaClass mc;
        string ingpath;
        bool FirstSettings = false, FirstThema = false, disableWeb = false;
        Boolean format = true;

        public MediaDownloader()
        {
            InitializeComponent();
            mc = new MediaClass();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButtonYTMP3.Checked = true;

            if (comboBoxLanguange.Text == "English" || comboBoxLanguange.Text == "") { labelDisableEnableWeb.Text = "Enable Web:"; }
            else { labelDisableEnableWeb.Text = "Web Etkinlştr:"; }
            buttonDisableWeb.Visible = false;
            buttonEnableWeb.Visible = true;
            disableWeb = true;
            labelWebDisabled.Visible = true;
            labelytdisable.Visible = true;
        }

        void GoInstagram()
        {
            panelSelect.Hide();
            panelSettings.Hide();
            panelYoutube.Hide();

            panelInstagram.Show();
            if(disableWeb == false) { webBrowser1.Navigate("www.instagram.com"); }
            else { return; }
        }

        void GoYoutube()
        {
            panelSelect.Hide();
            panelSettings.Hide();
            panelInstagram.Hide();

            panelYoutube.Show();
            if(disableWeb == false) { webBrowser2.Navigate("www.youtube.com"); }
            else { return; }
        }

        void GoSettings()
        {
            panelSelect.Hide();
            panelInstagram.Hide();
            panelYoutube.Hide();

            panelSettings.Show();
        }

        void GoSelect()
        {
            panelSettings.Hide();
            panelInstagram.Hide();
            panelYoutube.Hide();

            panelSelect.Show();
        }

        private void pictureBoxInstagram_Click(object sender, EventArgs e)
        {
            GoInstagram();
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GoSettings();

            if(comboBoxLanguange.Text == "")
            {
                FirstSettings = true;
                comboBoxLanguange.Text = "English";
            }
            else if(comboBoxLanguange.Text == "English" || comboBoxLanguange.Text == "Türkçe")
            {
                FirstSettings = false;
            }

            if(comboBoxThema.Text == "")
            {
                FirstThema = true;
                comboBoxThema.Text = "Royal Blue";
            }   
            else if(comboBoxThema.Text == "Royal Blue" || comboBoxThema.Text == "Black White")
            {
                FirstThema = false;
            }
        }

        private void pictureBoxInstagram2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://instagram.com");
        }

        private void buttonSettingsBack_Click(object sender, EventArgs e)
        {
            GoSelect();
        }

        private void buttonInstagramBack_Click_1(object sender, EventArgs e)
        {
            GoSelect();
        }

        private void buttonInstagramPath_Click(object sender, EventArgs e)
        {
            if(comboBoxLanguange.Text == "")
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                fbd.Description = "Select the folder you want to save.";
                fbd.ShowNewFolderButton = true;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBoxInstagramPath.Text = fbd.SelectedPath;
                }
            }
            else if(comboBoxLanguange.Text == "English")
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                fbd.Description = "Select the folder you want to save.";
                fbd.ShowNewFolderButton = true;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBoxInstagramPath.Text = fbd.SelectedPath;
                }
            }
            else if(comboBoxLanguange.Text == "Türkçe")
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                fbd.Description = "Kaydetmek istediğiniz klasörü seçin.";
                fbd.ShowNewFolderButton = true;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBoxInstagramPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void buttonInstagramImage_Click(object sender, EventArgs e)
        {
            ingpath = textBoxInstagramPath.Text;
            mc.inputUrl = textBoxInstagramURL.Text;

            try
            {
                if (textBoxInstagramPath.Text.Length < 1 && textBoxInstagramURL.Text.Length > 1)
                {
                    if(comboBoxLanguange.Text == "") { MessageBox.Show("Please fill in the path section correctly and try again!", "Instagram Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else if(comboBoxLanguange.Text == "English") { MessageBox.Show("Please fill in the path section correctly and try again!", "Instagram Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else if(comboBoxLanguange.Text == "Türkçe") { MessageBox.Show("Lütfen yol bölümünü doğru bir şekilde doldurun ve tekrar deneyin!", "Instagram İndirici Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                else if(textBoxInstagramPath.Text.Length > 1 && textBoxInstagramURL.Text.Length < 1)
                {
                    if(comboBoxLanguange.Text == "") { MessageBox.Show("Please fill in the Url section correctly and try again!", "Instagram Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else if(comboBoxLanguange.Text == "English") { MessageBox.Show("Please fill in the Url section correctly and try again!", "Instagram Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else if(comboBoxLanguange.Text == "Türkçe") { MessageBox.Show("Lütfen Url bölümünü doğru bir şekilde doldurun ve tekrar deneyin!", "Instagram İndirici Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                else
                {
                    GetTitleInstagram();

                    progressBarInstagram.Visible = true;
                    labelInstagramProgress.Visible = true;

                    mc.DownloadImage(ingpath);

                    progressBarInstagram.Value = 100;
                    labelInstagramProgress.Text = "%100";

                    if(comboBoxLanguange.Text == "") { MessageBox.Show("Instagram image successfully installed.", "Instagram Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else if(comboBoxLanguange.Text == "English") { MessageBox.Show("Instagram image successfully installed.", "Instagram Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else if(comboBoxLanguange.Text == "Türkçe") { MessageBox.Show("Instagram resmi başarıyla yüklendi.", "Instagram İndirici", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
            }
            catch
            {
                if(comboBoxLanguange.Text == "") { MessageBox.Show("Please fill in the blanks correctly and try again!", "Instagram Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else if(comboBoxLanguange.Text == "English") { MessageBox.Show("Please fill in the blanks correctly and try again!", "Instagram Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else if(comboBoxLanguange.Text == "Türkçe") { MessageBox.Show("Lütfen boşlukları doğru bir şekilde doldurun ve tekrar deneyin!", "Instagram İndirici Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void buttonInstagramVideo_Click(object sender, EventArgs e)
        {
            ingpath = textBoxInstagramPath.Text;
            mc.inputUrl = textBoxInstagramURL.Text;

            try
            {
                if (textBoxInstagramPath.Text.Length < 1 && textBoxInstagramURL.Text.Length > 1)
                {
                    if (comboBoxLanguange.Text == "") { MessageBox.Show("Please fill in the path section correctly and try again!", "Instagram Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else if (comboBoxLanguange.Text == "English") { MessageBox.Show("Please fill in the path section correctly and try again!", "Instagram Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else if (comboBoxLanguange.Text == "Türkçe") { MessageBox.Show("Lütfen yol bölümünü doğru bir şekilde doldurun ve tekrar deneyin!", "Instagram İndirici Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                else if(textBoxInstagramPath.Text.Length > 1 && textBoxInstagramURL.Text.Length < 1)
                {
                    if (comboBoxLanguange.Text == "") { MessageBox.Show("Please fill in the Url section correctly and try again!", "Instagram Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else if (comboBoxLanguange.Text == "English") { MessageBox.Show("Please fill in the Url section correctly and try again!", "Instagram Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else if (comboBoxLanguange.Text == "Türkçe") { MessageBox.Show("Lütfen Url bölümünü doğru bir şekilde doldurun ve tekrar deneyin!", "Instagram İndirici Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                else
                {
                    GetTitleInstagram();

                    progressBarInstagram.Visible = true;
                    labelInstagramProgress.Visible = true;

                    mc.DownloadVideo(ingpath);

                    progressBarInstagram.Value = 100;
                    labelInstagramProgress.Text = "%100";

                    if (comboBoxLanguange.Text == "") { MessageBox.Show("Instagram video successfully installed.", "Instagram Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else if (comboBoxLanguange.Text == "English") { MessageBox.Show("Instagram video successfully installed.", "Instagram Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else if (comboBoxLanguange.Text == "Türkçe") { MessageBox.Show("Instagram video başarıyla yüklendi.", "Instagram İndirici", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
            }
            catch
            {
                if (comboBoxLanguange.Text == "") { MessageBox.Show("Please fill in the blanks correctly and try again!", "Instagram Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else if (comboBoxLanguange.Text == "English") { MessageBox.Show("Please fill in the blanks correctly and try again!", "Instagram Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else if (comboBoxLanguange.Text == "Türkçe") { MessageBox.Show("Lütfen boşlukları doğru bir şekilde doldurun ve tekrar deneyin!", "Instagram İndirici Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void pictureBoxYoutube_Click(object sender, EventArgs e)
        {
            GoYoutube();
        }

        private void pictureBoxTiktok_Click(object sender, EventArgs e)
        {
            if(comboBoxLanguange.Text == "") { MessageBox.Show("Tiktok Soon", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else if(comboBoxLanguange.Text == "English") { MessageBox.Show("Tiktok Soon", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else if(comboBoxLanguange.Text == "Türkçe") { MessageBox.Show("Tiktok Yakında", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void pictureBoxWeb_Click(object sender, EventArgs e)
        {
            if (comboBoxLanguange.Text == "") { MessageBox.Show("Web Soon", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else if (comboBoxLanguange.Text == "English") { MessageBox.Show("Web Soon", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else if (comboBoxLanguange.Text == "Türkçe") { MessageBox.Show("Web Yakında", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void comboBoxLanguange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FirstSettings)
            {
                FirstSettings = false;
            }
            else if(comboBoxLanguange.Text == "English")
            {
                MessageBox.Show("Your language is set to English.", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LanguangeEnglish();
            }
            else if(comboBoxLanguange.Text == "Türkçe")
            {
                MessageBox.Show("Diliniz türkçe olarak ayarlandı.","Media Downloader",MessageBoxButtons.OK,MessageBoxIcon.Information);
                LanguangeTurkish();
            }
        }

        void LanguangeEnglish()
        {
            settingsToolStripMenuItem.Text = "Options";
            quitToolStripMenuItem.Text = "Quit";
            labelSelectMessage.Text = "Select and download videos, photos and files from web.";
            labelSelectUpdate.Text = "Update Available Click Here To Download";
            labelSelectVersion.Text = "Version: 1.0";
            labelYT.Text = "Youtube Downloader";
            labelYTPath.Text = "Path:";
            buttonYTDownload.Text = "Download";
            labelYT2.Text = "You can change url from this web";
            toolStripStatusYoutubeLabelStatus.Text = "Status:";
            labelSettings.Text = "Options";
            labelSettingsVersion.Text = "Version: 1.0";
            labelLanguange.Text = "Languange:";
            labelThema.Text = "Thema:";
            labelSettingsUpdate.Text = "Update Available Click Here To Download";
            labelInstagram.Text = "Instagram Downloader";
            labelPath.Text = "Path:";
            buttonInstagramImage.Text = "Download Image";
            buttonInstagramVideo.Text = "Download Video";
            labelInstagramMessage.Text = "You can change url from this web";
            toolStripStatusInstagramLabelStatus.Text = "Status:";
            toolStripStatusInstagramLabelStatusMessage.Text = "Not Ready To Download";
            toolStripStatusYoutubeLabelStatusMessage.Text = "Not Ready To Download";
            buttonDisableWeb.Text = "Disable";
            buttonEnableWeb.Text = "Enable";

            if(disableWeb == false) { labelDisableEnableWeb.Text = "Disable Web:"; }
            else if(disableWeb == true) { labelDisableEnableWeb.Text = "Enable Web:"; }
        }

        void LanguangeTurkish()
        {
            settingsToolStripMenuItem.Text = "Seçenekler";
            quitToolStripMenuItem.Text = "Çıkış";
            labelSelectMessage.Text = "Web'den videolar, fotoğraflar ve dosyalar seçin ve indirin.";
            labelSelectUpdate.Text = "Güncelleme Mevcut İndirmek İçin Buraya Tıklayın";
            labelSelectVersion.Text = "Versiyon: 1.0";
            labelYT.Text = "Youtube İndirici";
            labelYTPath.Text = "Yol:";
            buttonYTDownload.Text = "Indir";
            labelYT2.Text = "Url'yi bu web'den değiştirebilirsiniz";
            toolStripStatusYoutubeLabelStatus.Text = "Durum:";
            labelSettings.Text = "Seçenekler";
            labelSettingsVersion.Text = "Versiyon: 1.0";
            labelLanguange.Text = "       Diller:";
            labelThema.Text = "Tema:";
            labelSettingsUpdate.Text = "Güncelleme Mevcut İndirmek İçin Buraya Tıklayın";
            labelInstagram.Text = "Instagram İndirici";
            labelPath.Text = "Yol:";
            buttonInstagramImage.Text = "Fotograf Indir";
            buttonInstagramVideo.Text = "Video Indir";
            labelInstagramMessage.Text = "Url'yi bu web'den değiştirebilirsiniz";
            toolStripStatusInstagramLabelStatus.Text = "Durum:";
            toolStripStatusInstagramLabelStatusMessage.Text = "İndirmek İçin Hazır Değil";
            toolStripStatusYoutubeLabelStatusMessage.Text = "İndirmek İçin Hazır Değil";
            buttonDisableWeb.Text = "Devre Dışı Bırak";
            buttonEnableWeb.Text = "Etkinleştir";

            if (disableWeb == false) { labelDisableEnableWeb.Text = "Web Kapat:"; }
            else if (disableWeb == true) { labelDisableEnableWeb.Text = "Web Etkinlştr:"; }
        }

        private void comboBoxThema_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FirstThema)
            {
                FirstThema = false;
            }
            else if(comboBoxThema.Text == "Royal Blue")
            {
                /*if (comboBoxLanguange.Text == "") { }
                else if (comboBoxLanguange.Text == "English") { }
                else if (comboBoxLanguange.Text == "Turkish") { }*/
                if (comboBoxLanguange.Text == "") { MessageBox.Show("Thema Soon", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else if (comboBoxLanguange.Text == "English") { MessageBox.Show("Thema Soon", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else if (comboBoxLanguange.Text == "Türkçe") { MessageBox.Show("Tema Yakında", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            else if(comboBoxThema.Text == "Black White")
            {
                if (comboBoxLanguange.Text == "") { MessageBox.Show("Thema Soon", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else if (comboBoxLanguange.Text == "English") { MessageBox.Show("Thema Soon", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else if (comboBoxLanguange.Text == "Türkçe") { MessageBox.Show("Tema Yakında", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
        }

        private void pictureBoxYoutube2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://youtube.com");
        }

        private void buttonYTBack_Click(object sender, EventArgs e)
        {
            GoSelect();
        }

        private void buttonYTPath_Click(object sender, EventArgs e)
        {
            if (comboBoxLanguange.Text == "")
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                fbd.Description = "Select the folder you want to save.";
                fbd.ShowNewFolderButton = true;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBoxYTPath.Text = fbd.SelectedPath;
                }
            }
            else if (comboBoxLanguange.Text == "English")
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                fbd.Description = "Select the folder you want to save.";
                fbd.ShowNewFolderButton = true;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBoxYTPath.Text = fbd.SelectedPath;
                }
            }
            else if (comboBoxLanguange.Text == "Türkçe")
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                fbd.Description = "Kaydetmek istediğiniz klasörü seçin.";
                fbd.ShowNewFolderButton = true;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBoxYTPath.Text = fbd.SelectedPath;
                }
            }
        }

        void InstagramCheck()
        {
            GetTitleInstagram();

            if (textBoxInstagramPath.Text.Length < 1 && textBoxInstagramURL.Text.Length > 1)
            {
                /*if (comboBoxLanguange.Text == "") { }
                else if (comboBoxLanguange.Text == "English") { }
                else if (comboBoxLanguange.Text == "Turkish") { }*/
                if (comboBoxLanguange.Text == "") { toolStripStatusInstagramLabelStatusMessage.Text = "Not Ready To Download"; }
                else if (comboBoxLanguange.Text == "English") { toolStripStatusInstagramLabelStatusMessage.Text = "Not Ready To Download"; }
                else if (comboBoxLanguange.Text == "Türkçe") { toolStripStatusInstagramLabelStatusMessage.Text = "İndirmek İçin Hazır Değil"; }
            }
            else if (textBoxInstagramPath.Text.Length > 1 && textBoxInstagramURL.Text.Length < 1)
            {
                if (comboBoxLanguange.Text == "") { toolStripStatusInstagramLabelStatusMessage.Text = "Not Ready To Download"; }
                else if (comboBoxLanguange.Text == "English") { toolStripStatusInstagramLabelStatusMessage.Text = "Not Ready To Download"; }
                else if (comboBoxLanguange.Text == "Türkçe") { toolStripStatusInstagramLabelStatusMessage.Text = "İndirmek İçin Hazır Değil"; }
            }
            else
            {
                if (comboBoxLanguange.Text == "") { toolStripStatusInstagramLabelStatusMessage.Text = "Ready To Download"; }
                else if (comboBoxLanguange.Text == "English") { toolStripStatusInstagramLabelStatusMessage.Text = "Ready To Download"; }
                else if (comboBoxLanguange.Text == "Türkçe") { toolStripStatusInstagramLabelStatusMessage.Text = "İndirmek İçin Hazır"; }
            }
        }

        void YoutubeCheck()
        {
            /*if(comboBoxLanguange.Text == "" || comboBoxLanguange.Text == "English") { }
            else { }*/
            GetTitle();

            if(textBoxYTPath.Text.Length < 1 && textBoxYTUrl.Text.Length > 1)
            {
                if (comboBoxLanguange.Text == "" || comboBoxLanguange.Text == "English") { toolStripStatusYoutubeLabelStatusMessage.Text = "Not Ready To Download"; }
                else { toolStripStatusYoutubeLabelStatusMessage.Text = "İndirmek İçin Hazır Değil"; }
            }
            else if(textBoxYTPath.Text.Length > 1 && textBoxYTUrl.Text.Length < 1)
            {
                if (comboBoxLanguange.Text == "" || comboBoxLanguange.Text == "English") { toolStripStatusYoutubeLabelStatusMessage.Text = "Not Ready To Download"; }
                else { toolStripStatusYoutubeLabelStatusMessage.Text = "İndirmek İçin Hazır Değil"; }
            }
            else
            {
                if (comboBoxLanguange.Text == "" || comboBoxLanguange.Text == "English") { toolStripStatusYoutubeLabelStatusMessage.Text = "Ready To Download"; }
                else { toolStripStatusYoutubeLabelStatusMessage.Text = "İndirmek İçin Hazır"; }
            }
        }

        private void textBoxInstagramURL_TextChanged(object sender, EventArgs e)
        {
            if(disableWeb == false) { webBrowser1.Navigate(textBoxInstagramURL.Text); }
            else { return; }

            InstagramCheck();
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if(disableWeb == false) { textBoxInstagramURL.Text = webBrowser1.Url.ToString(); }
            else { return; }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            GoInstagram();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            GoYoutube();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if (comboBoxLanguange.Text == "" || comboBoxLanguange.Text == "English") { MessageBox.Show("Tiktok Soon", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else { MessageBox.Show("Tiktok Yakında", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void textBoxInstagramPath_TextChanged(object sender, EventArgs e)
        {
            InstagramCheck();
        }

        private async void buttonYTDownload_Click(object sender, EventArgs e)
        {
            /*if (comboBoxLanguange.Text == "" || comboBoxLanguange.Text == "English") { }
            else { }*/

            try
            {
                if (textBoxYTPath.Text.Length < 1 && textBoxYTUrl.Text.Length > 1)
                {
                    if (comboBoxLanguange.Text == "" || comboBoxLanguange.Text == "English") { MessageBox.Show("Please fill in the path section correctly and try again!", "Youtube Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else { MessageBox.Show("Lütfen yol bölümünü doğru bir şekilde doldurun ve tekrar deneyin!", "Youtube İndirici Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                else if (textBoxYTPath.Text.Length > 1 && textBoxYTUrl.Text.Length < 1)
                {
                    if (comboBoxLanguange.Text == "" || comboBoxLanguange.Text == "English") { MessageBox.Show("Please fill in the Url section correctly and try again!", "Youtube Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else { MessageBox.Show("Lütfen URL bölümünü doğru bir şekilde doldurun ve tekrar deneyin!", "Youtube İndirici Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                else
                {
                    GetTitle();

                    progressBarYT.Visible = true;
                    labelYTProgress.Visible = true;

                    var yt = YouTube.Default;
                    var video = await yt.GetVideoAsync(textBoxYTUrl.Text);
                    File.WriteAllBytes(textBoxYTPath.Text + @"\" + video.FullName, await video.GetBytesAsync());

                    var inputfile = new MediaToolkit.Model.MediaFile { Filename = textBoxYTPath.Text + @"\" + video.FullName };
                    var outputfile = new MediaToolkit.Model.MediaFile { Filename = $"{textBoxYTPath.Text + @"\" + video.FullName}.mp3" };

                    using (var enging = new Engine())
                    {
                        enging.GetMetadata(inputfile);
                        enging.Convert(inputfile, outputfile);
                    }

                    if (format == true)
                    {
                        File.Delete(textBoxYTPath.Text + @"\" + video.FullName);
                    }
                    else
                    {
                        File.Delete($"{textBoxYTPath.Text + @"\" + video.FullName}.mp3");
                    }

                    progressBarYT.Value = 100;
                    labelYTProgress.Text = "%100";

                    if (comboBoxLanguange.Text == "" || comboBoxLanguange.Text == "English") { MessageBox.Show("Youtube video successfully installed.", "Youtube Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else { MessageBox.Show("Youtube videosu başarıyla yüklendi.", "Youtube İndirici", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
            }
            catch
            {
                if (comboBoxLanguange.Text == "" || comboBoxLanguange.Text == "English") { MessageBox.Show("Please fill in the blanks correctly and try again!", "Youtube Downloader Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else { MessageBox.Show("Lütfen boşlukları doğru bir şekilde doldurun ve tekrar deneyin!", "Youtube İndirici Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        void GetTitle()
        {
            try
            {
                WebRequest request = HttpWebRequest.Create(textBoxYTUrl.Text);
                WebResponse response;
                response = request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                string coming = sr.ReadToEnd();
                int beginning = coming.IndexOf("<title>") + 7;
                int finish = coming.Substring(beginning).IndexOf("</title>");
                string comingInfo = coming.Substring(beginning, finish);
                labelYTTitle.Text = (comingInfo);
            }
            catch { return; }
        }

        void GetTitleInstagram()
        {
            try
            {
                WebRequest request = HttpWebRequest.Create(textBoxInstagramURL.Text);
                WebResponse response;
                response = request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                string coming = sr.ReadToEnd();
                int beginning = coming.IndexOf("<title>") + 7;
                int finish = coming.Substring(beginning).IndexOf("</title>");
                string comingInfo = coming.Substring(beginning, finish);
                labelInstagramTitle.Text = (comingInfo);
            }
            catch { return; }
        }

        private void textBoxYTUrl_TextChanged(object sender, EventArgs e)
        {
            YoutubeCheck();

            if(disableWeb == false) { webBrowser2.Navigate(textBoxYTUrl.Text); }
            else { return; }
        }

        private void webBrowser2_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if(disableWeb == false) { textBoxYTUrl.Text = webBrowser2.Url.ToString(); }
            else { return; }
        }

        private void radioButtonYTMP3_CheckedChanged(object sender, EventArgs e)
        {
            format = true;
        }

        private void radioButtonYTMP4_CheckedChanged(object sender, EventArgs e)
        {
            format = false;
        }

        private void textBoxYTPath_TextChanged(object sender, EventArgs e)
        {
            YoutubeCheck();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Mertsayar6623");
        }

        private void label16_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Mertsayar6623");
        }

        private void buttonDisableWeb_Click(object sender, EventArgs e)
        {
            if(comboBoxLanguange.Text == "English" || comboBoxLanguange.Text == "") { labelDisableEnableWeb.Text = "Enable Web:"; }
            else { labelDisableEnableWeb.Text = "Web Etkinlştr:"; }
            buttonDisableWeb.Visible = false;
            buttonEnableWeb.Visible = true;
            disableWeb = true;
            labelWebDisabled.Visible = true;
            labelytdisable.Visible = true;
        }

        private void buttonFullscreenInstagram_Click(object sender, EventArgs e)
        {
            if (comboBoxLanguange.Text == "" || comboBoxLanguange.Text == "English") { MessageBox.Show("Fullscreen Feature Soon", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else { MessageBox.Show("Fullscreen Özelliği Yakında", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void buttonFullscreenYT_Click(object sender, EventArgs e)
        {
            if(comboBoxLanguange.Text == "" || comboBoxLanguange.Text == "English") { MessageBox.Show("Fullscreen Feature Soon", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else { MessageBox.Show("Fullscreen Özelliği Yakında", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        void UpdateProgram()
        {

        }

        private void labelSelectUpdate_Click(object sender, EventArgs e)
        {
            UpdateProgram();
        }

        private void labelSettingsUpdate_Click(object sender, EventArgs e)
        {
            UpdateProgram();
        }

        private void buttonEnableWeb_Click(object sender, EventArgs e)
        {
            if(comboBoxLanguange.Text == "English" || comboBoxLanguange.Text == "") { labelDisableEnableWeb.Text = "Disable Web:"; }
            else { labelDisableEnableWeb.Text = "Web Kapat:"; }
            buttonDisableWeb.Visible = true;
            buttonEnableWeb.Visible = false;
            disableWeb = false;
            labelWebDisabled.Visible = false;
            labelytdisable.Visible = false;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (comboBoxLanguange.Text == "" || comboBoxLanguange.Text == "English") { MessageBox.Show("Web Soon", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else { MessageBox.Show("Web Yakında", "Media Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
    }
}
