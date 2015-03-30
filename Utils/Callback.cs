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

using System.Windows.Forms;

namespace System
{
    public static class Callback
    {
        public delegate void PfnVoid();
        public delegate void PfnVoid<in T>(T arg);
        public delegate void PfnVoid<in T1, in T2>(T1 arg1, T2 arg2);
        public delegate void PfnVoid<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);
        public delegate void PfnVoid<in T1, in T2, in T3, in T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        public delegate TResult Pfn<out TResult>();
        public delegate TResult Pfn<out TResult, in T>(T arg);
        public delegate TResult Pfn<out TResult, in T1, in T2>(T1 arg1, T2 arg2);
        public delegate TResult Pfn<out TResult, in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);
        public delegate TResult Pfn<out TResult, in T1, in T2, in T3, in T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        public static TResult Invoke<TResult>(Control ctrl, Pfn<TResult> target)
        {
            var result = ctrl.BeginInvoke(target);
            using (result.AsyncWaitHandle)
            {
                return (TResult)ctrl.EndInvoke(result);
            }
        }

        public static TResult Invoke<TResult, T>(Control ctrl, Pfn<TResult, T> target, T arg)
        {
            var result = ctrl.BeginInvoke(target, arg);
            using (result.AsyncWaitHandle)
            {
                return (TResult)ctrl.EndInvoke(result);
            }
        }

        public static TResult Invoke<TResult, T1, T2>(Control ctrl, Pfn<TResult, T1, T2> target, T1 arg1, T2 arg2)
        {
            var result = ctrl.BeginInvoke(target, arg1, arg2);
            using (result.AsyncWaitHandle)
            {
                return (TResult)ctrl.EndInvoke(result);
            }
        }

        public static TResult Invoke<TResult, T1, T2, T3>(Control ctrl, Pfn<TResult, T1, T2, T3> target, T1 arg1, T2 arg2, T3 arg3)
        {
            var result = ctrl.BeginInvoke(target, arg1, arg2, arg3);
            using (result.AsyncWaitHandle)
            {
                return (TResult)ctrl.EndInvoke(result);
            }
        }

        public static TResult Invoke<TResult, T1, T2, T3, T4>(Control ctrl, Pfn<TResult, T1, T2, T3, T4> target, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var result = ctrl.BeginInvoke(target, arg1, arg2, arg3, arg4);
            using (result.AsyncWaitHandle)
            {
                return (TResult)ctrl.EndInvoke(result);
            }
        }

        public static void Invoke(Control ctrl, PfnVoid target)
        {
            var result = ctrl.BeginInvoke(target);
            using (result.AsyncWaitHandle)
            {
                ctrl.EndInvoke(result);
            }
        }

        public static void Invoke<T>(Control ctrl, PfnVoid<T> target, T arg)
        {
            var result = ctrl.BeginInvoke(target, arg);
            using (result.AsyncWaitHandle)
            {
                ctrl.EndInvoke(result);
            }
        }

        public static void Invoke<T1, T2>(Control ctrl, PfnVoid<T1, T2> target, T1 arg1, T2 arg2)
        {
            var result = ctrl.BeginInvoke(target, arg1, arg2);
            using (result.AsyncWaitHandle)
            {
                ctrl.EndInvoke(result);
            }
        }

        public static void Invoke<T1, T2, T3>(Control ctrl, PfnVoid<T1, T2, T3> target, T1 arg1, T2 arg2, T3 arg3)
        {
            var result = ctrl.BeginInvoke(target, arg1, arg2, arg3);
            using (result.AsyncWaitHandle)
            {
                ctrl.EndInvoke(result);
            }
        }

        public static void Invoke<T1, T2, T3, T4>(Control ctrl, PfnVoid<T1, T2, T3, T4> target, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var result = ctrl.BeginInvoke(target, arg1, arg2, arg3, arg4);
            using (result.AsyncWaitHandle)
            {
                ctrl.EndInvoke(result);
            }
        }
    }
}
