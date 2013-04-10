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
using System.Windows.Forms;
using System.Reflection;
using ListPlayers.Service;
using Microsoft.Win32;
using System.IO;
using ListPlayers.Common;
using ListPlayers.PcdbModel;
using ListPlayers.Dialogs;


namespace ListPlayers
{
    public static class Root
    {
        private const string buildDate = "Apr 11 2013";
        public static readonly Version Version = Assembly.GetExecutingAssembly().GetName().Version;
        public static readonly string BuildString = String.Format("v. {0}.{1}.{2}, {3}", Version.Major, Version.Minor, Version.Build, buildDate);
        public static string SelfPath = "null";


        [STAThread]
        private static void Main(string[] args)
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                if (args.Length == 2)
                {
                    switch (args[0])
                    {
                        case "-clean":
                        {
                            CleanDatabase(args[1]);
                            return;
                        }
                    }
                }

                SelfPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
                CheckFileType();

                Settings.Load();

                var mainDialog = new MainDialog();
                Utils.CreateThread(Updater.CheckForUpdates, "Updater");

                Application.Run(mainDialog);

                Settings.Save();
            }
            catch (Exception e)
            {
                MsgBox.Error("Fatal error:\n" + e.Message);
            }
        }

        private static void CleanDatabase(string path)
        {
            if (!File.Exists(path))
            {
                MsgBox.Error("Can't open file: " + path);
                return;
            }
            var database = PcdbFile.Open(path);
            database.OpenConnection();
            int names;
            int ips;
            database.Clean(out names, out ips);
            database.CloseConnection();

            MsgBox.Info(String.Format("Cleaned:\t\t\nNames: {0}\nIPs: {1}", names, ips));
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MsgBox.Error("Fatal error:\n" + ((Exception)e.ExceptionObject).Message);
        }

        private static void CheckFileType()
        {
            const string pcdbExtKey  = @"HKEY_CLASSES_ROOT\.pcdb";
            const string pcdbTypeKey = @"HKEY_CLASSES_ROOT\PlayersCorrespondenceDatabase";

            if ((string)Registry.GetValue(pcdbExtKey, null, null) == "PlayersCorrespondenceDatabase")
            {
                return;
            }

            Registry.SetValue(pcdbExtKey,  null, "PlayersCorrespondenceDatabase",   RegistryValueKind.String);
            Registry.SetValue(pcdbTypeKey, null, "Players Correspondence Database", RegistryValueKind.String);
            Registry.SetValue(pcdbTypeKey + "\\DefaultIcon", null, "imageres.dll,-67", RegistryValueKind.String);
        }
    }
}
