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
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

public static partial class Utils
{
    /// <summary>
    /// "dd.MM.yy HH:mm:ss"
    /// </summary>
    public const string DateTimePatternShort = "dd.MM.yy HH:mm:ss";

    /// <summary>
    /// "dd.MM.yyyy HH:mm:ss"
    /// </summary>
    public const string DateTimePatternLong  = "dd.MM.yyyy HH:mm:ss";
    
    public static Thread CreateThread(ThreadStart target, string name, bool background = true, bool suspended = false)
    {
        var trd = new Thread(target)
        {
            Name = name,
            IsBackground = background,
            CurrentCulture = CultureInfo.InvariantCulture
        };
        if (!suspended)
            trd.Start();
        return trd;
    }

    public static Thread CreateThread(ParameterizedThreadStart target, object arg, string name, bool background = true, bool suspended = false)
    {
        var trd = new Thread(target)
        {
            Name            = name,
            IsBackground    = background,
            CurrentCulture  = CultureInfo.InvariantCulture
        };
        if (!suspended)
            trd.Start(arg);
        return trd;
    }

    public static string BoolToString(bool b)
    {
        return b ? "1" : "0";
    }

    public static bool StringToBool(string a)
    {
        if (a == "1" || a == "on")
            return true;
        if (a == "0" || a == "off")
            return false;
        throw new ArgumentException();
    }

    public static bool Atob(string a, out bool result)
    {
        if (a == "1" || a == "on")
        {
            result = true;
            return true;
        }
        if (a == "0" || a == "off")
        {
            result = false;
            return true;
        }
        result = false;
        return false;
    }

    public static string Itoa(int i)
    {
        return i.ToString(CultureInfo.InvariantCulture);
    }

    public static int Atoi(string a)
    {
        return int.Parse(a, NumberStyles.Integer, CultureInfo.InvariantCulture);
    }

    public static bool Atoi(string a, out int result)
    {
        return int.TryParse(a, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
    }

    public static string Ltoa(long i)
    {
        return i.ToString(CultureInfo.InvariantCulture);
    }

    public static long Atol(string a)
    {
        return long.Parse(a, NumberStyles.Integer, CultureInfo.InvariantCulture);
    }

    public static bool Atol(string a, out long result)
    {
        return long.TryParse(a, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
    }

    public static string Ftoa(float f)
    {
        return f.ToString("#0.000###", CultureInfo.InvariantCulture);
    }

    public static float Atof(string a)
    {
        return float.Parse(a, NumberStyles.Float, CultureInfo.InvariantCulture);
    }

    public static bool Atof(string a, out float result)
    {
        return float.TryParse(a, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
    }

    public static string Dtoa(double d)
    {
        return d.ToString("#0.000000", CultureInfo.InvariantCulture);
    }

    public static double Atod(string a)
    {
        return double.Parse(a, NumberStyles.Float, CultureInfo.InvariantCulture);
    }

    public static bool Atod(string a, out double result)
    {
        return double.TryParse(a, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
    }
    
    public static string GetCurrentDateTime()
    {
        return DateTime.Now.ToString(DateTimePatternShort, CultureInfo.InvariantCulture);
    }
}
