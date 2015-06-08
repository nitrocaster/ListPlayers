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
using System.Windows.Forms;
using ListPlayers.PcdbModel;
using ListPlayers.Properties;

namespace ListPlayers.Parsers
{
    public sealed class OldPcdbParser : ISpecificParser
    {
        private sealed class OldPcdbParserImpl : ParserBase
        {
            public OldPcdbParserImpl(HostParser host, PcdbFile database)
                : base(host)
            { Database = database; }

            public override void Parse(string path)
            {
                var digest = "";
                var insideName = false;
                var insideIp   = false;
                var insideGsid = false;
                var progress = new Progress(100);
                OnProgressChanged(progress);
                var currentLine = -1;
                char[] hexDigestTrimChars = {'[', ']', ' ', '\t'};
                if (!VerifyOldDatabase(path, Database.GetGameVersion()) || Cancelled)
                    return;
                using (var reader = new StreamReader(path, Encoding.Default))
                {
                    try
                    {
                        while (!reader.EndOfStream)
                        {
                            if (Cancelled)
                                break;
                            currentLine++;
                            var newProgress = (uint)Math.Round(
                                100.0 * reader.BaseStream.Position / reader.BaseStream.Length);
                            if (newProgress > progress.Current)
                            {
                                progress.Current = newProgress;
                                OnProgressChanged(progress);
                            }
                            var buf = reader.ReadLine().TrimEnd();
                            if (buf.StartsWith("[") && buf.EndsWith("]") && buf.Length == 34)
                            {
                                OnFoundData(DatabaseTableId.Hash);
                                digest = buf.Trim(hexDigestTrimChars);
                                continue;
                            }
                            if (buf == "" || buf == "\t{" || buf.Trim() == "")
                                continue;
                            switch (buf.Remove(0, 1))
                            {
                            case "player_name":
                                insideName = true;
                                continue;
                            case "player_ip":
                                insideIp = true;
                                continue;
                            case "player_profile_id":
                                insideGsid = true;
                                continue;
                            case "}":
                                insideName = false;
                                insideIp = false;
                                insideGsid = false;
                                continue;
                            }
                            if (insideName)
                            {
                                OnFoundData(DatabaseTableId.Name);
                                var name = buf.Trim();
                                if (name != "" && digest != "")
                                    Database.AppendName(digest, name, PcdbFile.InvalidDateTime);
                                continue;
                            }
                            if (insideIp)
                            {
                                OnFoundData(DatabaseTableId.Ip);
                                var ip = buf.Trim();
                                if (ip != "" & digest != "")
                                    Database.AppendIp(digest, ip, PcdbFile.InvalidDateTime);
                                continue;
                            }
                            if (insideGsid)
                            {
                                OnFoundData(DatabaseTableId.Gsid);
                                var profileID = buf.Trim();
                                if (profileID != "" & digest != "")
                                {
                                    Database.AppendGsid(digest,
                                        Convert.ToUInt32(profileID), PcdbFile.InvalidDateTime);
                                }
                                continue;
                            }
                        }
                        progress.Current = progress.Maximum;
                        OnProgressChanged(progress);
                    }
                    catch (NullReferenceException)
                    {
                        progress.Current = progress.Maximum;
                        OnProgressChanged(progress);
                        MsgBox.Error(String.Format("{0}\n[{1}]", StringTable.CorruptedDatabase, currentLine));
                    }
                }
            }

            /// <summary>
            /// Returns true, if source and destination databases use same game version 
            /// or if verification process has been cancelled by user; otherwise, returns false.
            /// </summary>
            private bool VerifyOldDatabase(string path, PcdbGameVersion version)
            {
                var hashCount = 0;
                var gsidCount = 0;
                using (var reader = new StreamReader(path, Encoding.Default))
                {
                    while (!reader.EndOfStream)
                    {
                        if (Cancelled)
                            return false;
                        var buf = reader.ReadLine().TrimEnd();
                        if (buf.StartsWith("[") && buf.EndsWith("]") && buf.Length == 34)
                        {
                            hashCount++;
                            continue;
                        }
                        if (buf == "\tplayer_profile_id")
                        {
                            gsidCount++;
                            continue;
                        }
                    }
                }
                if (version == PcdbGameVersion.COP && hashCount != gsidCount)
                    return false;
                if (version != PcdbGameVersion.COP && gsidCount != 0)
                    return false;
                return true;
            }
        }

        public ParserBase GetParser(HostParser host, PcdbFile database)
        { return new OldPcdbParserImpl(host, database); }

        public string AcceptedFileExtension
        { get { return ".pcdb"; } }

        public bool CheckFormat(string path)
        { return (Path.GetExtension(path).ToLowerInvariant() == AcceptedFileExtension); }

        #region Singleton implementation

        private OldPcdbParser() {}

        private static readonly OldPcdbParser instance = new OldPcdbParser();

        public static OldPcdbParser Instance
        { get { return instance; } }

        #endregion
    }
}
