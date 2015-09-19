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
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using ListPlayers.PcdbModel;

namespace ListPlayers.Parsers
{
    public sealed class PcdbParser : ISpecificParser
    {
        // rev0-rev2 (102)
        private sealed class PcdbRx2File
        {
            // db layout:
            /*
            CREATE TABLE DBVERSION (VERSION INTEGER UNSIGNED NOT NULL)
            CREATE TABLE DBTYPE (TYPEID TINYINT UNSIGNED NOT NULL)
            CREATE TABLE HASHES (ID INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTOINCREMENT,
                HASH CHAR(32) NOT NULL UNIQUE, INFO TINYTEXT NULL)
            CREATE TABLE NAMES (ID INTEGER UNSIGNED NOT NULL REFERENCES HASHES(ID),
                NAME TINYTEXT NOT NULL, DATEINFO TIMESTAMP NULL)
            CREATE TABLE IPS (ID INTEGER UNSIGNED NOT NULL REFERENCES HASHES(ID),
                IP TINYTEXT NOT NULL, DATEINFO TIMESTAMP NULL)
            CREATE TABLE GSIDS (ID INTEGER UNSIGNED NOT NULL REFERENCES HASHES(ID),
                GSID INT UNSIGNED NOT NULL, DATEINFO TIMESTAMP NULL)
            */

            private readonly SQLiteDatabase database;

            private PcdbRx2File(string path)
            {
                FileName = path;
                database = new SQLiteDatabase(FileName);
                Revision = PcdbUtil.GetRevision(database);
                database.Close();
            }
            
            public int Revision
            {
                get;
                private set;
            }

            public string FileName
            {
                get;
                private set;
            }
            
            public PcdbGameVersion GetGameVersion() { return PcdbUtil.GetGameVersion(database); }

            public static PcdbRx2File Open(string filename) { return new PcdbRx2File(filename); }

            public bool OpenConnection() { return database.Open(); }

            public bool CloseConnection() { return database.Close(); }

            public void BeginTransaction() { database.BeginTransaction(); }

            public void CommitTransaction() { database.CommitTransaction(); }

            public void RollBackTransaction() { database.RollBackTransaction(); }

            public void Compress() { database.Vacuum(); }
            
            private string EscapeString(string str) { return str.Replace("'", "''"); }

            private string GetTableName(DatabaseTableId table)
            {
                switch (table)
                {
                case DatabaseTableId.Hash: return "HASHES";
                case DatabaseTableId.Name: return "NAMES";
                case DatabaseTableId.Ip: return "IPS";
                case DatabaseTableId.Gsid: return "GSIDS";
                default: return null;
                }
            }

            private string GetFieldName(DatabaseTableId table)
            {
                switch (table)
                {
                case DatabaseTableId.Hash: return "HASH";
                case DatabaseTableId.Name: return "NAME";
                case DatabaseTableId.Ip: return "IP";
                case DatabaseTableId.Gsid: return "GSID";
                default: return null;
                }
            }

            public DataTable Select(DatabaseTableId table, uint[] ids, string[] filter, bool asPattern = false)
            {
                var tableName = GetTableName(table);
                var fieldName = GetFieldName(table);
                DataTable result = null;
                var query = new StringBuilder("SELECT * FROM ");
                query.Append(tableName);
                query.Append(" WHERE");
                var idUsed = false;
                var len = ids.Length;
                if (len > 0)
                {
                    idUsed = true;
                    query.Append(" ID IN (");
                    query.Append(ids[0]);
                    for (var j = 1; j < len; j++)
                    {
                        query.Append(',');
                        query.Append(ids[j]);
                    }
                    query.Append(')');
                }
                len = filter.Length;
                if (len > 0)
                {
                    if (idUsed)
                        query.Append(" AND ");
                    if (asPattern)
                    {
                        query.Append(fieldName);
                        query.Append(" LIKE '");
                        query.Append(EscapeString(filter[0]));
                        query.Append('\'');
                        for (var j = 1; j < len; j++)
                        {
                            query.Append(" OR ");
                            query.Append(fieldName);
                            query.Append(" LIKE '");
                            query.Append(EscapeString(filter[j]));
                            query.Append('\'');
                        }
                    }
                    else
                    {
                        query.Append(fieldName);
                        query.Append(" IN ('");
                        query.Append(EscapeString(filter[0]));
                        query.Append('\'');
                        for (var j = 1; j < len; j++)
                        {
                            query.Append(",'");
                            query.Append(EscapeString(filter[j]));
                            query.Append('\'');
                        }
                        query.Append(')');
                    }
                    idUsed = true;
                }
                if (idUsed)
                    result = database.Execute(query.ToString());
                return result ?? new DataTable();
            }

            public DataTable Select(DatabaseTableId table, uint id)
            {
                var query = new StringBuilder("SELECT * FROM ");
                query.Append(GetTableName(table));
                query.Append(" WHERE ID = ");
                query.Append(id);
                return database.Execute(query.ToString());
            }

            public DataTable SelectIds(DatabaseTableId table, string[] filter, bool asPattern = false)
            {
                var len = filter.Length;
                if (len == 0)
                    return null;
                var tableName = GetTableName(table);
                var fieldName = GetFieldName(table);
                var query = new StringBuilder("SELECT DISTINCT ID FROM ");
                query.Append(tableName);
                if (asPattern)
                {
                    query.Append(" WHERE ");
                    query.Append(fieldName);
                    query.Append(" LIKE '");
                    query.Append(EscapeString(filter[0]));
                    query.Append('\'');
                    for (var j = 1; j < len; j++)
                    {
                        query.Append(" OR ");
                        query.Append(fieldName);
                        query.Append(" LIKE '");
                        query.Append(EscapeString(filter[j]));
                        query.Append('\'');
                    }
                }
                else
                {
                    query.Append(" WHERE ");
                    query.Append(fieldName);
                    query.Append(" IN ('");
                    query.Append(EscapeString(filter[0]));
                    query.Append('\'');
                    for (var j = 1; j < len; j++)
                    {
                        query.Append(",'");
                        query.Append(EscapeString(filter[j]));
                        query.Append('\'');
                    }
                    query.Append(')');
                }
                return database.Execute(query.ToString());
            }

            public DataTable SelectIds(DatabaseTableId table)
            { return database.Execute("SELECT DISTINCT ID FROM " + GetTableName(table)); }

            public static uint[] ExtractIds(DataTable src)
            {
                var count = src.Rows.Count;
                var ids = new uint[count];
                for (var i = 0; i < count; i++)
                    ids[i] = Convert.ToUInt32(src.Rows[i][0]);
                return ids;
            }

            public bool HashExist(string hash)
            {
                uint dummy = 0;
                return GetIdByHash(hash, ref dummy);
            }

            public bool NameExist(string hash, string unescapedName)
            {
                uint id = 0;
                if (!GetIdByHash(hash, ref id))
                    return false;
                return NameExist(id, EscapeString(unescapedName));
            }

            public bool IpExist(string hash, string ip)
            {
                uint id = 0;
                if (!GetIdByHash(hash, ref id))
                    return false;
                return IpExist(id, ip);
            }

            public bool GsidExist(string hash, uint gsid)
            {
                uint id = 0;
                if (!GetIdByHash(hash, ref id))
                    return false;
                return GsidExist(id, gsid);
            }

            private bool GetIdByHash(string hash, ref uint id)
            {
                var query = new StringBuilder("SELECT ID FROM HASHES WHERE HASH = '", 100);
                query.Append(hash);
                query.Append('\'');
                var table = database.Execute(query.ToString());
                if (table.Rows.Count == 0)
                    return false;
                id = Convert.ToUInt32(table.Rows[0][0]);
                return true;
            }

            private bool NameExist(uint id, string escapedName)
            {
                var query = new StringBuilder("SELECT COUNT(*) FROM NAMES WHERE ID = '", 100);
                query.Append(id);
                query.Append("' AND NAME = '");
                query.Append(escapedName);
                query.Append('\'');
                return (long)database.Execute(query.ToString()).Rows[0][0] > 0;
            }

            private bool IpExist(uint id, string ip)
            {
                var query = new StringBuilder("SELECT COUNT(*) FROM IPS WHERE ID = '", 100);
                query.Append(id);
                query.Append("' AND IP = '");
                query.Append(ip);
                query.Append('\'');
                return (long)database.Execute(query.ToString()).Rows[0][0] > 0;
            }

            private bool GsidExist(uint id, uint gsid)
            {
                var query = new StringBuilder("SELECT COUNT(*) FROM GSIDS WHERE ID = '", 100);
                query.Append(id);
                query.Append("' AND GSID = '");
                query.Append(gsid);
                query.Append('\'');
                return (long)database.Execute(query.ToString()).Rows[0][0] > 0;
            }
        }
        
        private sealed class PcdbParserImpl : ParserBase
        {
            public PcdbParserImpl(HostParser host, PcdbFile database)
                : base(host)
            { Database = database; }

            public override void Parse(string path)
            {
                var info = new PcdbFileInfo(path);
                if (info.GameVersion != Database.GetGameVersion())
                    return;
                if (info.Revision<=(int)PcdbRevision.Rev2)
                {
                    var src = PcdbRx2File.Open(path);
                    src.OpenConnection();
                    src.BeginTransaction();
                    switch (src.Revision)
                    {
                    case (int)PcdbRevision.Rev0: ParseRev0(src); break;
                    case (int)PcdbRevision.Rev1: ParseRev1(src); break;
                    case (int)PcdbRevision.Rev2: ParseRev2(src); break;
                    }
                    src.RollBackTransaction();
                    src.CloseConnection();
                }
                else
                {
                    var src = PcdbFile.Open(path);
                    src.OpenConnection();
                    src.BeginTransaction();
                    switch (src.Revision)
                    {
                    case (int)PcdbRevision.Rev3: ParseRev3(src); break;
                    }
                    src.RollBackTransaction();
                    src.CloseConnection();
                }
            }

            private void ParseRev0(PcdbRx2File src)
            {
                var srcGameVersion = src.GetGameVersion();
                var selfGameVersion = Database.GetGameVersion();
                if (srcGameVersion != selfGameVersion)
                    return;
                var progress = new Progress(100);
                OnProgressChanged(progress);
                var ids = src.SelectIds(DatabaseTableId.Hash);
                var idCount = ids.Rows.Count;
                OnFoundData(DatabaseTableId.Hash, idCount);
                for (var idIndex = 0; idIndex < idCount; idIndex++)
                {
                    if (Cancelled)
                        break;
                    var id = Convert.ToUInt32(ids.Rows[idIndex][0]);
                    var hashRow = src.Select(DatabaseTableId.Hash, id).Rows[0];
                    var hash = (string)hashRow.ItemArray[1];
                    var info = Convert.ToString(hashRow.ItemArray[2]);
                    if (String.IsNullOrEmpty(info))
                        Database.InsertHash(hash);
                    else
                        Database.InsertUpdateHash(hash, info);
                    // names
                    var names = src.Select(DatabaseTableId.Name, id);
                    OnFoundData(DatabaseTableId.Name, names.Rows.Count);
                    for (var i = 0; i < names.Rows.Count; i++)
                    {
                        var nameRow = names.Rows[i];
                        var name = (string)nameRow[1];
                        Database.InsertName(hash, name, PcdbFile.InvalidDateTime);
                    }
                    // ips
                    var ips = src.Select(DatabaseTableId.Ip, id);
                    OnFoundData(DatabaseTableId.Ip, ips.Rows.Count);
                    for (var i = 0; i < ips.Rows.Count; i++)
                    {
                        var ipRow = ips.Rows[i];
                        var ip = (string)ipRow[1];
                        Database.InsertIp(hash, ip, PcdbFile.InvalidDateTime);
                    }
                    // gsids
                    if (srcGameVersion == PcdbGameVersion.COP)
                    {
                        var gsids = src.Select(DatabaseTableId.Gsid, id);
                        OnFoundData(DatabaseTableId.Gsid, gsids.Rows.Count);
                        for (var i = 0; i < gsids.Rows.Count; i++)
                        {
                            var gsidRow = gsids.Rows[i];
                            var gsid = Convert.ToUInt32(gsidRow[1]);
                            Database.InsertGsid(hash, gsid, PcdbFile.InvalidDateTime);
                        }
                    }
                    var newProgress = (uint)Math.Round(100.0*(idIndex + 1)/idCount);
                    if (newProgress > progress.Current)
                    {
                        progress.Current = newProgress;
                        OnProgressChanged(progress);
                    }
                }
            }

            private void ParseRev1(PcdbRx2File src)
            {
                var srcGameVersion = src.GetGameVersion();
                var selfGameVersion = Database.GetGameVersion();
                if (srcGameVersion != selfGameVersion)
                    return;
                var progress = new Progress(100);
                OnProgressChanged(progress);
                var ids = src.SelectIds(DatabaseTableId.Hash);
                var idCount = ids.Rows.Count;
                OnFoundData(DatabaseTableId.Hash, idCount);
                for (var idIndex = 0; idIndex < idCount; idIndex++)
                {
                    if (Cancelled)
                        break;
                    var id = Convert.ToUInt32(ids.Rows[idIndex][0]);
                    var hashRow = src.Select(DatabaseTableId.Hash, id).Rows[0];
                    var hash = (string)hashRow.ItemArray[1];
                    var info = Convert.ToString(hashRow.ItemArray[2]);
                    if (String.IsNullOrEmpty(info))
                        Database.InsertHash(hash);
                    else
                        Database.InsertUpdateHash(hash, info);
                    // names
                    var names = src.Select(DatabaseTableId.Name, id);
                    OnFoundData(DatabaseTableId.Name, names.Rows.Count);
                    for (var i = 0; i < names.Rows.Count; i++)
                    {
                        var nameRow = names.Rows[i];
                        var name = (string)nameRow[1];
                        Database.InsertName(hash, name, (DateTime)nameRow[2]);
                    }
                    // ips
                    var ips = src.Select(DatabaseTableId.Ip, id);
                    OnFoundData(DatabaseTableId.Ip, ips.Rows.Count);
                    for (var i = 0; i < ips.Rows.Count; i++)
                    {
                        var ipRow = ips.Rows[i];
                        var ip = (string)ipRow[1];
                        Database.InsertIp(hash, ip, (DateTime)ipRow[2]);
                    }
                    // gsids
                    if (srcGameVersion == PcdbGameVersion.COP)
                    {
                        var gsids = src.Select(DatabaseTableId.Gsid, id);
                        OnFoundData(DatabaseTableId.Gsid, gsids.Rows.Count);
                        for (var i = 0; i < gsids.Rows.Count; i++)
                        {
                            var gsidRow = gsids.Rows[i];
                            var gsid = Convert.ToUInt32(gsidRow[1]);
                            Database.InsertGsid(hash, gsid, (DateTime)gsidRow[2]);
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

            private void ParseRev2(PcdbRx2File src)
            { ParseRev1(src); }

            private void ParseRev3(PcdbFile src)
            {
                var srcGameVersion = src.GetGameVersion();
                var selfGameVersion = Database.GetGameVersion();
                if (srcGameVersion != selfGameVersion)
                    return;
                var progress = new Progress(100);
                OnProgressChanged(progress);
                var hashes = src.SelectHashes(DatabaseTableId.Hash);
                var hashCount = hashes.Rows.Count;
                OnFoundData(DatabaseTableId.Hash, hashCount);
                for (var hashIndex = 0; hashIndex < hashCount; hashIndex++)
                {
                    if (Cancelled)
                        break;
                    var hash = (string)hashes.Rows[hashIndex][0];
                    var hashRow = src.Select(DatabaseTableId.Hash, hash).Rows[0];
                    var info = (string)hashRow.ItemArray[1];
                    if (String.IsNullOrEmpty(info))
                        Database.InsertHash(hash);
                    else
                        Database.InsertUpdateHash(hash, info);
                    // names
                    var names = src.Select(DatabaseTableId.Name, hash);
                    OnFoundData(DatabaseTableId.Name, names.Rows.Count);
                    for (var i = 0; i < names.Rows.Count; i++)
                    {
                        var nameRow = names.Rows[i];
                        var name = (string)nameRow[1];
                        Database.InsertName(hash, name, (DateTime)nameRow[2]);
                    }
                    // ips
                    var ips = src.Select(DatabaseTableId.Ip, hash);
                    OnFoundData(DatabaseTableId.Ip, ips.Rows.Count);
                    for (var i = 0; i < ips.Rows.Count; i++)
                    {
                        var ipRow = ips.Rows[i];
                        var ip = (string)ipRow[1];
                        Database.InsertIp(hash, ip, (DateTime)ipRow[2]);
                    }
                    // gsids
                    if (srcGameVersion == PcdbGameVersion.COP)
                    {
                        var gsids = src.Select(DatabaseTableId.Gsid, hash);
                        OnFoundData(DatabaseTableId.Gsid, gsids.Rows.Count);
                        for (var i = 0; i < gsids.Rows.Count; i++)
                        {
                            var gsidRow = gsids.Rows[i];
                            var gsid = Convert.ToUInt32(gsidRow[1]);
                            Database.InsertGsid(hash, gsid, (DateTime)gsidRow[2]);
                        }
                    }
                    var newProgress = (uint)Math.Round(100.0*(hashIndex + 1)/hashCount);
                    if (newProgress > progress.Current)
                    {
                        progress.Current = newProgress;
                        OnProgressChanged(progress);
                    }
                }
            }
        }

        public ParserBase GetParser(HostParser host, PcdbFile database)
        { return new PcdbParserImpl(host, database); }

        public string AcceptedFileExtension
        { get { return ".pcdb"; } }
        
        public bool CheckFormat(string path)
        {
            return Path.GetExtension(path).ToLowerInvariant() == AcceptedFileExtension && PcdbFile.CheckFormat(path);
        }

        #region Singleton implementation

        private PcdbParser() {}

        private static readonly PcdbParser instance = new PcdbParser();

        public static PcdbParser Instance
        {
            get { return instance; }
        }

        #endregion
    }
}
