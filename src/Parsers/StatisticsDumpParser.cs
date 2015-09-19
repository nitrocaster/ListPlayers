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
using System.IO;
using System.Text;
using FileSystem;
using ListPlayers.PcdbModel;

namespace ListPlayers.Parsers
{
    public sealed class StatisticsDumpParser : ISpecificParser
    {
        private sealed class StatisticsDumpParserImpl : ParserBase
        {
            public StatisticsDumpParserImpl(HostParser host, PcdbFile database)
                : base(host)
            { Database = database; }

            public override void Parse(string path)
            {
                // player info section
                //      player_ip                        = 0.0.0.0
                //      player_name                      = Flammable
                //      player_unique_digest             = 22ddb0d3db3caea4ba85f7cc06a4e7e1
                var ip = "";
                var name = "";
                var gsid = "";
                DateTime dumpTime = PcdbFile.InvalidDateTime, endTime = PcdbFile.InvalidDateTime;
                var timestampsFound = false;
                var onlineDump = false;
                var selfGameVersion = Database.GetGameVersion();
                using (var reader = new StreamReader(path, Encoding.Default))
                {
                    var profileIDsFound = false;
                    while (!reader.EndOfStream)
                    {
                        var buf = reader.ReadLine().Trim();
                        if (buf == "")
                            continue;
                        var pair = IniFile.ExtractKeyValuePair(buf, false);
                        if (!profileIDsFound && pair.Key == "player_profile_id")
                        {
                            profileIDsFound = true;
                            // skip dump with mismatching game version
                            if (selfGameVersion != PcdbGameVersion.COP)
                                return;
                        }
                    }
                    if (selfGameVersion == PcdbGameVersion.COP && !profileIDsFound)
                        return;
                    reader.DiscardBufferedData();
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    while (!reader.EndOfStream)
                    {
                        if (Cancelled)
                            break;
                        var buf = reader.ReadLine().Trim();
                        if (buf == "")
                            continue;
                        var pair = IniFile.ExtractKeyValuePair(buf, false);
                        switch (pair.Key)
                        {
                        case "dump_time":
                            // online dump
                            dumpTime = Utils.StrToDateTime(pair.Value, Utils.DateTimePatternStats);
                            onlineDump = true;
                            timestampsFound = true;
                            break;
                        case "end_time":
                            // extended dump
                            endTime = Utils.StrToDateTime(pair.Value, Utils.DateTimePatternStats);
                            continue;
                        case "start_time":
                            // extended dump
                            timestampsFound = true;
                            break;
                        }
                        if (timestampsFound)
                            break;
                    }
                    if (!timestampsFound || Cancelled)
                        return;
                    // treating the source file as statistics dump
                    while (!reader.EndOfStream)
                    {
                        if (Cancelled)
                            break;
                        var buf = reader.ReadLine().Trim();
                        if (buf == "")
                            continue;
                        var pair = IniFile.ExtractKeyValuePair(buf, false);
                        // process next line in the current section
                        while (!pair.Key.StartsWith("[player_") && !pair.Key.StartsWith("[wpn"))
                        {
                            switch (pair.Key)
                            {
                            case "player_ip":
                                ip = pair.Value;
                                if (!string.IsNullOrEmpty(ip))
                                    OnFoundData(DatabaseTableId.Ip);
                                break;
                            case "player_name":
                            case "name":
                                name = pair.Value;
                                if (!string.IsNullOrEmpty(name))
                                    OnFoundData(DatabaseTableId.Name);
                                break;
                            case "player_profile_id":
                                gsid = pair.Value;
                                if (!string.IsNullOrEmpty(gsid))
                                    OnFoundData(DatabaseTableId.Gsid);
                                break;
                            case "player_unique_digest":
                            {
                                var digest = pair.Value;
                                if (!string.IsNullOrEmpty(digest))
                                {
                                    OnFoundData(DatabaseTableId.Hash);
                                    // player_unique_digest is the last entry
                                    DateTime ts = onlineDump ? dumpTime : endTime;
                                    Database.InsertHash(digest);
                                    if (!string.IsNullOrEmpty(name))
                                        Database.InsertName(digest, name, ts);
                                    if (!string.IsNullOrEmpty(ip))
                                        Database.InsertIp(digest, ip, ts);
                                    if (!string.IsNullOrEmpty(gsid))
                                        Database.InsertGsid(digest, Convert.ToUInt32(gsid), ts);
                                }
                                break;
                            }
                            }
                            if (!reader.EndOfStream)
                            {
                                buf = reader.ReadLine().Trim();
                                pair = IniFile.ExtractKeyValuePair(buf, false);
                            }
                            if (reader.EndOfStream || Cancelled)
                                break;
                        }
                        name = "";
                        ip   = "";
                        gsid = "";
                    }
                }
            }
        }

        public ParserBase GetParser(HostParser host, PcdbFile database)
        { return new StatisticsDumpParserImpl(host, database); }

        public string AcceptedFileExtension
        {
            get { return ".ltx"; }
        }

        public bool CheckFormat(string path)
        {
            if (Path.GetExtension(path).ToLowerInvariant() != AcceptedFileExtension)
                return false;
            using (var reader = new StreamReader(path, Encoding.Default))
            {
                return reader.ReadLine() == "[global]";
            }
        }

        #region Singleton implementation

        private StatisticsDumpParser() {}

        private static readonly StatisticsDumpParser instance = new StatisticsDumpParser();

        public static StatisticsDumpParser Instance
        {
            get { return instance; }
        }

        #endregion
    }
}
