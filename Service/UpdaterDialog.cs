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
using System.Diagnostics;
using System.Windows.Forms;

namespace ListPlayers.Service
{
    public sealed partial class UpdaterDialog : Form
    {
        private readonly string updateLink = "";

        public UpdaterDialog(UpdateInfo info)
        {
            InitializeComponent();
            AcceptButton = btnDownload;
            lVersionNum.Text = info.Version;
            tbVersionInfo.Text = info.Description;
            updateLink = info.Link;
            tbVersionInfo.Select(0, 0);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            Process.Start(updateLink);
            Close();
        }
    }
}
