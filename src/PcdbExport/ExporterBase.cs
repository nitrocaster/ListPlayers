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
using System.ComponentModel;
using System.IO;
using System.Text;
using ListPlayers.PcdbModel;
using ListPlayers.Properties;

namespace ListPlayers.PcdbExport
{
    public abstract class ExporterBase : IExporter
    {
        protected readonly BackgroundWorker Worker;
        protected PcdbChunk Chunk;
        protected volatile int CurrentProgress;
        protected int MaximumProgress;
        protected StreamWriter Writer;

        protected ExporterBase(ITextExporterView view)
        {
            View = view;
            Worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            Worker.DoWork += ExportProc;
            Worker.ProgressChanged += OnProgressChanged;
            Worker.RunWorkerCompleted += OnExportCompleted;
        }
        
        public abstract ExportFormat Format { get; }

        public ITextExporterView View { get; private set; }

        public void Export(PcdbChunk chunk, string path)
        {
            if (Worker.IsBusy)
                throw new InvalidOperationException("Export has been already started.");
            Chunk = chunk;
            Writer = new StreamWriter(path, false, Encoding.Default);
            View.IsBusy = true;
            View.InvokeAsync(() =>
            {
                MaximumProgress = Chunk.Hashes.Rows.Count;
                CurrentProgress = 0;
                View.ProgressMin = 0;
                View.ProgressMax = MaximumProgress;
                View.ProgressValue = 0;
                View.StatusText = StringTable.ExportEtc;
            });
            Worker.RunWorkerAsync();
        }

        public virtual void Dispose() { Worker.Dispose(); }

        protected abstract void ExportProc(object sender, DoWorkEventArgs e);

        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            View.InvokeAsync(() =>
            {
                View.ProgressValue = CurrentProgress;
                View.CounterText = Math.Round(100.0 * CurrentProgress / MaximumProgress) + "%";
            });
        }

        private void OnExportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            View.InvokeAsync(() =>
            {
                View.IsBusy = false;
                View.StatusText = View.Cancelled ? StringTable.OperationCancelled : StringTable.ExportCompleted;
                View.CancelButtonText = StringTable.Close;
                if (View.Cancelled && View.UserClose)
                    View.Close();
            });
        }
    }
}
