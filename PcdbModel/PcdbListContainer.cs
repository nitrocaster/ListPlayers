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

using System.Collections.Generic;

namespace ListPlayers.PcdbModel
{
    public sealed class PcdbListContainer<T> : List<T>, IPcdbField
        where T : IPcdbField
    {
        private readonly PcdbFieldId itemId;

        public PcdbListContainer(PcdbFieldId itemId)
        {
            this.itemId = itemId;
        }

        public PcdbFieldId Id
        {
            get
            {
                return PcdbFieldId.Group | itemId;
            }
        }
    }
}
