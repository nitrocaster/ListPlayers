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
using System.Net;
using System.Text;


namespace ListPlayers.Service
{
    public struct UpdateInfo
    {
        public bool   IsNewer;
        public string Link;
        public string Version;
        public string Description;
    }

    public static class Updater
    {
        private const string updateUrl = "http://mpnetworks.ru/downloads/cop_soft/listplayers/upd_info/";

        private static UpdaterDialog updaterDialog;

        public static void CheckForUpdates()
        {
            if (updaterDialog != null && !updaterDialog.IsDisposed)
            {
                return;
            }

            UpdateInfo info;
            if (GetUpdateInfo(out info) && info.IsNewer)
            {
                updaterDialog = new UpdaterDialog(info);
                updaterDialog.ShowDialog();
            }
        }

        private static bool GetUpdateInfo(out UpdateInfo info)
        {
            const string keyLink    = "link =";
            const string keyVersion = "version =";

            info = default(UpdateInfo);
            using (var client = new WebClient())
            {
                try
                {
                    var data = client.DownloadData(updateUrl + "index.txt");
                    var updIndex = Encoding.Default.GetString(data).Split('\n');
                    var updInfo = new StringBuilder();
                    for (var i = 0; i < updIndex.Length; ++i)
                    {
                        var buf = updIndex[i].Trim();
                        var version = new Version(buf);
                        if (version <= Root.Version)
                        {
                            continue;
                        }

                        info.IsNewer = true;

                        for (var j = i; j < updIndex.Length; ++j)
                        {
                            data = client.DownloadData(updateUrl + updIndex[j].Trim() + ".txt");
                            var lines = Encoding.Default.GetString(data).Split('\n');
                            for (var k = 0; k < lines.Length; ++k)
                            {
                                updInfo.Append(lines[k]);
                                updInfo.Append("\r\n");
                            }
                            updInfo.Append("\r\n\r\n");
                        }
                        break;
                    }

                    if (updInfo.Length == 0)
                    {
                        return true;
                    }

                    info.Description = updInfo.ToString();
                    data = client.DownloadData(updateUrl + "latest.txt");
                    updIndex = Encoding.Default.GetString(data).Split('\n');

                    for (var i = 0; i < updIndex.Length; ++i)
                    {
                        if (updIndex[i].StartsWith(keyLink))
                        {
                            info.Link = updIndex[i].Substring(keyLink.Length).Trim();
                            continue;
                        }
                        if (updIndex[i].StartsWith(keyVersion))
                        {
                            info.Version = updIndex[i].Substring(keyVersion.Length).Trim();
                            break;
                        }
                    }
                    return true;
                }
                catch (Exception)
                {
                    //Log("! Update attempt failed: " + e.Message);
                    return false;
                }
            }
        }
    }
}