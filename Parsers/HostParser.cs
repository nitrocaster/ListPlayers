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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ListPlayers.PcdbModel;

namespace ListPlayers.Parsers
{
    public sealed class HostParser : ParserBase
    {
        private readonly List<ISpecificParser> parserProviders;

        public HostParser(PcdbFile database)
        {
            Database = database;
            parserProviders = new List<ISpecificParser>();
        }

        public HostParser(PcdbFile database, IEnumerable<ISpecificParser> providers)
            : this(database)
        {
            AddProviders(providers);
        }

        public HostParser(PcdbFile database, ISpecificParser provider)
            : this(database)
        {
            AddProvider(provider);
        }

        public override void Cancel()
        {
            base.Cancel();
            OnParsingCancelled();
        }

        public override void Parse(string path)
        {
            var extension = Path.GetExtension(path).ToLowerInvariant();
            var provider = parserProviders.FirstOrDefault(pr => pr.AcceptedFileExtension == extension && pr.CheckFormat(path));
            if (provider == null)
                return;
            using (var parser = provider.GetParser(this, Database))
            {
                parser.Parse(path);
            }
        }
        
        public void AddProvider(ISpecificParser provider)
        {
            parserProviders.Add(provider);
        }

        public void AddProviders(IEnumerable<ISpecificParser> providers)
        {
            parserProviders.AddRange(providers);
        }
        
        public event Action ParsingCancelled;

        private void OnParsingCancelled()
        {
            if (ParsingCancelled != null)
                ParsingCancelled();
        }
    }
}
