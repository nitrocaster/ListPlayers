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
        public const PcdbRevision SupportedRevision = PcdbRevision.Rev3;
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
            Revision = PcdbUtil.GetRevision(database);
            database.Close();
        }

        private PcdbFile(string path, PcdbGameVersion gameVersion)
        {
            FileName = path;
            database = new SQLiteDatabase(FileName);
            database.BeginTransaction();
            {
                database.ExecuteNonQuery(new[] {"PRAGMA encoding = 'UTF-8'", "PRAGMA foreign_keys = ON"});
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
                        "CREATE TABLE HASHES (HASH CHAR(32) NOT NULL PRIMARY KEY, INFO TINYTEXT NULL)",
                        "CREATE TABLE NAMES (HASH CHAR(32) NOT NULL, NAME TINYTEXT NOT NULL, " +
                            "DATEINFO TIMESTAMP NULL, FOREIGN KEY (HASH) REFERENCES HASHES(HASH), " +
                            "PRIMARY KEY (HASH, NAME))",
                        "CREATE TABLE IPS (HASH CHAR(32) NOT NULL, IP TINYTEXT NOT NULL, " +
                            "DATEINFO TIMESTAMP NULL, FOREIGN KEY (HASH) REFERENCES HASHES(HASH), " +
                            "PRIMARY KEY (HASH, IP))"
                    });
                    break;
                case PcdbGameVersion.COP:
                    database.ExecuteNonQuery(new[]
                    {
                        "CREATE TABLE DBVERSION (VERSION INTEGER UNSIGNED NOT NULL)",
                        "INSERT INTO DBVERSION VALUES ('" + (int)SupportedRevision + "')",
                        "CREATE TABLE DBTYPE (TYPEID TINYINT UNSIGNED NOT NULL)",
                        "INSERT INTO DBTYPE VALUES ('3')",
                        "CREATE TABLE HASHES (HASH CHAR(32) NOT NULL PRIMARY KEY, INFO TINYTEXT NULL)",
                        "CREATE TABLE NAMES (HASH CHAR(32) NOT NULL, NAME TINYTEXT NOT NULL, " +
                            "DATEINFO TIMESTAMP NULL, FOREIGN KEY (HASH) REFERENCES HASHES(HASH), " +
                            "PRIMARY KEY (HASH, NAME))",
                        "CREATE TABLE IPS (HASH CHAR(32) NOT NULL, IP TINYTEXT NOT NULL, " +
                            "DATEINFO TIMESTAMP NULL, FOREIGN KEY (HASH) REFERENCES HASHES(HASH), " +
                            "PRIMARY KEY (HASH, IP))",
                        "CREATE TABLE GSIDS (HASH CHAR(32) NOT NULL, GSID INT UNSIGNED NOT NULL, " +
                            "DATEINFO TIMESTAMP NULL, FOREIGN KEY (HASH) REFERENCES HASHES(HASH), " +
                            "PRIMARY KEY (HASH, GSID))"
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
            db.Open();
            try
            {
                return PcdbUtil.GetGameVersion(db);
            }
            finally
            {
                db.Close();
            }
        }

        public PcdbGameVersion GetGameVersion() { return PcdbUtil.GetGameVersion(database); }

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
            var data = database.Execute("SELECT COUNT(*) FROM NAMES WHERE NAME = \"\"");
            if (data != null)
                names = data.Rows.Count;
            data = database.Execute("SELECT COUNT(*) FROM IPS WHERE IP = \"\"");
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

        public DataTable Select(DatabaseTableId table, string[] hashes, string[] filter, bool asPattern = false)
        {
            var tableName = GetTableName(table);
            var fieldName = GetFieldName(table);
            DataTable result = null;
            var query = new StringBuilder("SELECT * FROM ");
            query.Append(tableName);
            query.Append(" WHERE");
            var idUsed = false;
            var len = hashes.Length;
            if (len > 0)
            {
                idUsed = true;
                query.Append(" HASH IN ('");
                query.Append(hashes[0]);
                for (var j = 1; j < len; j++)
                {
                    query.Append("','");
                    query.Append(hashes[j]);
                }
                query.Append("')");
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

        public DataTable Select(DatabaseTableId table, string hash)
        {
            var query = new StringBuilder("SELECT * FROM ");
            query.Append(GetTableName(table));
            query.Append(" WHERE HASH = '");
            query.Append(hash);
            query.Append('\'');
            return database.Execute(query.ToString());
        }

        public DataTable SelectHashes(DatabaseTableId table, string[] filter, bool asPattern = false)
        {
            var len = filter.Length;
            if (len == 0)
                return null;
            var tableName = GetTableName(table);
            var fieldName = GetFieldName(table);
            var query = new StringBuilder("SELECT DISTINCT HASH FROM ");
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

        public DataTable SelectHashes(DatabaseTableId table)
        { return database.Execute("SELECT DISTINCT HASH FROM " + GetTableName(table)); }
        
        public static string[] ExtractHashes(DataTable src)
        {
            var count = src.Rows.Count;
            var hashes = new string[count];
            for (var i = 0; i < count; i++)
                hashes[i] = Convert.ToString(src.Rows[i][0]);
            return hashes;
        }
        
        public void InsertHash(string hash, string unescapedInfo)
        {
            var inserted = PrivateInsertHash(hash, unescapedInfo);
            if (inserted > 0)
                OnAppendedData(DatabaseTableId.Hash, inserted);
        }

        public void InsertHash(string hash)
        {
            var query = new StringBuilder("INSERT OR IGNORE INTO HASHES(HASH) VALUES ('", 100);
            query.Append(hash);
            query.Append("')");
            var inserted = database.ExecuteNonQuery(query.ToString());
            if (inserted > 0)
                OnAppendedData(DatabaseTableId.Hash, inserted);
        }

        public void UpdateHash(string hash, string unescapedInfo)
        {
            var query = new StringBuilder("UPDATE HASHES SET INFO = '");
            query.Append(EscapeString(unescapedInfo));
            query.Append("' WHERE HASH = '");
            query.Append(hash);
            query.Append("'");
            database.ExecuteNonQuery(query.ToString());
        }

        public void InsertUpdateHash(string hash, string unescapedInfo)
        {
            if (PrivateInsertHash(hash, unescapedInfo) == 0)
                UpdateHash(hash, unescapedInfo);
        }
        
        private int PrivateInsertHash(string hash, string unescapedInfo)
        {
            var query = new StringBuilder("INSERT OR IGNORE INTO HASHES(HASH, INFO) VALUES ('", 512);
            query.Append(hash);
            query.Append("', '");
            query.Append(EscapeString(unescapedInfo));
            query.Append("')");
            return database.ExecuteNonQuery(query.ToString());
        }

        public void InsertName(string hash, string escapedName, DateTime date)
        {
            var query = new StringBuilder("INSERT OR IGNORE INTO NAMES(HASH, NAME, DATEINFO) VALUES ('");
            query.Append(hash);
            query.Append("', '");
            query.Append(escapedName);
            query.Append("', '");
            query.Append(date.ToString(sqlDateTimeFormat));
            query.Append("')");
            var inserted = database.ExecuteNonQuery(query.ToString());
            if (inserted > 0)
                OnAppendedData(DatabaseTableId.Name, inserted);
        }

        public void UpdateName(string hash, string escapedName, DateTime newDate)
        {
            var query = new StringBuilder("UPDATE NAMES SET DATEINFO = '");
            query.Append(newDate.ToString(sqlDateTimeFormat));
            query.Append("' WHERE HASH = '");
            query.Append(hash);
            query.Append("' AND NAME = '");
            query.Append(escapedName);
            query.Append('\'');
            database.ExecuteNonQuery(query.ToString());
        }

        public void InsertIp(string hash, string ip, DateTime date)
        {
            var query = new StringBuilder("INSERT OR IGNORE INTO IPS(HASH, IP, DATEINFO) VALUES ('");
            query.Append(hash);
            query.Append("', '");
            query.Append(ip);
            query.Append("', '");
            query.Append(date.ToString(sqlDateTimeFormat));
            query.Append("')");
            var inserted = database.ExecuteNonQuery(query.ToString());
            if (inserted > 0)
                OnAppendedData(DatabaseTableId.Ip, inserted);
        }

        public void UpdateIp(string hash, string ip, DateTime newDate)
        {
            var query = new StringBuilder("UPDATE IPS SET DATEINFO = '", 100);
            query.Append(newDate.ToString(sqlDateTimeFormat));
            query.Append("' WHERE HASH = '");
            query.Append(hash);
            query.Append("' AND IP = '");
            query.Append(ip);
            query.Append('\'');
            database.ExecuteNonQuery(query.ToString());
        }

        public void InsertGsid(string hash, uint gsid, DateTime date)
        {
            var query = new StringBuilder("INSERT OR IGNORE INTO GSIDS(HASH, GSID, DATEINFO) VALUES ('", 100);
            query.Append(hash);
            query.Append("', '");
            query.Append(gsid);
            query.Append("', '");
            query.Append(date.ToString(sqlDateTimeFormat));
            query.Append("')");
            var inserted = database.ExecuteNonQuery(query.ToString());
            if (inserted > 0)
                OnAppendedData(DatabaseTableId.Gsid, inserted);
        }

        public void UpdateGsid(string hash, uint gsid, DateTime newDate)
        {
            var query = new StringBuilder("UPDATE GSIDS SET DATEINFO = '", 100);
            query.Append(newDate.ToString(sqlDateTimeFormat));
            query.Append("' WHERE HASH = '");
            query.Append(hash);
            query.Append("' AND GSID = '");
            query.Append(gsid);
            query.Append('\'');
            database.ExecuteNonQuery(query.ToString());
        }
    }
}
