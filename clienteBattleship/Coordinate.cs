using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clienteBattleship
{
    class Coordinate
    {
        private int column;
        private int row;

        public int Column
        {
            get
            {
                return column;
            }

            set
            {
                column = value;
            }
        }

        public int Row
        {
            get
            {
                return row;
            }

            set
            {
                row = value;
            }
        }
    }
}
