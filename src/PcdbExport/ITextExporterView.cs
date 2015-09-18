﻿/*
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
    public interface ITextExporterView
    {
        string StatusText { set; }

        string CounterText { set; }

        string CancelButtonText { set; }

        bool Cancelled { get; }

        bool IsBusy { set; }

        bool UserClose { get; }

        int ProgressValue { set; }

        int ProgressMin { set; }

        int ProgressMax { set; }

        void InvokeAsync(Action callback);
        DialogResult ShowDialog();
        void Close();
    }
}
