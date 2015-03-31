// Original source code taken from Vista Bridge Sample Library (http://archive.msdn.microsoft.com/VistaBridge)
// Copyright (c) Microsoft Corporation. All rights reserved.

using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace System.Windows.Forms
{
    /// <summary>
    ///     Represents a CommandLink button.
    /// </summary>
    [ToolboxBitmap(typeof(Button))]
    public class CommandLink : Button
    {
        private bool shieldIconDisplayed;

        #region Interop helpers

        private static int AddCommandLinkStyle(int style)
        {
            var newStyle = style;
            // Only add BS_COMMANDLINK style on Windows Vista or above.
            // Otherwise, button creation will fail.
            if (OSSupport.IsVistaOrLater)
                newStyle |= WinAPI.BS_COMMANDLINK;
            return newStyle;
        }

        private static unsafe string GetNote(Button button)
        {
            var retVal = WinAPI.SendMessage(button.Handle, WinAPI.BCM_GETNOTELENGTH, VoidPtr.Null, VoidPtr.Null);
            // Add 1 for null terminator, to get the entire string back.
            var len = (int)retVal + 1;
            var builder = new StringBuilder(len);
            retVal = WinAPI.SendMessage(button.Handle, WinAPI.BCM_GETNOTE, &len, builder);
            return builder.ToString();
        }

        private static void SetNote(Button button, string text)
        {
            // This call will be ignored on versions earlier than 
            // Windows Vista.
            WinAPI.SendMessage(button.Handle, WinAPI.BCM_SETNOTE, VoidPtr.Null, text);
        }

        internal static void SetShieldIcon(Button button, bool show)
        {
            var fRequired = (VoidPtr)(show ? 1 : 0);
            WinAPI.SendMessage(button.Handle, WinAPI.BCM_SETSHIELD, VoidPtr.Null, fRequired);
        }

        #endregion

        public CommandLink() { FlatStyle = FlatStyle.System; }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.Style = AddCommandLinkStyle(cp.Style);
                return cp;
            }
        }

        // Add Design-Time Support.

        // Increase default width.
        protected override Size DefaultSize
        {
            get { return new Size(180, 60); }
        }

        // Enable note text to be set at design-time.
        [Category("Appearance")]
        [Description("Specifies the supporting note text.")]
        [Browsable(true)]
        [DefaultValue("(Note Text)")]
        public string NoteText
        {
            get { return (GetNote(this)); }
            set { SetNote(this, value); }
        }

        // Enable shield icon to be set at design-time.
        [Category("Appearance")]
        [Description("Indicates whether the button should be " +
            "decorated with the security shield icon (Windows Vista only).")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool ShieldIcon
        {
            get { return (shieldIconDisplayed); }
            set
            {
                shieldIconDisplayed = value;
                SetShieldIcon(this, shieldIconDisplayed);
            }
        }
    }
}
