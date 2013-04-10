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

using System.Diagnostics;
using System.Windows.Forms;
using ListPlayers.Common;

namespace ListPlayers.Dialogs
{
    public sealed partial class MainDialog : FormEx
    {
        public MainDialog()
        {
            InitializeComponent();

            lBuild.Text = Root.BuildString;

            clDatabaseConstructor.Click += (sender, args) => OpenConstructor();
            clDatabaseView.Click += (sender, args) => OpenViewer();
            linkMpn.LinkClicked += (sender, args) => OpenHomepage();
        }

        private void OpenConstructor()
        {
            Hide();
            using (var ctor = new DatabaseConstructorDialog())
            {
                ctor.ShowDialog();
                if (!(ctor.Back || ctor.Cancel)) 
                {
                    Settings.SetLastSource(ctor.Source);
                    Settings.SetLastDatabase(ctor.Destination);
                    var updater = new DatabaseUpdater(ctor.Destination, ctor.SelectedFiles);
                    using (var dlg = new DatabaseUpdaterDialog(updater))
                    {
                        dlg.ShowDialog();
                    }
                }
            }
            Show();
            BringToFront();
            Activate();
        }

        private void OpenViewer()
        {
            using (var viewer = new DatabaseViewer())
            {
                using (var dlg = new DatabaseViewDialog(viewer))
                {
                    Hide();
                    dlg.ShowDialog();
                    Show();
                    BringToFront();
                    Activate();
                }
            }
        }

        private void OpenHomepage()
        {
            Process.Start("http://mpnetworks.ru");
        }
    }
}