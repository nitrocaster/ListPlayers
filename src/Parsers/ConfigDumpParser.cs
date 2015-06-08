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

using System.IO;
using ListPlayers.PcdbModel;

namespace ListPlayers.Parsers
{
    public sealed class ConfigDumpParser : ISpecificParser
    {
        private sealed class ConfigDumpParserImpl : PlayerDumpParser
        {
            public ConfigDumpParserImpl(HostParser host, PcdbFile database)
                : base(host, database)
            {}

            public override void Parse(string path) { InternalParseDumpedFile(path, "config_dump_info"); }
        }

        public ParserBase GetParser(HostParser host, PcdbFile database)
        { return new ConfigDumpParserImpl(host, database); }

        public string AcceptedFileExtension
        {
            get { return ".ltx"; }
        }

        public bool CheckFormat(string path)
        { return (Path.GetExtension(path).ToLowerInvariant() == AcceptedFileExtension); }

        #region Singleton implementation

        private ConfigDumpParser() {}

        private static readonly ConfigDumpParser instance = new ConfigDumpParser();

        public static ConfigDumpParser Instance
        {
            get { return instance; }
        }

        #endregion
    }
}
