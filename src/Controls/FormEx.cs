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

using System.Collections.Concurrent;

namespace System.Windows.Forms
{
    /// <summary>
    ///     Represents a window or dialog box that makes up an application's user interface.
    /// </summary>
    public class FormEx : Form
    {
        private readonly ConcurrentQueue<Action> messageQueue;
        private IntPtr handle;
        private bool mqCaptured = false;

        public FormEx() { messageQueue = new ConcurrentQueue<Action>(); }

        /// <summary>
        ///     Gets a value indicating whether the form is active.
        /// </summary>
        public bool IsActive
        {
            get;
            private set;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            handle = Handle;
            base.OnHandleCreated(e);
        }

        protected override void WndProc(ref Message m)
        {
            switch (unchecked((uint)m.Msg))
            {
            case WinAPI.WM.WM_NCACTIVATE:
                IsActive = (m.WParam != IntPtr.Zero);
                break;
            }
            // check if queue is already captured because callback code may cause
            // this method to be called again, breaking the callback execution order
            if (!mqCaptured)
            {
                mqCaptured = true;
                Action callback;
                while (messageQueue.Count > 0 && messageQueue.TryDequeue(out callback))
                    callback();
                mqCaptured = false;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Same as Invoke, but closes created handle.
        /// </summary>
        public void InvokeSync(Action callback)
        {
            var result = BeginInvoke(callback);
            using (result.AsyncWaitHandle)
            {
                EndInvoke(result);
            }
        }
        
        /// <summary>
        ///     Adds a callback to the queue to be invoked from WndProc.
        /// </summary>
        public void InvokeAsync(Action callback)
        {
            messageQueue.Enqueue(callback);
            WinAPI.PostMessage(handle, 0, 0, 0);
        }
    }
}
