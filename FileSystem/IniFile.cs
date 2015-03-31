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
using System.IO;
using System.Text;

namespace FileSystem
{
    public enum IniExceptionCode
    {
        StrToInt,
        StrToBool,
        KeyNotFound,
        SectionNotFound
    };

    public sealed class IniException : Exception
    {
        public IniExceptionCode Details
        {
            get;
            private set;
        }

        public IniException(IniExceptionCode details) { Details = details; }
    }

    public sealed class IniFile
    {
        private readonly string[] lines;
        private readonly SortedList<string, int> sections;

        public IniFile(string filename, Encoding encoding)
        {
            lines = File.ReadAllLines(filename, encoding);
            sections = new SortedList<string, int>(32);
            ScanSections();
        }

        public static KeyValuePair<string, string> ExtractKeyValuePair(string line, bool trimComment)
        {
            string key = null;
            string value = null;
            var buf = line.Trim();
            if (trimComment)
            {
                var semicolon = buf.IndexOf(';');
                if (semicolon == 0)
                    return new KeyValuePair<string, string>(key, value);
                if (semicolon > 0)
                    buf = buf.Substring(0, semicolon).Trim();
            }            
            var delim = buf.IndexOf('=');
            if (delim == -1)
            {
                key = buf;
                return new KeyValuePair<string, string>(key, value);
            }
            key = buf.Substring(0, delim).Trim();
            if (delim < buf.Length-1)
                value = buf.Substring(delim + 1).Trim();
            return new KeyValuePair<string, string>(key, value);
        }
        
        public string GetString(string section, string key)
        {
            if (!sections.ContainsKey(section))
                throw new IniException(IniExceptionCode.SectionNotFound);
            var value = "";
            var keyFound = false;
            for (var i = sections[section]; i < lines.Length; ++i)
            {
                var pair = ExtractKeyValuePair(lines[i], true);
                if (pair.Key == key)
                {
                    value = pair.Value;
                    keyFound = true;
                    break;
                }
            }
            if (!keyFound)
                throw new IniException(IniExceptionCode.KeyNotFound);
            return value;
        }

        public int GetInt32(string section, string key)
        {
            var buf = GetString(section, key);
            try
            {
                return Convert.ToInt32(buf);
            }
            catch
            {
                throw new IniException(IniExceptionCode.StrToInt);
            }
        }

        public bool GetBool(string section, string key)
        {
            var buf = GetString(section, key);
            buf = buf.ToLower();
            try
            {
                return Utils.StringToBool(buf);
            }
            catch
            {
                throw new IniException(IniExceptionCode.StrToBool);
            }
        }

        public bool TryGetString(string section, string key, ref string result)
        {
            try
            {
                result = GetString(section, key);
                return true;
            }
            catch (IniException)
            {
                return false;
            }
        }

        public bool TryGetInt32(string section, string key, ref int result)
        {
            try
            {
                result = GetInt32(section, key);
                return true;
            }
            catch (IniException)
            {
                return false;
            }
        }

        public bool TryGetBool(string section, string key, ref bool result)
        {
            try
            {
                result = GetBool(section, key);
                return true;
            }
            catch (IniException)
            {
                return false;
            }
        }

        public bool ContainsSection(string section) { return sections.ContainsKey(section); }

        public int GetSectionCount(Predicate<string> match = null)
        {
            var sectionCount = sections.Count;
            if (match == null)
                return sectionCount;
            var result = 0;
            for (var i = 0; i < sectionCount; ++i)
            {
                if (match(sections.Keys[i]))
                    ++result;
            }
            return result;
        }

        private void ScanSections()
        {
            for (var i = 0; i < lines.Length; ++i)
            {
                var len = lines[i].Length;
                if (len < 3)
                    continue;
                if (lines[i][0] == ';')
                    continue;
                var sect1 = lines[i].IndexOf('[');
                var sect2 = lines[i].IndexOf(']');
                if (sect2 - sect1 < 2)
                    continue;
                if (sect1 < sect2 && sect1 >= 0 && sect2 >= 0)
                {
                    var buf = lines[i].Substring(sect1 + 1, sect2 - sect1 - 1);
                    if (!sections.ContainsKey(buf))
                        sections.Add(buf, i);
                }
            }
        }
    }
}
