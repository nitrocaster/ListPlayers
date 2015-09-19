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

namespace ListPlayers.PcdbModel
{
    public enum PcdbRevision : int
    {
        /// <summary>
        /// 100 : first release
        /// </summary>
        Rev0 = 100,
        /// <summary>
        /// 101 : added timestamps
        /// </summary>
        Rev1 = 101,
        /// <summary>
        /// 102 : added version info (DBVERSION)
        /// </summary>
        Rev2 = 102,
        /// <summary>
        /// 103 : switched to natural key (hash)
        /// </summary>
        Rev3 = 103
    }
}
