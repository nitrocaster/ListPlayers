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
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    [ToolboxBitmap(typeof(Panel))]
    public class HorizontalPanel : Panel
    {
        private static readonly Bitmap gradient;
        private static readonly Color defaultBackground = Color.FromArgb(241, 245, 251);
        private Font defaultFont;

        static HorizontalPanel()
        {
            var gradientColors = new[]
            {
                Color.FromArgb(204, 217, 234),
                Color.FromArgb(217, 227, 240),
                Color.FromArgb(232, 238, 247),
                Color.FromArgb(237, 242, 249),
                Color.FromArgb(240, 244, 250),
                Color.FromArgb(241, 245, 251)
            };
            gradient = new Bitmap(1, 6);
            for (var i = 0; i < 6; ++i)
            {
                gradient.SetPixel(0, i, gradientColors[i]);
            }
        }

        public HorizontalPanel()
        {
            defaultFont = new Font("Segoe UI", 9, FontStyle.Regular, GraphicsUnit.Point, 0);
            BackColor = defaultBackground;
            Font = defaultFont;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            var label = e.Control as LinkLabel;
            if (label != null)
            {
                label.Font = defaultFont;
                label.LinkBehavior = LinkBehavior.HoverUnderline;
                label.LinkColor = Color.FromArgb(64, 64, 64);
                label.ActiveLinkColor = Color.Blue;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var brush = new TextureBrush(gradient, WrapMode.TileFlipX))
            {
                e.Graphics.FillRectangle(brush, 0, 0, Width, gradient.Height);
            }
        }
    }
}
