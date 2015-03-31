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

using System.Data;

namespace ListPlayers.PcdbModel
{
    public sealed class PcdbChunk
    {
        public DataTable Hashes;
        public DataTable Names;
        public DataTable Ips;
        public DataTable Gsids;

        public PcdbChunk() {}

        public PcdbChunk(DataTable hashes, DataTable names, DataTable ips)
        {
            Hashes = hashes;
            Names = names;
            Ips = ips;
        }

        public PcdbChunk(DataTable hashes, DataTable names, DataTable ips, DataTable gsids)
        {
            Hashes = hashes;
            Names = names;
            Ips = ips;
            Gsids = gsids;
        }
    }
}
