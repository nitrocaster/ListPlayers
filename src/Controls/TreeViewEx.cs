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
    /// Displays a hierarchical collection of labeled items, each represented by System.Windows.Forms.TreeNode.
    /// </summary>
    /// <remarks>Added support for Windows Explorer style.</remarks>
    [ToolboxBitmap(typeof(TreeView))]
    public class TreeViewEx : TreeView
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (OSSupport.IsVistaOrLater)
                WinAPI.SetWindowTheme(Handle, "Explorer", null);
        }
    }
}
