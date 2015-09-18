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

namespace ListPlayers.PcdbExport
{
    public static class ExportManager
    {
        public static ExportFormat GetFormat(string ext)
        {
            switch (ext.ToLowerInvariant())
            {
            case ".txt": return ExportFormat.Txt;
            case ".radb": return ExportFormat.Radb;
            default: throw new ArgumentException("Specified format is not supported.");
            }
        }
        
        public static IExporter GetExporter(ExportFormat fmt)
        {
            switch (fmt)
            {
            case ExportFormat.Txt: return new ExporterTxt(CreateView());
            case ExportFormat.Radb: return new ExporterRadb(CreateView());
            default: throw new ArgumentException("Specified format is not supported.");
            }
        }

        private static ITextExporterView CreateView()
        { return new TextExporterDialog(); }
    }
}
