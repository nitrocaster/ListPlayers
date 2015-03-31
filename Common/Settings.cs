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
using System.Windows.Forms;
using FileSystem;

namespace ListPlayers.Common
{
    public static class Settings
    {
        private const string configFileName = "listplayers.ini";
        public const int LastPathesCapacity = 5;
        private static readonly List<string> recentDatabases = new List<string>(LastPathesCapacity + 1);
        private static readonly List<string> recentSources   = new List<string>(LastPathesCapacity + 1);
        public static bool SearchSubfolders = true;
        public static bool ShowAllRelatedData = false;
        public static bool HashPattern = false;
        public static bool NamePattern = false;
        public static bool IpPattern = false;

        public static string[] RecentDatabases
        {
            get { return recentDatabases.ToArray(); }
        }

        public static string[] RecentSources
        {
            get { return recentSources.ToArray(); }
        }

        public static void Load() { Load(configFileName); }

        public static void Save() { Save(configFileName); }

        private static void Load(string path)
        {
            if (!File.Exists(path))
            {
                Save(path);
                return;
            }
            IniFile cfg;
            try
            {
                cfg = new IniFile(path, Encoding.Default);
            }
            catch (Exception e)
            {
                MsgBox.Error(String.Format("Can't read file: {0}\n\n{1}", Path.GetFullPath(path), e.Message));
                return;
            }
            if (cfg.ContainsSection("global"))
            {
                cfg.TryGetBool("global", "search_subfolders", ref SearchSubfolders);
                cfg.TryGetBool("global", "show_all_related_data", ref ShowAllRelatedData);
                cfg.TryGetBool("global", "hashes_pattern", ref HashPattern);
                cfg.TryGetBool("global", "names_pattern", ref NamePattern);
                cfg.TryGetBool("global", "ips_pattern", ref IpPattern);
            }
            if (cfg.ContainsSection("last_databases"))
            {
                for (var i = 0; i < LastPathesCapacity; ++i)
                {
                    string buf = null;
                    if (!cfg.TryGetString("last_databases", i.ToString(), ref buf))
                        break;
                    recentDatabases.Insert(0, buf);
                }
            }
            if (cfg.ContainsSection("last_sources"))
            {
                for (var i = 0; i < LastPathesCapacity; ++i)
                {
                    string buf = null;
                    if (!cfg.TryGetString("last_sources", i.ToString(), ref buf))
                        break;
                    recentSources.Insert(0, buf);
                }
            }
        }

        private static void Save(string path)
        {
            try
            {
                using (var writer = new StreamWriter(path, false, Encoding.Default))
                {
                    writer.WriteLine("[global]");
                    writer.WriteLine("search_subfolders     =   {0}", Utils.BoolToString(SearchSubfolders));
                    writer.WriteLine("show_all_related_data =   {0}", Utils.BoolToString(ShowAllRelatedData));
                    writer.WriteLine("hashes_pattern        =   {0}", Utils.BoolToString(HashPattern));
                    writer.WriteLine("names_pattern         =   {0}", Utils.BoolToString(NamePattern));
                    writer.WriteLine("ips_pattern           =   {0}", Utils.BoolToString(IpPattern));
                    writer.WriteLine("\r\n[last_databases]");
                    var tmp = recentDatabases;
                    for (var i = tmp.Count - 1; i >= 0; --i)
                        writer.WriteLine((tmp.Count - i - 1) + " = " + tmp[i]);
                    writer.WriteLine("\r\n[last_sources]");
                    tmp = recentSources;
                    for (var i = tmp.Count - 1; i >= 0; --i)
                        writer.WriteLine((tmp.Count - i - 1) + " = " + tmp[i]);
                }
            }
            catch (Exception e)
            {
                MsgBox.Error(String.Format("Can't write file: {0}\n\n{1}", Path.GetFullPath(path), e.Message));
            }
        }

        public static bool SetLastDatabase(string path)
        {
            if (recentDatabases.Contains(path))
                return false;
            if (recentDatabases.Count >= LastPathesCapacity)
                recentDatabases.RemoveAt(recentDatabases.Count - 1);
            recentDatabases.Insert(0, path);
            return true;
        }

        public static bool SetLastSource(string path)
        {
            if (recentSources.Contains(path))
                return false;
            if (recentSources.Count >= LastPathesCapacity)
                recentSources.RemoveAt(recentSources.Count - 1);
            recentSources.Insert(0, path);
            return true;
        }
    }
}
