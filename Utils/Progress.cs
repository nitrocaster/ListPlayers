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

namespace ListPlayers
{
    public struct Progress
    {
        private uint current;
        private uint maximum;

        public Progress(uint max)
        {
            current = 0;
            maximum = max;
        }

        public uint Current
        {
            get { return current; }
            set
            {
                if (value > maximum)
                    value = maximum;
                current = value;
            }
        }

        public uint Maximum
        {
            get { return maximum; }
            set
            {
                maximum = value;
                if (current > maximum)
                    current = maximum;
            }
        }

        public double Percentage
        {
            get { return 1.0 * current / maximum; }
        }
    }
}
