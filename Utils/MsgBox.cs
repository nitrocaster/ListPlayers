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

namespace System.Windows.Forms
{
    public static class MsgBox
    {
        private static readonly string appName = Application.ProductName;

        public static void Error(string text)
        {
            Show(text, MessageBoxIcon.Error);
        }

        public static void Warning(string text)
        {
            Show(text, MessageBoxIcon.Warning);
        }

        public static void Info(string text)
        {
            Show(text, MessageBoxIcon.Information);
        }

        public static DialogResult Question(string text, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return MessageBox.Show(text, appName, buttons, MessageBoxIcon.Question, defaultButton);
        }

        private static void Show(string text, MessageBoxIcon icon)
        {
            MessageBox.Show(text, appName, MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1);
        }
    }
}
