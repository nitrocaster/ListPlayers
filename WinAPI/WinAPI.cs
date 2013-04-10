// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

[SuppressUnmanagedCodeSecurity]
public static unsafe partial class WinAPI
{
    public const int IDC_HAND = 0x7F89;

    //Button styles
    public const int BS_COMMANDLINK = 0x0000000E;
    //Button messages
    public const uint BCM_SETNOTE = 0x00001609;
    public const uint BCM_SETSHIELD = 0x0000160C;
    public const uint BM_SETIMAGE = 0x00F7;
    public const uint BCM_GETNOTE = 0x0000160A;
    public const uint BCM_GETNOTELENGTH = 0x0000160B;


    [DllImport(ExternDll.USER32, SetLastError = true)]
    public static extern VoidPtr LoadCursor(VoidPtr hInstance, int lpCursorName);

    [DllImport(ExternDll.USER32, SetLastError = true)]
    public static extern VoidPtr SetCursor(VoidPtr hCursor);
    

    [DllImport(ExternDll.USER32, CharSet = CharSet.Auto, SetLastError = true)]
    public static extern VoidPtr SendMessage(VoidPtr hWnd, uint Msg, VoidPtr wParam, VoidPtr lParam);

    [DllImport(ExternDll.USER32, CharSet = CharSet.Auto, SetLastError = true)]
    public static extern VoidPtr SendMessage(VoidPtr hWnd, uint Msg, VoidPtr wParam, string lParam);

    [DllImport(ExternDll.USER32, CharSet = CharSet.Auto, SetLastError = true)]
    public static extern VoidPtr SendMessage(VoidPtr hWnd, uint Msg, VoidPtr wParam, StringBuilder lParam);

    [DllImport(ExternDll.USER32, SetLastError = true)]
    public static extern bool PostMessage(VoidPtr hWnd, uint Msg, int wParam, int lParam);

    [DllImport(ExternDll.UXTHEME, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int SetWindowTheme(VoidPtr hWnd, string pszSubAppName, string pszSubIdList);
}