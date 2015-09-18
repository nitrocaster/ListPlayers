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
using System.Windows.Forms;

namespace ListPlayers.PcdbExport
{
    public sealed partial class TextExporterDialog : FormEx, ITextExporterView
    {
        private bool cancelled;
        private bool isBusy = false;
        private bool userclose = false;

        public TextExporterDialog() { InitializeComponent(); }
        
        DialogResult ITextExporterView.ShowDialog() { return base.ShowDialog(); }
        
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
