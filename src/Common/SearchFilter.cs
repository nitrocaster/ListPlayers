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

namespace ListPlayers.Common
{
    public struct SearchFilter
    {
        public string[] Hashes;
        public string[] Names;
        public string[] Ips;
        public string[] Gsids;

        public bool UseNamePattern;
        public bool UseIpPattern;
        public bool IncludeRelatedData;

        public bool Empty
        {
            get
            {
                return Hashes.Length == 0 && Names.Length == 0 &&
                    Ips.Length == 0 && Gsids.Length == 0;
            }
        }
    }
}
