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
using ListPlayers.PcdbModel;

namespace ListPlayers.Parsers
{
    public abstract class ParserBase : IDisposable
    {
        private readonly HostParser host;

        protected PcdbFile Database;
        
        protected ParserBase(HostParser host = null)
        {
            this.host = host;
            if (host != null)
                host.ParsingCancelled += Cancel;
        }

        public virtual void Dispose()
        {
            if (host != null)
                host.ParsingCancelled -= Cancel;
        }

        public abstract void Parse(string path);

        public virtual void Cancel()
        {
            Cancelled = true;
        }

        protected bool Cancelled = false;

        public event Action<DatabaseTableId, int> FoundData;

        public event Action<Progress> ProgressChanged;

        protected void OnFoundData(DatabaseTableId field, int count = 1)
        {
            var target = host ?? this;
            if (target.FoundData != null)
                target.FoundData(field, count);
        }

        protected void OnProgressChanged(Progress progress)
        {
            var target = host ?? this;
            if (target.ProgressChanged != null)
                target.ProgressChanged(progress);
        }
    }
}
