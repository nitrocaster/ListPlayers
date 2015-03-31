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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using ListPlayers.Parsers;
using ListPlayers.PcdbExport;
using ListPlayers.PcdbModel;
using ListPlayers.Properties;

namespace ListPlayers.Common
{
    public sealed class DatabaseViewer : IDisposable
    {
        public event Action<List<PcdbEntry>, bool> SearchCompleted;
        public event Action<bool> ConnectionStatusChanged;
        private readonly object syncSearch = new object();
        private bool cancel;
        private PcdbFile database;
        private PcdbGameVersion dbGameVersion = PcdbGameVersion.Unknown;
        private PcdbChunk lastChunk;
        private bool shutdown;
        private ManualResetEventSlim syncSearchCompleted = new ManualResetEventSlim(false);

        public DatabaseViewer() { Utils.CreateThread(SearchProc, "Seeker"); }

        public string FileName
        {
            get { return database != null ? database.FileName : ""; }
        }

        public PcdbGameVersion GameVersion
        {
            get { return dbGameVersion; }
        }

        public SearchFilter Filter
        {
            get;
            set;
        }

        public bool IsBusy
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (syncSearchCompleted == null)
                return;
            shutdown = true;
            syncSearchCompleted.Reset();
            lock (syncSearch)
            {
                Monitor.Pulse(syncSearch);
            }
            syncSearchCompleted.Wait();
            syncSearchCompleted.Dispose();
            syncSearchCompleted = null;
        }

        private void OnSearchCompleted(List<PcdbEntry> result, bool cancelled)
        {
            if (SearchCompleted != null)
                SearchCompleted(result, cancelled);
        }

        private void OnConnectionStatusChanged(bool connected)
        {
            if (connected)
                dbGameVersion = database.GetGameVersion();
            if (ConnectionStatusChanged != null)
                ConnectionStatusChanged(connected);
        }

        public void SafeOpenDatabase(string path)
        {
            CloseDatabase();
            if (!File.Exists(path))
                throw new DatabaseViewerException("Выбранный файл не найден.");
            if (!PcdbFile.CheckFormat(path) | !PcdbFile.CheckGameVersion(path))
                throw new DatabaseViewerException("Формат выбранной базы данных не поддерживается.");
            OpenDatabase(path);
        }

        public void OpenDatabase(string path)
        {
            CloseDatabase();
            database = PcdbFile.Open(path);
            database.OpenConnection();
            OnConnectionStatusChanged(true);
        }

        public void BeginSearch()
        {
            if (IsBusy)
                throw new InvalidOperationException("Search has been already started");
            lock (syncSearch)
            {
                Monitor.Pulse(syncSearch);
            }
        }

        public void CancelSearch()
        {
            syncSearchCompleted.Reset();
            cancel = true;
            syncSearchCompleted.Wait();
        }

        public void CloseDatabase()
        {
            if (database == null)
                return;
            database.CloseConnection();
            database = null;
            OnConnectionStatusChanged(false);
        }

        public void UpdateEntry(PcdbEntry parent, IPcdbField field)
        {
            database.BeginTransaction();
            switch (field.Id)
            {
            case PcdbFieldId.Comment:
                var info = (PcdbItemContainer<string>)field;
                database.UpdateHash(parent.Hash, info);
                break;
            case PcdbFieldId.Name:
                var name = (PcdbName)field;
                database.UpdateName(parent.Hash, name.Name, name.Timestamp);
                break;
            case PcdbFieldId.Ip:
                var ip = (PcdbIp)field;
                database.UpdateIp(parent.Hash, ip.Ip, ip.Timestamp);
                break;
            case PcdbFieldId.Gsid:
                var gsid = (PcdbGsid)field;
                database.UpdateGsid(parent.Hash, gsid.Gsid, gsid.Timestamp);
                break;
            }
            database.CommitTransaction();
        }
        
        private void SearchProc()
        {
            while (!shutdown)
            {
                lock (syncSearch)
                {
                    Monitor.Wait(syncSearch);
                }
                if (shutdown)
                    break;
                IsBusy = true;
                var sample = new List<PcdbEntry>();
                lastChunk = CollectData();
                var chunk = lastChunk;
                if (chunk != null)
                {
                    var haveTs = database.Revision >= (int)PcdbRevision.Rev1;
                    foreach (DataRow hashRow in chunk.Hashes.Rows)
                    {
                        if (cancel || shutdown)
                            break;
                        var entry = new PcdbEntry(hashRow[1].ToString());
                        var currentId = Convert.ToInt32(hashRow[0]);
                        foreach (DataRow row in chunk.Names.Rows)
                        {
                            if (Convert.ToInt32(row[0]) == currentId)
                            {
                                var name = new PcdbName();
                                name.Name = row[1].ToString();
                                if (haveTs)
                                    name.Timestamp = (DateTime)row[2];
                                entry.Names.Add(name);
                            }
                        }
                        if (cancel || shutdown)
                            break;
                        foreach (DataRow row in chunk.Ips.Rows)
                        {
                            if (Convert.ToInt32(row[0]) == currentId)
                            {
                                var ip = new PcdbIp();
                                ip.Ip = row[1].ToString();
                                if (haveTs)
                                    ip.Timestamp = (DateTime)row[2];
                                entry.Ips.Add(ip);
                            }
                        }
                        if (cancel || shutdown)
                            break;
                        if (dbGameVersion == PcdbGameVersion.COP && chunk.Gsids != null)
                        {
                            foreach (DataRow row in chunk.Gsids.Rows)
                            {
                                if (Convert.ToInt32(row[0]) == currentId)
                                {
                                    var gsid = new PcdbGsid();
                                    gsid.Gsid = (uint)row[1];
                                    if (haveTs)
                                        gsid.Timestamp = (DateTime)row[2];
                                    entry.Gsids.Add(gsid);
                                }
                            }
                        }
                        entry.Info.Item = hashRow[2].ToString();
                        sample.Add(entry);
                    }
                }
                OnSearchCompleted(sample, cancel);
                cancel = false;
                IsBusy = false;
                syncSearchCompleted.Set();
            }
            IsBusy = false;
            syncSearchCompleted.Set();
        }

        private PcdbChunk CollectData()
        {
            var filter = Filter;
            PcdbChunk result = null;            
            database.BeginTransaction();
            do
            {
                var chunk = new PcdbChunk();
                uint[] ids;
                if (filter.Hashes.Length > 0)
                {
                    chunk.Hashes = database.SelectIds(DatabaseTableId.Hash, filter.Hashes, filter.UseHashPattern);
                    ids = PcdbFile.ExtractIds(chunk.Hashes);
                }
                else if (filter.Names.Length > 0)
                {
                    chunk.Names = database.SelectIds(DatabaseTableId.Name, filter.Names, filter.UseNamePattern);
                    ids = PcdbFile.ExtractIds(chunk.Names);
                }
                else if (filter.Ips.Length > 0)
                {
                    chunk.Ips = database.SelectIds(DatabaseTableId.Ip, filter.Ips, filter.UseIpPattern);
                    ids = PcdbFile.ExtractIds(chunk.Ips);
                }
                else if (dbGameVersion == PcdbGameVersion.COP && filter.Gsids.Length > 0)
                {
                    chunk.Gsids = database.SelectIds(DatabaseTableId.Gsid, filter.Gsids);
                    ids = PcdbFile.ExtractIds(chunk.Gsids);
                }
                else
                    break;
                if (filter.IncludeRelatedData)
                {
                    filter.Hashes = new string[] { };
                    filter.Names = new string[] { };
                    filter.Ips = new string[] { };
                    if (dbGameVersion == PcdbGameVersion.COP)
                        filter.Gsids = new string[] { };
                }
                chunk.Hashes = database.Select(DatabaseTableId.Hash, ids, filter.Hashes, filter.UseHashPattern);
                if (chunk.Hashes.Rows.Count == 0 && filter.Hashes.Length > 0)
                    break;
                chunk.Names = database.Select(DatabaseTableId.Name, ids, filter.Names, filter.UseNamePattern);
                if (chunk.Names.Rows.Count == 0 && filter.Names.Length > 0)
                    break;
                chunk.Ips = database.Select(DatabaseTableId.Ip, ids, filter.Ips, filter.UseIpPattern);
                if (chunk.Ips.Rows.Count == 0 && filter.Ips.Length > 0)
                    break;
                if (dbGameVersion == PcdbGameVersion.COP)
                {
                    chunk.Gsids = database.Select(DatabaseTableId.Gsid, ids, filter.Gsids);
                    if (chunk.Gsids.Rows.Count == 0 && filter.Gsids.Length > 0)
                        break;
                }
                result = chunk;
            } while (false);
            database.CommitTransaction();
            return result;
        }

        public void ExportCurrentSample()
        {
            using (var saver = new SaveFileDialog())
            {
                saver.Filter = StringTable.TextDocuments +
                    " (*.txt)|*.txt|RAdmin Panel Database (*.radb)|*.radb";
                saver.Title = StringTable.Export;
                saver.InitialDirectory = Root.SelfPath;
                if (saver.ShowDialog() != DialogResult.OK)
                    return;
                var ext = Path.GetExtension(saver.FileName);
                switch (ext)
                {
                // todo: GetExporter(...)
                case ".txt":
                {
                    var exporter = new TextExporterDialog();
                    exporter.Destination = saver.FileName;
                    exporter.Export(lastChunk, ExportFormat.Txt);
                    break;
                }
                case ".radb":
                {
                    var exporter = new TextExporterDialog();
                    exporter.Destination = saver.FileName;
                    exporter.Export(lastChunk, ExportFormat.Radb);
                    break;
                }
                }
            }
        }

        private void FormatField(IPcdbField root, StringBuilder data)
        {
            switch (root.Id)
            {
            case PcdbFieldId.Group | PcdbFieldId.Hash:
            {
                var entry = (PcdbEntry)root;
                data.AppendLine(entry.Hash);
                FormatField(entry.Names, data);
                FormatField(entry.Ips, data);
                if (dbGameVersion == PcdbGameVersion.COP)
                    FormatField(entry.Gsids, data);
                FormatField(entry.Info, data);
                break;
            }
            case (PcdbFieldId.Name | PcdbFieldId.Group):
            {
                data.AppendLine("\r\n; names\r\n");
                var groupName = (PcdbListContainer<PcdbName>)root;
                foreach (var fieldName in groupName)
                    FormatField(fieldName, data);
                break;
            }
            case PcdbFieldId.Name:
            {
                var fieldName = (PcdbName)root;
                data.AppendLine(String.Format("    {0,-32} | {1}",
                    fieldName.Name, fieldName.Timestamp.ToString(Utils.DateTimePatternLong)));
                break;
            }
            case (PcdbFieldId.Ip | PcdbFieldId.Group):
            {
                data.AppendLine("\r\n; ip addresses\r\n");
                var groupIp = (PcdbListContainer<PcdbIp>)root;
                foreach (var fieldIp in groupIp)
                    FormatField(fieldIp, data);
                break;
            }
            case PcdbFieldId.Ip:
            {
                var fieldIp = (PcdbIp)root;
                data.AppendLine(String.Format("    {0,-32} | {1}",
                    fieldIp.Ip, fieldIp.Timestamp.ToString(Utils.DateTimePatternLong)));
                break;
            }
            case (PcdbFieldId.Gsid | PcdbFieldId.Group):
            {
                data.AppendLine("\r\n; gamespy id's\r\n");
                var groupGsid = (PcdbListContainer<PcdbGsid>)root;
                foreach (var fieldGsid in groupGsid)
                    FormatField(fieldGsid, data);
                break;
            }
            case PcdbFieldId.Gsid:
            {
                var fieldGsid = (PcdbGsid)root;
                data.AppendLine(String.Format("    {0,-32} | {1}",
                    fieldGsid.Gsid, fieldGsid.Timestamp.ToString(Utils.DateTimePatternLong)));
                break;
            }
            case PcdbFieldId.Comment:
            {
                data.AppendLine("\r\n; comments\r\n");
                data.AppendLine((PcdbItemContainer<string>)root);
                data.AppendLine("\r\n; ----\r\n");
                break;
            }
            }
        }

        public void DataToClipboard(IPcdbField root)
        {
            var data = new StringBuilder();
            FormatField(root, data);
            Clipboard.SetText(data.ToString());
        }
    }
}
