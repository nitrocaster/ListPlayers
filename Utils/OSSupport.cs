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

namespace System
{
	public static class OSSupport
    {
        private const int vistaMajorVersion = 6;
        private const int sevenMajorVersion = vistaMajorVersion;
        private const int sevenMinorVersion = 1;

        private static readonly bool isVistaOrLater =
            (  Environment.OSVersion.Platform == PlatformID.Win32NT
            && Environment.OSVersion.Version.Major >= vistaMajorVersion);

		public static bool IsVistaOrLater
        {
			get
			{
			    return isVistaOrLater;
			}
		}

        private static readonly bool isSevenOrLater = 
            (  Environment.OSVersion.Platform == PlatformID.Win32NT
            && Environment.OSVersion.Version.Major >= sevenMajorVersion
            && Environment.OSVersion.Version.Minor >= sevenMinorVersion);

        public static bool IsSevenOrLater
        {
            get
            {
                return isSevenOrLater;
            }
        }
	}
}
