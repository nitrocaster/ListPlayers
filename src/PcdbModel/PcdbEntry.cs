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
    public sealed class PcdbEntry : IPcdbField
    {
        public readonly PcdbItemContainer<string> Hash = new PcdbItemContainer<string>(PcdbFieldId.Hash);
        public readonly PcdbItemContainer<string> Info = new PcdbItemContainer<string>(PcdbFieldId.Comment);
        public readonly PcdbListContainer<PcdbName> Names = new PcdbListContainer<PcdbName>(PcdbFieldId.Name);
        public readonly PcdbListContainer<PcdbIp> Ips = new PcdbListContainer<PcdbIp>(PcdbFieldId.Ip);
        public readonly PcdbListContainer<PcdbGsid> Gsids = new PcdbListContainer<PcdbGsid>(PcdbFieldId.Gsid);
        
        public PcdbEntry(string hash) { Hash.Item = hash; }

        public PcdbFieldId Id
        {
            get { return PcdbFieldId.Group | PcdbFieldId.Hash; }
        }
    }
}
