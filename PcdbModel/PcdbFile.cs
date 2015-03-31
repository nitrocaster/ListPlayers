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
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using ListPlayers.Parsers;

namespace ListPlayers.PcdbModel
{
    public sealed class PcdbFile
    {
        public const PcdbRevision SupportedRevision = PcdbRevision.Rev2;
        private const string sqlDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        private static readonly char[] sqliteSignature = "SQLite format 3".ToCharArray();
        /// <summary>
        ///     "01.01.1970 12:00:00"
        /// </summary>
        public static readonly DateTime InvalidDateTime =
            DateTime.ParseExact("01.01.1970 12:00:00", Utils.DateTimePatternLong, CultureInfo.InvariantCulture);

        private readonly SQLiteDatabase database;

        private PcdbFile(string path)
        {
            FileName = path;
            database = new SQLiteDatabase(FileName);
            database.BeginTransaction();
            if (database.Execute("SELECT DATEINFO FROM NAMES WHERE 0") == null)
                Revision = (int)PcdbRevision.Rev0;
            else if (database.Execute("SELECT 1 FROM DBVERSION WHERE 0") == null)
                Revision = (int)PcdbRevision.Rev1;
            else
                Revision = (int)PcdbRevision.Rev2;
            database.CommitTransaction();
            database.Close();
        }

        private PcdbFile(string path, PcdbGameVersion gameVersion)
        {
            FileName = path;
            database = new SQLiteDatabase(FileName);
            database.BeginTransaction();
            {
                database.ExecuteNonQuery("PRAGMA encoding = 'UTF-8'");
                switch (gameVersion)
                {
                case PcdbGameVersion.Unknown:
                    database.RollBackTransaction();
                    database.Close();
                    throw new NotSupportedException("Unknown game version.");
                case PcdbGameVersion.SHOC:
                    database.RollBackTransaction();
                    database.Close();
                    throw new NotSupportedException("S.T.A.L.K.E.R.: Shadow of Chernobyl is not supported.");
                case PcdbGameVersion.CS:
                    database.ExecuteNonQuery(new[]
                    {
                        "CREATE TABLE DBVERSION (VERSION INTEGER UNSIGNED NOT NULL)",
                        "INSERT INTO DBVERSION VALUES ('" + (int)SupportedRevision + "')",
                        "CREATE TABLE DBTYPE (TYPEID TINYINT UNSIGNED NOT NULL)",
                        "INSERT INTO DBTYPE VALUES ('2')",
                        "CREATE TABLE HASHES (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                            "HASH CHAR(32) NOT NULL, INFO TINYTEXT NULL)",
                        "CREATE TABLE NAMES (ID INTEGER NOT NULL, NAME TINYTEXT NOT NULL, DATEINFO TIMESTAMP NULL)",
                        "CREATE TABLE IPS (ID INTEGER NOT NULL, IP TINYTEXT NOT NULL, DATEINFO TIMESTAMP NULL)"
                    });
                    break;
                case PcdbGameVersion.COP:
                    database.ExecuteNonQuery(new[]
                    {
                        "CREATE TABLE DBVERSION (VERSION INTEGER UNSIGNED NOT NULL)",
                        "INSERT INTO DBVERSION VALUES ('" + (int)SupportedRevision + "')",
                        "CREATE TABLE DBTYPE (TYPEID TINYINT UNSIGNED NOT NULL)",
                        "INSERT INTO DBTYPE VALUES ('3')",
                        "CREATE TABLE HASHES (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                            "HASH CHAR(32) NOT NULL, INFO TINYTEXT NULL)",
                        "CREATE TABLE NAMES (ID INTEGER NOT NULL, NAME TINYTEXT NOT NULL, DATEINFO TIMESTAMP NULL)",
                        "CREATE TABLE IPS (ID INTEGER NOT NULL, IP TINYTEXT NOT NULL, DATEINFO TIMESTAMP NULL)",
                        "CREATE TABLE GSIDS (ID INTEGER NOT NULL, GSID INT UNSIGNED NOT NULL, " +
                            "DATEINFO TIMESTAMP NULL)"
                    });
                    break;
                }
            }
            database.CommitTransaction();
            database.Close();
            Revision = (int)SupportedRevision;
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

        public event Action<DatabaseTableId, int> AppendedData;

        private void OnAppendedData(DatabaseTableId field, int count = 1)
        {
            if (AppendedData != null)
                AppendedData(field, count);
        }

        public static bool CheckFormat(string filename)
        {
            var buf = new char[sqliteSignature.Length];
            using (var reader = new StreamReader(filename, Encoding.ASCII))
                reader.Read(buf, 0, sqliteSignature.Length);
            return !sqliteSignature.Where((t, i) => buf[i] != t).Any();
        }

        public static bool CheckGameVersion(string filename)
        {
            var ver = GetGameVersion(filename);
            return ver == PcdbGameVersion.CS || ver == PcdbGameVersion.COP;
        }

        public static PcdbGameVersion GetGameVersion(string filename)
        {
            var db = new SQLiteDatabase(filename);
            try
            {
                return (PcdbGameVersion)Convert.ToInt32(
                    db.Execute("SELECT TYPEID FROM DBTYPE").Rows[0][0]);
            }
            finally
            {
                db.Close();
            }
        }

        public PcdbGameVersion GetGameVersion()
        {
            var result = (PcdbGameVersion)Convert.ToInt32(
                database.Execute("SELECT TYPEID FROM DBTYPE").Rows[0][0]);
            return result;
        }

        public static PcdbFile Create(string filename, PcdbGameVersion type)
        {
            if (File.Exists(filename))
                File.Delete(filename);
            return new PcdbFile(filename, type);
        }

        public static PcdbFile Open(string filename) { return new PcdbFile(filename); }

        public bool OpenConnection() { return database.Open(); }

        public bool CloseConnection() { return database.Close(); }

        public void BeginTransaction() { database.BeginTransaction(); }

        public void CommitTransaction() { database.CommitTransaction(); }

        public void RollBackTransaction() { database.RollBackTransaction(); }

        public void Compress() { database.Vacuum(); }

        /// <summary>
        ///     Removes empty names and ips.
        /// </summary>
        /// <param name="names">Removed name count.</param>
        /// <param name="ips">Removed ip count.</param>
        public void Clean(out int names, out int ips)
        {
            names = 0;
            ips = 0;
            database.BeginTransaction();
            var data = database.Execute("SELECT COUNT(ID) FROM NAMES WHERE NAME = \"\"");
            if (data != null)
                names = data.Rows.Count;
            data = database.Execute("SELECT COUNT(ID) FROM IPS WHERE IP = \"\"");
            if (data != null)
                ips = data.Rows.Count;
            if (names > 0)
                database.ExecuteNonQuery("DELETE FROM NAMES WHERE NAME = \"\"");
            if (ips > 0)
                database.ExecuteNonQuery("DELETE FROM IPS WHERE IP = \"\"");
            database.CommitTransaction();
            if (names > 0 || ips > 0)
                database.Vacuum();
        }

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
                for (var j = 1; j < len; ++j)
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
                    for (var j = 1; j < len; ++j)
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
                    for (var j = 1; j < len; ++j)
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
                for (var j = 1; j < len; ++j)
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
                for (var j = 1; j < len; ++j)
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

        public void UpdateHash(string hash, string unescapedInfo)
        {
            uint id = 0;
            if (!GetIdByHash(hash, ref id))
                return;
            UpdateHash(id, unescapedInfo);
        }

        public void UpdateName(string hash, string unescapedName, DateTime date)
        {
            uint id = 0;
            if (!GetIdByHash(hash, ref id))
                return;
            UpdateName(id, EscapeString(unescapedName), date);
        }

        public void UpdateIp(string hash, string ip, DateTime date)
        {
            uint id = 0;
            if (!GetIdByHash(hash, ref id))
                return;
            UpdateIp(id, ip, date);
        }

        public void UpdateGsid(string hash, uint gsid, DateTime date)
        {
            uint id = 0;
            if (!GetIdByHash(hash, ref id))
                return;
            UpdateGsid(id, gsid, date);
        }

        public static uint[] ExtractIds(DataTable src)
        {
            var count = src.Rows.Count;
            var ids = new uint[count];
            for (var i = 0; i < count; ++i)
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

        public void AppendNew(string hash)
        {
            if (hash == "")
                return;
            OnAppendedData(DatabaseTableId.Hash);
            InsertHash(hash);
        }

        public void AppendNew(string hash, string unescapedInfo)
        {
            if (hash == "")
                return;
            OnAppendedData(DatabaseTableId.Hash);
            InsertHash(hash, unescapedInfo);
        }

        public void AppendNew(string hash, string unescapedInfo, string unescapedName, DateTime date)
        {
            if (hash == "")
                return;
            OnAppendedData(DatabaseTableId.Hash);
            InsertHash(hash, unescapedInfo);
            uint id = 0;
            GetIdByHash(hash, ref id);
            if (unescapedName != "")
            {
                OnAppendedData(DatabaseTableId.Name);
                InsertName(id, EscapeString(unescapedName), date);
            }
        }

        public void AppendNew(string hash, string unescapedInfo, string unescapedName, string ip, DateTime date)
        {
            if (hash == "")
                return;
            OnAppendedData(DatabaseTableId.Hash);
            InsertHash(hash, unescapedInfo);
            uint id = 0;
            GetIdByHash(hash, ref id);
            if (unescapedName != "")
            {
                OnAppendedData(DatabaseTableId.Name);
                InsertName(id, EscapeString(unescapedName), date);
            }
            if (ip != "")
            {
                OnAppendedData(DatabaseTableId.Ip);
                InsertIp(id, ip, date);
            }
        }

        public void AppendNew(string hash, string unescapedInfo, string unescapedName,
            string ip, uint gsid, DateTime date)
        {
            if (hash == "")
                return;
            OnAppendedData(DatabaseTableId.Hash);
            InsertHash(hash, unescapedInfo);
            uint id = 0;
            GetIdByHash(hash, ref id);
            if (unescapedName != "")
            {
                OnAppendedData(DatabaseTableId.Name);
                InsertName(id, EscapeString(unescapedName), date);
            }
            if (ip != "")
            {
                OnAppendedData(DatabaseTableId.Ip);
                InsertIp(id, ip, date);
            }
            OnAppendedData(DatabaseTableId.Gsid);
            InsertGsid(id, gsid, date);
        }

        public void AppendName(string hash, string unescapedName, DateTime date)
        {
            if (unescapedName == "")
                return;
            uint id = 0;
            if (!GetIdByHash(hash, ref id))
            {
                AppendNew(hash);
                GetIdByHash(hash, ref id);
            }
            AppendName(id, unescapedName, date);
        }

        public void AppendIp(string hash, string ip, DateTime date)
        {
            if (ip == "")
                return;
            uint id = 0;
            if (!GetIdByHash(hash, ref id))
            {
                AppendNew(hash);
                GetIdByHash(hash, ref id);
            }
            AppendIp(id, ip, date);
        }

        public void AppendGsid(string hash, uint gsid, DateTime date)
        {
            uint id = 0;
            if (!GetIdByHash(hash, ref id))
            {
                AppendNew(hash);
                GetIdByHash(hash, ref id);
            }
            AppendGsid(id, gsid, date);
        }
        
        public void Append(string hash, string unescapedName, string ip, DateTime date)
        {
            uint id = 0;
            if (!GetIdByHash(hash, ref id))
            {
                AppendNew(hash, "", unescapedName, ip, date);
                GetIdByHash(hash, ref id);
            }
            AppendName(id, unescapedName, date);
            AppendIp(id, ip, date);
        }

        public void Append(string hash, string unescapedName, string ip, uint gsid, DateTime date)
        {
            uint id = 0;
            if (!GetIdByHash(hash, ref id))
            {
                AppendNew(hash, "", unescapedName, ip, gsid, date);
                GetIdByHash(hash, ref id);
            }
            AppendName(id, unescapedName, date);
            AppendIp(id, ip, date);
            AppendGsid(id, gsid, date);
        }

        private void AppendName(uint id, string unescapedName, DateTime date)
        {
            if (unescapedName == "")
                return;
            var escapedName = EscapeString(unescapedName);
            if (!NameExist(id, escapedName))
            {
                OnAppendedData(DatabaseTableId.Name);
                InsertName(id, escapedName, date);
            }
            else
                UpdateName(id, escapedName, date);
        }

        private void AppendIp(uint id, string ip, DateTime date)
        {
            if (ip == "")
                return;
            if (!IpExist(id, ip))
            {
                OnAppendedData(DatabaseTableId.Ip);
                InsertIp(id, ip, date);
            }
            else
                UpdateIp(id, ip, date);
        }

        private void AppendGsid(uint id, uint gsid, DateTime date)
        {
            if (!GsidExist(id, gsid))
            {
                OnAppendedData(DatabaseTableId.Gsid);
                InsertGsid(id, gsid, date);
            }
            else
                UpdateGsid(id, gsid, date);
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

        private void InsertHash(string hash, string unescapedInfo)
        {
            var query = new StringBuilder("INSERT INTO HASHES(HASH, INFO) VALUES ('", 512);
            query.Append(hash);
            query.Append("', '");
            query.Append(EscapeString(unescapedInfo));
            query.Append("')");
            database.ExecuteNonQuery(query.ToString());
        }

        private void InsertHash(string hash)
        {
            var query = new StringBuilder("INSERT INTO HASHES(HASH) VALUES ('", 100);
            query.Append(hash);
            query.Append("')");
            database.ExecuteNonQuery(query.ToString());
        }

        private void UpdateHash(uint id, string unescapedInfo)
        {
            var query = new StringBuilder("UPDATE HASHES SET INFO = '");
            query.Append(EscapeString(unescapedInfo));
            query.Append("' WHERE ID = ");
            query.Append(id);
            database.ExecuteNonQuery(query.ToString());
        }

        private void InsertName(uint id, string escapedName, DateTime date)
        {
            var query = new StringBuilder("INSERT INTO NAMES(ID, NAME, DATEINFO) VALUES ('");
            query.Append(id);
            query.Append("', '");
            query.Append(escapedName);
            query.Append("', '");
            query.Append(date.ToString(sqlDateTimeFormat));
            query.Append("')");
            database.ExecuteNonQuery(query.ToString());
        }

        private void UpdateName(uint id, string escapedName, DateTime newDate)
        {
            var query = new StringBuilder("UPDATE NAMES SET DATEINFO = '");
            query.Append(newDate.ToString(sqlDateTimeFormat));
            query.Append("' WHERE ID = ");
            query.Append(id);
            query.Append(" AND NAME = '");
            query.Append(escapedName);
            query.Append('\'');
            database.ExecuteNonQuery(query.ToString());
        }

        private void InsertIp(uint id, string ip, DateTime date)
        {
            var query = new StringBuilder("INSERT INTO IPS(ID, IP, DATEINFO) VALUES ('");
            query.Append(id);
            query.Append("', '");
            query.Append(ip);
            query.Append("', '");
            query.Append(date.ToString(sqlDateTimeFormat));
            query.Append("')");
            database.ExecuteNonQuery(query.ToString());
        }

        private void UpdateIp(uint id, string ip, DateTime newDate)
        {
            var query = new StringBuilder("UPDATE IPS SET DATEINFO = '", 100);
            query.Append(newDate.ToString(sqlDateTimeFormat));
            query.Append("' WHERE ID = ");
            query.Append(id);
            query.Append(" AND IP = '");
            query.Append(ip);
            query.Append('\'');
            database.ExecuteNonQuery(query.ToString());
        }

        private void InsertGsid(uint id, uint gsid, DateTime date)
        {
            var query = new StringBuilder("INSERT INTO GSIDS(ID, GSID, DATEINFO) VALUES ('", 100);
            query.Append(id);
            query.Append("', '");
            query.Append(gsid);
            query.Append("', '");
            query.Append(date.ToString(sqlDateTimeFormat));
            query.Append("')");
            database.ExecuteNonQuery(query.ToString());
        }

        private void UpdateGsid(uint id, uint gsid, DateTime newDate)
        {
            var query = new StringBuilder("UPDATE GSIDS SET DATEINFO = '", 100);
            query.Append(newDate.ToString(sqlDateTimeFormat));
            query.Append("' WHERE ID = ");
            query.Append(id);
            query.Append(" AND GSID = '");
            query.Append(gsid);
            query.Append('\'');
            database.ExecuteNonQuery(query.ToString());
        }
    }
}
