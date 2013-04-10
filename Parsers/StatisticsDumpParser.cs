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
    public sealed class StatisticsDumpParser : ISpecificParser
    {
        private sealed class StatisticsDumpParserImpl : ParserBase
        {
            public StatisticsDumpParserImpl(HostParser host, PcdbFile database)
                : base(host)
            {
                Database = database;
            }

            public override void Parse(string path)
            {
                // player info section
                //      player_ip                        = 0.0.0.0
                //      player_name                      = Flammable
                //      player_unique_digest             = 22ddb0d3db3caea4ba85f7cc06a4e7e1

                var ip = "";
                var name = "";
                var gsid = "";

                DateTime dumpTime = PcdbFile.InvalidDateTime,
                    //startTime = PcdbFile.InvalidDateTime,
                         endTime = PcdbFile.InvalidDateTime;
#pragma warning disable 219
                var dumpTimeFound = false;
                var startTimeFound = false;
                var endTimeFound = false;
#pragma warning restore 219
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
                        {
                            continue;
                        }

                        var pair = IniFile.ExtractKeyValuePair(buf, false);

                        if (!profileIDsFound && pair.Key == "player_profile_id")
                        {
                            profileIDsFound = true;
                            // skip dump with mismatching game version
                            if (selfGameVersion != PcdbGameVersion.COP)
                            {
                                return;
                            }
                        }
                    }

                    if (selfGameVersion == PcdbGameVersion.COP && !profileIDsFound)
                    {
                        return;
                    }

                    reader.DiscardBufferedData();
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);

                    while (!reader.EndOfStream)
                    {
                        if (Cancelled)
                        {
                            break;
                        }

                        var buf = reader.ReadLine().Trim();
                        if (buf == "")
                        {
                            continue;
                        }

                        var pair = IniFile.ExtractKeyValuePair(buf, false);

                        switch (pair.Key)
                        {
                            case "dump_time":
                                // online dump
                                dumpTime = StrToDateTime(pair.Value);
                                onlineDump = true;
                                dumpTimeFound = true;
                                timestampsFound = true;
                                break;

                            case "end_time":
                                // extended dump
                                endTime = StrToDateTime(pair.Value);
                                endTimeFound = true;
                                continue;

                            case "start_time":
                                // extended dump
                                //startTime = StrToDateTime(INIFile.ExtractKeyValuePair(buf, true).Value, false);
                                startTimeFound = true;
                                timestampsFound = true;
                                break;
                        }

                        if (timestampsFound)
                        {
                            break;
                        }
                    }

                    if (!timestampsFound || Cancelled)
                    {
                        return;
                    }

                    // treating the source file as statistics dump
                    while (!reader.EndOfStream)
                    {
                        if (Cancelled)
                        {
                            break;
                        }

                        var buf = reader.ReadLine().Trim();

                        if (buf == "")
                        {
                            continue;
                        }

                        var pair = IniFile.ExtractKeyValuePair(buf, false);

                        // process next line in the current section
                        while (!pair.Key.StartsWith("[player_") && !pair.Key.StartsWith("[wpn"))
                        {
                            switch (pair.Key)
                            {
                                case "player_ip":
                                {
                                    ip = pair.Value;
                                    if (ip.Length > 0)
                                    {
                                        OnFoundData(DatabaseTableId.Ip);
                                    }
                                    break;
                                }
                                case "player_name":
                                case "name":
                                {
                                    name = pair.Value;
                                    if (name != "")
                                    {
                                        OnFoundData(DatabaseTableId.Name);
                                    }
                                    break;
                                }
                                case "player_profile_id":
                                {
                                    gsid = pair.Value;
                                    if (gsid != "")
                                    {
                                        OnFoundData(DatabaseTableId.Gsid);
                                    }
                                    break;
                                }
                                case "player_unique_digest":
                                {
                                    // dump_time   = 12-26-10_18-32-24 // online_dump.ltx
                                    // end_time    = 12-26-10_18-32-24 // dmp12-26-10_18-26-48.ltx
                                    var digest = pair.Value;
                                    if (digest != "")
                                    {
                                        OnFoundData(DatabaseTableId.Hash);
                                        if (gsid == "") // player_unique_digest is the last entry
                                        {
                                            Database.Append(digest, name, ip, onlineDump ? dumpTime : endTime);
                                        }
                                        else
                                        {
                                            Database.Append(digest, name, ip, Convert.ToUInt32(gsid), onlineDump ? dumpTime : endTime);
                                        }
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
                            {
                                break;
                            }
                        }
                        name = "";
                        ip   = "";
                        gsid = "";
                    }
                }
            }

            private static readonly char[] dateTimeSplitChars = new[] { '.', ' ', '_', ':', '-' };

            /// <summary>
            ///     Extracts DateTime from it's special string representation (used in config dumps and screenshots).
            /// </summary>
            private static DateTime StrToDateTime(string src)
            {
                // "MM-DD-YY_hh-mm-ss"
                // dateString = "14 11 11 08 30 48";
                // format =     "dd.MM.yyyy_HH:mm:ss";

                var dateparts = src.Split(dateTimeSplitChars);
                var builder = new StringBuilder(64);
                for (var i = 0; i < dateparts.Length; ++i)
                {
                    builder.Append(dateparts[i]);
                }
                var result = DateTime.ParseExact(src, "MMddyyHHmmss", CultureInfo.InvariantCulture);
                return result;
            }
        }

        public ParserBase GetParser(HostParser host, PcdbFile database)
        {
            return new StatisticsDumpParserImpl(host, database);
        }

        public string AcceptedFileExtension
        {
            get
            {
                return ".ltx";
            }
        }

        public bool CheckFormat(string path)
        {
            if (Path.GetExtension(path).ToLowerInvariant() != AcceptedFileExtension)
            {
                return false;
            }
            using (var reader = new StreamReader(path, Encoding.Default))
            {
                return reader.ReadLine() == "[global]";
            }
        }

        #region Singleton implementation

        private StatisticsDumpParser()
        {
        }

        private static readonly StatisticsDumpParser instance = new StatisticsDumpParser();

        public static StatisticsDumpParser Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
    }
}