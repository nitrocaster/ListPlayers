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
using System.IO;
using ListPlayers.PcdbModel;

namespace ListPlayers.Parsers
{
    public sealed class PcdbParser : ISpecificParser
    {
        private sealed class PcdbParserImpl : ParserBase
        {
            public PcdbParserImpl(HostParser host, PcdbFile database)
                : base(host)
            {
                Database = database;
            }

            public override void Parse(string path)
            {
                var src = PcdbFile.Open(path);
                src.OpenConnection();
                src.BeginTransaction();
                {
                    var srcGameVersion = src.GetGameVersion();
                    var selfGameVersion = Database.GetGameVersion();
                    if (srcGameVersion != selfGameVersion)
                    {
                        src.CommitTransaction();
                        src.CloseConnection();
                        return;
                    }
                    var progress = new Progress(100);
                    OnProgressChanged(progress);
                    var ids = src.SelectIds(DatabaseTableId.Hash);
                    var idCount = ids.Rows.Count;
                    OnFoundData(DatabaseTableId.Hash, idCount);
                    for (var idIndex = 0; idIndex < idCount; ++idIndex)
                    {
                        if (Cancelled)
                            break;
                        var id = Convert.ToUInt32(ids.Rows[idIndex][0]);
                        var hashRow = src.Select(DatabaseTableId.Hash, id).Rows[0];
                        var hash = (string)hashRow.ItemArray[1];
                        var info = Convert.ToString(hashRow.ItemArray[2]);
                        if (Database.HashExist(hash) && !String.IsNullOrEmpty(info))
                            Database.UpdateHash(hash, info);
                        else
                            Database.AppendNew(hash, info);
                        // names
                        var names = src.Select(DatabaseTableId.Name, id);
                        OnFoundData(DatabaseTableId.Name, names.Rows.Count);
                        for (var i = 0; i < names.Rows.Count; ++i)
                        {
                            var nameRow = names.Rows[i];
                            var name = (string)nameRow[1];
                            if (src.Revision >= (int)PcdbRevision.Rev1)
                                Database.AppendName(hash, name, (DateTime)nameRow[2]);
                            else
                                Database.AppendName(hash, name, PcdbFile.InvalidDateTime);
                        }
                        // ips
                        var ips = src.Select(DatabaseTableId.Ip, id);
                        OnFoundData(DatabaseTableId.Ip, ips.Rows.Count);
                        for (var i = 0; i < ips.Rows.Count; ++i)
                        {
                            var ipRow = ips.Rows[i];
                            var ip = (string)ipRow[1];
                            if (src.Revision >= (int)PcdbRevision.Rev1)
                                Database.AppendIp(hash, ip, (DateTime)ipRow[2]);
                            else
                                Database.AppendIp(hash, ip, PcdbFile.InvalidDateTime);
                        }
                        // gsids
                        if (srcGameVersion == PcdbGameVersion.COP)
                        {
                            var gsids = src.Select(DatabaseTableId.Gsid, id);
                            OnFoundData(DatabaseTableId.Gsid, gsids.Rows.Count);
                            for (var i = 0; i < gsids.Rows.Count; ++i)
                            {
                                var gsidRow = gsids.Rows[i];
                                var gsid = Convert.ToUInt32(gsidRow[1]);
                                if (src.Revision >= (int)PcdbRevision.Rev1)
                                    Database.AppendGsid(hash, gsid, (DateTime)gsidRow[2]);
                                else
                                    Database.AppendGsid(hash, gsid, PcdbFile.InvalidDateTime);
                            }
                        }
                        var newProgress = (uint)Math.Round(100.0*idIndex/idCount);
                        if (newProgress > progress.Current)
                        {
                            progress.Current = newProgress;
                            OnProgressChanged(progress);
                        }
                    }
                }
                src.CommitTransaction();
                src.CloseConnection();
            }
        }

        public ParserBase GetParser(HostParser host, PcdbFile database)
        {
            return new PcdbParserImpl(host, database);
        }

        public string AcceptedFileExtension
        {
            get
            {
                return ".pcdb";
            }
        }

        public bool CheckFormat(string path)
        {
            return (Path.GetExtension(path).ToLowerInvariant() == AcceptedFileExtension);
        }

        #region Singleton implementation

        private PcdbParser()
        {
        }

        private static readonly PcdbParser instance = new PcdbParser();

        public static PcdbParser Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
    }
}
