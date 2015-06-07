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
using ListPlayers.Parsers;
using ListPlayers.PcdbModel;

namespace ListPlayers.Common
{
    public sealed class DatabaseUpdater
    {
        public class GlobalProgressInfo
        {
            public bool Startup;
            public string File;
            public Progress Progress;

            public GlobalProgressInfo(string file, Progress progress, bool startup)
            {
                File = file;
                Progress = progress;
                Startup = startup;
            }
        }

        public event Action<DatabaseTableId, int> FoundData;
        public event Action<DatabaseTableId, int> AppendedData;
        public event Action<GlobalProgressInfo> GlobalProgressChanged;
        public event Action<Progress> FileProgressChanged;
        public event Action<int, bool> UpdateCompleted;
        public event Action CommitTransaction;
        public event Action Compress;
        public event Action RollBackTransaction;
        public event Action OpenConnection;
        public event Action BeginTransaction;

        private readonly IList<string> files;
        private readonly string dbPath;        
        private HostParser host;

        public DatabaseUpdater(string dbPath, IList<string> files)
        {
            this.dbPath = dbPath;
            this.files = files;
        }

        public void BeginUpdate() { Utils.CreateThread(BeginUpdateProc, "Updater"); }

        public void Update() { BeginUpdateProc(); }

        private void BeginUpdateProc()
        {
            var db = PcdbFile.Open(dbPath);
            OnOpenConnection();
            db.OpenConnection();
            OnBeginTransaction();
            db.BeginTransaction();
            var count = files.Count;
            var progressInfo = new GlobalProgressInfo(null, new Progress((uint)count), true);
            OnGlobalProgressChanged(progressInfo);
            progressInfo.Startup = false;
            using (host = new HostParser(db, ApplicationPersistent.ParserProviders))
            {
                db.AppendedData += OnAppendedData;
                host.FoundData += OnFoundData;
                host.ProgressChanged += OnFileProgressChanged;
                for (var i = 0; i < count; i++)
                {
                    if (Cancelled)
                    {
                        OnRollBackTransaction();
                        db.RollBackTransaction();
                        db.CloseConnection();
                        break;
                    }
                    progressInfo.Progress.Current = (uint)i;
                    progressInfo.File = files[i];
                    OnGlobalProgressChanged(progressInfo);
                    host.Parse(files[i]);
                }
                host.FoundData -= OnFoundData;
                host.ProgressChanged -= OnFileProgressChanged;
                db.AppendedData -= OnAppendedData;
            }
            host = null;
            if (!Cancelled)
            {
                OnCommitTransaction();
                db.CommitTransaction();
                db.CloseConnection();
                OnCompress();
                db.Compress();
            }
            Completed = true;
            OnUpdateCompleted(count, !Cancelled);
        }

        public bool Completed
        {
            get;
            private set;
        }

        public void Cancel()
        {
            Cancelled = true;
            if (host != null)
                host.Cancel();
        }

        public bool Cancelled
        {
            get;
            private set;
        }

        private void OnFoundData(DatabaseTableId field, int count = 1)
        {
            if (FoundData != null)
                FoundData(field, count);
        }

        private void OnAppendedData(DatabaseTableId field, int count = 1)
        {
            if (AppendedData != null)
                AppendedData(field, count);
        }

        private void OnGlobalProgressChanged(GlobalProgressInfo info)
        {
            if (GlobalProgressChanged != null)
                GlobalProgressChanged(info);
        }

        private void OnFileProgressChanged(Progress progress)
        {
            if (FileProgressChanged != null)
                FileProgressChanged(progress);
        }

        private void OnUpdateCompleted(int fileCount, bool success)
        {
            if (UpdateCompleted != null)
                UpdateCompleted(fileCount, success);
        }

        private void OnCommitTransaction()
        {
            if (CommitTransaction != null)
                CommitTransaction();
        }

        private void OnCompress()
        {
            if (Compress != null)
                Compress();
        }

        private void OnRollBackTransaction()
        {
            if (RollBackTransaction != null)
                RollBackTransaction();
        }

        private void OnOpenConnection()
        {
            if (OpenConnection != null)
                OpenConnection();
        }

        private void OnBeginTransaction()
        {
            if (BeginTransaction != null)
                BeginTransaction();
        }
    }
}
