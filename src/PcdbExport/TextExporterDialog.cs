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
using System.Windows.Forms;
using ListPlayers.PcdbModel;

namespace ListPlayers.PcdbExport
{
    public sealed partial class TextExporterDialog : FormEx, ITextExporterView
    {
        private bool cancelled;
        private Action startExport;
        private bool isBusy = false;
        private bool userclose = false;

        public TextExporterDialog() { InitializeComponent(); }

        public string Destination
        {
            get;
            set;
        }

        public DialogResult Export(PcdbChunk chunk, ExportFormat format)
        {
            if (isBusy)
                throw new InvalidOperationException("Export has been already started.");
            switch (format)
            {
            case ExportFormat.Txt:
            {
                var exporter = new ExporterTxt();
                startExport = () => exporter.RunExport(this, Destination, chunk);
                var result = ShowDialog();
                exporter.Dispose();
                return result;
            }
            case ExportFormat.Radb:
            {
                var exporter = new ExporterRadb();
                startExport = () => exporter.RunExport(this, Destination, chunk);
                var result = ShowDialog();
                exporter.Dispose();
                return result;
            }
            default:
                throw new InvalidEnumArgumentException("Format not supported: " + format);
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!isBusy)
            {
                Close();
                return;
            }
            cancelled = true;
        }

        private void TextExporterDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isBusy)
                return;
            cancelled = true;
            userclose = true;
            e.Cancel = true;
        }

        private void TextExporterDialog_VisibleChanged(object sender, EventArgs e)
        {
            if (isBusy)
                return;
            isBusy = true;
            startExport();
        }

        #region ITextExporterView Members

        public string StatusText
        {
            set
            {
                lStatus.Text = value;
            }
        }

        public string CounterText
        {
            set
            {
                tbCounter.Text = value;
            }
        }

        public string CancelButtonText
        {
            set
            {
                btnCancel.Text = value;
            }
        }

        public bool Cancelled
        {
            get
            {
                return cancelled;
            }
        }

        public bool IsBusy
        {
            set
            {
                isBusy = value;
            }
        }

        public bool UserClose
        {
            get
            {
                return userclose;
            }
        }

        public int ProgressValue
        {
            set
            {
                pbProgress.Value = value;
            }
        }

        public int ProgressMin
        {
            set
            {
                pbProgress.Minimum = value;
            }
        }

        public int ProgressMax
        {
            set
            {
                pbProgress.Maximum = value;
            }
        }

        #endregion
    }
}
