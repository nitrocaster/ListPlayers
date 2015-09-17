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
using System.Globalization;
using System.IO;
using System.Text;
using FileSystem;
using ListPlayers.PcdbModel;

namespace ListPlayers.Parsers
{
    public abstract class PlayerDumpParser : ParserBase
    {
        protected PlayerDumpParser(HostParser host, PcdbFile database)
            : base(host)
        { Database = database; }

        protected void InternalParseDumpedFile(string path, string infoSectionName)
        {
            var infoSection = "[" + infoSectionName + "]\r\n";
            var digest = "";
            var timeStamp = PcdbFile.InvalidDateTime;
            using (var reader = new StreamReader(path, Encoding.Default))
            {
                var pos = StreamScanner.FindString(reader.BaseStream, reader.CurrentEncoding, infoSection);
                if (pos == -1)
                    return;
                reader.DiscardBufferedData();
                reader.BaseStream.Seek(pos, SeekOrigin.Begin);
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    if (Cancelled)
                        break;
                    var buf = reader.ReadLine();
                    var entry = IniFile.ExtractKeyValuePair(buf, false);
                    switch (entry.Key)
                    {
                    case "creation_date":
                        timeStamp = Utils.StrToDateTime(entry.Value, Utils.DateTimePatternCfg);
                        continue;
                    case "player_digest":
                        digest = entry.Value;
                        if (digest == "")
                            return;
                        OnFoundData(DatabaseTableId.Hash);
                        continue;
                    case "player_name":
                    {
                        var name = entry.Value;
                        if (digest != "")
                        {
                            if (!Database.HashExists(digest))
                                Database.InsertHash(digest);
                            OnFoundData(DatabaseTableId.Name);
                            if (name != "")
                                Database.InsertName(digest, name, timeStamp);
                        }
                        return;
                    }
                    }
                }
            }
        }
    }
}
