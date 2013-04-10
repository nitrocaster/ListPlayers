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


namespace ListPlayers.PcdbExport
{
    public abstract class ExporterBase : IDisposable
    {
        protected readonly BackgroundWorker Worker;
        protected PcdbChunk Chunk;
        protected volatile int CurrentProgress;
        protected ITextExporterView Dialog;
        protected int MaximumProgress;
        protected StreamWriter Writer;


        protected ExporterBase()
        {
            Worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            Worker.DoWork += ExportProc;
            Worker.ProgressChanged += OnProgressChanged;
            Worker.RunWorkerCompleted += OnExportCompleted;
        }

        public virtual void Dispose()
        {
            Worker.Dispose();
        }

        public void RunExport(ITextExporterView dialog, string destination, PcdbChunk chunk)
        {
            if (Worker.IsBusy)
            {
                throw new InvalidOperationException("Export has been already started.");
            }

            Dialog = dialog;
            Chunk = chunk;
            Writer = new StreamWriter(destination, false, Encoding.Default);

            Dialog.InvokeAsync(() =>
            {
                MaximumProgress      = Chunk.Hashes.Rows.Count;
                CurrentProgress      = 0;
                Dialog.ProgressMin   = 0;
                Dialog.ProgressMax   = MaximumProgress;
                Dialog.ProgressValue = 0;
                Dialog.StatusText    = "Экспорт...";
            });

            Worker.RunWorkerAsync();
        }

        protected abstract void ExportProc(object sender, DoWorkEventArgs e);

        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Dialog.InvokeAsync(() =>
            {
                Dialog.ProgressValue = CurrentProgress;
                Dialog.CounterText = Math.Round(100.0 * CurrentProgress / MaximumProgress) + "%";
            });
        }

        private void OnExportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dialog.InvokeAsync(() =>
            {
                Dialog.IsBusy = false;
                Dialog.StatusText = Dialog.Cancelled ? "Операция отменена." : "Экспорт завершен.";
                Dialog.CancelButtonText = "Закрыть";
                if (Dialog.Cancelled && Dialog.UserClose)
                {
                    Dialog.Close();
                }
            });
        }
    }
}
