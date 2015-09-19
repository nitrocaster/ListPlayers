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
    public sealed class ExporterRadb : ExporterBase
    {
        private const char delimiter = '|';

        public ExporterRadb(ITextExporterView view) : base(view) {}

        protected override void ExportProc(object sender, DoWorkEventArgs e)
        {
            var hashes = Chunk.Hashes.Rows;
            var names = Chunk.Names.Rows;
            var ips = Chunk.Ips.Rows;
            var hashCount = hashes.Count;
            var nameCount = names.Count;
            var ipCount = ips.Count;
            try
            {
                for (var i = 0; i < hashCount; i++)
                {
                    if (View.Cancelled)
                        break;
                    var currentHash = hashes[i][0].ToString();
                    Writer.WriteLine();
                    Writer.WriteLine(currentHash);
                    for (var j = 0; j < nameCount; j++)
                    {
                        if (View.Cancelled)
                            break;
                        if (names[j][0].ToString() != currentHash)
                            continue;
                        Writer.Write(names[j][1]);
                        Writer.Write(delimiter);
                        Writer.WriteLine(((DateTime)names[j][2]).ToString(Utils.DateTimePatternLong));
                    }
                    Writer.WriteLine();
                    for (var j = 0; j < ipCount; j++)
                    {
                        if (View.Cancelled)
                            break;
                        if (ips[j][0].ToString() != currentHash)
                            continue;
                        Writer.Write(ips[j][1]);
                        Writer.Write(delimiter);
                        Writer.WriteLine(((DateTime)ips[j][2]).ToString(Utils.DateTimePatternLong));
                    }
                    if (Chunk.Gsids != null)
                    {
                        var gsids = Chunk.Gsids.Rows;
                        var gsidsCount = gsids.Count;
                        Writer.WriteLine();
                        for (var j = 0; j < gsidsCount; j++)
                        {
                            if (View.Cancelled)
                                break;
                            if (gsids[j][0].ToString() != currentHash)
                                continue;
                            Writer.Write(gsids[j][1]);
                            Writer.Write(delimiter);
                            Writer.WriteLine(((DateTime)gsids[j][2]).ToString(Utils.DateTimePatternLong));
                        }
                    }
                    CurrentProgress++;
                    Worker.ReportProgress(0);
                }
            }
            finally
            {
                Writer.Close();
            }
        }

        public override ExportFormat Format { get { return ExportFormat.Radb; } }
    }
}
