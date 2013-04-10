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
using System.Windows.Forms;
using ListPlayers.Common;
using ListPlayers.Parsers;
using ListPlayers.Properties;

namespace ListPlayers.Dialogs
{
    public sealed partial class DatabaseUpdaterDialog : FormEx
    {
        private int  appendedGsids;
        private int  appendedHashes;
        private int  appendedIps;
        private int  appendedNames;

        private int  foundGsids;
        private int  foundHashes;
        private int  foundIps;
        private int  foundNames;
        
        private readonly DatabaseUpdater updater;


        public DatabaseUpdaterDialog(DatabaseUpdater updater)
        {
            InitializeComponent();

            this.updater = updater;
            ConnectUpdater();
        }

        private void ConnectUpdater()
        {
            updater.BeginTransaction      += OnBeginTransaction;
            updater.OpenConnection        += OnOpenConnection;
            updater.Compress              += OnCompress;
            updater.CommitTransaction     += OnCommitTransaction;
            updater.RollBackTransaction   += OnRollBackTransaction;
            updater.GlobalProgressChanged += OnGlobalProgressChanged;
            updater.FileProgressChanged   += OnFileProgressChanged;
            updater.UpdateCompleted       += OnUpdateCompleted;
            updater.FoundData             += OnFoundData;
            updater.AppendedData          += OnAppendedData;
        }

        private void DisconnectUpdater()
        {
            updater.BeginTransaction      -= OnBeginTransaction;
            updater.OpenConnection        -= OnOpenConnection;
            updater.Compress              -= OnCompress;
            updater.CommitTransaction     -= OnCommitTransaction;
            updater.RollBackTransaction   -= OnRollBackTransaction;
            updater.GlobalProgressChanged -= OnGlobalProgressChanged;
            updater.FileProgressChanged   -= OnFileProgressChanged;
            updater.UpdateCompleted       -= OnUpdateCompleted;
            updater.FoundData             -= OnFoundData;
            updater.AppendedData          -= OnAppendedData;
        }

        private void OnBeginTransaction()
        {
            InvokeAsync(() => lCurrentFile.Text = StringTable.OpeningTransaction);
        }

        private void OnOpenConnection()
        {
        }
        
        private void OnCompress()
        {
            InvokeAsync(() => lCurrentFile.Text = StringTable.Compressing);
        }

        private void OnCommitTransaction()
        {
            InvokeAsync(() =>
            {
                lCurrentFile.Text = StringTable.CommittingTransaction;
                pbProgress.Value  = pbProgress.Maximum;
                Text              = String.Format("{0} ({1}%)", StringTable.UpdatingDatabase, 100);
            });
        }

        private void OnRollBackTransaction()
        {
            InvokeAsync(() => lCurrentFile.Text = StringTable.CancellingTransaction);
        }

        private void OnGlobalProgressChanged(DatabaseUpdater.GlobalProgressInfo info)
        {
            if (info.Startup)
            {
                InvokeAsync(() =>
                {
                    pbProgress.Minimum = 0;
                    pbProgress.Maximum = (int)info.Progress.Maximum;
                    pbProgress.Value   = 0;
                });
                return;
            }

            var fileName = Path.GetFileName(info.File);
            var counter = String.Format("({0}/{1})", (info.Progress.Current + 1), info.Progress.Maximum);
            InvokeAsync(() =>
            {
                lCurrentFile.Text = fileName;
                tbCounter.Text    = counter;
                pbProgress.Value  = (int)info.Progress.Current;
                Text              = String.Format("{0} ({1}%)", StringTable.UpdatingDatabase, Math.Round(info.Progress.Percentage));
            });
        }

        private void OnFileProgressChanged(Progress progress)
        {
            InvokeAsync(() =>
            {
                pbFileProgress.Value = (int)progress.Current;
            });
        }

        public new DialogResult ShowDialog()
        {
            updater.BeginUpdate();
            return base.ShowDialog();
        }


        private void OnUpdateCompleted(int fileCount, bool success)
        {
            InvokeAsync(() =>
            {
                btnCancel.Text = StringTable.Close;

                if (!success)
                {
                    lCurrentFile.Text = StringTable.TransactionCancelled;
                    return;
                }

                lCurrentFile.Text = StringTable.UpdateCompleted;

                switch (fileCount%10)
                {
                    case 1:
                    {
                        MsgBox.Info(fileCount + StringTable.FilesProcessedX1);
                        break;
                    }
                    case 2:
                    case 3:
                    case 4:
                    {
                        MsgBox.Info(fileCount + StringTable.FilesProcessedX234);
                        break;
                    }
                    default:
                    {
                        MsgBox.Info(fileCount + StringTable.FilesProcessedXX);
                        break;
                    }
                }
            });
        }
        
        private void OnFoundData(DatabaseTableId field, int count)
        {
            InvokeAsync(() =>
            {
                switch (field)
                {
                    case DatabaseTableId.Hash:
                        foundHashes += count;
                        tbHashesFound.Text = foundHashes.ToString();
                        break;

                    case DatabaseTableId.Name:
                        foundNames += count;
                        tbNamesFound.Text = foundNames.ToString();
                        break;

                    case DatabaseTableId.Ip:
                        foundIps += count;
                        tbIpsFound.Text = foundIps.ToString();
                        break;

                    case DatabaseTableId.Gsid:
                        foundGsids += count;
                        tbGsidsFound.Text = foundGsids.ToString();
                        break;
                }
            });
        }

        private void OnAppendedData(DatabaseTableId field, int count = 1)
        {
            InvokeAsync(() =>
            {
                switch (field)
                {
                    case DatabaseTableId.Hash:
                        appendedHashes += count;
                tbHashesAppended.Text = appendedHashes.ToString();
                        break;

                    case DatabaseTableId.Name:
                        appendedNames += count;
                        tbNamesAppended.Text = appendedNames.ToString();
                        break;

                    case DatabaseTableId.Ip:
                        appendedIps += count;
                        tbIpsAppended.Text = appendedIps.ToString();
                        break;

                    case DatabaseTableId.Gsid:
                        appendedGsids += count;
                        tbGsidsAppended.Text = appendedGsids.ToString();
                        break;
                }
            });
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (updater.Completed)
            {
                Close();
                return;
            }
            if (!updater.Cancelled)
            {
                updater.Cancel();
            }
        }

        private void DatabaseUpdateDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!updater.Completed && !updater.Cancelled)
            {
                updater.Cancel();
                e.Cancel = true;
            }
        }
    }
}
