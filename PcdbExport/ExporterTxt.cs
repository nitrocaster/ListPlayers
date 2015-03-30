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
using System.ComponentModel;

namespace ListPlayers.PcdbExport
{
    public sealed class ExporterTxt : ExporterBase
    {
        protected override void ExportProc(object sender, DoWorkEventArgs e)
        {
            var hashes = Chunk.Hashes.Rows;
            var names = Chunk.Names.Rows;
            var ips = Chunk.Ips.Rows;            
            var hashesCount = hashes.Count;
            var namesCount = names.Count;
            var ipsCount = ips.Count;            
            try
            {
                for (var i = 0; i < hashesCount; ++i)
                {
                    if (Dialog.Cancelled)
                        break;
                    var currentId = Convert.ToInt32(hashes[i][0]);
                    Writer.WriteLine(hashes[i][1].ToString());
                    Writer.WriteLine("\r\n; names\r\n");
                    for (var j = 0; j < namesCount; ++j)
                    {
                        if (Dialog.Cancelled)
                            break;
                        if (Convert.ToInt32(names[j][0]) != currentId)
                            continue;
                        Writer.WriteLine("    {0,-32} | {1}",
                            names[j][1], ((DateTime)names[j][2]).ToString(Utils.DateTimePatternLong));
                    }

                    Writer.WriteLine("\r\n; ip addresses\r\n");
                    for (var j = 0; j < ipsCount; ++j)
                    {
                        if (Dialog.Cancelled)
                            break;
                        if (Convert.ToInt32(ips[j][0]) != currentId)
                            continue;
                        Writer.WriteLine("    {0,-32} | {1}",
                            ips[j][1], ((DateTime)ips[j][2]).ToString(Utils.DateTimePatternLong));
                    }

                    if (Chunk.Gsids != null)
                    {
                        var gsids = Chunk.Gsids.Rows;
                        var gsidsCount = gsids.Count;
                        Writer.WriteLine("\r\n; gamespy id's\r\n");
                        for (var j = 0; j < gsidsCount; ++j)
                        {
                            if (Dialog.Cancelled)
                                break;
                            if (Convert.ToInt32(gsids[j][0]) != currentId)
                                continue;
                            Writer.WriteLine("    {0,-32} | {1}",
                                gsids[j][1], ((DateTime)gsids[j][2]).ToString(Utils.DateTimePatternLong));
                        }
                    }
                    Writer.WriteLine("\r\n; comments\r\n");
                    Writer.WriteLine(hashes[i][2]);
                    Writer.WriteLine("\r\n; ----\r\n");
                    ++CurrentProgress;
                    Worker.ReportProgress(0);
                }
            }
            finally
            {
                Writer.Close();
            }
        }
    }
}
