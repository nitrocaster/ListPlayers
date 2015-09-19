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
using System.Data.SQLite;

namespace ListPlayers.PcdbModel
{
    internal static class PcdbUtil
    {
        public static int GetRevision(SQLiteDatabase db)
        {
            if (db.Execute("SELECT DATEINFO FROM NAMES WHERE 0") == null)
                return (int)PcdbRevision.Rev0;
            if (db.Execute("SELECT 1 FROM DBVERSION WHERE 0") == null)
                return (int)PcdbRevision.Rev1;
            return Convert.ToInt32(db.Execute("SELECT VERSION FROM DBVERSION").Rows[0][0]);
        }

        public static PcdbGameVersion GetGameVersion(SQLiteDatabase db)
        { return (PcdbGameVersion)Convert.ToInt32(db.Execute("SELECT TYPEID FROM DBTYPE").Rows[0][0]); }
    }
}
