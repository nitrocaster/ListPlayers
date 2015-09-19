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

using System.Drawing;

namespace System.Windows.Forms
{
    /// <summary>
    ///     Represents a Windows label control that can display hyperlinks.
    /// </summary>
    /// <remarks>Fixed incorrect hand cursor.</remarks>
    [ToolboxBitmap(typeof(LinkLabel))]
    public class LinkLabelEx : LinkLabel
    {
        private readonly IntPtr hCursorHand;

        public LinkLabelEx()
        { hCursorHand = WinAPI.LoadCursor(VoidPtr.Null, WinAPI.IDC_HAND); }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WinAPI.WM.WM_SETCURSOR)
            {
                if (OverrideCursor == Cursors.Hand)
                    WinAPI.SetCursor(hCursorHand);
                else
                    WinAPI.SetCursor(Cursors.Arrow.Handle);
                m.Result = IntPtr.Zero;
                return;
            }
            base.WndProc(ref m);
        }
    }
}
