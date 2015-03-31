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
    public sealed class PcdbItemContainer<T> : IPcdbField
    {
        public T Item;
        private readonly PcdbFieldId id;

        public PcdbItemContainer(PcdbFieldId id) { this.id = id; }

        public PcdbItemContainer(PcdbFieldId id, T item)
            : this(id)
        { Item = item; }

        public static implicit operator T(PcdbItemContainer<T> src) { return src.Item; }

        public PcdbFieldId Id
        {
            get { return id; }
        }
    }
}
