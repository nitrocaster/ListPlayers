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

namespace ListPlayers.PcdbModel
{
    [Flags]
    public enum PcdbFieldId : int
    {
        Undefined   = 0,
        Group       = 1,
        Hash        = 2,
        Name        = 4,
        Ip          = 8,
        Gsid        = 16,
        Comment     = 32
    }
}
