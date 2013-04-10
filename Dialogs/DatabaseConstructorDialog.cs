/*
====================================================================================
This file is part of ListPlayers, the open-source S.T.A.L.K.E.R. multiplayer
statistics organizing tool for game server administrators.
Copyright (C) 2013 Pavel Kovalenko.

You should have received a copy of the MIT License along with ListPlayers sources.
If not, see <http://www.opensource.org/licenses/mit-license.php>.

For support and more information about ListPlayers,
visit <http://mpnetworks.ru> or <https://github.com/nitrocaster/ListPlayers>
====================================================================================
*/

using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using ListPlayers.PcdbModel;
using ListPlayers.Properties;
using Settings = ListPlayers.Common.Settings;

namespace ListPlayers.Dialogs
{
    public sealed partial class DatabaseConstructorDialog : FormEx
    {
        private List<string> files;

        public bool   Back;
        public bool   Cancel;
        public string Destination;
        public string Source;

        public DatabaseConstructorDialog()
        {
            InitializeComponent();

            files = new List<string>();
            
            Cancel = true;
            cbSubFolders.Checked = Settings.SearchSubfolders;

            cbSrc.Items.AddRange(Settings.RecentSources);
            cbDest.Items.AddRange(Settings.RecentDatabases);
        }

        public string[] SelectedFiles
        {
            get
            {
                return files.ToArray();
            }
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(cbSrc.Text))
            {
                if (cbSrc.Text.Trim() == "")
                {
                    MsgBox.Info(StringTable.SelectSourceFolder);
                }
                else
                {
                    MsgBox.Warning(StringTable.SelectedFolderNotFound);
                }
                return;
            }

            var option = cbSubFolders.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            files.AddRange(Directory.GetFiles(cbSrc.Text, "*.ltx", option));
            files.AddRange(Directory.GetFiles(cbSrc.Text, "*.jpg", option));
            files.AddRange(Directory.GetFiles(cbSrc.Text, "*.pcdb", option));

            if (files.Count == 0)
            {
                MsgBox.Warning(StringTable.SelectedFolderDoesntContainAcceptedFiles);
                return;
            }

            if (!File.Exists(cbDest.Text))
            {
                if (cbDest.Text.Trim() == "")
                {
                    MsgBox.Info(StringTable.SelectDatabaseFile);
                }
                else
                {
                    MsgBox.Warning(StringTable.SelectedFileNotFound);
                }
                return;
            }

            var dst = cbDest.Text;
            var count = files.Count;
            for (var i = 0; i < count; ++i)
            {
                if (files[i] != dst)
                {
                    continue;
                }
                files.RemoveAt(i);
                break;
            }

            if (files.Count == 0)
            {
                MsgBox.Warning(StringTable.SelectedFolderDoesntContainAcceptedFiles);
                return;
            }

            Source       = cbSrc.Text;
            Destination  = cbDest.Text;
            Cancel       = false;
            Close();
        }

        private void btnBrowseSrc_Click(object sender, EventArgs e)
        {
            using (var browser = new FolderBrowserDialog())
            {
                browser.Description = StringTable.BrowserSelectSourceFolder;
                if (browser.ShowDialog() == DialogResult.OK)
                {
                    cbSrc.Text = browser.SelectedPath;
                }
            }
        }

        private void btnBrowseDest_Click(object sender, EventArgs e)
        {
            string path;
            if (PcdbFileDialog.ShowOpenDialog(out path))
            {
                cbDest.Text = path;
            }
        }

        private void linkCreateEmptyDbCS_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string path;
            if (PcdbFileDialog.ShowSaveDialog(out path))
            {
                cbDest.Text = path;
                PcdbFile.Create(path, PcdbGameVersion.CS);
            }
        }

        private void linkCreateEmptyDbCOP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string path;
            if (PcdbFileDialog.ShowSaveDialog(out path))
            {
                cbDest.Text = path;
                PcdbFile.Create(path, PcdbGameVersion.COP);
            }
        }

        public new void ShowDialog()
        {
            base.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Back   = false;
            Cancel = true;
            Close();
        }

        private void DatabaseConstructorDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.SearchSubfolders = cbSubFolders.Checked;
        }
    }
}
