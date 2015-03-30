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

using System.Windows.Forms;

namespace ListPlayers.Dialogs
{
    public static class PcdbFileDialog
    {
        private const string fileNameFilter = "Players Correspondence Database (*.pcdb)|*.pcdb";

        public static bool ShowOpenDialog(out string path)
        {
            path = null;
            var result = false;            
            var browser = new OpenFileDialog
            {
                AutoUpgradeEnabled = true,
                Filter = fileNameFilter
            };
            if (browser.ShowDialog() == DialogResult.OK)
            {
                path = browser.FileName;
                result = true;
            }
            browser.Dispose();
            return result;
        }

        public static bool ShowSaveDialog(out string path)
        {
            path        = null;
            var result  = false;
            var saver = new SaveFileDialog
            {
                AddExtension       = true,
                AutoUpgradeEnabled = true,
                DefaultExt         = "pcdb",
                Filter             = fileNameFilter
            };
            if (saver.ShowDialog() == DialogResult.OK)
            {
                path = saver.FileName;
                result = true;
            }
            saver.Dispose();
            return result;
        }
    }
}
